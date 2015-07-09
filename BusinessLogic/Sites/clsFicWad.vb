Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.Encoding
Imports System.Net
Imports System.Xml
Imports HtmlAgilityPack
Imports System.Web.HttpUtility

Class FicWad
    Inherits clsFanfic

#Region "Downloading HTML"

    Private AgeCheck As Boolean = True
    Private Username As String
    Private Password As String
    Private cookie_name As String = "ficwad_com.cookie"

    Public Overrides Function GrabData(ByVal url As String) As String

        Dim html As String
        Dim doc As HtmlDocument

        Dim check As String = "<li class=""blocked"">"

        html = Browser.DownloadPage(url, Me.cookie_name)
        doc = CleanHTML(html)
        html = doc.DocumentNode.InnerHtml

        html = CheckBlocked(url, html, check)

        doc = Nothing

        Return html

    End Function

    Function CheckBlocked(ByVal url As String, ByVal html As String, ByVal check As String) As String

        Dim ret As MsgBoxResult
        Dim doc As HtmlDocument
        Dim msg As String
        Dim fields As List(Of KeyValuePair(Of String, String))

        Dim link As URL
        Dim target As String = ""
        link = ExtractUrl(url)

        msg = link.Host
        msg += vbCrLf
        msg += vbCrLf
        msg += "Warning: This site requires you to be logged in to view NC-17 stories."
        msg += vbCrLf
        msg += vbCrLf
        msg += "Note: This information is requested in an effort to comply with "
        msg += vbCrLf
        msg += "the Child Online Protection Act (COPA) and related state law. "
        msg += vbCrLf
        msg += vbCrLf
        msg += "Do you wish to proceed?"

        If InStr(html, check) > 0 Then

            ret = MsgBox(msg, MsgBoxStyle.YesNo)

            If ret = MsgBoxResult.Yes Then
                Me.AgeCheck = True
            Else
                Me.AgeCheck = False
                html = ""
            End If

            If Me.AgeCheck Then

                Me.Username = InputBox("Enter Username")
                Me.Password = InputBox("Enter Password")

                target = "http://www." & Me.HostName & "/account/login"

                fields = New List(Of KeyValuePair(Of String, String))

                fields.Add(New KeyValuePair(Of String, String)("username", Me.Username))
                fields.Add(New KeyValuePair(Of String, String)("password", Me.Password))
                fields.Add(New KeyValuePair(Of String, String)("keeploggedin", "on"))
                fields.Add(New KeyValuePair(Of String, String)("submit", "Log in"))

                Browser.LogIn(target, "login", fields, Me.cookie_name)

                html = Browser.DownloadPage(url, Me.cookie_name)
                doc = CleanHTML(html)
                html = doc.DocumentNode.InnerHtml

            End If
        Else
            Me.AgeCheck = True
        End If

        Return html

    End Function

#End Region

