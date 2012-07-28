Imports System.Xml

Public MustInherit Class Fanfic

    Public MustOverride _
    Function ProcessChapters( _
                              ByVal URL As String, _
                              ByVal list As ListBox, _
                              ByVal index As Integer _
                            ) As XmlDocument

    Public MustOverride _
    Function WriteDate( _
                        ByVal publish As String, _
                        ByVal update As String, _
                        ByVal index As Integer, _
                        ByVal lstop As Integer _
                      ) As String

#Region "Data Extraction"

    Public MustOverride _
    Function GrabTitle( _
                        ByVal page As XmlDocument _
                      ) As String

    Public MustOverride _
    Function GrabSeries(ByVal page As XmlDocument) As String

    Public MustOverride _
    Function GrabDate( _
                       ByVal xmldoc As XmlDocument, _
                       ByVal title As String _
                     ) As String

    Public MustOverride _
    Function GrabAuthor(ByVal xmldoc As XmlDocument) As String

    Public MustOverride _
    Function GrabBody(ByVal xmldoc As XmlDocument) As String

#Region "Chapter Extraction"

    Public Overridable _
    Sub GetChapters( _
                     ByVal lst As ListBox, _
                     ByVal xmlDoc As XmlDocument _
                   )

        Dim data() As String
        Dim count As Integer

        data = GetOptionValues(xmlDoc)

        If Not IsNothing(data) Then
            For count = 0 To UBound(data)
                lst.Items.Add(data(count))
            Next
        Else
            lst.Items.Add("Chapter 1")
        End If

    End Sub

    Protected _
    Function GetOptionValues( _
                              ByVal xmldoc As XmlDocument, _
                              Optional ByVal param As String = "" _
                            ) As String()

        Dim result As String = ""
        Dim values() As String
        Dim XmlList As XmlNodeList

        If param = "" Then
            XmlList = GetNodes(xmldoc, "//select")
        Else
            XmlList = GetNodes(xmldoc, "//select[@title='" & param & "']")
        End If


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

#End Region

#End Region

#Region "Download Routines"

    Public Overridable _
    Function InitialDownload(ByVal url As String) As XmlDocument

        Dim htmldoc As XmlDocument

        htmldoc = GrabData(url)

        Return htmldoc

    End Function

    Public Overridable Function GrabData(ByVal url As String) As XmlDocument

        Dim htmldoc As XmlDocument
        Dim html As String

        html = DownloadPage(url)
        htmldoc = CleanHTML(html)

        Return htmldoc

    End Function

#End Region

End Class
