Imports System
Imports System.text
Imports System.net
Imports System.Xml
Imports System.Data
Imports System.Collections.Generic
Imports HtmlAgilityPack
Imports System.Web.HttpUtility


Class AFF
    Inherits clsFanfic

#Region "Downloading HTML"

    Private cookie_name As String = "adultfanfiction_org.cookie"
    Private AgeCheck As Boolean = True
    Private FullName As String
    Private DOB As String

    Public Overrides Function GrabData(ByVal url As String) As String

        Dim html As String
        Dim title As String = ""
        Dim check1 As Boolean
        Dim check2 As Boolean
        Dim target As String = ""
        Dim u As Uri
        Dim fields As List(Of KeyValuePair(Of String, String))

        html = Browser.DownloadPage(url, cookie_name)

        title = modHTML.GetTitle(html)

        check1 = CheckPage(html)
        check2 = CheckAge(title)

        If Not Me.AgeCheck Then
            html = "<AgeCheck value='False' />"
        Else
            If Not check1 Then

                target = "http://www.adult-fanfiction.org/"

                Browser.FollowLink(target, "I am 18 years of age or older.", cookie_name)

                html = Browser.DownloadPage(url, cookie_name)

            End If

            If Not check2 Then

                u = New Uri(url)

                target = u.Scheme & Uri.SchemeDelimiter & u.Host & "/form_adult.php"

                fields = Nothing
                fields = New List(Of KeyValuePair(Of String, String))

                fields.Add(New KeyValuePair(Of String, String)("cmbmonth", CDate(Me.DOB).Month))
                fields.Add(New KeyValuePair(Of String, String)("cmbday", CDate(Me.DOB).Day))
                fields.Add(New KeyValuePair(Of String, String)("cmbyear", CDate(Me.DOB).Year))
                fields.Add(New KeyValuePair(Of String, String)("cmbname", Me.FullName))
                fields.Add(New KeyValuePair(Of String, String)("submit", "Click here to submit"))

                Browser.LogIn(target, "form", fields, cookie_name)

                html = Browser.DownloadPage(url, cookie_name)

            End If

        End If

        Return html

    End Function

    Function CheckAge(ByVal title As String) As Boolean

        Dim ok As Boolean = True

        Dim ret As MsgBoxResult
        Dim msg As String

        Dim dob As String = ""
        Dim tsDOB As TimeSpan

        Dim name As String = ""

        msg = "Warning: This site requires age verification using e-signing"
        msg += vbCrLf
        msg += vbCrLf
        msg += "I hereby affirm, under the penalties of perjury pursuant to "
        msg += vbCrLf
        msg += "Title 28 U.S.C. secion 1746, that I was born on [Date of Birth]"
        msg += vbCrLf
        msg += vbCrLf
        msg += "Signed [YOUR NAME]"
        msg += vbCrLf
        msg += vbCrLf
        msg += "Note: This information is provided in an effort to comply with "
        msg += vbCrLf
        msg += "the Child Online Protection Act (COPA) and related state law. "
        msg += vbCrLf
        msg += "Providing a false declaration under the penalties of perjury is "
        msg += "a criminal offense. This document constitutes an un-sworn "
        msg += "declaration under federal law. "
        msg += vbCrLf
        msg += vbCrLf
        msg += "The information provided here will only be provided to a third party "
        msg += vbCrLf
        msg += "when legally required (i.e. when/if a subpoena is served to us). "
        msg += vbCrLf
        msg += "Your information will not be given out except in the case mentioned above."
        msg += vbCrLf
        msg += vbCrLf
        msg += "Do you wish to proceed?"

        If InStr(title, "Birthdate Verification Page") > 0 Then

            ret = MsgBox(msg, MsgBoxStyle.YesNo)

            If ret = MsgBoxResult.Yes Then
                Me.AgeCheck = True
                ok = False
            Else
                Me.AgeCheck = False
                ok = False
            End If

            If Me.AgeCheck Then

                dob = InputBox("Enter the day you were born (mm/dd/yyyy)")
                If IsDate(dob) Then
                    tsDOB = Date.Today.Subtract(CDate(dob))
                    If tsDOB.Days > (365 * 18) Then
                        name = InputBox("Enter First and Last Name")
                    Else
                        Me.AgeCheck = False
                    End If
                End If

                Me.FullName = name
                Me.DOB = dob


            End If

        Else
            Me.AgeCheck = True
            ok = True
        End If

        Return ok

    End Function


    Function CheckPage(ByVal html As String) As Boolean

        Dim ok As Boolean = True

        Dim ret As MsgBoxResult
        Dim msg As String = "In order to proceed, you must be at least 18 years of age (21 years of age in some jurisdictions)"

        If InStr(html, msg) > 0 Then

            msg += "Welcome to adult-fanfiction.org.  "
            msg += "In order to proceed, you must be at least 18 years of age (21 years of age in some jurisdictions), "
            msg += "and legally permitted to view Adult Content in your area.  "
            msg += "This is an archive of literature written by and for Adults, only. "
            msg += vbCrLf
            msg += vbCrLf
            msg += "Do you wish to proceed?"

            ret = MsgBox(msg, MsgBoxStyle.YesNo)

            If ret = MsgBoxResult.Yes Then
                Me.AgeCheck = True
            End If

        Else
            Me.AgeCheck = True
        End If

        Return Me.AgeCheck

    End Function

