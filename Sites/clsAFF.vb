Imports System
Imports System.text
Imports System.net
'Imports System.IO
Imports System.Xml
Imports HtmlAgilityPack
Imports System.Web.HttpUtility


Class AFF
    Inherits Fanfic

#Region "RSS"

    Private AgeCheck As Boolean = True
    Private FullName As String
    Private DOB As String

    Public Overrides Function GrabFeed(ByRef rss As String) As System.Xml.XmlDocument

        Dim html As String
        Dim doc As HtmlDocument = Nothing

        Dim nodes As HtmlNodeCollection
        Dim temp As HtmlNodeCollection

        Dim link As String
        Dim url As URL

        Dim node_idx As Integer

        Dim author As String
        Dim author_url As String

        Dim summary() As String
        Dim fic() As Fanfic.Story

        Dim xmldoc As XmlDocument = Nothing

        If InStr(rss, "zone=") = 0 Then
            If InStr(rss, "view=story") = 0 Then

                html = DownloadPage(rss)
                doc = CleanHTML(html)

                nodes = FindLinksByHref(doc.DocumentNode, "view=story")
                link = nodes(0).Attributes("href").Value

                author_url = rss

            Else
                link = rss
            End If

            html = DownloadPage(link)
            doc = CleanHTML(html)

            nodes = FindLinksByHref(doc.DocumentNode, "zone=")
            link = nodes(0).Attributes("href").Value

        Else
            link = rss
        End If

        html = DownloadPage(link)
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
            fic(node_idx).URL = temp(0).Attributes("href").Value
            fic(node_idx).Title = temp(0).InnerText

            html = Replace(html, temp(0).OuterHtml, "")
            temp = FindLinksByHref(doc.DocumentNode, "review.php")
            html = Replace(html, temp(0).OuterHtml, "")

            doc = CleanHTML(html)
            html = doc.DocumentNode.SelectSingleNode("//span").InnerHtml

            summary = Split(html, "<br />")

            fic(node_idx).Summary = summary(1)

            summary = Split(summary(2), "-:-")

            fic(node_idx).PublishDate = Trim(Replace(summary(0), "Posted : ", ""))
            fic(node_idx).UpdateDate = Trim(Replace(summary(1), "Edited : ", ""))

            url = ExtractUrl(fic(node_idx).URL)

            fic(node_idx).Category = Split(url.Host, ".")(0)

            fic(node_idx).ID = Me.GetStoryID(fic(node_idx).URL)

            html = Me.GrabData(fic(node_idx).URL)
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

            html = "<feed>"

            For node_idx = 0 To UBound(fic)

                html += "<entry>"
                html += "<author>"
                html += "<name>"
                html += fic(node_idx).Author
                html += "</name>"
                html += "<uri>"
                html += author_url
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
                html += "<link rel=""alternate"" href=""" & fic(node_idx).URL & """ />"
                html += "<id>"
                html += fic(node_idx).ID
                html += ":"
                html += fic(node_idx).ChapterCount
                html += "</id>"
                html += "<summary type=""html"">"
                html += HtmlEncode(fic(node_idx).Summary)
                html += "</summary>"
                html += "</entry>"

            Next

            html += "</feed>"

            doc = CleanHTML(html)

            html = doc.DocumentNode.OuterHtml

            xmldoc = New XmlDocument

            xmldoc.LoadXml(html)

        End If

        Return xmldoc

    End Function

    Public Overrides Function GrabData(ByVal url As String) As String

        Dim html As String
        Dim title As String = ""
        Dim doc As HtmlDocument
        Dim link As URL

        Dim rest As Boolean

        Dim target As String = ""
        Dim postData As String = ""


        Dim nodes As HtmlNodeCollection

        html = DownloadPage(url, "adultfanfiction_net.cookie")
        doc = CleanHTML(html)

        Try
            title = doc.DocumentNode.SelectSingleNode("//title").InnerText
        Catch
            title = ""
        End Try

        html = doc.DocumentNode.OuterHtml

        rest = CheckAge(url, title)

        If Not Me.AgeCheck Then
            html = "<AgeCheck value='False' />"
        Else
            If Not rest Then

                nodes = FindNodesByAttribute(doc.DocumentNode, "form", "action", "check.php")

                If nodes.Count > 0 Then

                    link = ExtractUrl(url)
                    target = link.Scheme & "://" & link.Host & "/check.php"

                    postData = "cmbmonth=" & CDate(Me.DOB).Month
                    postData += "&"
                    postData += "cmbday=" & CDate(Me.DOB).Day
                    postData += "&"
                    postData += "cmbyear=" & CDate(Me.DOB).Year
                    postData += "&"
                    postData += "cmbname=" & URLEncode(Me.FullName)

                    DownloadCookies(target, postData)


                End If

            End If

        End If

        Return html

    End Function

    Function CheckAge(ByVal url As String, ByVal title As String) As Boolean

        Dim ok As Boolean = True

        Dim ret As MsgBoxResult
        Dim msg As String

        Dim dob As String = ""
        Dim tsDOB As TimeSpan

        Dim name As String = ""

        Dim link As URL
        link = ExtractUrl(url)

        msg = url
        msg += vbCrLf
        msg += vbCrLf
        msg += "Warning: This site requires age verification using e-signing"
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

    Public Overrides Function GrabStoryInfo(ByRef dsRSS As System.Data.DataSet, ByVal idx As Integer) As Fanfic.Story

        Dim fic As New Fanfic.Story

        Dim temp() As String

        ' Story Name
        fic.Title = dsRSS.Tables("entry"). _
                        Rows(idx).Item("title")

        ' Story Author
        fic.Author = dsRSS.Tables("author"). _
                          Rows(idx).Item(0)
        ' Story Location
        fic.URL = dsRSS.Tables("link"). _
                          Rows(idx).Item(1)

        fic.ID = Me.GetStoryID(fic.URL)

        fic.PublishDate = CDate(dsRSS.Tables("entry").Rows(idx).Item("published"))

        fic.UpdateDate = CDate(dsRSS.Tables("entry").Rows(idx).Item("updated"))

        temp = Split(dsRSS.Tables("entry").Rows(idx).Item("id"), ":")

        fic.Category = temp(0)
        fic.ChapterCount = temp(2)

        fic.Summary = dsRSS.Tables("summary").Rows(idx).Item("summary_Text")

        Return fic

    End Function

#End Region

#Region "Retrieve Navigation Info"

    Public Overrides Function GetAuthorURL(ByVal link As String) As String

    End Function

    Public Overrides Function GetStoryID(ByVal link As String) As String

        Dim ret As String = ""
        Dim url As URL

        url = ExtractUrl(link)

        ret = Split(url.Host, ".")(0)
        ret += ":"
        ret += Split(url.Query, "=")(1)

        Return ret

    End Function

    Public Overrides Function GetStoryURL(ByVal id As String) As String

    End Function

#End Region

#Region "HTML Processing"

    Public Overrides Function GrabAuthor(ByVal htmlDoc As String) As String

    End Function

    Public Overrides Function GrabBody(ByVal htmldoc As String) As String

    End Function

    Public Overrides Function GrabDate(ByVal htmlDoc As String, ByVal title As String) As String

    End Function

    Public Overrides Function GrabSeries(ByVal htmlDoc As String) As String

    End Function

    Public Overrides Function GrabTitle(ByVal htmlDoc As String) As String

    End Function

#End Region

#Region "Chapter Navigation"

    Public Overrides Function ProcessChapters(ByVal URL As String, ByVal list As System.Windows.Forms.ListBox, ByVal index As Integer) As String

    End Function

#End Region

#Region "Properties"

    Public Overrides ReadOnly Property HostName() As String
        Get
            Return "adultfanfiction.net"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Adult FanFiction"
        End Get
    End Property

    Public Overrides ReadOnly Property ErrorMessage() As String
        Get
            Return "The member you are looking for dose not exsist."
        End Get
    End Property

#End Region

End Class
