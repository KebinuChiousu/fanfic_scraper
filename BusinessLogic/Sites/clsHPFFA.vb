Imports System
Imports System.Text
Imports System.Net
Imports System.Xml
Imports System.Data
Imports HtmlAgilityPack
Imports System.Web.HttpUtility

Public Class HPFFA
    Inherits clsFanfic

    Private Browser As clsWeb

#Region "Chapter Navigation"

    Public Overrides Function ProcessChapters(link As String, index As Integer) As String

    End Function

#End Region

#Region "Data Extraction"

    Public Overrides Function GrabTitle(htmlDoc As String) As String

    End Function

    Public Overrides Function GrabSeries(htmlDoc As String) As String

    End Function

    Public Overrides Function GrabDate(htmlDoc As String, title As String) As String

    End Function

    Public Overrides Function GrabAuthor(htmlDoc As String) As String

    End Function

    Public Overrides Function GrabBody(htmlDoc As String) As String

    End Function

    Public Overrides Function GetAuthorURL(link As String) As String

    End Function

    Public Overrides Function GetStoryID(link As String) As String

        Dim ret As String = ""
        Dim url As URL

        url = ExtractUrl(link)
        ret = url.Query(0).Value

        Return ret

    End Function

    Public Overrides Function GetStoryURL(id As String) As String

        Return "http://" & Me.HostName & "/stories/viewstory.php?sid=" & id

    End Function


#End Region

#Region "Download Routines"

    Protected Overrides Function GrabFeedData(ByRef rss As String) As System.Xml.XmlDocument

        Dim html As String
        Dim doc As HtmlDocument = Nothing
        Dim tdoc As HtmlDocument = Nothing

        Dim nodes As HtmlNodeCollection
        Dim temp As HtmlNodeCollection

        Dim dummy As String

        Dim temp_idx As Integer
        Dim node_idx As Integer

        Dim author_url As String

        Dim summary() As String

        Dim ch_idx As Integer

        Dim fic() As clsFanfic.Story

        Dim idx As Integer

        Dim xmldoc As XmlDocument = Nothing

        author_url = rss

        html = Me.GrabData(rss)

        doc = CleanHTML(html)

        nodes = FindNodesByAttribute(doc.DocumentNode, "div", "class", "listbox", True)

        ReDim fic(nodes.Count - 1)

        For node_idx = 0 To nodes.Count - 1

            doc = CleanHTML(nodes(node_idx).InnerHtml)

            temp = FindNodesByAttribute(doc.DocumentNode, "div", "class", "title", False)
            tdoc = CleanHTML(temp(0).InnerHtml)
            temp = tdoc.DocumentNode.SelectNodes("//a")

            fic(node_idx).Category = "Harry Potter"

            fic(node_idx).StoryURL = "http://" & Me.HostName & "/stories/" & temp(0).Attributes("href").Value
            fic(node_idx).AuthorURL = "http://" & Me.HostName & "/stories/" & temp(1).Attributes("href").Value

            fic(node_idx).Title = temp(0).InnerText
            fic(node_idx).Author = temp(1).InnerText


            temp = FindNodesByAttribute(doc.DocumentNode, "div", "class", "tail", False)

            dummy = temp(0).InnerHtml

            tdoc = CleanHTML(dummy)

            temp = FindNodesByAttribute(doc.DocumentNode, "span", "class", "label", False)

            For temp_idx = 0 To temp.Count - 1

                dummy = Replace(dummy, temp(temp_idx).OuterHtml, "|" & temp(temp_idx).InnerText)

            Next

            dummy = Replace(dummy, "</span>", "")
            summary = Split(dummy, "|")

            fic(node_idx).PublishDate = CDate(Mid(summary(1), InStr(summary(1), ":") + 1)).ToShortDateString
            fic(node_idx).UpdateDate = CDate(Mid(summary(2), InStr(summary(2), ":") + 1)).ToShortDateString

            temp = FindNodesByAttribute(doc.DocumentNode, "div", "class", "content", False)

            html = temp(0).InnerHtml

            doc = CleanHTML(html)

            temp = FindNodesByAttribute(doc.DocumentNode, "span", "class", "label", False)

            For temp_idx = 0 To temp.Count - 1

                html = Replace(html, temp(temp_idx).OuterHtml, "|" & temp(temp_idx).InnerText)

            Next

            summary = Split(html, "|")

            tdoc = CleanHTML(summary(1))

            dummy = tdoc.DocumentNode.InnerText & vbCrLf

            For idx = 0 To UBound(summary)
                If InStr(summary(idx), "Chapters:") <> 0 Then
                    ch_idx = idx
                    Exit For
                End If
            Next

            For idx = 2 To (ch_idx - 2)

                If idx <> 4 Then

                    tdoc = CleanHTML(summary(idx))
                    dummy += HtmlDecode(tdoc.DocumentNode.InnerText) & vbCrLf

                End If

            Next

            fic(node_idx).Summary = dummy

            tdoc = CleanHTML(summary(ch_idx))

            dummy = tdoc.DocumentNode.InnerText
            dummy = Left(dummy, InStr(dummy, "Table") - 1)
            dummy = Mid(dummy, InStr(dummy, ":") + 1)

            fic(node_idx).ChapterCount = Trim(dummy)

            fic(node_idx).ID = GetStoryID(fic(node_idx).StoryURL)

        Next node_idx

        html = GenerateAtomFeed(fic)

        doc = CleanHTML(html)

        html = doc.DocumentNode.OuterHtml

        xmldoc = New XmlDocument

        xmldoc.LoadXml(html)

        Return xmldoc

    End Function

#End Region


#Region "RSS Routines"

    Public Overrides Function GrabStoryInfo(idx As Integer) As clsFanfic.Story

        Dim dsRSS As DataSet = MyBase.datasetRSS

        Dim fic As New clsFanfic.Story

        Dim temp() As String

        ' Story Name
        fic.Title = dsRSS.Tables("entry"). _
                        Rows(idx).Item("title")

        ' Story Author
        fic.Author = dsRSS.Tables("author"). _
                          Rows(idx).Item(0)
        ' Story Location
        fic.StoryURL = dsRSS.Tables("link"). _
                          Rows(idx).Item(1)

        fic.ID = Me.GetStoryID(fic.StoryURL)

        fic.PublishDate = CDate(dsRSS.Tables("entry").Rows(idx).Item("published"))

        fic.UpdateDate = CDate(dsRSS.Tables("entry").Rows(idx).Item("updated"))

        temp = Split(dsRSS.Tables("entry").Rows(idx).Item("id"), ":")

        fic.Category = temp(1)
        fic.ChapterCount = temp(2)

        fic.Summary = dsRSS.Tables("summary").Rows(idx).Item("summary_Text")

        Return fic

    End Function

#End Region

#Region "Properties"

    Public Overrides ReadOnly Property HostName() As String
        Get
            Return "www.hpfanficarchive.com"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "HPFFA"
        End Get
    End Property

    Public Overrides ReadOnly Property ErrorMessage As String
        Get
            Return "You are not authorized to access that function."
        End Get
    End Property

#End Region

    Public Sub New()

        Browser = New clsWeb

    End Sub
End Class
