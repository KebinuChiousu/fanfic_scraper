Imports System
Imports System.text
Imports System.net
Imports System.IO
Imports System.Xml

Class FFNet

#Region "Retrieval Functions"

    Public Function GrabTitle(ByVal page As XmlDocument) As String

        Dim value As String = GetFirstNodeValue(page, "//title")

        GrabTitle = Replace(value, " - FanFiction.Net", "")
        GrabTitle = Replace(GrabTitle, "", "")

    End Function

    Public Function GrabDate(ByVal xmldoc As XmlDocument, ByVal title As String) As String

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

    Public Function GrabAuthor(ByVal xmldoc As XmlDocument) As String

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

    Public Function GrabBody(ByVal xmldoc As XmlDocument) As String

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

    Function GetOptionValues( _
                                 ByVal xmldoc As XmlDocument, _
                                 ByVal param As String _
                               ) As String()

        Dim result As String = ""

        Dim values() As String

        Dim XmlList As XmlNodeList

        XmlList = GetNodes(xmldoc, "//select[@title='" & param & "']")

        If XmlList.Count = 0 Then
            Return Nothing
        End If

        result = XmlList(0).InnerXml

        Dim xml_doc As New XmlDocument

        result = "<select>" & result & "</select>"

        xml_doc.LoadXml(result)

        Dim count As Integer
        Dim node As XmlNode

        With xml_doc.DocumentElement.ChildNodes

            ReDim values(.Count - 1)

            For count = 0 To (.Count - 1)
                node = .Item(count)
                values(count) = node.InnerText
            Next

        End With

        Return values

    End Function

    Public Sub GetChapters(ByVal lst As ListBox, ByVal page As XmlDocument)

        Dim data() As String
        Dim count As Integer

        data = GetOptionValues(page, "chapter navigation")

        If Not IsNothing(data) Then
            For count = 0 To UBound(data)
                lst.Items.Add(data(count))
            Next
        Else
            lst.Items.Add("Chapter 1")
        End If

    End Sub


#End Region

    Function GetXML(ByVal html As String) As XmlDocument

        Dim htmlDoc As New XmlDocument

        htmlDoc = CleanHTML(html)

        htmlDoc = StripTags(htmlDoc, "menulinks", paramType.Attribute)
        htmlDoc = StripTags(htmlDoc, "menu-child xxhide", paramType.Attribute)
        htmlDoc = StripTags(htmlDoc, "xxmenu", paramType.Attribute)
        htmlDoc = StripTags(htmlDoc, "javascript", paramType.Attribute, partialM.Yes)
        htmlDoc = StripTags(htmlDoc, "#", paramType.Attribute, partialM.Yes)

        Return htmlDoc

    End Function
   
    Public Function InitialDownload(ByRef url As String) As XmlDocument
        Dim host As String = "http://www.fanfiction.net/s/"
        Dim txtResult As String

        txtResult = Replace(url, host, "")
        host = host & Mid(txtResult, 1, InStr(txtResult, "/") - 1)

        url = host & "/"
        txtResult = DownloadPage(url & "1/")

        Dim xmlDoc As XmlDocument

        xmlDoc = GetXML(txtResult)

        InitialDownload = xmlDoc

    End Function

    Public Function ProcessChapters( _
                                     ByVal URL As String, _
                                     ByVal list As ListBox, _
                                     ByVal index As Integer _
                                   ) As XmlDocument

        Dim xmldoc As XmlDocument

        Dim txtResult As String

retry:
        txtResult = DownloadPage(URL & (index + 1) & "/")
        If txtResult = "" Then GoTo retry

        xmldoc = GetXML(txtResult)

        ProcessChapters = xmldoc

    End Function

    Public Function WriteDate( _
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