#Region "RSS"

    Protected Overrides Function GrabFeedData(ByRef rss As String) As System.Xml.XmlDocument

        Dim html As String
        Dim doc As HtmlDocument = Nothing
        Dim tdoc As HtmlDocument = Nothing
        Dim nodes As HtmlNodeCollection
        Dim temp As HtmlNodeCollection

        Dim dte As DateTime
        Dim ymd() As String

        Dim dummy As String

        Dim temp_idx As Integer
        Dim node_idx As Integer

        Dim warning() As String = Nothing

        Dim author_url As String

        Dim summary() As String

        Dim ch_idx As Integer = 5

        Dim fic() As clsFanfic.Story

        Dim idx As Integer

        Dim xmldoc As XmlDocument = Nothing

        author_url = rss

        html = Me.GrabData(rss)

        doc = CleanHTML(html)

        nodes = GetListNodes(doc.DocumentNode, "class", "storylist", True)

        Try

            ReDim fic(nodes.Count - 1)

            For node_idx = 0 To nodes.Count - 1

                doc = CleanHTML(nodes(node_idx).InnerHtml)

                temp = doc.DocumentNode.SelectNodes("//a")

                fic(node_idx).Title = temp(0).InnerText
                fic(node_idx).StoryURL = "http://www." & Me.HostName & temp(0).Attributes("href").Value
                fic(node_idx).Author = temp(1).InnerText
                fic(node_idx).AuthorURL = "http://www." & Me.HostName & temp(1).Attributes("href").Value
                fic(node_idx).Category = temp(3).InnerText

                temp = FindLinksByHref(doc.DocumentNode, "/help#38")

                If Not IsNothing(temp) Then

                    ReDim warning(temp.Count - 1)

                    For temp_idx = 0 To temp.Count - 1
                        warning(temp_idx) = temp(temp_idx).Attributes("title").Value
                    Next

                End If

                temp = FindNodesByAttribute(doc.DocumentNode, "blockquote", "class", "summary", False)

                dummy = temp(0).InnerText

                fic(node_idx).Summary = dummy

                temp = FindNodesByAttribute(doc.DocumentNode, "p", "class", "meta", False)

                dummy = temp(0).InnerText
                dummy = DecodeHTML(dummy)

                summary = Split(dummy, " - ")

                If InStr(summary(5), "Chapter") > 0 Then
                    ch_idx = 5
                    fic(node_idx).ChapterCount = CleanString(summary(ch_idx).Split(":")(1).Trim)
                Else
                    ch_idx = 4
                    fic(node_idx).ChapterCount = 1
                End If

                dummy = CleanString(summary(ch_idx + 1).Split(":")(1).Trim)
                ymd = Split(dummy, "-")
                dte = New DateTime(ymd(0), ymd(1), ymd(2))

                fic(node_idx).PublishDate = dte.ToShortDateString

                dummy = CleanString(summary(ch_idx + 2).Split(":")(1).Trim)
                ymd = Split(dummy, "-")
                dte = New DateTime(ymd(0), ymd(1), ymd(2))

                fic(node_idx).UpdateDate = dte.ToShortDateString



                dummy = fic(node_idx).Summary
                dummy += vbCrLf
                dummy += CleanString(summary(1))
                dummy += vbCrLf
                dummy += CleanString(summary(2))
                dummy += vbCrLf
                dummy += CleanString(summary(3))
                dummy += vbCrLf
                dummy += "Warnings: "

                For idx = 0 To UBound(warning)
                    If idx < UBound(warning) Then
                        dummy += warning(idx) & ","
                    Else
                        dummy += warning(idx)
                    End If
                Next

                fic(node_idx).Summary = dummy

                fic(node_idx).ID = GetStoryID(fic(node_idx).StoryURL)

            Next node_idx

            html = GenerateAtomFeed(fic)

            doc = CleanHTML(html)

            html = doc.DocumentNode.OuterHtml

            xmldoc = New XmlDocument

            xmldoc.LoadXml(html)

        Catch
            xmldoc = Nothing
        Finally
            doc = Nothing
            tdoc = Nothing
            nodes = Nothing
            temp = Nothing
            fic = Nothing
        End Try

        Return xmldoc

    End Function

    Public Overrides Function GrabStoryInfo(ByVal idx As Integer) As clsFanfic.Story

        Dim dsRSS As DataSet = MyBase.datasetRSS

        Dim fic As New clsFanfic.Story

        Dim temp() As String

        ' Story Name
        fic.Title = dsRSS.Tables("entry"). _
                        Rows(idx).Item("title")


        ' Story Author
        fic.Author = dsRSS.Tables("author"). _
                          Rows(idx).Item("name")

        fic.AuthorURL = dsRSS.Tables("author"). _
                          Rows(idx).Item("uri")

        ' Story Location
        fic.StoryURL = dsRSS.Tables("link"). _
                          Rows(idx).Item("href")

        fic.PublishDate = CDate(dsRSS.Tables("entry").Rows(idx).Item("published"))

        fic.UpdateDate = CDate(dsRSS.Tables("entry").Rows(idx).Item("updated"))

        temp = Split(dsRSS.Tables("entry").Rows(idx).Item("id"), ":")

        fic.Category = UrlDecode(temp(1))
        fic.ID = temp(0)
        fic.ChapterCount = temp(2)

        fic.Summary = dsRSS.Tables("summary").Rows(idx).Item("summary_Text")

        Return fic

    End Function

#End Region

#Region "Chapter Navigation"

    Public Overrides Function GetChapters(ByVal htmlDoc As String) As String()

        Dim ret() As String

        Dim doc As HtmlDocument
        Dim node As HtmlNodeCollection
        Dim temp As HtmlNodeCollection
        Dim link As String

        Dim idx As Integer

        doc = CleanHTML(htmlDoc)
        htmlDoc = doc.DocumentNode.InnerHtml

        temp = FindNodesByAttribute(doc.DocumentNode, "ul", "class", "storylist")
        htmlDoc = temp(0).OuterHtml

        doc = CleanHTML(htmlDoc)
        htmlDoc = doc.DocumentNode.InnerHtml

        node = doc.DocumentNode.SelectNodes("//li")

        If Not IsNothing(node) Then
            ReDim ret(node.Count - 1)
            For idx = 0 To node.Count - 1

                htmlDoc = node(idx).InnerHtml
                doc = CleanHTML(htmlDoc)

                temp = FindLinksByHref(doc.DocumentNode, "/story/")

                link = temp(0).Attributes("href").Value
                link = Split(link, "/")(2)
                link = GetStoryURL(link)

                ret(idx) = link
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
            link = Me.Chapters(index)
        End If

        htmldoc = GrabData(link)

        Return htmldoc

    End Function

    Public Overrides Function GetAuthorURL(ByVal link As String) As String

        Dim ret As String

        If InStr(link, "/author") > 0 Then
            ret = link
        Else
            ret = ""
        End If

        Return ret

    End Function

    Public Overrides Function GetStoryID(ByVal link As String) As String

        Dim ret As String
        Dim hl As URL

        Dim uri() As String

        hl = ExtractUrl(link)

        uri = Split(hl.URI, "/")

        ret = uri(UBound(uri))

        hl = Nothing

        Return ret

    End Function

    Public Overrides Function GetStoryURL(ByVal id As String) As String

        Dim ret As String = ""

        ret = "http://" & Me.HostName & "/story/" & id

        Return ret

    End Function

