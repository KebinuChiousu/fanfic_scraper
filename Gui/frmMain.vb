Imports System.IO

Imports System.Xml

Imports System.data
Imports System.Data.OleDb


Public Class HtmlGrabber
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents btnURL As System.Windows.Forms.Button
    Friend WithEvents txtUrl As System.Windows.Forms.TextBox
    Friend WithEvents txtSource As System.Windows.Forms.TextBox
    Friend WithEvents ListChapters As System.Windows.Forms.ListBox
    Friend WithEvents lblPublish As System.Windows.Forms.Label
    Friend WithEvents lblUpdate As System.Windows.Forms.Label
    Friend WithEvents txtFileMask As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblChapterCount As System.Windows.Forms.Label
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents txtStart As System.Windows.Forms.TextBox
    Friend WithEvents btnRSS As System.Windows.Forms.Button
    Friend WithEvents urlAtom As System.Windows.Forms.TextBox
    Friend WithEvents cmbStory As System.Windows.Forms.ComboBox
    Friend WithEvents lstStory As System.Windows.Forms.ListBox
    Friend WithEvents cmbType As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblStory As System.Windows.Forms.Label
    Friend WithEvents lblStart As System.Windows.Forms.Label
    Friend WithEvents lblAnime As System.Windows.Forms.Label
    Friend WithEvents btnDebug As System.Windows.Forms.Button
    Friend WithEvents lblAuthor As System.Windows.Forms.TextBox
    Friend WithEvents lblStoryID As System.Windows.Forms.TextBox
    Friend WithEvents lblAtom As System.Windows.Forms.Label
    Friend WithEvents btnHtml As System.Windows.Forms.Button
    Friend WithEvents lblTitle As System.Windows.Forms.TextBox





    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnURL = New System.Windows.Forms.Button()
        Me.txtUrl = New System.Windows.Forms.TextBox()
        Me.txtSource = New System.Windows.Forms.TextBox()
        Me.ListChapters = New System.Windows.Forms.ListBox()
        Me.lblPublish = New System.Windows.Forms.Label()
        Me.lblUpdate = New System.Windows.Forms.Label()
        Me.txtFileMask = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblChapterCount = New System.Windows.Forms.Label()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.lblStart = New System.Windows.Forms.Label()
        Me.txtStart = New System.Windows.Forms.TextBox()
        Me.btnRSS = New System.Windows.Forms.Button()
        Me.urlAtom = New System.Windows.Forms.TextBox()
        Me.lblAtom = New System.Windows.Forms.Label()
        Me.cmbStory = New System.Windows.Forms.ComboBox()
        Me.lblStory = New System.Windows.Forms.Label()
        Me.lstStory = New System.Windows.Forms.ListBox()
        Me.cmbType = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblAnime = New System.Windows.Forms.Label()
        Me.btnDebug = New System.Windows.Forms.Button()
        Me.lblTitle = New System.Windows.Forms.TextBox()
        Me.lblAuthor = New System.Windows.Forms.TextBox()
        Me.lblStoryID = New System.Windows.Forms.TextBox()
        Me.btnHtml = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnURL
        '
        Me.btnURL.Location = New System.Drawing.Point(31, 367)
        Me.btnURL.Name = "btnURL"
        Me.btnURL.Size = New System.Drawing.Size(160, 48)
        Me.btnURL.TabIndex = 1
        Me.btnURL.Text = "Get Chapters"
        '
        'txtUrl
        '
        Me.txtUrl.Location = New System.Drawing.Point(31, 456)
        Me.txtUrl.Name = "txtUrl"
        Me.txtUrl.Size = New System.Drawing.Size(312, 22)
        Me.txtUrl.TabIndex = 2
        '
        'txtSource
        '
        Me.txtSource.Location = New System.Drawing.Point(32, 128)
        Me.txtSource.MaxLength = 9999999
        Me.txtSource.Multiline = True
        Me.txtSource.Name = "txtSource"
        Me.txtSource.ReadOnly = True
        Me.txtSource.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtSource.Size = New System.Drawing.Size(800, 218)
        Me.txtSource.TabIndex = 3
        '
        'ListChapters
        '
        Me.ListChapters.ItemHeight = 16
        Me.ListChapters.Location = New System.Drawing.Point(192, 303)
        Me.ListChapters.Name = "ListChapters"
        Me.ListChapters.Size = New System.Drawing.Size(464, 4)
        Me.ListChapters.TabIndex = 5
        Me.ListChapters.Visible = False
        '
        'lblPublish
        '
        Me.lblPublish.Location = New System.Drawing.Point(632, 40)
        Me.lblPublish.Name = "lblPublish"
        Me.lblPublish.Size = New System.Drawing.Size(200, 32)
        Me.lblPublish.TabIndex = 6
        Me.lblPublish.Text = "Published Date"
        Me.lblPublish.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblUpdate
        '
        Me.lblUpdate.Location = New System.Drawing.Point(632, 72)
        Me.lblUpdate.Name = "lblUpdate"
        Me.lblUpdate.Size = New System.Drawing.Size(200, 32)
        Me.lblUpdate.TabIndex = 7
        Me.lblUpdate.Text = "Updated Date"
        Me.lblUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtFileMask
        '
        Me.txtFileMask.Location = New System.Drawing.Point(208, 391)
        Me.txtFileMask.Name = "txtFileMask"
        Me.txtFileMask.Size = New System.Drawing.Size(135, 22)
        Me.txtFileMask.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(215, 367)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 24)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "File Prefix"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(31, 440)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(153, 16)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Fanfiction Story Url"
        '
        'lblChapterCount
        '
        Me.lblChapterCount.Location = New System.Drawing.Point(32, 88)
        Me.lblChapterCount.Name = "lblChapterCount"
        Me.lblChapterCount.Size = New System.Drawing.Size(112, 25)
        Me.lblChapterCount.TabIndex = 13
        '
        'lblProgress
        '
        Me.lblProgress.Location = New System.Drawing.Point(352, 80)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(280, 40)
        Me.lblProgress.TabIndex = 14
        Me.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblStart
        '
        Me.lblStart.Location = New System.Drawing.Point(160, 88)
        Me.lblStart.Name = "lblStart"
        Me.lblStart.Size = New System.Drawing.Size(112, 25)
        Me.lblStart.TabIndex = 15
        Me.lblStart.Text = "Start @ Chapter: "
        Me.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblStart.Visible = False
        '
        'txtStart
        '
        Me.txtStart.Location = New System.Drawing.Point(272, 88)
        Me.txtStart.Name = "txtStart"
        Me.txtStart.Size = New System.Drawing.Size(72, 22)
        Me.txtStart.TabIndex = 16
        Me.txtStart.Visible = False
        '
        'btnRSS
        '
        Me.btnRSS.Location = New System.Drawing.Point(391, 367)
        Me.btnRSS.Name = "btnRSS"
        Me.btnRSS.Size = New System.Drawing.Size(105, 47)
        Me.btnRSS.TabIndex = 17
        Me.btnRSS.Text = "Obtain Feed"
        '
        'urlAtom
        '
        Me.urlAtom.Location = New System.Drawing.Point(511, 391)
        Me.urlAtom.Name = "urlAtom"
        Me.urlAtom.Size = New System.Drawing.Size(312, 22)
        Me.urlAtom.TabIndex = 18
        '
        'lblAtom
        '
        Me.lblAtom.Location = New System.Drawing.Point(511, 367)
        Me.lblAtom.Name = "lblAtom"
        Me.lblAtom.Size = New System.Drawing.Size(312, 21)
        Me.lblAtom.TabIndex = 19
        Me.lblAtom.Text = "Atom Feed or Author URL"
        '
        'cmbStory
        '
        Me.cmbStory.Location = New System.Drawing.Point(391, 456)
        Me.cmbStory.Name = "cmbStory"
        Me.cmbStory.Size = New System.Drawing.Size(432, 24)
        Me.cmbStory.TabIndex = 21
        '
        'lblStory
        '
        Me.lblStory.Location = New System.Drawing.Point(391, 440)
        Me.lblStory.Name = "lblStory"
        Me.lblStory.Size = New System.Drawing.Size(153, 16)
        Me.lblStory.TabIndex = 22
        Me.lblStory.Text = "Fanfiction Story List"
        '
        'lstStory
        '
        Me.lstStory.ItemHeight = 16
        Me.lstStory.Location = New System.Drawing.Point(192, 152)
        Me.lstStory.Name = "lstStory"
        Me.lstStory.Size = New System.Drawing.Size(464, 4)
        Me.lstStory.TabIndex = 23
        Me.lstStory.Visible = False
        '
        'cmbType
        '
        Me.cmbType.Items.AddRange(New Object() {"FFNet", "AFF", "FicWad", "MediaMiner", "HPFFA"})
        Me.cmbType.Location = New System.Drawing.Point(440, 8)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.Size = New System.Drawing.Size(152, 24)
        Me.cmbType.TabIndex = 24
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(336, 8)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(136, 24)
        Me.Label6.TabIndex = 25
        Me.Label6.Text = "Download Site:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblAnime
        '
        Me.lblAnime.Location = New System.Drawing.Point(104, 224)
        Me.lblAnime.Name = "lblAnime"
        Me.lblAnime.Size = New System.Drawing.Size(640, 32)
        Me.lblAnime.TabIndex = 27
        Me.lblAnime.Visible = False
        '
        'btnDebug
        '
        Me.btnDebug.Location = New System.Drawing.Point(4, 1)
        Me.btnDebug.Name = "btnDebug"
        Me.btnDebug.Size = New System.Drawing.Size(48, 28)
        Me.btnDebug.TabIndex = 28
        Me.btnDebug.Text = "Auto"
        '
        'lblTitle
        '
        Me.lblTitle.Location = New System.Drawing.Point(29, 46)
        Me.lblTitle.MaxLength = 99
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.ReadOnly = True
        Me.lblTitle.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.lblTitle.Size = New System.Drawing.Size(355, 22)
        Me.lblTitle.TabIndex = 31
        '
        'lblAuthor
        '
        Me.lblAuthor.Location = New System.Drawing.Point(403, 46)
        Me.lblAuthor.MaxLength = 99
        Me.lblAuthor.Name = "lblAuthor"
        Me.lblAuthor.ReadOnly = True
        Me.lblAuthor.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.lblAuthor.Size = New System.Drawing.Size(201, 22)
        Me.lblAuthor.TabIndex = 32
        '
        'lblStoryID
        '
        Me.lblStoryID.Location = New System.Drawing.Point(692, 9)
        Me.lblStoryID.MaxLength = 99
        Me.lblStoryID.Name = "lblStoryID"
        Me.lblStoryID.ReadOnly = True
        Me.lblStoryID.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.lblStoryID.Size = New System.Drawing.Size(140, 22)
        Me.lblStoryID.TabIndex = 33
        '
        'btnHtml
        '
        Me.btnHtml.Location = New System.Drawing.Point(53, 1)
        Me.btnHtml.Name = "btnHtml"
        Me.btnHtml.Size = New System.Drawing.Size(48, 28)
        Me.btnHtml.TabIndex = 34
        Me.btnHtml.Text = "Html"
        '
        'HtmlGrabber
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(843, 496)
        Me.Controls.Add(Me.btnHtml)
        Me.Controls.Add(Me.lblStoryID)
        Me.Controls.Add(Me.lblAuthor)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.urlAtom)
        Me.Controls.Add(Me.txtStart)
        Me.Controls.Add(Me.txtFileMask)
        Me.Controls.Add(Me.txtUrl)
        Me.Controls.Add(Me.txtSource)
        Me.Controls.Add(Me.btnDebug)
        Me.Controls.Add(Me.lblAnime)
        Me.Controls.Add(Me.lblStory)
        Me.Controls.Add(Me.cmbType)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.lstStory)
        Me.Controls.Add(Me.cmbStory)
        Me.Controls.Add(Me.lblAtom)
        Me.Controls.Add(Me.btnRSS)
        Me.Controls.Add(Me.lblStart)
        Me.Controls.Add(Me.lblProgress)
        Me.Controls.Add(Me.lblChapterCount)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblUpdate)
        Me.Controls.Add(Me.lblPublish)
        Me.Controls.Add(Me.ListChapters)
        Me.Controls.Add(Me.btnURL)
        Me.Name = "HtmlGrabber"
        Me.Text = "Fanfiction.net Story Grabber"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    'Declarations used within class
    Public clsname As String

    Dim Title As String
    Dim dsRSS As DataSet

    Dim i As Integer

