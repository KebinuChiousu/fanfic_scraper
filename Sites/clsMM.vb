Imports System
Imports System.text
Imports System.net
Imports System.IO
Imports System.Xml
Imports HtmlAgilityPack
Imports System.Web.HttpUtility

Public Class MM
    Inherits Fanfic

    Private Browser As clsWeb

    Public Overrides Function GetAuthorURL(ByVal link As String) As String

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

    Public Overrides Function GrabAuthor(ByVal htmlDoc As String) As String

    End Function

    Public Overrides Function GrabBody(ByVal htmldoc As String) As String

    End Function

    Public Overrides Function GrabDate(ByVal htmlDoc As String, ByVal title As String) As String

    End Function

    Public Overrides Function GrabData(ByVal url As String) As String

        Dim htmldoc As String

        htmldoc = MyBase.GrabData(url)

        Return htmldoc

    End Function

    Public Overrides Function GrabFeed(ByRef rss As String) As System.Xml.XmlDocument

        Dim html As String
        Dim doc As HtmlDocument = Nothing

        Dim nodes As HtmlNodeCollection
        Dim temp As HtmlNodeCollection

        Dim dummy As String

        Dim title As String
        Dim link As String
        Dim url As URL

        Dim node_idx As Integer

        Dim author As String
        Dim author_url As String

        Dim summary() As String
        Dim fic() As Fanfic.Story

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
            temp = doc.DocumentNode.SelectNodes("//option")

            If IsNothing(temp) Then
                fic(node_idx).ChapterCount = 1
                fic(node_idx).PublishDate = fic(node_idx).UpdateDate
            Else

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

    Public Overrides Function GrabSeries(ByVal htmlDoc As String) As String

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

    Public Overrides Function GrabTitle(ByVal htmlDoc As String) As String

    End Function

    

    Public Overrides Function ProcessChapters(ByVal link As String, ByVal index As Integer) As String

    End Function

#Region "Properties"

    Public Overrides ReadOnly Property ErrorMessage() As String
        Get

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