#End Region

#Region "RSS"

    Protected Overrides Function GrabFeedData(ByRef rss As String) As System.Xml.XmlDocument

        Dim html As String
        Dim doc As HtmlDocument = Nothing

        Dim nodes As HtmlNodeCollection
        Dim temp As HtmlNodeCollection

        Dim link As String = ""
        Dim url As URL

        Dim node_idx As Integer

        Dim author As String
        Dim author_url As String

        Dim summary() As String
        Dim fic() As clsFanfic.Story

        Dim xmldoc As XmlDocument = Nothing

        author_url = rss

        If InStr(rss, "zone=") = 0 Then
            If InStr(rss, "view=story") = 0 Then

                html = Me.GrabData(rss)
                doc = CleanHTML(html)

                nodes = FindLinksByHref(doc.DocumentNode, "view=story")
                link = nodes(0).Attributes("href").Value
                link = DecodeHTML(link)

            Else
                link = rss
            End If

            html = Me.GrabData(link)
            doc = CleanHTML(html)

            nodes = FindNodesByAttribute(doc.DocumentNode, "div", "id", "contentdata", False)

            doc = CleanHTML(nodes(0).OuterHtml)

            nodes = FindLinksByHref(doc.DocumentNode, "zone=")
            link = nodes(0).Attributes("href").Value
            link = DecodeHTML(link)

        Else
            link = rss
        End If

        html = Me.GrabData(link)
        doc = CleanHTML(html)

        nodes = FindNodesByAttribute(doc.DocumentNode, "div", "id", "contentdata")

        html = nodes(0).OuterHtml
        doc = CleanHTML(html)

        author = doc.DocumentNode.SelectSingleNode("//h2").InnerText

        nodes = FindNodesByAttribute(doc.DocumentNode, "div", "class", "alist", False)
        html = nodes(0).OuterHtml
        doc = CleanHTML(html)

        nodes = doc.DocumentNode.SelectNodes("//li")

        ReDim fic(nodes.Count - 1)

        For node_idx = 0 To nodes.Count - 1

            html = nodes(node_idx).InnerHtml
            doc = CleanHTML(html)

            temp = FindLinksByHref(doc.DocumentNode, "story.php")

            fic(node_idx).Author = author
            fic(node_idx).AuthorURL = author_url
            fic(node_idx).StoryURL = temp(0).Attributes("href").Value
            fic(node_idx).Title = temp(0).InnerText

            html = Replace(html, temp(0).OuterHtml, "")
            temp = FindLinksByHref(doc.DocumentNode, "review.php")
            html = Replace(html, temp(0).OuterHtml, "")

            doc = CleanHTML(html)
            html = doc.DocumentNode.SelectSingleNode("//span").InnerHtml

            summary = Split(html, "<br />")

            fic(node_idx).Summary = DecodeHTML(summary(1))

            summary = Split(summary(2), "-:-")

            fic(node_idx).PublishDate = Trim(Replace(summary(0), "Posted : ", ""))
            fic(node_idx).UpdateDate = Trim(Replace(summary(1), "Edited : ", ""))

            url = ExtractUrl(fic(node_idx).StoryURL)

            fic(node_idx).Category = Split(url.Host, ".")(0)

            fic(node_idx).ID = Me.GetStoryID(fic(node_idx).StoryURL)

            html = Me.GrabData(fic(node_idx).StoryURL)
            doc = CleanHTML(html)

            temp = FindNodesByAttribute(doc.DocumentNode, "select", "name", "chapnav")
            html = temp(0).InnerHtml
            doc = CleanHTML(html)

            fic(node_idx).ChapterCount = doc.DocumentNode.SelectNodes("//option").Count

            If Not Me.AgeCheck Then
                Exit For
            End If

        Next

        If Not Me.AgeCheck Then
            xmldoc = Nothing
        Else

            html = GenerateAtomFeed(fic)

            doc = CleanHTML(html)

            html = doc.DocumentNode.OuterHtml

            xmldoc = New XmlDocument

            xmldoc.LoadXml(html)

        End If

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
                          Rows(idx).Item(0)
        ' Story Location
        fic.StoryURL = dsRSS.Tables("link"). _
                          Rows(idx).Item(1)

        fic.PublishDate = CDate(dsRSS.Tables("entry").Rows(idx).Item("published"))

        fic.UpdateDate = CDate(dsRSS.Tables("entry").Rows(idx).Item("updated"))

        temp = Split(dsRSS.Tables("entry").Rows(idx).Item("id"), ":")

        fic.ID = temp(0)
        fic.Category = temp(1)
        fic.ChapterCount = temp(2)

        fic.Summary = dsRSS.Tables("summary").Rows(idx).Item("summary_Text")

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

        temp = FindNodesByAttribute(doc.DocumentNode, "select", "name", "chapnav")
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

        hl = ExtractUrl(link)
        host = hl.Host


        link = hl.Scheme & "://" & hl.Host & hl.URI
        link += "?"
        link += hl.Query(0).Name & "=" & hl.Query(0).Value
        link += "&"
        link += "chapter=" & (index + 1)

        Dim htmldoc As String

        htmldoc = GrabData(link)

        Me.StoryURL = link

        Return htmldoc

    End Function

    Public Overrides Function GetAuthorURL(ByVal link As String) As String

        Dim htmldoc As String
        Dim doc As HtmlDocument
        Dim temp As HtmlNodeCollection

        Dim ret As String = ""

        If InStr(link, "profile.php") > 0 Then
            ret = link
        Else
            htmldoc = GrabData(link)
            htmldoc = GrabHeaderRow(htmldoc)
            doc = CleanHTML(htmldoc)
            temp = FindLinksByHref(doc.DocumentNode, "profile.php")
            ret = temp(0).Attributes("href").Value
        End If

        Return ret

    End Function

    Public Overrides Function GetStoryID(ByVal link As String) As String

        Dim ret As String = ""
        Dim url As URL

        url = ExtractUrl(link)

        ret = Split(url.Host, ".")(0)
        ret += "|"
        ret += url.Query(0).Value

        Return ret

    End Function

    Public Overrides Function GetStoryURL(ByVal id As String) As String

        Dim temp As String()
        Dim link As String
        Dim category As String

        temp = Split(id, "|")
        category = temp(0)
        id = temp(1)

        link = "http://" & category & "." & Me.HostName & "/story.php?no=" & id

        Return link

    End Function

