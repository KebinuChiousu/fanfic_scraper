Imports System
Imports System.text
Imports System.net
Imports System.IO
Imports System.Xml

Class FFNet
    Inherits clsFanfic

    Private Browser As clsWeb

#Region "Retrieval Functions"

    Public Overrides _
    Function GrabTitle( _
                        ByVal htmlDoc As String _
                      ) As String

        Dim xmldoc As New XmlDocument

        xmldoc.LoadXml(htmlDoc)

        Dim ch As Integer = 0

        Dim title As String

        Dim value As String = GetFirstNodeValue(xmldoc, "//title")

        value = Replace(value, "| FanFiction", "")

        ch = InStr(value, "Chapter")

        If ch > 0 Then
            title = Trim(Mid(value, 1, ch - 1))
            value = Replace(value, title, "")
            ch = LastPos(value, ",")
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
        value = Replace(value, "| FanFiction", "")

        If value <> "" Then
            value = Trim(Mid(value, LastPos(value, ",") + 1))
        End If

        xmldoc = Nothing

        Return value

    End Function

    Public Overrides _
    Function GrabDate( _
                       ByVal htmlDoc As String, _
                       ByVal title As String _
                     ) As String

        Dim ret As String = ""

        Dim author As String = GrabAuthor(htmlDoc)
        Dim rss_link As String = author
        Dim fic As clsFanfic.Story = Nothing

Retry:

        If IsNothing(datasetRSS) Then
            MyBase.GrabFeed(rss_link)
        End If

        fic = GetStoryInfoByID(GetStoryID(StoryURL))

        If LCase(fic.Author) <> LCase(author) Then
            datasetRSS = Nothing
            GoTo Retry
        End If

        Select Case title
            Case "Published: "
                ret = fic.PublishDate
            Case "Updated: "
                ret = fic.UpdateDate
        End Select

        Return ret

    End Function

    Public Overrides _
    Function GrabAuthor(ByVal htmldoc As String) As String

        Dim xmldoc As New XmlDocument

        xmldoc.LoadXml(htmldoc)

        Dim temp As String = ""
        Dim XmlList As XmlNodeList
        Dim node As XmlNode
        Dim count As Integer

        XmlList = GetNodes(xmldoc, "//a")

        For count = 0 To XmlList.Count - 1
            node = XmlList(count)
            temp = node.OuterXml
            If InStr(temp, "/u/") <> 0 Then
                temp = node.InnerText
                Exit For
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
    Function GetChapters( _
                          ByVal htmlDoc As String _
                        ) As String()

        Dim data() As String

        data = GetOptionValues(htmlDoc, "Chapter Navigation")

        If IsNothing(data) Then
            ReDim data(0)
            data(0) = "Chapter 1"
        End If

        Me.Chapters = data

        Return data

    End Function

