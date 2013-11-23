Imports System.IO
Imports System.Web
Imports System.Text

Public Class clsBL

    Private cls As clsFanfic

    Private txtResult As String

    Dim Story As New clsFanfic.Story

    Public Function GetChapters(ByVal link As String) As Boolean

        Dim htmldoc As String

        'Download Information from Source

        If link = "" Then
            Return False
        End If

        htmldoc = cls.GrabData(link)
        txtResult = htmldoc

        If InStr(LCase(txtResult), cls.ErrorMessage) = 0 Then

            If txtResult = "" Then
                Return False
            End If


            Story.Chapters = cls.GetChapters(htmldoc)



            htmldoc = cls.ProcessChapters( _
                                           link, _
                                           0 _
                                         )

            Story.StoryURL = link

            Story.ID = cls.GetStoryID(link)

            Story.Title = cls.GrabTitle(htmldoc)
            Story.Author = cls.GrabAuthor(htmldoc)

            Story.PublishDate = cls.GrabDate(htmldoc, "Published: ")
            Story.UpdateDate = cls.GrabDate(htmldoc, "Updated: ")

            If Story.UpdateDate = "" Then Story.UpdateDate = Story.PublishDate

            Story.ChapterCount = UBound(Story.Chapters)

            txtResult = cls.GrabBody(htmldoc)

            Return True

        Else
            Return False
        End If

    End Function

    Public Sub ProcessChapters( _
                                ByVal link As String, _
                                ByVal Start As Integer, _
                                ByVal Count As Integer, _
                                ByVal FileMask As String, _
                                Optional ByVal Category As String = "" _
                              )

        Dim htmldoc As String
        Dim idx As Integer
        Dim msg As String

        'Process Chapters from Source

        For idx = (Start - 1) To (Count - 1)

            msg = "Chapter " & _
                  (idx + 1) & _
                  " of " & _
                  Count

            frmMain.UpdateProgess(msg)

            htmldoc = cls.ProcessChapters( _
                                           link, _
                                           idx _
                                         )

            ProcessChapter( _
                            htmldoc, _
                            FileMask, _
                            idx, _
                            Count, _
                            link
                          )

        Next

    End Sub

    Public Sub ProcessChapter( _
                              ByRef htmlDoc As String, _
                              ByVal FileMask As String, _
                              ByVal chapter As Integer, _
                              Optional ByVal ChapterCount As Integer = 0, _
                              Optional ByVal link As String = "", _
                              Optional ByVal Category As String = ""
                            )

        Dim ecp1252 As Encoding = Encoding.GetEncoding(1252)
        Dim sr As StreamReader
        Dim sw As StreamWriter

        Dim Body As String
        Dim data As String = ""

        If ChapterCount = 0 Then
            ChapterCount = chapter
        End If

        Story.Title = cls.GrabTitle(htmlDoc)
        Story.Author = cls.GrabAuthor(htmlDoc)
        Story.Category = cls.GrabSeries(htmlDoc)
        Story.PublishDate = cls.GrabDate(htmlDoc, "Published: ")
        Story.UpdateDate = cls.GrabDate(htmlDoc, "Updated: ")

        If Story.UpdateDate = "" Then Story.UpdateDate = Story.PublishDate

        txtResult = htmlDoc

        If InStr(LCase(txtResult), "chapter not found") = 0 _
        And InStr(LCase(txtResult), "story not found") = 0 _
        Then

            data = cls.GrabBody(htmlDoc)

            Body = "<p></p>" & data

            txtResult = "<html><body>"
            txtResult += "<p>" & Story.Title & "</p>"
            txtResult += "<p>" & Story.Author & "</p>"

            If Story.Category <> "" Then
                txtResult += "<p>" & Story.Category & "</p>"
            End If

            txtResult += cls.WriteDate( _
                                        Story.PublishDate, _
                                        Story.UpdateDate, _
                                        chapter, _
                                        ChapterCount _
                                      )
            txtResult += "<p>----------------------------------</p>"
            txtResult += Body
        Else
            txtResult = "<p>Error writing file</p>"
            txtResult += "<p>Try downloading the file from " & _
                        "<a href=" & Chr(34) & link & (chapter + 1) & "/" & _
                        Chr(34) & ">here</a> in a regular browser</p>"
            txtResult += "<p>DOWNLOAD ERROR</p>"
        End If

        txtResult = HttpUtility.HtmlDecode(txtResult)

        sr = New StreamReader(StringToStream(txtResult))

        FileMask = Replace(FileMask, "-", "")

        If IsNumeric(Mid(FileMask, Len(FileMask), 1)) Then
            FileMask += "-"
        End If

        sw = New StreamWriter(Environment.GetFolderPath( _
                               Environment.SpecialFolder.Desktop) _
                               & "\" & FileMask & _
                               Format(chapter, "0#") & ".htm", _
                               False, ecp1252)
        sw.Write(sr.ReadToEnd)
        sr.Close()
        sw.Close()
        sr = Nothing
        sw = Nothing

    End Sub