#Region "UI Update Code"

    Sub ClearUI()

        lblTitle.Text = ""
        lblAuthor.Text = ""
        lblPublish.Text = ""
        lblUpdate.Text = ""
        lblChapterCount.Text = ""
        txtStart.Visible = False
        lblStart.Visible = False
        lblProgress.Text = ""
        txtSource.Text = ""
        txtFileMask.Text = ""

        btnURL.Text = "Get Chapters"
        txtUrl.Text = ""

        ListChapters.Items.Clear()
        lstStory.Items.Clear()
        cmbStory.Items.Clear()
        cmbStory.Text = ""
        urlAtom.Text = ""

        Application.DoEvents()

    End Sub

    Sub UpdateUI(ByRef fic As clsFanfic.Story, ByRef Result As String, Optional ByVal Start As Integer = 1)

        Dim idx As Integer

        btnURL.Text = "Process Chapters"

        lblChapterCount.Text = fic.ChapterCount

        For idx = 0 To UBound(fic.Chapters)
            ListChapters.Items.Add(fic.Chapters(idx))
        Next

        lblStoryID.Text = fic.ID
        lblTitle.Text = fic.Title
        lblAuthor.Text = fic.Author
        lblPublish.Text = fic.PublishDate
        lblUpdate.Text = fic.UpdateDate

        lblChapterCount.Text = fic.ChapterCount
        lblProgress.Text = "< -- Enter Starting Chapter"
        txtStart.Text = Start

        txtSource.Text = Result

        lblStart.Visible = True
        txtStart.Visible = True

        Application.DoEvents()

    End Sub

