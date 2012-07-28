Imports System
Imports System.text
Imports System.net
Imports System.IO
Imports System.Web
Imports System.xml

Class MediaMiner
    Inherits Fanfic

#Region "Data Extraction"

    Public Overrides _
    Function GrabTitle(ByVal page As XmlDocument) As String

        Dim value As String = GetFirstNodeValue(page, "//title")

        GrabTitle = Replace(value, "MediaMiner - Fan Fic: ", "")
        GrabTitle = Replace(GrabTitle, "", "")

    End Function

    Public Overrides _
    Function GrabSeries(ByVal page As XmlDocument) As String

        Dim result As String = ""

        Return result
        
    End Function

    Public Overrides _
    Function GrabAuthor(ByVal htmldoc As XmlDocument) As String

        Dim result As String
        Dim xpath As String

        xpath = "//td[@class='smtxt']"

        htmldoc = ReturnNodes(htmldoc, xpath)
        htmldoc = ReturnNodes(htmldoc, "//a")

        result = GetAttrValue(htmldoc, "a", "href", "/fanfic/src.php/u/")
        result = Replace(result, "/fanfic/src.php/u/", "")

        Return result

    End Function

    Public Overrides _
    Function GrabBody(ByVal xmldoc As XmlDocument) As String

        Dim result As String
        Dim xpath As String

        Dim xml_doc As XmlDocument
        Dim xmllist As XmlNodeList

        xml_doc = xmldoc

        xpath = "//td[@class='m']"
        xml_doc = ReturnNodes(xml_doc, xpath)
        StripTags(xml_doc, "table")
        'xml_doc = StripTags(xml_doc, "div")
        StripTags(xml_doc, "a")

        xmllist = xml_doc.SelectNodes("//td")

        result = xmllist(0).InnerXml


        Return result

    End Function

    Public Overrides _
    Function GrabDate( _
                       ByVal xmldoc As XmlDocument, _
                       ByVal title As String _
                     ) As String

        GrabDate = ""

        Select Case title
            Case "Published: "
                GrabDate = GrabUpload(xmldoc)
            Case "Updated: "
                GrabDate = GrabUpdate(xmldoc)
        End Select

    End Function

#Region "Date Extraction Routines"

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

#End Region

#End Region

    Public Overrides _
    Function ProcessChapters( _
                              ByVal URL As String, _
                              ByVal lst As ListBox, _
                              ByVal index As Integer _
                            ) As XmlDocument

        Dim host As String
        Dim storyid As String
        Dim xmldoc As XmlDocument

        host = "http://www.mediaminer.org/fanfic/view_ch.php?"
        storyid = Replace( _
                           URL, _
                           "http://www.mediaminer.org/fanfic/view_st.php/", _
                           "" _
                         )

        URL = host & "cid=" & _
              lst.Items(index) & _
              "&submit=View+Chapter&id=" & storyid



        xmldoc = MyBase.GrabData(URL)

        ProcessChapters = xmldoc

    End Function

    Public Overrides _
    Function WriteDate( _
                        ByVal publish As String, _
                        ByVal update As String, _
                        ByVal index As Integer, _
                        ByVal lstop As Integer _
                      ) As String

        WriteDate = "<p>" & publish & "</p>"
        WriteDate &= "<p>" & update & "</p>"

    End Function

End Class
