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
                        ByVal page As XmlDocument _
                      ) As String

        Dim ch As Integer = 0

        Dim title As String

        Dim value As String = GetFirstNodeValue(page, "//title")

        value = Replace(value, " - FanFiction.Net", "")

        ch = InStr(value, "Chapter")

        If ch > 0 Then
            title = Trim(Mid(value, 1, ch - 1))
            value = Replace(value, title, "")
            ch = InStr(value, ", a")
            value = title & "<br><br>" & Mid(value, 1, ch - 1)
            'value = title

        End If

        Return value


    End Function


    Public Overrides _
    Function GrabSeries(ByVal page As XmlDocument) As String


        Dim value As String = GetFirstNodeValue(page, "//title")
        value = Replace(value, "FanFiction.Net", "")

        If value <> "" Then
            value = Replace(value, " - ", "")
            value = Trim(Mid(value, InStr(value, ", a") + 1))
        End If

        Return value

    End Function

    Public Overrides _
    Function GrabDate( _
                       ByVal xmldoc As XmlDocument, _
                       ByVal title As String _
                     ) As String

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

    End Function

    Public Overrides _
    Function GrabAuthor(ByVal xmldoc As XmlDocument) As String

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

        GrabAuthor = temp

    End Function

    Public Overrides _
    Function GrabBody(ByVal xmldoc As XmlDocument) As String

        Dim XmlList As XmlNodeList
        Dim value As String

        XmlList = GetNodes(xmldoc, "//div[@id='storytext']")

        Try
            value = XmlList(0).InnerXml
        Catch
            value = xmldoc.InnerXml
        End Try

        Return value

    End Function

    Public Overrides _
    Sub GetChapters( _
                     ByVal lst As ListBox, _
                     ByVal xmldoc As XmlDocument _
                   )

        Dim data() As String
        Dim count As Integer

        data = GetOptionValues(xmldoc, "Chapter Navigation")

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
    Function GrabData(ByVal url As String) As System.Xml.XmlDocument

        Dim htmldoc As XmlDocument

        htmldoc = MyBase.GrabData(url)

        StripTags(htmldoc, "menulinks", paramType.Attribute)
        StripTags(htmldoc, "menu-child xxhide", paramType.Attribute)
        StripTags(htmldoc, "xxmenu", paramType.Attribute)
        StripTags(htmldoc, "javascript", paramType.Attribute, partialM.Yes)
        StripTags(htmldoc, "#", paramType.Attribute, partialM.Yes)
        StripTags(htmldoc, "a2a", paramType.Attribute, partialM.Yes)

        Return htmldoc

    End Function

    Public Overrides _
    Function InitialDownload(ByVal url As String) As XmlDocument

        Dim host As String
        Dim xmlDoc As XmlDocument

        host = "http://www.fanfiction.net/s/"
        url = Replace(url, host, "")
        host = host & Mid(url, 1, InStr(url, "/") - 1)
        url = host & "/1/"

        xmlDoc = GrabData(url)

        InitialDownload = xmlDoc

    End Function

    Public Overrides _
    Function ProcessChapters( _
                              ByVal URL As String, _
                              ByVal list As ListBox, _
                              ByVal index As Integer _
                            ) As XmlDocument

        Dim host As String
        Dim temp As String
        Dim params() As String
        host = "http://www.fanfiction.net/s/"

        temp = Replace(URL, host, "")
        params = Split(temp, "/")

        URL = host & params(0) & "/"

        Dim xmldoc As XmlDocument

        xmldoc = GrabData(URL & (index + 1) & "/")

        ProcessChapters = xmldoc

    End Function

    Public Overrides _
    Function WriteDate( _
                        ByVal publish As String, _
                        ByVal update As String, _
                        ByVal index As Integer, _
                        ByVal lstop As Integer _
                      ) As String

        WriteDate = ""

        If index = 1 Then
            WriteDate = "<p>" & publish & "</p>"
        ElseIf index = lstop Then
            WriteDate = "<p>" & update & "</p>"
        End If

    End Function

End Class