#End Region

#Region "HTML Processing"

    Private Function GrabPageTable(ByVal htmldoc As String) As String

        Dim ret As String = ""
        Dim temp As HtmlNodeCollection
        Dim doc As HtmlDocument

        doc = CleanHTML(htmldoc)

        temp = doc.DocumentNode.SelectNodes("//table")

        htmldoc = "<table>"
        htmldoc += temp(6).InnerHtml
        htmldoc += "</table>"

        ret = htmldoc

        temp = Nothing
        doc = Nothing

        Return ret

    End Function

    Private Function GrabHeaderRow(ByVal htmldoc As String) As String

        Dim ret As String
        Dim temp As HtmlNodeCollection
        Dim doc As HtmlDocument

        htmldoc = GrabPageTable(htmldoc)

        doc = CleanHTML(htmldoc)
        temp = doc.DocumentNode.SelectNodes("//tr")

        htmldoc = "<table>"
        htmldoc += "<tr>"
        htmldoc += temp(0).InnerHtml
        htmldoc += "</tr>"
        htmldoc += "</table>"

        ret = htmldoc

        temp = Nothing
        doc = Nothing

        Return ret

    End Function

    Public Overrides Function GrabAuthor(ByVal htmlDoc As String) As String

        Dim ret As String
        Dim temp As HtmlNodeCollection
        Dim doc As HtmlDocument

        htmlDoc = GrabHeaderRow(htmlDoc)

        doc = CleanHTML(htmlDoc)

        temp = FindLinksByHref(doc.DocumentNode, "profile.php")

        ret = CleanString(temp(0).InnerText)

        temp = Nothing
        doc = Nothing

        Return ret

    End Function

    Public Overrides Function GrabBody(ByVal htmldoc As String) As String

        Dim ret As String = ""

        Dim doc As HtmlDocument
        Dim temp As HtmlNodeCollection

        htmldoc = GrabPageTable(htmldoc)
        doc = CleanHTML(htmldoc)

        temp = doc.DocumentNode.SelectNodes("//tr")

        htmldoc = temp(3).ChildNodes(1).InnerHtml

        doc = Nothing
        temp = Nothing

        ret = htmldoc

        Return ret

    End Function

    Public Overrides Function GrabDate(ByVal htmlDoc As String, ByVal title As String) As String

        Dim ret As String = ""
        Dim temp As HtmlNodeCollection
        Dim doc As HtmlDocument

        Dim category As String
        Dim summary As String()
        Dim link As String

        Dim u As URL

        u = ExtractUrl(Me.StoryURL)

        htmlDoc = GrabHeaderRow(htmlDoc)

        doc = CleanHTML(htmlDoc)

        category = Replace(u.Host, Me.HostName, "").Replace(".", "")

        temp = FindLinksByHref(doc.DocumentNode, "profile.php")

        link = temp(0).Attributes("href").Value
        link = DecodeHTML(link)

        link += "&view=story&zone=" & category

        htmlDoc = Me.GrabData(link)
        doc = CleanHTML(htmlDoc)

        link = u.Scheme & Uri.SchemeDelimiter & u.Host & u.URI & "?" & u.Query(0).Name & "=" & u.Query(0).Value

        temp = FindLinksByHref(doc.DocumentNode, link)

        htmlDoc = temp(0).ParentNode.InnerHtml

        summary = Split(htmlDoc, "<br />")
        summary = Split(summary(2), "-:-")

        Select Case title
            Case "Published: "
                ret = CDate(Trim(Replace(summary(0), "Posted :", ""))).ToShortDateString
            Case "Updated: "
                ret = CDate(Trim(Replace(summary(1), "Edited :", ""))).ToShortDateString
        End Select

        temp = Nothing
        doc = Nothing

        Return ret

    End Function

    Public Overrides Function GrabSeries(ByVal htmlDoc As String) As String

        Dim ret As String
        Dim temp As HtmlNodeCollection
        Dim doc As HtmlDocument

        Dim category As String

        htmlDoc = GrabHeaderRow(htmlDoc)
        doc = CleanHTML(htmlDoc)

        temp = FindLinksByHref(doc.DocumentNode, "main.php")

        category = temp(0).InnerText

        ret = category

        temp = Nothing
        doc = Nothing

        Return ret

    End Function

    Public Overrides Function GrabTitle(ByVal htmlDoc As String) As String

        Dim ret As String
        Dim doc As HtmlDocument

        doc = CleanHTML(htmlDoc)

        ret = doc.DocumentNode.SelectSingleNode("//title").InnerText

        ret = Trim(Replace(ret, "Story:", ""))

        doc = Nothing

        Return ret

    End Function

#End Region



#Region "Properties"

    Public Overrides ReadOnly Property HostName() As String
        Get
            Return "adult-fanfiction.org"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "AFF"
        End Get
    End Property

    Public Overrides ReadOnly Property ErrorMessage() As String
        Get
            Return "The member you are looking for does not exsist."
        End Get
    End Property

#End Region

End Class