#End Region

    Public Overrides _
    Function GrabData(ByVal url As String) As String

        Dim htmldoc As String

        htmldoc = MyBase.GrabData(url)
        
        StripTags(htmldoc, "menu", paramType.Attribute, partialM.Yes)
        StripTags(htmldoc, "javascript", paramType.Attribute, partialM.Yes)
        StripTags(htmldoc, "a2a", paramType.Attribute, partialM.Yes)

        'StripTags(htmldoc, "a", paramType.Tag, partialM.No)
        StripTags(htmldoc, "button", paramType.Tag, partialM.Yes)
        StripTags(htmldoc, "link", paramType.Tag, partialM.Yes)
        StripTags(htmldoc, "img", paramType.Tag, partialM.Yes)
        StripTags(htmldoc, "script", paramType.Tag, partialM.Yes)
        StripTags(htmldoc, "style", paramType.Tag, partialM.Yes)
        StripTags(htmldoc, "meta", paramType.Tag, partialM.Yes)

        StripTags(htmldoc, "dropdown", paramType.Attribute, partialM.Yes)
        StripTags(htmldoc, "cursor", paramType.Attribute, partialM.Yes)

        Return htmldoc

    End Function

    Protected Overrides Function GrabFeedData(ByRef rss As String) As System.Xml.XmlDocument

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

            txtresult = Browser.DownloadPage(rss)

            Try

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

            Catch
                rss = ""
            End Try

        End If

        If rss = "" Then
            xmldoc = Nothing
        Else
            xmldoc = DownloadXML(rss)
            xmldoc = CleanXML(xmldoc)
            xmldoc = CleanFeed(xmldoc)
        End If

        If Not IsNothing(xmldoc) Then

        End If

        Return xmldoc

    End Function

    Public Overrides _
    Function InitialDownload(ByVal link As String) As String

        Dim host As String
        Dim htmlDoc As String

        Dim URL As URL

        URL = ExtractUrl(link)

        host = URL.Scheme & "://" & URL.Host & "/s/"
        link = Replace(link, host, "")
        host = host & Mid(link, 1, InStr(link, "/") - 1)
        link = host & "/1/"

        htmlDoc = GrabData(link)

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

        Dim url As URL

        url = ExtractUrl(link)

        host = url.Scheme & "://" & url.Host & "/s/"

        temp = Replace(link, host, "")
        params = Split(temp, "/")

        link = host & params(0) & "/"

        Dim htmldoc As String

        htmldoc = GrabData(link & (index + 1) & "/")

        ProcessChapters = htmldoc

    End Function

    Public Overrides Function GrabStoryInfo(ByVal idx As Integer) As clsFanfic.Story

        Dim dsRSS As DataSet = MyBase.datasetRSS

        Dim fic As New clsFanfic.Story

        Dim txtSummary() As String
        Dim txtResult As String
        Dim category As String

        Dim index As Integer

        Dim rss_url As String

        ' Story Name
        fic.Title = dsRSS.Tables("entry"). _
                        Rows(idx).Item("title")



        ' Story Author
        fic.Author = dsRSS.Tables("author"). _
                          Rows(idx).Item(0)

        rss_url = fic.Author

        fic.AuthorURL = GetAuthorURL(rss_url)

        ' Story Location
        fic.StoryURL = dsRSS.Tables("link"). _
                          Rows(idx).Item(1)

        'Process Summary
        txtResult = dsRSS.Tables("summary"). _
                    Rows(idx).Item(1) & vbCrLf

        fic.ID = Me.GetStoryID(fic.StoryURL)

        txtResult = Replace(txtResult, "<hr>", "<br>")
        txtResult = Replace(txtResult, "<hr size=1>", "<br>")

        txtResult = Replace(txtResult, ", Words", "<br>Words")
        txtResult = Replace(txtResult, ", Reviews", "<br>Reviews")

        txtSummary = Split(txtResult, "<br>")

        'Category
        category = Mid(txtSummary(1), Len("Category: "))
        category = Mid(category, (InStr(category, ">") + 1))
        category = Replace(category, "</a>", "")

        fic.Category = category

        If InStr(txtSummary(5), "Words") = 0 Then
            index = 5
        Else
            index = 4
        End If


        'Chapter Count
        fic.ChapterCount = txtSummary(index)

        fic.PublishDate = dsRSS.Tables("entry"). _
                          Rows(idx).Item("published")

        'Last Updated
        fic.UpdateDate = dsRSS.Tables("entry"). _
                         Rows(idx).Item("id")
        fic.UpdateDate = Split(fic.UpdateDate, ",")(1)
        fic.UpdateDate = Split(fic.UpdateDate, ":")(0)
        fic.UpdateDate = CDate(fic.UpdateDate).ToShortDateString

        'Published
        fic.PublishDate = dsRSS.Tables("entry"). _
                          Rows(idx).Item("published")
        fic.PublishDate = CDate(fic.PublishDate).ToShortDateString

        'Summary

        fic.Summary = txtSummary(UBound(txtSummary))

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

        If InStr(link, Me.HostName) = 0 Then
            link = Replace(link, ".", "")
            link = "http://www.fanfiction.net/~" & link
        End If

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

    Public Sub New()

        Browser = New clsWeb

    End Sub
End Class
