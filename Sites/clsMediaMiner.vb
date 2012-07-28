Imports System
Imports System.text
Imports System.net
Imports System.IO
Imports System.Web
Imports System.xml

Class MediaMiner

    Public Function GrabTitle(ByVal page As XmlDocument) As String

        Dim value As String = GetFirstNodeValue(page, "//title")

        GrabTitle = Replace(value, "MediaMiner - Fan Fic: ", "")
        GrabTitle = Replace(GrabTitle, "", "")

    End Function

    Public Function ParseOptionBox(ByVal page As String) As String

        Dim sstart As Integer
        Dim sstop As Integer

        sstart = InStr(LCase(page), "<select")

        If sstart <> 0 Then

            page = Mid(page, sstart)
            sstart = InStr(LCase(page), ">") + 1
            page = Mid(page, sstart)
            sstop = InStr(LCase(page), "</select>")
            page = Mid(page, 1, sstop)

        Else
            frmMain.ListChapters.Items.Add(frmMain.txtUrl.Text)
            page = ""
        End If

        ParseOptionBox = page

    End Function

    Private Function GrabUpload(ByVal xmldoc As XmlDocument) As String

        Dim count As Integer
        Dim temp As String = ""

        Dim values() As String

        Dim xml_doc As XmlDocument

        Dim title As String = "<b>Uploaded On: </b>"

        Dim xmllist As XmlNodeList

        xml_doc = ReturnNodes(xmldoc, "//table[@class='hdr']")
        xml_doc = ReturnNodes(xml_doc, "//table[@align='center']")
        xml_doc = ReturnNodes(xml_doc, "//tr")

        xmllist = xml_doc.SelectNodes("//td")

        For count = 0 To xmllist.Count - 1

            temp = xmllist(count).InnerXml
            If InStr(temp, title) <> 0 Then
                temp = Mid(temp, InStr(temp, title))
                values = Split(temp, "<br />")
                values = Split(values(0), "|")
                temp = values(0)
                temp = Replace(values(0), title, "")

                Try
                    temp = Format(CDate(temp), "MM/dd/yyyy")
                Catch
                    temp = Mid(temp, 1, Len(temp) - 4)
                End Try


                temp = Format(CDate(temp), "MM/dd/yyyy")
                Exit For
            End If

        Next

        Return temp

    End Function

    Private Function GrabUpdate(ByVal xmldoc As XmlDocument) As String

        Dim count As Integer
        Dim temp As String = ""

        Dim values() As String

        Dim xml_doc As XmlDocument

        Dim title As String = "<b>Updated On: </b>"

        Dim xmllist As XmlNodeList

        xml_doc = ReturnNodes(xmldoc, "//table[@class='hdr']")
        xml_doc = ReturnNodes(xml_doc, "//table[@align='center']")
        xml_doc = ReturnNodes(xml_doc, "//tr")

        xmllist = xml_doc.SelectNodes("//td")

        If Not IsNothing(xmllist) Then
            For count = 0 To xmllist.Count - 1

                temp = xmllist(count).InnerXml
                If InStr(temp, title) <> 0 Then
                    temp = Mid(temp, InStr(temp, title))
                    values = Split(temp, "<br />")
                    temp = values(0)
                    temp = Replace(temp, title, "")

                    Try
                        temp = Format(CDate(temp), "MM/dd/yyyy")
                        Exit For
                    Catch
                        temp = Mid(temp, 1, Len(temp) - 4)
                    End Try

                    temp = Format(CDate(temp), "MM/dd/yyyy")
                    Exit For
                End If

            Next
        End If

        Return temp


    End Function

    Public Function GrabDate(ByVal xmldoc As XmlDocument, ByVal title As String) As String

        GrabDate = ""

        Select Case title
            Case "Published: "
                GrabDate = GrabUpload(xmldoc)
            Case "Updated: "
                GrabDate = GrabUpdate(xmldoc)
        End Select

    End Function

    Public Function GrabAuthor(ByVal htmldoc As XmlDocument) As String

        Dim result As String
        Dim xpath As String

        xpath = "//td[@class='smtxt']"

        htmldoc = ReturnNodes(htmldoc, xpath)
        htmldoc = ReturnNodes(htmldoc, "//a")

        result = GetAttrValue(htmldoc, "a", "href", "/fanfic/src.php/u/")
        result = Replace(result, "/fanfic/src.php/u/", "")

        Return result

    End Function

    Public Function GrabBody(ByVal xmldoc As XmlDocument) As String

        Dim result As String
        Dim xpath As String

        Dim xml_doc As XmlDocument
        Dim xmllist As XmlNodeList

        xml_doc = xmldoc

        xpath = "//td[@class='m']"
        xml_doc = ReturnNodes(xml_doc, xpath)
        xml_doc = StripTags(xml_doc, "table")
        'xml_doc = StripTags(xml_doc, "div")
        xml_doc = StripTags(xml_doc, "a")

        xmllist = xml_doc.SelectNodes("//td")

        result = xmllist(0).InnerXml


        Return result

    End Function

    Public Sub GetChapters(ByVal lst As ListBox, ByVal xmlDoc As XmlDocument)

        Dim temp As String

        Dim count As Integer
        Dim max As Integer

        Dim xmlList As XmlNodeList

        xmlList = GetNodes(xmlDoc, "//select")

        If xmlList.Count > 0 Then
            xmlList = xmlList(0).ChildNodes
        End If

        max = xmlList.Count - 1

        If Not IsNothing(xmlList) Then
            If xmlList.Count <> 0 Then
                For count = 0 To max
                    temp = xmlList(count).Attributes("value").InnerText
                    lst.Items.Add(temp)
                Next
            Else
                lst.Items.Add("Chapter 1")
            End If
        Else
            lst.Items.Add("Chapter 1")
        End If

    End Sub

    Function GetXML(ByVal html As String) As XmlDocument

        Dim htmlDoc As New XmlDocument

        htmlDoc = CleanHTML(html)

        'htmlDoc = StripTags(htmlDoc, "menulinks", paramType.Attribute)
        'htmlDoc = StripTags(htmlDoc, "menu-child xxhide", paramType.Attribute)
        'htmlDoc = StripTags(htmlDoc, "xxmenu", paramType.Attribute)
        'htmlDoc = StripTags(htmlDoc, "javascript", paramType.Attribute, partialM.Yes)
        'htmlDoc = StripTags(htmlDoc, "#", paramType.Attribute, partialM.Yes)

        Return htmlDoc

    End Function
    Public Function InitialDownload(ByVal url As String) As XmlDocument

        Dim htmldoc As XmlDocument
        Dim html As String

        html = DownloadPage(url)
        htmldoc = GetXML(html)

        Return htmldoc

    End Function

    Public Function ProcessChapters( _
                                     ByVal URL As String, _
                                     ByVal lst As ListBox, _
                                     ByVal index As Integer _
                                   ) As XmlDocument

        Dim host As String = "http://www.mediaminer.org/fanfic/view_ch.php?"

        Dim storyid As String

        storyid = Replace(URL, "http://www.mediaminer.org/fanfic/view_st.php/", "")

        URL = host & "cid=" & lst.Items(index) & _
              "&submit=View+Chapter&id=" & storyid

        Dim xmldoc As XmlDocument

        Dim txtResult As String

retry:
        txtResult = DownloadPage(URL)
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

        WriteDate = "<p>" & publish & "</p>"
        WriteDate &= "<p>" & update & "</p>"

    End Function

End Class
