Imports System
Imports System.Text
Imports System.Net
Imports System.Xml
Imports HtmlAgilityPack
Imports System.Web.HttpUtility


Class YFF
    Inherits Fanfic

#Region "Downloading HTML"

    Public Overrides Function GrabData(ByVal url As String) As String

        Dim html As String
        Dim doc As HtmlDocument


        html = DownloadPage(url)
        doc = CleanHTML(html)

        html = doc.DocumentNode.OuterHtml

        doc = Nothing

        Return html

    End Function

#End Region

#Region "RSS"

    Public Overrides Function GrabFeed(ByRef rss As String) As System.Xml.XmlDocument

    End Function

    Public Overrides Function GrabStoryInfo(ByRef dsRSS As System.Data.DataSet, ByVal idx As Integer) As Fanfic.Story

    End Function

#End Region

#Region "Chapter Navigation"

    Public Overrides Sub GetChapters(ByVal lst As System.Windows.Forms.ListBox, ByVal htmlDoc As String)

    End Sub

    Public Overrides Function ProcessChapters(ByVal link As String, ByVal index As Integer) As String

    End Function

    Public Overrides Function GetAuthorURL(ByVal link As String) As String

    End Function

    Public Overrides Function GetStoryID(ByVal link As String) As String

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

#Region "Properties"

    Public Overrides ReadOnly Property HostName() As String
        Get
            Return "yourfanfiction.com"
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

End Class