Imports HtmlAgilityPack
Imports System.Xml
Imports System.Data
Imports System.Web.HttpUtility

Public MustInherit Class clsFanfic

    Protected datasetRSS As DataSet
    Protected StoryURL As String

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
    Function GetChapters( _
                          ByVal htmlDoc As String _
                        ) As String()

        Dim data() As String

        data = GetOptionValues(htmlDoc)

        If IsNothing(data) Then
            ReDim data(0)
            data(0) = "Chapter 1"
        End If

        Me.Chapters = data

        Return data

    End Function

    Protected _
    Function GetOptionValues( _
                              ByVal htmlDoc As String, _
                              Optional ByVal param As String = "" _
                            ) As String()

        Dim doc As HtmlDocument

        doc = CleanHTML(htmlDoc)

        Dim result As String = ""
        Dim values() As String
        Dim idx As Integer = 0

        ReDim values(0)

        Dim temp As HtmlNodeCollection

        If param = "" Then
            temp = doc.DocumentNode.SelectNodes("//select")
        Else
            temp = doc.DocumentNode.SelectNodes("//select[@title='" & param & "']")
        End If


        If temp.Count = 0 Then
            Return Nothing
        End If

        result = temp(0).InnerHtml

        result = "<select>" & result & "</select>"

        doc = CleanHTML(result)

        Dim count As Integer
        Dim node As HtmlNode

        temp = doc.DocumentNode.SelectNodes("//option")

        With temp

            For count = 0 To (.Count - 1)
                node = .Item(count)

                If node.NextSibling.InnerText <> "" Then
                    ReDim Preserve values(idx)
                    values(idx) = node.NextSibling.InnerText
                    idx = idx + 1
                End If
            Next

        End With

        doc = Nothing

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

        Dim Browser As New clsWeb
        Dim html As String

        Me.StoryURL = url

        html = Browser.DownloadPage(url)

        CleanHTML(html)

        Return html

        Browser = Nothing

    End Function

    Protected MustOverride _
    Function GrabFeedData(ByRef rss As String) As System.Xml.XmlDocument

    Public _
    Function GrabFeed(ByRef rss As String) As System.Data.DataSet

        Dim xmldoc As XmlDocument

        datasetRSS = Nothing
        datasetRSS = New DataSet

        xmldoc = GrabFeedData(rss)

        If Not IsNothing(xmldoc) Then
            ' Read in XML from file
            datasetRSS.ReadXml(StringToStream(xmldoc.OuterXml))
        Else
            datasetRSS = Nothing
        End If

        Return datasetRSS

    End Function
#End Region

#Region "RSS Routines"

    Public MustOverride _
    Function GrabStoryInfo(ByVal idx As Integer) As clsFanfic.Story

    Structure Story
        Dim ID As String
        Dim Title As String
        Dim Author As String
        Dim AuthorURL As String
        Dim StoryURL As String
        Dim Category As String
        Dim Chapters() As String
        Dim ChapterCount As String
        Dim PublishDate As String
        Dim UpdateDate As String
        Dim Summary As String
    End Structure

    Public _
    Function GetStoryInfoByID(ByVal StoryID As String) As clsFanfic.Story

        Dim fic As clsFanfic.Story = Nothing

        Dim idx As Integer

        For idx = 0 To datasetRSS.Tables(0).Rows.Count - 1

            fic = GrabStoryInfo(idx)

            If InStr(fic.ID, StoryID) <> 0 Then
                Exit For
            End If

        Next

        Return fic

    End Function

    Protected Chapters() As String

    Protected Function GenerateAtomFeed(ByVal fic As clsFanfic.Story()) As String

        Dim html As String = ""
        Dim node_idx As Integer

        html = "<feed>"

        For node_idx = 0 To UBound(fic)

            html += "<entry>"
            html += "<author>"
            html += "<name>"
            html += fic(node_idx).Author
            html += "</name>"
            html += "<uri>"
            html += fic(node_idx).AuthorURL
            html += "</uri>"
            html += "</author>"
            html += "<published>"
            html += fic(node_idx).PublishDate
            html += "</published>"
            html += "<updated>"
            html += fic(node_idx).UpdateDate
            html += "</updated>"
            html += "<title>"
            html += fic(node_idx).Title
            html += "</title>"
            html += "<link rel=""alternate"" href=""" & fic(node_idx).StoryURL & """ />"
            html += "<id>"
            html += fic(node_idx).ID
            html += ":"
            html += fic(node_idx).Category
            html += ":"
            html += fic(node_idx).ChapterCount
            html += "</id>"
            html += "<summary type=""html"">"
            html += HtmlEncode(fic(node_idx).Summary)
            html += "</summary>"
            html += "</entry>"

        Next

        html += "</feed>"

        Return html

    End Function

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

    Public ReadOnly Property dsRSS As DataSet
        Get
            Return datasetRSS
        End Get
    End Property

#End Region

End Class
