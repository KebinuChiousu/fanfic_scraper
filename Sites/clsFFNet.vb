Imports System
Imports System.text
Imports System.net
Imports System.IO
Imports System.Xml

Class FFNet
    Inherits Fanfic

#Region "Retrieval Functions"

    Public Overrides _
    Function GrabTitle( _
                        ByVal htmlDoc As String _
                      ) As String

        Dim xmldoc As New XmlDocument

        xmldoc.LoadXml(htmlDoc)

        Dim ch As Integer = 0

        Dim title As String

        Dim value As String = GetFirstNodeValue(xmlDoc, "//title")

        value = Replace(value, " - FanFiction.Net", "")

        ch = InStr(value, "Chapter")

        If ch > 0 Then
            title = Trim(Mid(value, 1, ch - 1))
            value = Replace(value, title, "")
            ch = InStr(value, ", a")
            value = title & "<br><br>" & Mid(value, 1, ch - 1)
            'value = title

        End If

        xmldoc = Nothing

        Return value


    End Function


    Public Overrides _
    Function GrabSeries(ByVal htmlDoc As String) As String

        Dim xmldoc As New XmlDocument

        xmldoc.LoadXml(htmlDoc)

        Dim value As String = GetFirstNodeValue(xmldoc, "//title")
        value = Replace(value, "FanFiction.Net", "")

        If value <> "" Then
            value = Replace(value, " - ", "")
            value = Trim(Mid(value, InStr(value, ", a") + 1))
        End If

        xmldoc = Nothing

        Return value

    End Function

    Public Overrides _
    Function GrabDate( _
                       ByVal htmlDoc As String, _
                       ByVal title As String _
                     ) As String

        Dim xmldoc As New XmlDocument

        xmldoc.LoadXml(htmlDoc)

        Dim temp As String = ""
        Dim XmlList As XmlNodeList
        Dim node As XmlNode
        Dim count As Integer

        XmlList = GetNodes(xmldoc, "//div")

        For count = 0 To XmlList.Count - 1
            node = XmlList(count)
            temp = node.InnerXml
            If InStr(temp, title) <> 0 Then
                temp = node.InnerText
                Exit For
            End If
        Next

        Dim sstart As Integer

        sstart = InStr(temp, title) + Len(title)

        GrabDate = Mid(temp, sstart, 8)

        xmldoc = Nothing

    End Function

    Public Overrides _
    Function GrabAuthor(ByVal htmldoc As String) As String

        Dim xmldoc As New XmlDocument

        xmldoc.LoadXml(htmldoc)

        Dim temp As String = ""
        Dim XmlList As XmlNodeList
        Dim node As XmlNode
        Dim count As Integer
        Dim XmlList2 As XmlNodeList

        XmlList = GetNodes(xmldoc, "//td")

        For count = 0 To XmlList.Count - 1
            node = XmlList(count)
            temp = node.InnerXml
            If InStr(temp, "Author") <> 0 Then
                XmlList2 = node.SelectNodes(".//a")
                Try
                    node = XmlList2(0)
                    temp = node.InnerText
                Catch
                    temp = ""
                End Try
                If temp <> "" Then
                    Exit For
                End If
            End If
        Next

        xmldoc = Nothing

        GrabAuthor = temp

    End Function

    Public Overrides _
    Function GrabBody(ByVal htmldoc As String) As String

        Dim xmldoc As New XmlDocument

        xmldoc.LoadXml(htmldoc)

        Dim XmlList As XmlNodeList
        Dim value As String

        XmlList = GetNodes(xmldoc, "//div[@id='storytext']")

        Try
            value = XmlList(0).InnerXml
        Catch
            value = xmldoc.InnerXml
        End Try

        xmldoc = Nothing

        Return value

    End Function

    Public Overrides _
    Sub GetChapters( _
                     ByVal lst As ListBox, _
                     ByVal htmlDoc As String _
                   )

        Dim data() As String
        Dim count As Integer

        data = GetOptionValues(htmlDoc, "Chapter Navigation")

        If Not IsNothing(data) Then
            For count = 0 To UBound(data)
                lst.Items.Add(data(count))
            Next
        Else
            lst.Items.Add("Chapter 1")
        End If

    End Sub

