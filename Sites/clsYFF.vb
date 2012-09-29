Imports System
Imports System.Text
Imports System.Net
Imports System.Xml
Imports HtmlAgilityPack
Imports System.Web.HttpUtility


Class YFF
    Inherits Fanfic

#Region "Downloading HTML"

    Private Browser As New clsWeb

    Public Overrides Function GrabData(ByVal url As String) As String

        Dim html As String
        Dim doc As HtmlDocument

        Dim check As String = "<div style='text-align: center; margin: 1em;' class='errortext'>Age Consent Required"

        Dim temp As HtmlNodeCollection


        'html = DownloadPage(url, "yourfanfiction_com.cookie")
        html = Browser.DownloadPage(url)
        doc = CleanHTML(html)
        html = doc.DocumentNode.OuterHtml

        If InStr(html, check) > 0 Then
            temp = FindLinksByHref(doc.DocumentNode, "viewstory.php")
            url = "http://www." & Me.HostName & "/" & HtmlDecode(temp(0).Attributes("href").Value)
            html = Browser.DownloadPage(url)
            doc = CleanHTML(html)
            html = doc.DocumentNode.OuterHtml
        End If

        doc = Nothing

        Return html

    End Function

#End Region

#Region "RSS"

    Public Overrides Function GrabFeed(ByRef rss As String) As System.Xml.XmlDocument

        Dim html As String
        Dim doc As HtmlDocument = Nothing

        Dim nodes As HtmlNodeCollection
        Dim temp As HtmlNodeCollection

        Dim link As String

        Dim node_idx As Integer

        Dim author_url As String

        Dim idx As Integer

        Dim summary() As String
        Dim fic() As Fanfic.Story

        Dim xmldoc As XmlDocument = Nothing

        author_url = rss

        html = GrabData(rss)
        doc = CleanHTML(html)
        html = doc.DocumentNode.OuterHtml

        nodes = FindNodesByAttribute(doc.DocumentNode, "div", "class", "listbox")

        ReDim fic(nodes.Count - 1)

        For node_idx = 0 To nodes.Count - 1

            doc = CleanHTML(nodes(node_idx).OuterHtml)
            temp = FindNodesByAttribute(doc.DocumentNode, "div", "class", "title")

            html = temp(0).OuterHtml
            doc = CleanHTML(html)

            temp = FindLinksByHref(doc.DocumentNode, "viewstory.php")

            link = temp(0).Attributes("href").Value
            link = Mid(link, InStr(link, "viewstory.php"))
            link = Replace(link, "'", "")
            link = HtmlDecode(link)
            link = "http://www." & Me.HostName & "/" & link

            fic(node_idx).StoryURL = link
            fic(node_idx).Title = temp(0).InnerText

            temp = FindLinksByHref(doc.DocumentNode, "viewuser.php")

            link = temp(0).Attributes("href").Value
            link = "http://www." & Me.HostName & "/" & link

            fic(node_idx).AuthorURL = link
            fic(node_idx).Author = temp(0).InnerText

            doc = CleanHTML(nodes(node_idx).OuterHtml)

            temp = FindNodesByAttribute(doc.DocumentNode, "div", "class", "content")

            summary = Split(temp(0).InnerHtml, "<br />")

            doc = CleanHTML(summary(0))
            summary(0) = doc.DocumentNode.OuterHtml
            StripTags(summary(0), "img")
            summary(0) = Replace(summary(0), "<img />", "")
            doc = CleanHTML(summary(0))
            summary(0) = doc.DocumentNode.InnerHtml

            fic(node_idx).Summary = summary(0)

            doc = CleanHTML(summary(1))
            summary(1) = HtmlDecode(doc.DocumentNode.InnerText)
            summary(1) = Trim(Replace(summary(1), "Categories: ", ""))

            fic(node_idx).Category = summary(1)

            Select Case UBound(summary)
                Case 8
                    idx = 7
                Case 9
                    idx = 8
            End Select

            doc = CleanHTML(summary(idx))
            summary(idx) = HtmlDecode(doc.DocumentNode.InnerText)
            summary(idx) = Replace(summary(idx), "Table of Contents", "")
            summary(idx) = Replace(summary(idx), "Chapters:", "")
            summary(idx) = CInt(summary(idx))

            fic(node_idx).ChapterCount = summary(idx)

            fic(node_idx).ID = Me.GetStoryID(fic(node_idx).StoryURL)

            doc = CleanHTML(nodes(node_idx).OuterHtml)

            temp = FindNodesByAttribute(doc.DocumentNode, "div", "class", "tail")

            html = temp(0).InnerText
            html = Replace(html, "[Report This] ", "")
            html = Replace(html, "Published: ", "")

            summary = Split(html, " Updated: ")

            fic(node_idx).PublishDate = summary(0)
            fic(node_idx).UpdateDate = summary(1)

            fic(node_idx).Summary += "|" & fic(node_idx).Category


        Next

        html = GenerateAtomFeed(fic)
        doc = CleanHTML(html)
        html = doc.DocumentNode.OuterHtml
        xmldoc = New XmlDocument
        xmldoc.LoadXml(html)

        Return xmldoc

    End Function

    Public Overrides Function GrabStoryInfo(ByRef dsRSS As System.Data.DataSet, ByVal idx As Integer) As Fanfic.Story

        Dim fic As New Fanfic.Story

        Dim temp() As String

        Dim summary() As String

        ' Story Name
        fic.Title = dsRSS.Tables("entry"). _
                        Rows(idx).Item("title")

        ' Story Author
        fic.Author = dsRSS.Tables("author"). _
                          Rows(idx).Item(0)

        fic.AuthorURL = dsRSS.Tables("author"). _
                          Rows(idx).Item(1)

        ' Story Location
        fic.StoryURL = dsRSS.Tables("link"). _
                          Rows(idx).Item(1)

        fic.ID = Me.GetStoryID(fic.StoryURL)

        fic.PublishDate = CDate(dsRSS.Tables("entry").Rows(idx).Item("published"))

        fic.UpdateDate = CDate(dsRSS.Tables("entry").Rows(idx).Item("updated"))

        temp = Split(dsRSS.Tables("entry").Rows(idx).Item("id"), ":")

        fic.ChapterCount = temp(2)

        summary = Split(dsRSS.Tables("summary").Rows(idx).Item("summary_Text"), "|")

        fic.Summary = summary(0)
        fic.Category = summary(1)

        Return fic

    End Function