#End Region

#Region "Interface Code"

    Private Sub btnURL_Click( _
                              ByVal sender As System.Object, _
                              ByVal e As System.EventArgs _
                            ) Handles btnURL.Click

        Dim link As String
        link = txtUrl.Text

        If CheckURL(link) Then
            DownloadData()
        Else
            MsgBox("Site: " & link & " is currently not supported.", MsgBoxStyle.Information)
        End If

    End Sub

    Private Sub btnRSS_Click( _
                              ByVal sender As System.Object, _
                              ByVal e As System.EventArgs _
                            ) Handles btnRSS.Click

        Dim link As String
        link = urlAtom.Text

        If CheckURL(link) Then
            ObtainFeed(urlAtom.Text)
        Else
            MsgBox("Site: " & link & " is currently not supported.", MsgBoxStyle.Information)
        End If

    End Sub

    Private Sub Story_Selected( _
                                ByVal sender As System.Object, _
                                ByVal e As System.EventArgs _
                              ) Handles cmbStory.SelectedIndexChanged

        LoadStoryInfo( _
                       cmbStory.SelectedIndex _
                     )
    End Sub

    Private Sub Source_Changed( _
                                ByVal sender As System.Object, _
                                ByVal e As System.EventArgs _
                              ) Handles cmbType.SelectedIndexChanged

        clsname = cmbType.Items(cmbType.SelectedIndex)

        ClearUI()

        LoadSiteByName(clsname)

    End Sub

    Private Sub HtmlGrabber_Load( _
                                  ByVal sender As System.Object, _
                                  ByVal e As System.EventArgs _
                                ) Handles MyBase.Load

        cmbType.SelectedIndex = 0
        frmMain = Me

    End Sub

    Private Sub HtmlGrabber_Closing( _
                                     ByVal sender As Object, _
                                     ByVal e As System.ComponentModel. _
                                                CancelEventArgs _
                                   ) Handles MyBase.Closing
        Application.Exit()
    End Sub

    Private Sub ShowDebugWindow( _
                                 ByVal sender As System.Object, _
                                 ByVal e As System.EventArgs _
                               ) Handles btnDebug.Click

        Initialize( _
                    forms.frmDebug _
                  )

        frmDebug.myCaller = Me

        PlaceDebugWindow()
        frmDebug.Show()

    End Sub

    Private Sub btnHtml_Click( _
                                   ByVal sender As System.Object, _
                                   ByVal e As System.EventArgs _
                                 ) Handles btnHtml.Click

        Initialize( _
                    forms.frmHtml _
                  )

        PlaceHtmlWindow()
        frmHtml.Show()

    End Sub