#End Region

    Public Overrides _
    Function GrabData(ByVal url As String) As String

        Dim htmldoc As String

        htmldoc = MyBase.GrabData(url)

        StripTags(htmldoc, "menulinks", paramType.Attribute)
        StripTags(htmldoc, "menu-child xxhide", paramType.Attribute)
        StripTags(htmldoc, "xxmenu", paramType.Attribute)
        StripTags(htmldoc, "javascript", paramType.Attribute, partialM.Yes)
        StripTags(htmldoc, "a2a", paramType.Attribute, partialM.Yes)

        Return htmldoc

    End Function

    Public Overrides Function GrabFeed(ByRef rss As String) As System.Xml.XmlDocument

        Dim txtatom As String
        Dim txtresult As String
        Dim xmldoc As XmlDocument

        rss = Replace(rss, " ", "")
        rss = Replace(rss, "-", "")
        rss = Replace(rss, "'", "")

        If InStr(rss, Me.HostName) = 0 Then
            rss = Replace(rss, ".", "")
            rss = "http://www.fanfiction.net/~" & rss
        End If

        If (InStr(rss, "atom") = 0) Then

            txtresult = DownloadPage(rss)

            txtresult = Mid(txtresult, InStr(txtresult, "id: "))
            txtresult = Mid(txtresult, 1, InStr(txtresult, "<") - 1)
            If InStr(txtresult, ",") > 0 Then
                txtresult = Mid(txtresult, 1, InStr(txtresult, ",") - 1)
            End If

            txtatom = "/atom/u/"
            txtatom += Replace(txtresult, "id: ", "")

            If txtatom = "" Then rss = ""

            Dim url As URL
            url = ExtractUrl(rss)

            If InStr(txtatom, url.Scheme) = 0 Then
                txtatom = url.Scheme & "://" & url.Host & txtatom
            End If

            rss = txtatom

        End If

        If rss = "" Then
            xmldoc = Nothing
        Else
            xmldoc = DownloadXML(rss)
            xmldoc = CleanXML(xmldoc)
            xmldoc = CleanFeed(xmldoc)
        End If

        Return xmldoc

    End Function

    Public Overrides _
    Function InitialDownload(ByVal url As String) As String

        Dim host As String
        Dim htmlDoc As String

        host = "http://www.fanfiction.net/s/"
        url = Replace(url, host, "")
        host = host & Mid(url, 1, InStr(url, "/") - 1)
        url = host & "/1/"

        htmlDoc = GrabData(url)

        InitialDownload = htmlDoc

    End Function

    Public Overrides _
    Function ProcessChapters( _
                              ByVal link As String, _
                              ByVal index As Integer _
                            ) As String

        Dim host As String
        Dim temp As String
        Dim params() As String
        host = "http://www.fanfiction.net/s/"

        temp = Replace(link, host, "")
        params = Split(temp, "/")

        link = host & params(0) & "/"

        Dim htmldoc As String

        htmldoc = GrabData(link & (index + 1) & "/")

        ProcessChapters = htmldoc

    End Function

   

    Public Overrides Function GrabStoryInfo(ByRef dsRSS As System.Data.DataSet, ByVal idx As Integer) As Fanfic.Story

        Dim fic As New Fanfic.Story

        Dim txtSummary() As String
        Dim txtResult As String
        Dim category As String

        Dim index As Integer


        ' Story Name
        fic.Title = dsRSS.Tables("entry"). _
                        Rows(idx).Item("title")

        ' Story Author
        fic.Author = dsRSS.Tables("author"). _
                          Rows(idx).Item(0)
        ' Story Location
        fic.URL = dsRSS.Tables("link"). _
                          Rows(idx).Item(1)

        'Process Summary
        txtResult = dsRSS.Tables("summary"). _
                    Rows(idx).Item(1) & vbCrLf

        fic.ID = Me.GetStoryID(fic.URL)

        txtResult = Replace(txtResult, "<hr>", "<br>")
        txtResult = Replace(txtResult, "<hr size=1>", "<br>")

        txtResult = Replace(txtResult, ", Words", "<br>Words")
        txtResult = Replace(txtResult, ", Reviews", "<br>Reviews")
        txtResult = Replace(txtResult, "Updated", "<br>Updated")

        If InStr(txtResult, "Updated") = 0 Then
            txtResult = Replace(txtResult, "Published", "<br><br>Published")
        Else
            txtResult = Replace(txtResult, "Published", "<br>Published")
        End If

        txtSummary = Split(txtResult, "<br>")

        'Category
        category = Mid(txtSummary(1), Len("Category: "))
        category = Mid(category, (InStr(category, ">") + 1))
        category = Replace(category, "</a>", "")

        fic.Category = category

        If InStr(txtSummary(4), "Pairing") <> 0 Then
            index = 5
        Else
            index = 4
        End If


        'Chapter Count
        fic.ChapterCount = txtSummary(index)

        'Last Updated
        fic.UpdateDate = txtSummary(index + 3)

        'Published
        fic.PublishDate = txtSummary(index + 4)

        'Summary

        fic.Summary = txtSummary(index + 6)

        Return fic

    End Function


    Public Overrides Function GetStoryID(ByVal link As String) As String

        Dim ret As String
        Dim hl As URL
        Dim parms() As String

        hl = ExtractUrl(link)

        parms = Split(hl.URI, "/")

        ret = parms(2)

        Return ret

    End Function

    Public Overrides Function GetStoryURL(ByVal id As String) As String

        Dim link As String

        link = "http://www.fanfiction.net/s/" & id & "/1/"

        Return link

    End Function

    Public Overrides Function GetAuthorURL(ByVal link As String) As String

        Dim ret As String

        ret = Replace(link, "atom/", "")

        Return ret

    End Function

#Region "Properties"

    Public Overrides ReadOnly Property ErrorMessage() As String
        Get
            Return "story not found"
        End Get
    End Property

    Public Overrides ReadOnly Property HostName() As String
        Get
            Return "fanfiction.net"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "FFNet"
        End Get
    End Property

#End Region

End Class