#End Region

#Region "Chapter Navigation"

    Public Overrides Function GetChapters(ByVal htmlDoc As String) As String()

        Dim ret() As String

        Dim doc As HtmlDocument
        Dim temp As HtmlNodeCollection

        Dim idx As Integer

        doc = CleanHTML(htmlDoc)

        temp = FindNodesByAttribute(doc.DocumentNode, "select", "name", "chapter")
        htmlDoc = temp(0).InnerHtml
        doc = CleanHTML(htmlDoc)

        temp = doc.DocumentNode.SelectNodes("//option")

        If Not IsNothing(temp) Then
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

    Public Overrides Function ProcessChapters( _
                                               ByVal link As String, _
                                               ByVal index As Integer _
                                             ) As String

        Dim hl As URL
        Dim host As String
        Dim idx As Integer

        hl = ExtractUrl(link)
        host = hl.Host


        link = hl.Scheme & "://" & hl.Host & hl.URI
        link += "?"

        For idx = 0 To UBound(hl.Query)
            link += hl.Query(idx).Name & "=" & hl.Query(idx).Value
            link += "&"
        Next

        link += "chapter=" & (index + 1)

        Dim htmldoc As String

        htmldoc = GrabData(link)

        Return htmldoc

    End Function

    Public Overrides Function GetAuthorURL(ByVal link As String) As String

        Dim ret As String

        If InStr(link, "viewuser.php") > 0 Then
            ret = link
        Else
            ret = ""
        End If

        Return ret

    End Function

    Public Overrides Function GetStoryID(ByVal link As String) As String

        Dim ret As String
        Dim hl As URL

        hl = ExtractUrl(link)

        ret = hl.Query(0).Value

        hl = Nothing

        Return ret

    End Function

    Public Overrides Function GetStoryURL(ByVal id As String) As String

        Dim ret As String = ""

        ret = "http://www." & Me.HostName & "/viewstory.php?sid=" & id

        Return ret

    End Function

#End Region

