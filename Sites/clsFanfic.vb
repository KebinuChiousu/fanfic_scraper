Imports System.Xml
Imports System.Data

Public MustInherit Class Fanfic

    Public MustOverride _
    Function ProcessChapters( _
                              ByVal link As String, _
                              ByVal index As Integer _
                            ) As String

    Public Overridable _
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

#Region "Data Extraction"

    Public MustOverride _
    Function GrabTitle( _
                        ByVal htmlDoc As String _
                      ) As String

    Public MustOverride _
    Function GrabSeries(ByVal htmlDoc As String) As String

    Public MustOverride _
    Function GrabDate( _
                       ByVal htmlDoc As String, _
                       ByVal title As String _
                     ) As String

    Public MustOverride _
    Function GrabAuthor(ByVal htmlDoc As String) As String

    Public MustOverride _
    Function GrabBody(ByVal htmldoc As String) As String

    Public MustOverride _
    Function GetAuthorURL(ByVal link As String) As String

    Public MustOverride _
    Function GetStoryID(ByVal link As String) As String

    Public MustOverride _
    Function GetStoryURL(ByVal id As String) As String


#Region "Chapter Extraction"

    Public Overridable _
    Sub GetChapters( _
                     ByVal lst As ListBox, _
                     ByVal htmlDoc As String _
                   )

        Dim data() As String
        Dim count As Integer

        data = GetOptionValues(htmlDoc)

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
                              ByVal htmlDoc As String, _
                              Optional ByVal param As String = "" _
                            ) As String()

        Dim xmldoc As New XmlDocument

        xmldoc.LoadXml(htmlDoc)

        Dim result As String = ""
        Dim values() As String
        Dim idx As Integer = 0

        ReDim values(0)

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


            For count = 0 To (.Count - 1)
                node = .Item(count)

                If node.InnerText <> "" Then
                    ReDim Preserve values(idx)
                    values(idx) = node.InnerText
                    idx = idx + 1
                End If
            Next

        End With

        xmldoc = Nothing

        Return values

    End Function

#End Region

#End Region

#Region "Download Routines"

    Public Overridable _
    Function InitialDownload(ByVal url As String) As String

        Dim htmldoc As String

        htmldoc = GrabData(url)

        Return htmldoc

    End Function

    Public Overridable Function GrabData(ByVal url As String) As String

        Dim html As String

        html = DownloadPage(url)

        CleanHTML(html)

        Return html

    End Function

    Public MustOverride _
    Function GrabFeed(ByRef rss As String) As XmlDocument

#End Region

#Region "RSS Routines"

    Public MustOverride _
    Function GrabStoryInfo(ByRef dsRSS As DataSet, ByVal idx As Integer) As Fanfic.Story

    Structure Story
        Dim ID As String
        Dim Title As String
        Dim Author As String
        Dim URL As String
        Dim Category As String
        Dim ChapterCount As String
        Dim PublishDate As String
        Dim UpdateDate As String
        Dim Summary As String
    End Structure

#End Region

#Region "Properties"

    Public MustOverride _
    ReadOnly Property ErrorMessage() As String

    Public Overridable _
    ReadOnly Property HostName() As String
        Get
            Return ""
        End Get
    End Property

    Public MustOverride _
    ReadOnly Property Name() As String

#End Region

End Class