#End Region

#Region "Business Logic"




    Sub LoadStoryInfo(ByVal idx As Integer)

        btnURL.Text = "Get Chapters"

        Dim fic As clsFanfic.Story

        fic = BL.GrabStoryInfo(idx)

        ' Story Name
        lblTitle.Text = fic.Title

        ' Story Author
        lblAuthor.Text = fic.Author

        ' Story Location
        txtUrl.Text = fic.StoryURL

        'Story ID
        lblStoryID.Text = fic.ID

        'Category
        lblProgress.Text = fic.Category
        lblAnime.Text = lblProgress.Text

        'Chapter Count
        lblChapterCount.Text = fic.ChapterCount

        'Last Updated
        lblUpdate.Text = fic.UpdateDate

        'Published
        lblPublish.Text = fic.PublishDate

        'Summary

        txtSource.Text = fic.Summary

    End Sub

    Sub DownloadData()

        Dim ret As Boolean
        Dim fic As clsFanfic.Story

        Select Case btnURL.Text
            Case "Get Chapters"
                ret = BL.GetChapters(txtUrl.Text)
                If Not ret Then GoTo oops

                fic = BL.FanFic

                UpdateUI(fic, BL.Result)

            Case "Process Chapters"

                BL.ProcessChapters( _
                                    txtUrl.Text, _
                                    CInt(txtStart.Text), _
                                    CInt(lblChapterCount.Text), _
                                    txtFileMask.Text _
                                  )

                ClearUI()

        End Select

        Exit Sub

