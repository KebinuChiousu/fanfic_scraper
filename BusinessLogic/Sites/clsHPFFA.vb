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

    End Function

    Public Overrides Function GetStoryURL(id As String) As String

    End Function


#End Region

#Region "Download Routines"

    Protected Overrides Function GrabFeedData(ByRef rss As String) As System.Xml.XmlDocument

        Dim html As String
        Dim doc As HtmlDocument = Nothing

        Dim nodes As HtmlNodeCollection
        Dim temp As HtmlNodeCollection

        Dim dummy As String

        Dim title As String
        Dim link As String

        Dim node_idx As Integer

        Dim author_url As String

        Dim summary() As String
        Dim fic() As clsFanfic.Story

        Dim idx As Integer

        Dim xmldoc As XmlDocument = Nothing

        author_url = rss

        html = Me.GrabData(rss)

        doc = CleanHTML(html)

        nodes = FindNodesByAttribute(doc.DocumentNode, "div", "class", "listbox", True)

        ReDim fic(nodes.Count - 1)

        For node_idx = 0 To nodes.Count - 1

            fic(node_idx).ID = nodes(node_idx).Attributes("value").Value
            fic(node_idx).StoryURL = "http://mediaminer.org/fanfic/view_st.php?id=" & fic(node_idx).ID

            title = nodes(node_idx).NextSibling.InnerText
            If InStr(title, "(") > 0 Then
                fic(node_idx).Title = Mid(title, 1, InStr(title, "(") - 1)
            Else
                fic(node_idx).Title = title
            End If

            html = Me.GrabData(fic(node_idx).StoryURL)
            doc = CleanHTML(html)

            temp = doc.DocumentNode.SelectNodes("//table")
            doc = CleanHTML(temp(4).InnerHtml)
            temp = doc.DocumentNode.SelectNodes("//td")
            dummy = temp(1).InnerHtml

            summary = Split(dummy, "<br />")

            doc = CleanHTML(summary(0))
            temp = doc.DocumentNode.SelectNodes("//a")

            fic(node_idx).Category = temp(0).InnerText

            idx = 2
            dummy = summary(idx)
            doc = CleanHTML(dummy)
            temp = doc.DocumentNode.SelectNodes("//a")

            If IsNothing(temp) Then
                idx = 3
                doc = CleanHTML(summary(idx))
                temp = doc.DocumentNode.SelectNodes("//a")
            End If

            fic(node_idx).Author = temp(0).InnerText
            fic(node_idx).AuthorURL = "http://" & Me.HostName & temp(0).Attributes("href").Value

            If idx = 3 Then
                idx = 2
            Else
                idx = 3
            End If

            dummy = Split(summary(idx), "</b>")(1)
            dummy = Trim(Split(dummy, "|")(0))
            If (InStr(dummy, "DT") = Len(dummy) - 1) Or (InStr(dummy, "ST") = Len(dummy) - 1) Then
                dummy = Mid(dummy, 1, Len(dummy) - 3)
            End If

            fic(node_idx).UpdateDate = CDate(dummy).ToShortDateString

            doc = CleanHTML(summary(6))
            dummy = doc.DocumentNode.InnerText
            dummy = HtmlDecode(dummy)
            dummy = HtmlDecode(dummy)
            dummy = Replace(dummy, Chr(160), "")

            fic(node_idx).Summary = dummy

            doc = CleanHTML(html)
            temp = FindNodesByAttribute(doc.DocumentNode, "select", "name", "cid", False)

            If IsNothing(temp) Then
                fic(node_idx).ChapterCount = 1
                fic(node_idx).PublishDate = fic(node_idx).UpdateDate
            Else

                dummy = temp(0).InnerHtml
                doc = CleanHTML(dummy)
                temp = doc.DocumentNode.SelectNodes("//option")

                fic(node_idx).ChapterCount = temp.Count

                link = "http://mediaminer.org/fanfic/view_ch.php?cid="
                link += temp(0).Attributes("value").Value
                link += "&id=" & fic(node_idx).ID

                html = Me.GrabData(link)
                doc = CleanHTML(html)

                temp = doc.DocumentNode.SelectNodes("//table")
                doc = CleanHTML(temp(4).InnerHtml)
                temp = doc.DocumentNode.SelectNodes("//td")
                html = temp(1).InnerHtml

                summary = Split(html, "<br />")

                dummy = Split(summary(3), "</b>")(1)
                dummy = Trim(Split(dummy, "|")(0))
                If (InStr(dummy, "DT") = Len(dummy) - 1) Or (InStr(dummy, "ST") = Len(dummy) - 1) Then
                    dummy = Mid(dummy, 1, Len(dummy) - 3)
                End If

                fic(node_idx).PublishDate = CDate(dummy).ToShortDateString

            End If

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

    End Function

#End Region

#Region "Properties"

    Public Overrides ReadOnly Property HostName() As String
        Get
            Return "hpfanficarchive.com"
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