#End Region

#Region "HTML Processing"

    Public Overrides Function GrabAuthor(ByVal htmlDoc As String) As String

        Dim ret As String
        Dim doc As HtmlDocument
        Dim temp As HtmlNodeCollection

        doc = CleanHTML(htmlDoc)
        htmlDoc = doc.DocumentNode.InnerHtml

        htmlDoc = GrabStoryDiv(htmlDoc)
        doc = CleanHTML(htmlDoc)

        temp = FindLinksByHref(doc.DocumentNode, "/a")

        ret = temp(0).InnerText

        temp = Nothing
        doc = Nothing

        Return ret

    End Function

    Public Overrides Function GrabBody(ByVal htmldoc As String) As String

        Dim ret As String
        Dim doc As HtmlDocument
        Dim temp As HtmlNodeCollection

        htmldoc = GrabStoryDiv(htmldoc)
        doc = CleanHTML(htmldoc)

        temp = FindNodesByAttribute(doc.DocumentNode, "div", "id", "storytext", False)

        ret = "<div>" & temp(0).InnerHtml & "</div>"

        doc = Nothing
        temp = Nothing

        Return ret

    End Function

    Public Overrides Function GrabDate(ByVal htmlDoc As String, ByVal title As String) As String

        Dim ret As String = ""
        Dim temp As HtmlNodeCollection
        Dim doc As HtmlDocument

        Dim dte As DateTime
        Dim ymd() As String
        Dim ch_idx As Integer

        Dim summary As String()

        doc = CleanHTML(htmlDoc)
        htmlDoc = doc.DocumentNode.InnerHtml

        temp = FindNodesByAttribute(doc.DocumentNode, "p", "class", "meta")
        ret = temp(0).InnerText
        ret = DecodeHTML(ret)

        summary = Split(ret, " - ")

        For ch_idx = 0 To UBound(summary)

            If InStr(summary(ch_idx), Trim(title)) > 0 Then
                Exit For
            End If

        Next

        ret = CleanString(summary(ch_idx).Split(":")(1).Trim)
        ymd = Split(ret, "-")
        dte = New DateTime(ymd(0), ymd(1), ymd(2))

        ret = dte.ToShortDateString

        temp = Nothing
        doc = Nothing

        Return ret

    End Function

    Public Overrides Function GrabSeries(ByVal htmlDoc As String) As String

        Dim ret As String

        Dim doc As HtmlDocument
        Dim node As HtmlNodeCollection
        Dim temp As HtmlNodeCollection

        doc = CleanHTML(htmlDoc)
        htmlDoc = doc.DocumentNode.InnerHtml

        temp = FindNodesByAttribute(doc.DocumentNode, "div", "class", "storylist")
        htmlDoc = temp(0).OuterHtml

        doc = CleanHTML(htmlDoc)
        htmlDoc = doc.DocumentNode.InnerHtml

        node = doc.DocumentNode.SelectNodes("//a")

        ret = node(3).InnerText

        Return ret

    End Function

    Private Function GrabStoryDiv(ByVal htmldoc As String) As String

        Dim ret As String = ""
        Dim temp As HtmlNodeCollection
        Dim doc As HtmlDocument

        doc = CleanHTML(htmldoc)

        temp = FindNodesByAttribute(doc.DocumentNode, "div", "id", "story")

        htmldoc = "<div>"
        htmldoc += temp(0).InnerHtml
        htmldoc += "</div>"

        ret = htmldoc

        temp = Nothing
        doc = Nothing

        Return ret

    End Function

    Public Overrides Function GrabTitle(ByVal htmlDoc As String) As String

        Dim ret As String

        Dim doc As HtmlDocument
        Dim node As HtmlNodeCollection
        Dim temp As HtmlNodeCollection

        doc = CleanHTML(htmlDoc)
        htmlDoc = doc.DocumentNode.InnerHtml

        temp = FindNodesByAttribute(doc.DocumentNode, "div", "class", "storylist")
        htmlDoc = temp(0).OuterHtml

        doc = CleanHTML(htmlDoc)
        htmlDoc = doc.DocumentNode.InnerHtml

        node = doc.DocumentNode.SelectNodes("//a")

        ret = node(0).InnerText

        Return ret

    End Function

#End Region

#Region "Properties"

    Public Overrides ReadOnly Property HostName() As String
        Get
            Return "ficwad.com"
            'Return ""
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "FicWad"
        End Get
    End Property

    Public Overrides ReadOnly Property ErrorMessage() As String
        Get
            Return "Unknown author"
        End Get
    End Property

#End Region

End Class