oops:
        txtSource.Text = BL.Result
        MsgBox("Valid URL Must be Entered", MsgBoxStyle.Information)

    End Sub

    Sub UpdateProgess(ByVal msg As String)

        lblProgress.Text = msg
        Application.DoEvents()

    End Sub

    Sub ObtainFeed(ByVal link As String)
        'Download List of stories for Given Author

        Dim idx As Integer
        'Dim sumcount As Integer

        Dim fic As clsFanfic.Story

        txtSource.Text = ""
        cmbStory.Items.Clear()
        lstStory.Items.Clear()

        dsRSS = Nothing

        If link = "" Then GoTo abort

        dsRSS = BL.GrabFeed(link)

        If IsNothing(dsRSS) Then GoTo abort

        urlAtom.Text = link

        btnURL.Text = "Get Chapters"
        ListChapters.Items.Clear()

        'Send Information to Debug Console
        Initialize( _
                    forms.frmDebug _
                  )

        frmDebug.UpdateRSS(dsRSS)

        For idx = 0 To dsRSS.Tables(0).Rows.Count - 1

            fic = BL.GrabStoryInfo(idx)

            ' Story Title
            cmbStory.Items.Add(fic.Title)

            ' Story Location
            lstStory.Items.Add(fic.StoryURL)

        Next

        'Load Info for First Story
        cmbStory.SelectedIndex = 0

        Exit Sub

abort:
        urlAtom.Text = ""
        MsgBox("Valid Feed Must Be Entered.", _
               MsgBoxStyle.Information, _
               Me.Text)
        Exit Sub

    End Sub

#Region "Site Logic"

    Function CheckURL(ByVal link As String) As Boolean

        Dim ret As Boolean

        ret = BL.CheckUrl(link)

        If ret Then
            LoadSiteByName(BL.Name, True)
        Else
            ret = LoadSiteByName(cmbType.Text, False)
        End If

        Return ret

    End Function

    Function LoadSiteByName(ByVal clsname As String, Optional ByVal bypass As Boolean = False) As Boolean

        Dim ret As Boolean = False
        Dim host As String = ""

        ret = BL.LoadSiteByName(clsname)

        If ret Then
            Me.Text = cls.HostName & " - Story Downloader"
            lblAtom.Text = "Valid Author URL"
        Else
            Me.Text = "Story Downloader"
        End If

        Return ret

    End Function

#End Region

#End Region

End Class
