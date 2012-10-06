Imports System
Imports System.Text
Imports System.Text.Encoding
Imports System.Net
Imports System.Xml
Imports HtmlAgilityPack
Imports System.Web.HttpUtility

Class FicWad
    Inherits clsFanfic

#Region "Downloading HTML"

    Private Browser As New clsWeb
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
        Dim postData As String = ""

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

                postData = "username=" & URLEncode(Me.Username)
                postData += "&"
                postData += "password=" & URLEncode(Me.Password)
                postData += "&"
                postData += "keeploggedin=on"

                Browser.DownloadCookies(target, postData, Me.cookie_name)

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

    Public Overrides Function GrabFeed(ByRef rss As String) As System.Xml.XmlDocument

        Dim html As String
        Dim doc As HtmlDocument = Nothing

        Dim temp As HtmlNodeCollection
        Dim node As HtmlNodeCollection
        Dim node_idx As Integer

        Dim fic As New clsFanfic.Story

        Dim story_url As String

        Dim link As URL
        Dim entry As String
        Dim value As String
        Dim text As String
        Dim summary() As String

        Dim xmldoc As XmlDocument = Nothing

        If InStr(rss, "author") = 0 Then Return Nothing

        If InStr(rss, "nc17") = 0 Then
            If InStr(rss, "feed") = 0 Then
                link = ExtractUrl(rss)
                rss = link.Scheme & "://" & link.Host & "/feed" & link.URI
            End If

            rss += "/nc17"

        End If

        html = GrabData(rss)

        If Me.AgeCheck Then

            html = Replace(html, "<!--", "")
            html = Replace(html, "DATA[", "")
            html = Replace(html, "-->", "")
            doc = CleanHTML(html)

            node = doc.DocumentNode.SelectNodes("//entry")

            html = "<feed>"

            For node_idx = 0 To node.Count - 1

                entry = node(node_idx).OuterHtml
                doc = CleanHTML(entry)
                entry = doc.DocumentNode.InnerHtml

                text = doc.DocumentNode.SelectSingleNode("//title").InnerHtml
                fic.Title = Trim(Mid(text, 1, LastPos(text, "(") - 1))

                entry = Replace(entry, text, fic.Title)

                text = Mid(text, LastPos(text, "(") + 1)
                fic.Category = Trim(Replace(text, ")", ""))
                fic.Category = URLEncode(fic.Category)

                value = doc.DocumentNode.SelectSingleNode("//id").OuterHtml
                story_url = doc.DocumentNode.SelectSingleNode("//id").InnerHtml

                fic.ID = Me.GetStoryID(story_url)

                text = Me.GrabData(story_url)
                doc = CleanHTML(text)

                temp = FindNodesByAttribute(doc.DocumentNode, "p", "class", "meta", False)
                text = temp(0).InnerHtml
                text = HtmlDecode(text)
                text = HtmlDecode(text)
                text = Replace(text, vbLf, "")
                text = Replace(text, vbTab, " ")

                summary = Split(text, " - ")

                If InStr(text, "Chapter") > 0 Then
                    fic.ChapterCount = summary(5)
                    fic.ChapterCount = Mid(fic.ChapterCount, InStr(fic.ChapterCount, ":") + 2)
                Else
                    fic.ChapterCount = 1
                End If

                text = "<id>"
                text += Trim(fic.Category) & ":" & fic.ID & ":" & fic.ChapterCount
                text += "</id>"

                entry = Replace(entry, value, text)

                html += entry


            Next

            html += "</feed>"

            xmldoc = New XmlDocument
            xmldoc.LoadXml(html)

        End If

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
                          Rows(idx).Item("name")

        fic.AuthorURL = dsRSS.Tables("author"). _
                          Rows(idx).Item("uri")

        ' Story Location
        fic.StoryURL = dsRSS.Tables("link"). _
                          Rows(idx).Item("href")

        fic.PublishDate = CDate(dsRSS.Tables("entry").Rows(idx).Item("published"))

        fic.UpdateDate = CDate(dsRSS.Tables("entry").Rows(idx).Item("updated"))

        temp = Split(dsRSS.Tables("entry").Rows(idx).Item("id"), ":")

        fic.Category = UrlDecode(temp(0))
        fic.ID = temp(1)
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

        temp = FindNodesByAttribute(doc.DocumentNode, "ul", "id", "storylist")
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

        ret = "http://www." & Me.HostName & "/story/" & id

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

        temp = FindLinksByHref(doc.DocumentNode, "/author")

        ret = temp(0).InnerText

        temp = Nothing
        doc = Nothing

        Return ret

    End Function

    Public Overrides Function GrabBody(ByVal htmldoc As String) As String

        Dim doc As HtmlDocument
        Dim temp As HtmlNodeCollection

        htmldoc = GrabStoryDiv(htmldoc)
        doc = CleanHTML(htmldoc)

        temp = FindNodesByAttribute(doc.DocumentNode, "div", "id", "storytext")

        htmldoc = "<div>"
        htmldoc = temp(0).InnerHtml
        htmldoc += "</div>"

        doc = Nothing
        temp = Nothing

        Return htmldoc

    End Function

    Public Overrides Function GrabDate(ByVal htmlDoc As String, ByVal title As String) As String

        Dim ret As String = ""
        Dim temp As HtmlNodeCollection
        Dim doc As HtmlDocument

        Dim summary As String()

        doc = CleanHTML(htmlDoc)
        htmlDoc = doc.DocumentNode.InnerHtml

        htmlDoc = GrabStoryDiv(htmlDoc)
        doc = CleanHTML(htmlDoc)

        temp = FindNodesByAttribute(doc.DocumentNode, "p", "class", "meta")
        ret = temp(0).InnerHtml
        ret = HtmlDecode(ret)
        ret = HtmlDecode(ret)
        ret = Replace(ret, vbLf, "")
        ret = Replace(ret, vbTab, " ")

        summary = Split(ret, " - ")


        Select Case title
            Case "Published: "
                ret = summary(5)
            Case "Updated: "
                ret = summary(6)
        End Select

        ret = ConvertToAscii(ret)
        ret = Replace(ret, "?", " ")
        ret = Replace(ret, title, "")
        ret = RTrim(ret)
        ret = LTrim(ret)

        temp = Nothing
        doc = Nothing

        Return ret

    End Function

    Public Overrides Function GrabSeries(ByVal htmlDoc As String) As String

        Dim ret As String = ""
        Dim temp As HtmlNodeCollection
        Dim doc As HtmlDocument

        Dim summary As String()
       
        htmlDoc = GrabStoryDiv(htmlDoc)
        doc = CleanHTML(htmlDoc)

        temp = FindNodesByAttribute(doc.DocumentNode, "p", "class", "meta")
        ret = temp(0).InnerHtml
        ret = HtmlDecode(ret)
        ret = HtmlDecode(ret)
        ret = Replace(ret, vbLf, "")
        ret = Replace(ret, vbTab, " ")

        summary = Split(ret, " - ")

        htmlDoc = "<div>"
        htmlDoc += summary(0)
        htmlDoc += "</div>"

        doc = CleanHTML(htmlDoc)

        temp = FindLinksByHref(doc.DocumentNode, "/category")

        ret = temp(0).InnerText

        temp = Nothing
        doc = Nothing

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
        Dim temp As HtmlNodeCollection

        htmlDoc = GrabStoryDiv(htmlDoc)
        doc = CleanHTML(htmlDoc)

        htmlDoc = doc.DocumentNode.SelectSingleNode("//h3").OuterHtml
        doc = CleanHTML(htmlDoc)

        temp = FindLinksByHref(doc.DocumentNode, "/story")

        ret = temp(0).InnerText

        temp = Nothing
        doc = Nothing

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

    Public Sub New()
        Browser = New clsWeb
    End Sub
End Class