#Region "Pass-Thru Functions"

    Public Function GrabStoryInfo(ByVal idx As Integer) As clsFanfic.Story

        Dim fic As clsFanfic.Story

        fic = cls.GrabStoryInfo(idx)

        Return fic

    End Function

    Public Function GrabFeed(link As String) As System.Data.DataSet

        Dim ds As DataSet

        ds = cls.GrabFeed(link)

        Return ds

    End Function

    Public Function GetStoryID(ByVal link As String) As String

        Dim id As String

        id = cls.GetStoryID(link)

        Return id

    End Function

    Public Function GetStoryInfoByID(ByVal StoryID As String) As clsFanfic.Story

        Dim fic As clsFanfic.Story

        fic = cls.GetStoryInfoByID(StoryID)

        Return fic

    End Function

    Public Function GetStoryURL(ByVal id As String) As String

        Dim link As String

        link = cls.GetStoryURL(id)

        Return link

    End Function

#End Region

#Region "Validation Routines"

    Function CheckUrl(ByRef link As String) As Boolean

        Dim host As String
        Dim ret As Boolean = False

        Dim url As URL
        url = ExtractUrl(link)

        host = url.Host

        If UBound(Split(host, ".")) = 2 Then
            host = Mid(host, InStr(host, ".") + 1)
        End If

        LoadSiteByHost(host)

        If IsNothing(cls) Then
            ret = False
        End If

        Return ret

    End Function

#End Region

#Region "Site Module Loading"

    Function LoadSiteByHost(ByVal host As String) As Boolean

        Dim ret As Boolean = False

        If Not IsNothing(cls) Then
            If host = cls.HostName Then
                Return True
            Else
                cls = Nothing
            End If
        End If

        Select Case host
            Case "fanfiction.net"
                cls = New FFNet
            Case "adultfanfiction.net"
                cls = New AFF
            Case "ficwad.com"
                cls = New FicWad
            Case "mediaminer.org"
                cls = New MM
            Case Else
                cls = Nothing
        End Select

        If Not IsNothing(cls) Then
            ret = True
        End If

        Return ret

    End Function

#End Region

#Region "Properties"

    Public ReadOnly Property Result As String
        Get
            Return txtResult
        End Get
    End Property

    Public ReadOnly Property FanFic As clsFanfic.Story
        Get
            Return Story
        End Get
    End Property

    Public ReadOnly Property Host As String
        Get
            If Not IsNothing(cls) Then
                Return cls.HostName
            End If
        End Get
    End Property

    Public ReadOnly Property Name As String
        Get
            If Not IsNothing(cls) Then
                Return cls.Name
            Else
                Return ""
            End If
        End Get
    End Property

    Public ReadOnly Property ErrorMessage As String
        Get
            If Not IsNothing(cls) Then
                Return cls.ErrorMessage
            Else
                Return ""
            End If
        End Get
    End Property

#End Region

End Class
