Imports System
Imports System.text
Imports System.net
Imports System.IO
Imports System.Xml
Imports HtmlAgilityPack
Imports System.Web.HttpUtility

Public Class MM
    Inherits clsFanfic

    Private Browser As clsWeb

#Region "Downloading HTML"

    Public Overrides Function GrabData(ByVal url As String) As String

        Dim htmldoc As String

        htmldoc = MyBase.GrabData(url)

        Return htmldoc

    End Function

#End Region

#Region "RSS"

    Public Overrides Function GrabFeed(ByRef rss As String) As System.Xml.XmlDocument

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

        temp = FindNodesByAttribute(doc.DocumentNode, "select", "name", "id", False)
        html = temp(0).InnerHtml

        doc = CleanHTML(html)

        nodes = doc.DocumentNode.SelectNodes("//option")

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

    Public Overrides Function GrabStoryInfo(ByRef dsRSS As System.Data.DataSet, ByVal idx As Integer) As clsFanfic.Story

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

#Region "Chapter Navigation"

    Public Overrides Function GetChapters(htmlDoc As String) As String()

        Dim ret() As String

        Dim doc As HtmlDocument
        Dim temp As HtmlNodeCollection

        Dim idx As Integer

        'Clean up html into xhtml
        doc = CleanHTML(htmlDoc)

        temp = FindNodesByAttribute(doc.DocumentNode, "select", "name", "cid", False)

        If Not IsNothing(temp) Then

            htmlDoc = temp(0).InnerHtml
            doc = CleanHTML(htmlDoc)
            temp = doc.DocumentNode.SelectNodes("//option")

            ReDim ret(temp.Count - 1)
            For idx = 0 To temp.Count - 1
                ret(idx) = temp(idx).Attributes("value").Value
            Next
        Else
            ReDim ret(0)
            ret(0) = "Chapter 1"
        End If

        Me.Chapters = ret

        Return ret

    End Function

    Public Overrides Function ProcessChapters(ByVal link As String, ByVal index As Integer) As String

        Dim htmldoc As String

        If UBound(Me.Chapters) > 1 Then
            link = GetChapterURL(link, Me.Chapters(index))
        End If

        htmldoc = GrabData(link)

        Return htmldoc

    End Function

    Private Function GetChapterURL(link As String, index As Integer) As String

        Dim ret As String

        ret = "http://mediaminer.org/fanfic/view_ch.php?cid="
        ret += CStr(index)
        ret += "&"
        ret += "id="
        ret += GetStoryID(link)

        Return ret

    End Function

    Public Overrides Function GetAuthorURL(ByVal link As String) As String

        Dim ret As String

        Dim temp() As String

        If InStr(link, "/src.php/u") > 0 Then

            temp = Split(link, "/")

            link += "http://mediaminer.org/user_info.php/" & temp(UBound(temp))

            ret = link
        Else
            If InStr(link, "user_info.php/") > 0 Then
                ret = link
            Else
                Return ""
            End If
        End If

        Return ret

    End Function

    Public Overrides Function GetStoryID(ByVal link As String) As String

        Dim ret As String = ""
        Dim url As URL

        url = ExtractUrl(link)
        ret = url.Query(0).Value

        Return ret

    End Function

    Public Overrides Function GetStoryURL(ByVal id As String) As String

        Dim link As String

        link = "http://mediaminer.org/fanfic/view_st.php?id=" & id

        Return link

    End Function

#End Region