#Region "HTML Processing"

    Public Overrides Function GrabAuthor(ByVal htmlDoc As String) As String

        Dim ret As String
        Dim doc As HtmlDocument

        doc = CleanHTML(htmlDoc)

        ret = doc.DocumentNode.SelectSingleNode("//title").InnerText

        ret = Mid(ret, InStr(ret, "by") + 2)

        doc = Nothing

        Return ret

    End Function

    Public Overrides Function GrabBody(ByVal htmldoc As String) As String

        Dim doc As HtmlDocument
        Dim temp As HtmlNodeCollection

        doc = CleanHTML(htmldoc)

        temp = FindNodesByAttribute(doc.DocumentNode, "div", "id", "story", False)

        htmldoc = temp(0).OuterHtml

        doc = Nothing
        temp = Nothing

        Return htmldoc

    End Function

    Public Overrides Function GrabDate(ByVal htmlDoc As String, ByVal title As String) As String

        Dim ret As String = ""
        Dim temp As HtmlNodeCollection
        Dim doc As HtmlDocument

        Dim summary As String()
        Dim author_link As String
        Dim story_link As String

        doc = CleanHTML(htmlDoc)

        temp = FindLinksByHref(doc.DocumentNode, "viewstory.php")
        story_link = HtmlDecode(temp(0).Attributes("href").Value)

        temp = FindLinksByHref(doc.DocumentNode, "viewuser.php")
        author_link = "http://www." & Me.HostName & "/" & HtmlDecode(temp(0).Attributes("href").Value)

        htmlDoc = Me.GrabData(author_link)
        doc = CleanHTML(htmlDoc)

        temp = FindLinksByHref(doc.DocumentNode, story_link)

        htmlDoc = temp(0).ParentNode.ParentNode.OuterHtml
        doc = CleanHTML(htmlDoc)

        temp = FindNodesByAttribute(doc.DocumentNode, "div", "class", "tail")

        htmlDoc = temp(0).InnerText
        htmlDoc = Replace(htmlDoc, "[Report This] ", "")
        htmlDoc = Replace(htmlDoc, "Published: ", "")

        summary = Split(htmlDoc, " Updated: ")

        Select Case title
            Case "Published: "
                ret = summary(0)
            Case "Updated: "
                ret = summary(1)
        End Select

        Return ret

        temp = Nothing
        doc = Nothing

    End Function

    Public Overrides Function GrabSeries(ByVal htmlDoc As String) As String

        Dim ret As String = ""
        Dim temp As HtmlNodeCollection
        Dim doc As HtmlDocument

        Dim summary As String()
        Dim author_link As String
        Dim story_link As String

        doc = CleanHTML(htmlDoc)

        temp = FindLinksByHref(doc.DocumentNode, "viewstory.php")
        story_link = HtmlDecode(temp(0).Attributes("href").Value)

        temp = FindLinksByHref(doc.DocumentNode, "viewuser.php")
        author_link = "http://www." & Me.HostName & "/" & HtmlDecode(temp(0).Attributes("href").Value)

        htmlDoc = Me.GrabData(author_link)
        doc = CleanHTML(htmlDoc)

        temp = FindLinksByHref(doc.DocumentNode, story_link)

        htmlDoc = temp(0).ParentNode.ParentNode.OuterHtml
        doc = CleanHTML(htmlDoc)

        temp = FindNodesByAttribute(doc.DocumentNode, "div", "class", "content")

        summary = Split(temp(0).InnerHtml, "<br />")

        doc = CleanHTML(summary(1))
        summary(1) = HtmlDecode(doc.DocumentNode.InnerText)
        summary(1) = Trim(Replace(summary(1), "Categories: ", ""))

        ret = summary(1)

        temp = Nothing
        doc = Nothing

        Return ret

    End Function

    Public Overrides Function GrabTitle(ByVal htmlDoc As String) As String

        Dim ret As String
        Dim doc As HtmlDocument

        doc = CleanHTML(htmlDoc)

        ret = doc.DocumentNode.SelectSingleNode("//title").InnerText

        ret = Mid(ret, 1, InStr(ret, "by") - 2)

        doc = Nothing

        Return ret

    End Function

#End Region

#Region "Properties"

    Public Overrides ReadOnly Property HostName() As String
        Get
            Return "yourfanfiction.com"
            'Return ""
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Your FanFiction"
        End Get
    End Property

    Public Overrides ReadOnly Property ErrorMessage() As String
        Get
            Return "No results found."
        End Get
    End Property

#End Region

    Public Sub New()
        Browser = New clsWeb
    End Sub
End Class