#Region "HTML Processing"

    Private Function GrabHeaderInfo(ByVal htmldoc As String) As String()

        Dim doc As HtmlDocument = Nothing
        Dim temp As HtmlNodeCollection

        Dim dummy As String
        Dim summary() As String

        doc = CleanHTML(htmldoc)

        temp = doc.DocumentNode.SelectNodes("//table")
        doc = CleanHTML(temp(4).InnerHtml)
        temp = doc.DocumentNode.SelectNodes("//td")
        dummy = temp(1).InnerHtml

        summary = Split(dummy, "<br />")

        doc = Nothing
        temp = Nothing

        Return summary

    End Function

    Public Overrides Function GrabAuthor(ByVal htmlDoc As String) As String

        Dim author As String

        Dim idx As Integer

        Dim doc As HtmlDocument = Nothing
        Dim temp As HtmlNodeCollection

        Dim dummy As String
        Dim summary() As String

        summary = GrabHeaderInfo(htmlDoc)

        idx = 2
        dummy = summary(idx)
        doc = CleanHTML(dummy)
        temp = doc.DocumentNode.SelectNodes("//a")

        If IsNothing(temp) Then
            idx = 3
            doc = CleanHTML(summary(idx))
            temp = doc.DocumentNode.SelectNodes("//a")
        End If

        author = temp(0).InnerText

        doc = Nothing
        temp = Nothing

        Return author

    End Function

    Public Overrides Function GrabBody(ByVal htmldoc As String) As String

        Dim idx As Integer

        Dim ret As String

        Dim content As String

        Dim doc As HtmlDocument = Nothing
        Dim temp As HtmlNodeCollection

        doc = CleanHTML(htmldoc)

        '<div align="left" style=" padding: 0.00mm 0.00mm 0.00mm 0.00mm;">

        temp = FindNodesByAttribute(doc.DocumentNode, "div", "style", "padding: 0.00mm 0.00mm 0.00mm 0.00mm;", True)

        ret = "<html>"
        ret += "<body>"

        For idx = 0 To temp.Count - 1

            content = HtmlDecode(HtmlDecode(temp(idx).OuterHtml))
            content = Replace(content, "<br />", "<br /><br />")

            ret += content
            ret += "<br /><br />"


        Next

        ret += "</body>"
        ret += "</html>"

        doc = Nothing
        temp = Nothing

        Return ret

    End Function

    Public Overrides Function GrabDate(ByVal htmlDoc As String, ByVal title As String) As String

        Dim ret As String

        Dim idx As Integer

        Dim doc As HtmlDocument = Nothing
        Dim temp As HtmlNodeCollection

        Dim dummy As String
        Dim summary() As String

        summary = GrabHeaderInfo(htmlDoc)

        idx = 2
        dummy = summary(idx)
        doc = CleanHTML(dummy)
        temp = doc.DocumentNode.SelectNodes("//a")

        If IsNothing(temp) Then
            idx = 3
        End If

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

        ret = CDate(dummy).ToShortDateString

        doc = Nothing
        temp = Nothing

        Return ret

    End Function

    Public Overrides Function GrabSeries(ByVal htmlDoc As String) As String

        Dim series As String

        Dim doc As HtmlDocument = Nothing
        Dim temp As HtmlNodeCollection

        Dim summary() As String

        summary = GrabHeaderInfo(htmlDoc)

        doc = CleanHTML(summary(0))
        temp = doc.DocumentNode.SelectNodes("//a")

        series = temp(0).InnerText

        doc = Nothing
        temp = Nothing

        Return series

    End Function

    Public Overrides Function GrabTitle(ByVal htmlDoc As String) As String

        Dim ret As String
        Dim pos As Integer

        Dim doc As HtmlDocument = Nothing
        Dim temp As HtmlNodeCollection

        doc = CleanHTML(htmlDoc)
        temp = FindNodesByAttribute(doc.DocumentNode, "td", "class", "ffh", False)

        ret = temp(0).InnerText

        pos = InStr(ret, ":")

        If pos > 1 Then
            ret = Mid(ret, 1, pos - 1)
        End If

        doc = Nothing
        temp = Nothing

        Return ret

    End Function

#End Region

#Region "Properties"

    Public Overrides ReadOnly Property ErrorMessage() As String
        Get

            Dim ret As String

            ret = "<h2 style=""margin: 5px;"">Jump to Anime Series:</h2>"

            Return ret

        End Get
    End Property

    Public Overrides ReadOnly Property HostName() As String
        Get
            Return "mediaminer.org"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "MM"
        End Get
    End Property

#End Region

    Public Sub New()
        Browser = New clsWeb
    End Sub
End Class
