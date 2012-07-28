Imports System.IO
Imports System.Web
Imports System.Text
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
    Friend WithEvents lblTitle As System.Windows.Forms.TextBox





    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnURL = New System.Windows.Forms.Button
        Me.txtUrl = New System.Windows.Forms.TextBox
        Me.txtSource = New System.Windows.Forms.TextBox
        Me.ListChapters = New System.Windows.Forms.ListBox
        Me.lblPublish = New System.Windows.Forms.Label
        Me.lblUpdate = New System.Windows.Forms.Label
        Me.txtFileMask = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblChapterCount = New System.Windows.Forms.Label
        Me.lblProgress = New System.Windows.Forms.Label
        Me.lblStart = New System.Windows.Forms.Label
        Me.txtStart = New System.Windows.Forms.TextBox
        Me.btnRSS = New System.Windows.Forms.Button
        Me.urlAtom = New System.Windows.Forms.TextBox
        Me.lblAtom = New System.Windows.Forms.Label
        Me.cmbStory = New System.Windows.Forms.ComboBox
        Me.lblStory = New System.Windows.Forms.Label
        Me.lstStory = New System.Windows.Forms.ListBox
        Me.cmbType = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.lblAnime = New System.Windows.Forms.Label
        Me.btnDebug = New System.Windows.Forms.Button
        Me.lblTitle = New System.Windows.Forms.TextBox
        Me.lblAuthor = New System.Windows.Forms.TextBox
        Me.lblStoryID = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'btnURL
        '
        Me.btnURL.Location = New System.Drawing.Point(26, 318)
        Me.btnURL.Name = "btnURL"
        Me.btnURL.Size = New System.Drawing.Size(133, 42)
        Me.btnURL.TabIndex = 1
        Me.btnURL.Text = "Get Chapters"
        '
        'txtUrl
        '
        Me.txtUrl.Location = New System.Drawing.Point(26, 395)
        Me.txtUrl.Name = "txtUrl"
        Me.txtUrl.Size = New System.Drawing.Size(260, 20)
        Me.txtUrl.TabIndex = 2
        '
        'txtSource
        '
        Me.txtSource.Location = New System.Drawing.Point(27, 111)
        Me.txtSource.MaxLength = 9999999
        Me.txtSource.Multiline = True
        Me.txtSource.Name = "txtSource"
        Me.txtSource.ReadOnly = True
        Me.txtSource.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtSource.Size = New System.Drawing.Size(666, 189)
        Me.txtSource.TabIndex = 3
        '
        'ListChapters
        '
        Me.ListChapters.Location = New System.Drawing.Point(160, 263)
        Me.ListChapters.Name = "ListChapters"
        Me.ListChapters.Size = New System.Drawing.Size(387, 4)
        Me.ListChapters.TabIndex = 5
        Me.ListChapters.Visible = False
        '
        'lblPublish
        '
        Me.lblPublish.Location = New System.Drawing.Point(527, 35)
        Me.lblPublish.Name = "lblPublish"
        Me.lblPublish.Size = New System.Drawing.Size(166, 27)
        Me.lblPublish.TabIndex = 6
        Me.lblPublish.Text = "Published Date"
        Me.lblPublish.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblUpdate
        '
        Me.lblUpdate.Location = New System.Drawing.Point(527, 62)
        Me.lblUpdate.Name = "lblUpdate"
        Me.lblUpdate.Size = New System.Drawing.Size(166, 28)
        Me.lblUpdate.TabIndex = 7
        Me.lblUpdate.Text = "Updated Date"
        Me.lblUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtFileMask
        '
        Me.txtFileMask.Location = New System.Drawing.Point(173, 339)
        Me.txtFileMask.Name = "txtFileMask"
        Me.txtFileMask.Size = New System.Drawing.Size(113, 20)
        Me.txtFileMask.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(179, 318)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 21)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "File Prefix"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(26, 381)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(127, 14)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Fanfiction Story Url"
        '
        'lblChapterCount
        '
        Me.lblChapterCount.Location = New System.Drawing.Point(27, 76)
        Me.lblChapterCount.Name = "lblChapterCount"
        Me.lblChapterCount.Size = New System.Drawing.Size(93, 22)
        Me.lblChapterCount.TabIndex = 13
        '
        'lblProgress
        '
        Me.lblProgress.Location = New System.Drawing.Point(293, 69)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(234, 35)
        Me.lblProgress.TabIndex = 14
        Me.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblStart
        '
        Me.lblStart.Location = New System.Drawing.Point(133, 76)
        Me.lblStart.Name = "lblStart"
        Me.lblStart.Size = New System.Drawing.Size(94, 22)
        Me.lblStart.TabIndex = 15
        Me.lblStart.Text = "Start @ Chapter: "
        Me.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblStart.Visible = False
        '
        'txtStart
        '
        Me.txtStart.Location = New System.Drawing.Point(227, 76)
        Me.txtStart.Name = "txtStart"
        Me.txtStart.Size = New System.Drawing.Size(60, 20)
        Me.txtStart.TabIndex = 16
        Me.txtStart.Visible = False
        '
        'btnRSS
        '
        Me.btnRSS.Location = New System.Drawing.Point(326, 318)
        Me.btnRSS.Name = "btnRSS"
        Me.btnRSS.Size = New System.Drawing.Size(87, 41)
        Me.btnRSS.TabIndex = 17
        Me.btnRSS.Text = "Obtain Feed"
        '
        'urlAtom
        '
        Me.urlAtom.Location = New System.Drawing.Point(426, 339)
        Me.urlAtom.Name = "urlAtom"
        Me.urlAtom.Size = New System.Drawing.Size(260, 20)
        Me.urlAtom.TabIndex = 18
        '
        'lblAtom
        '
        Me.lblAtom.Location = New System.Drawing.Point(426, 318)
        Me.lblAtom.Name = "lblAtom"
        Me.lblAtom.Size = New System.Drawing.Size(147, 13)
        Me.lblAtom.TabIndex = 19
        Me.lblAtom.Text = "Atom Feed or Author Name"
        '
        'cmbStory
        '
        Me.cmbStory.Location = New System.Drawing.Point(326, 395)
        Me.cmbStory.Name = "cmbStory"
        Me.cmbStory.Size = New System.Drawing.Size(360, 21)
        Me.cmbStory.TabIndex = 21
        '
        'lblStory
        '
        Me.lblStory.Location = New System.Drawing.Point(326, 381)
        Me.lblStory.Name = "lblStory"
        Me.lblStory.Size = New System.Drawing.Size(127, 14)
        Me.lblStory.TabIndex = 22
        Me.lblStory.Text = "Fanfiction Story List"
        '
        'lstStory
        '
        Me.lstStory.Location = New System.Drawing.Point(160, 132)
        Me.lstStory.Name = "lstStory"
        Me.lstStory.Size = New System.Drawing.Size(387, 4)
        Me.lstStory.TabIndex = 23
        Me.lstStory.Visible = False
        '
        'cmbType
        '
        Me.cmbType.Items.AddRange(New Object() {"FFNet", "MediaMiner"})
        Me.cmbType.Location = New System.Drawing.Point(367, 7)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.Size = New System.Drawing.Size(126, 21)
        Me.cmbType.TabIndex = 24
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(280, 7)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(113, 21)
        Me.Label6.TabIndex = 25
        Me.Label6.Text = "Download Site:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblAnime
        '
        Me.lblAnime.Location = New System.Drawing.Point(87, 194)
        Me.lblAnime.Name = "lblAnime"
        Me.lblAnime.Size = New System.Drawing.Size(533, 28)
        Me.lblAnime.TabIndex = 27
        Me.lblAnime.Visible = False
        '
        'btnDebug
        '
        Me.btnDebug.Location = New System.Drawing.Point(0, 0)
        Me.btnDebug.Name = "btnDebug"
        Me.btnDebug.Size = New System.Drawing.Size(40, 24)
        Me.btnDebug.TabIndex = 28
        Me.btnDebug.Text = "Auto"
        '
        'lblTitle
        '
        Me.lblTitle.Location = New System.Drawing.Point(24, 40)
        Me.lblTitle.MaxLength = 99
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.ReadOnly = True
        Me.lblTitle.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.lblTitle.Size = New System.Drawing.Size(296, 20)
        Me.lblTitle.TabIndex = 31
        '
        'lblAuthor
        '
        Me.lblAuthor.Location = New System.Drawing.Point(336, 40)
        Me.lblAuthor.MaxLength = 99
        Me.lblAuthor.Name = "lblAuthor"
        Me.lblAuthor.ReadOnly = True
        Me.lblAuthor.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.lblAuthor.Size = New System.Drawing.Size(167, 20)
        Me.lblAuthor.TabIndex = 32
        '
        'lblStoryID
        '
        Me.lblStoryID.Location = New System.Drawing.Point(600, 8)
        Me.lblStoryID.MaxLength = 99
        Me.lblStoryID.Name = "lblStoryID"
        Me.lblStoryID.ReadOnly = True
        Me.lblStoryID.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.lblStoryID.Size = New System.Drawing.Size(80, 20)
        Me.lblStoryID.TabIndex = 33
        '
        'HtmlGrabber
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(716, 430)
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
    Dim txtResult As String
    Dim Title As String
    Dim dsPubs As DataSet
    Dim cls As Object
    Dim clsname As String
    Dim i As Integer

#Region "Interface Code"

    Private Sub btnURL_Click( _
                              ByVal sender As System.Object, _
                              ByVal e As System.EventArgs _
                            ) Handles btnURL.Click
        DownloadData()
    End Sub

    Private Sub btnRSS_Click( _
                              ByVal sender As System.Object, _
                              ByVal e As System.EventArgs _
                            ) Handles btnRSS.Click
        ObtainFeed()
    End Sub

    Private Sub Story_Selected( _
                                ByVal sender As System.Object, _
                                ByVal e As System.EventArgs _
                              ) Handles cmbStory.SelectedIndexChanged

        LoadStoryInfo( _
                       cmbStory.SelectedIndex, _
                       cmbType.Items(cmbType.SelectedIndex) _
                     )
    End Sub

    Private Sub Source_Changed( _
                                ByVal sender As System.Object, _
                                ByVal e As System.EventArgs _
                              ) Handles cmbType.SelectedIndexChanged

        clsname = cmbType.Items(cmbType.SelectedIndex)

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

        cls = Nothing

        Select Case clsname
            Case "FFNet"
                Me.Text = "Fanfiction.net - Story Downloader"
                lblAtom.Text = "Atom Feed or Author Name"
                cls = New FFNet
            Case "MediaMiner"
                Me.Text = "MediaMiner.org - Story Grabber"
                lblAtom.Text = "Author Name"
                cls = New MediaMiner
        End Select

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

#End Region

    Sub LoadStoryInfo( _
                          ByVal rowcount As Integer, _
                          ByVal clsname As String _
                        )

        btnURL.Text = "Get Chapters"

        Dim txtSummary() As String
        Dim start As Long
        Dim index As Integer

        Select Case clsname

            Case "FFNet"

                ' Story Name
                lblTitle.Text = dsPubs.Tables("entry"). _
                                Rows(rowcount).Item("title")

                ' Story Author
                lblAuthor.Text = dsPubs.Tables("author"). _
                                  Rows(rowcount).Item(0)
                ' Story Location
                txtUrl.Text = dsPubs.Tables("link"). _
                                  Rows(rowcount).Item(1)

                'Process Summary
                txtResult = dsPubs.Tables("summary"). _
                            Rows(rowcount).Item(1) & vbCrLf

                start = InStr(dsPubs.Tables("entry").Rows(rowcount).Item("id"), "story.")
                start = start + Len("story.")

                lblStoryID.Text = Mid(dsPubs.Tables("entry"). _
                                  Rows(rowcount).Item("id"), _
                                  start)

                txtResult = Replace(txtResult, "<hr>", "<br>")
                txtResult = Replace(txtResult, "<hr size=1>", "<br>")

                txtResult = Replace(txtResult, ", Words", "<br>Words")
                txtResult = Replace(txtResult, ", Reviews", "<br>Reviews")
                txtResult = Replace(txtResult, "Updated", "<br>Updated")

                If InStr(txtResult, "Updated") = 0 Then
                    txtResult = Replace(txtResult, "Published", "<br><br>Published")
                Else
                    txtResult = Replace(txtResult, "Published", "<br>Published")
                End If

                txtSummary = Split(txtResult, "<br>")

                'Category
                lblProgress.Text = Mid(txtSummary(1), Len("Category: "))
                lblProgress.Text = Mid(lblProgress.Text, (InStr(lblProgress.Text, ">") + 1))
                lblProgress.Text = Replace(lblProgress.Text, "</a>", "")
                lblAnime.Text = lblProgress.Text

                If InStr(txtSummary(4), "Pairing") <> 0 Then
                    index = 5
                Else
                    index = 4
                End If


                'Chapter Count
                lblChapterCount.Text = txtSummary(index)

                'Last Updated
                lblUpdate.Text = txtSummary(index + 3)

                'Published
                lblPublish.Text = txtSummary(index + 4)

                'Summary

                txtSource.Text = txtSummary(index + 6)

            Case "MediaMiner"

                Dim htmldoc As XmlDocument

                ' Story Name
                lblTitle.Text = dsPubs.Tables(0). _
                                Rows(rowcount).Item("title")
                lblTitle.Text = Mid(lblTitle.Text, 1, InStr(lblTitle.Text, "[") - 1)

                lblAuthor.Text = dsPubs.Tables(0). _
                                 Rows(rowcount).Item("Author")

                ' Story Location
                txtUrl.Text = dsPubs.Tables(0). _
                                  Rows(rowcount).Item("link")

                lblPublish.Text = dsPubs.Tables(0). _
                                  Rows(rowcount).Item("pubdate")
                lblPublish.Text = "Last Updated: " & Format(CDate(lblPublish.Text), "MM/dd/yyyy")

                Dim category As String

                category = dsPubs.Tables(0). _
                           Rows(rowcount).Item("category")
                category = Mid(category, 1, InStr(category, "-") - 2)

                lblProgress.Text = category

                txtSource.Text = dsPubs.Tables(0).Rows(rowcount).Item("description")
                htmldoc = CleanHTML(txtSource.Text)
                txtSource.Text = htmldoc.InnerText

                lblStoryID.Text = Replace(txtUrl.Text, "http://www.mediaminer.org/fanfic/view_st.php/", "")

                htmldoc = cls.initialdownload(txtUrl.Text)

                ListChapters.Items.Clear()
                cls.GetChapters(Me.ListChapters, htmldoc)
                lblChapterCount.Text = ListChapters.Items.Count


        End Select


    End Sub

    Sub DownloadData()

        Dim htmldoc As XmlDocument
        Dim data As String = ""

        Select Case btnURL.Text

            Case "Get Chapters"

                'Downloas Information from Source

                If txtUrl.Text = "" Then GoTo oops

                htmldoc = cls.InitialDownload(txtUrl.Text)
                txtResult = htmldoc.OuterXml

                If InStr(LCase(txtResult), "story not found") = 0 Then

                    lblTitle.Text = cls.GrabTitle(htmldoc)

                    If txtResult = "" Then
oops:
                        MsgBox("Valid URL Must be Entered", _
                               MsgBoxStyle.Information)
                        Exit Sub

                    End If

                    ListChapters.Items.Clear()
                    cls.GetChapters(Me.ListChapters, htmldoc)

                    htmldoc = cls.ProcessChapters( _
                                                   txtUrl.Text, _
                                                   ListChapters, _
                                                   0 _
                                                 )

                    lblAuthor.Text = cls.GrabAuthor(htmldoc)
                    lblPublish.Text = cls.GrabDate(htmldoc, "Published: ")
                    lblUpdate.Text = cls.GrabDate(htmldoc, "Updated: ")

                    btnURL.Text = "Process Chapters"
                    lblChapterCount.Text = ListChapters.Items.Count
                    lblProgress.Text = "< -- Enter Starting Chapter"
                    txtStart.Text = 1

                    txtSource.Text = cls.grabbody(htmldoc)

                    lblStart.Visible = True
                    txtStart.Visible = True

                Else

                    btnURL.Text = "Process Chapters"
                    txtSource.Text = txtResult
                    lblStart.Visible = True
                    txtStart.Visible = True

                End If

            Case "Process Chapters"

                    'Process Chapters from Source

                    For i = (CInt(txtStart.Text) - 1) To (CInt(lblChapterCount.Text) - 1)

                        lblProgress.Text = "Chapter " & _
                                           (i + 1) & _
                                           " of " & _
                                           lblChapterCount.Text

                        htmldoc = cls.ProcessChapters( _
                                                       txtUrl.Text, _
                                                       ListChapters, _
                                                       i _
                                                     )

                        lblPublish.Text = cls.GrabDate(htmldoc, "Published: ")
                        lblUpdate.Text = cls.GrabDate(htmldoc, "Updated: ")

                        txtResult = htmldoc.OuterXml

                        Application.DoEvents()

                        If InStr(LCase(txtResult), "chapter not found") = 0 _
                        And InStr(LCase(txtResult), "story not found") = 0 _
                        Then

                            data = cls.GrabBody(htmldoc)

                            txtSource.Text = "<p></p>" & data
                            txtResult = "<html><body>"
                            txtResult &= "<p>" & lblTitle.Text & "</p>"
                            txtResult &= "<p>" & lblAuthor.Text & "</p>"
                            If lblAnime.Text <> "" Then
                                txtResult &= "<p>" & lblAnime.Text & "</p>"
                            End If
                            txtResult &= cls.WriteDate( _
                                                        lblPublish.Text, _
                                                        lblUpdate.Text, _
                                                        i + 1, _
                                                        CInt(lblChapterCount.Text) _
                                                      )
                            txtResult &= "<p>----------------------------------</p>"
                            txtResult &= txtSource.Text
                        Else
                            txtResult = "<p>Error writing file</p>"
                            txtResult = txtResult & "<p>Try downloading the file from " & _
                                        "<a href=" & Chr(34) & txtUrl.Text & (i + 1) & "/" & _
                                        Chr(34) & ">here</a> in a regular browser</p>"
                            txtResult = txtResult & "<p>DOWNLOAD ERROR</p>"
                        End If

                        txtResult = HttpUtility.HtmlDecode(txtResult)

                        Dim ecp1252 As Encoding = Encoding.GetEncoding(1252)
                        Dim sr As StreamReader
                        sr = New StreamReader(StringToStream(txtResult))
                        Dim sw As StreamWriter
                        sw = New StreamWriter(Environment.GetFolderPath( _
                                               Environment.SpecialFolder.Desktop) _
                                               & "\" & txtFileMask.Text & _
                                               Format(i + 1, "0#") & ".htm", _
                                               False, ecp1252)
                        sw.Write(sr.ReadToEnd)
                        sr.Close()
                        sw.Close()
                        sr = Nothing
                        sw = Nothing

                    Next

                    'Clear Information from source
                    ListChapters.Items.Clear()
                    lblChapterCount.Text = ""
                    lblProgress.Text = ""
                    lblStart.Visible = False
                    txtStart.Text = "1"
                    txtStart.Visible = False
                    txtSource.Text = ""

                    'Make Sure New Information is Downloaded
                    btnURL.Text = "Get Chapters"

        End Select

    End Sub

    Sub ObtainFeed()
        'Download List of stories for Given Author

        Dim clsname As String

        Dim host As String
        Dim temp As String

        Dim txtatom As String
        Dim txtresult As String

        Dim htmldoc As New XmlDocument

        Dim rowcount As Integer
        'Dim sumcount As Integer

        clsname = cmbType.Items(cmbType.SelectedIndex)

        txtresult = ""
        txtSource.Text = ""
        cmbStory.Items.Clear()
        lstStory.Items.Clear()

        dsPubs = Nothing
        dsPubs = New DataSet

        Select Case clsname

            Case "FFNet"

                If urlAtom.Text = "" Then GoTo abort

                urlAtom.Text = Replace(urlAtom.Text, " ", "")
                urlAtom.Text = Replace(urlAtom.Text, "-", "")
                
                If InStr(urlAtom.Text, "http://www.fanfiction.net") = 0 Then
                    urlAtom.Text = Replace(urlAtom.Text, ".", "")
                    urlAtom.Text = "http://www.fanfiction.net/~" & urlAtom.Text
                End If

                If (InStr(urlAtom.Text, "atom") = 0) Then

                    txtresult = DownloadPage(urlAtom.Text)


                    htmldoc = CleanHTML(txtresult)
                    htmldoc = ReturnNodes(htmldoc, "//a")
                    txtatom = GetAttrValue(htmldoc, "a", "href", "/atom/")

                    If txtatom = "" Then GoTo abort

                    Dim url As URL
                    url = ExtractUrl(urlAtom.Text)
                    txtatom = url.Scheme & "://" & url.Host & txtatom

                    urlAtom.Text = txtatom

                End If

            Case "MediaMiner"

                host = "http://www.mediaminer.org/"
                temp = ""

                Dim author As String = HttpUtility.UrlEncode(urlAtom.Text)

                If InStr(urlAtom.Text, host) = 0 Then
                    urlAtom.Text = host
                    urlAtom.Text += "fanfic/src.php?srcht=srcan&srch="
                    urlAtom.Text += author
                    urlAtom.Text += "&show=Show%2FSearch&sort=dateD&gnr=&type=&rate=AT&lang=english"
                End If

                If InStr(urlAtom.Text, "rss") = 0 Then

                    txtresult = DownloadPage(urlAtom.Text)

                    If txtresult = "" Then GoTo abort

                    htmldoc = CleanHTML(txtresult)
                    htmldoc = ReturnNodes(htmldoc, "//a")
                    txtatom = GetAttrValue(htmldoc, "a", "href", "/rss/")

                    If InStr(txtatom, "rss") = 0 Then GoTo abort

                    Dim url As URL
                    url = ExtractUrl(urlAtom.Text)
                    txtatom = url.Scheme & "://" & url.Host & txtatom

                    urlAtom.Text = txtatom

                End If

        End Select

        htmldoc = DownloadXML(urlAtom.Text)
        htmldoc = CleanXML(htmldoc)
        htmldoc = CleanFeed(htmldoc)

        If IsNothing(htmldoc) Then GoTo abort

        btnURL.Text = "Get Chapters"
        ListChapters.Items.Clear()

        ' Read in XML from file
        dsPubs.ReadXml(StringToStream(htmldoc.OuterXml))

        'Send Information to Debug Console
        Initialize( _
                    forms.frmDebug _
                  )

        frmDebug.UpdateData(dsPubs)

        For rowcount = 0 To dsPubs.Tables(0).Rows.Count - 1
            Select Case clsname
                Case "FFNet"
                    ' Story Title
                    cmbStory.Items.Add(dsPubs.Tables("entry"). _
                                       Rows(rowcount).Item("title"))

                    ' Story Location
                    lstStory.Items.Add(dsPubs.Tables("link"). _
                                      Rows(rowcount).Item(1))
                Case "MediaMiner"
                    ' Story Title
                    Dim title As String

                    title = dsPubs.Tables(0). _
                            Rows(rowcount).Item("title")
                    title = Mid(title, 1, InStr(title, "[") - 1)

                    cmbStory.Items.Add(title)

                    ' Story Location
                    lstStory.Items.Add(dsPubs.Tables(0). _
                                      Rows(rowcount).Item("link"))

            End Select
        Next

        'Load Info for First Story
        cmbStory.SelectedIndex = 0

        Exit Sub

abort:
        urlAtom.Text = ""
        MsgBox("Valid Atom Feed or Author Must Be Entered.", _
               MsgBoxStyle.Information, _
               "Fanfiction.Net Story Grabber")
        Exit Sub

    End Sub

    

    Function FormatLineEndings(ByVal str As String) As String
        ' this function converts all line endings to Windows CrLf line endings
        Dim prevChar As String
        Dim nextChar As String
        Dim curChar As String

        Dim strRet As String

        Dim X As Long

        prevChar = ""
        nextChar = ""
        curChar = ""
        strRet = ""

        For X = 1 To Len(str)
            prevChar = curChar
            curChar = Mid$(str, X, 1)

            If nextChar <> vbNullString And curChar <> nextChar Then
                curChar = curChar & nextChar
                nextChar = ""
            ElseIf curChar = vbLf Then
                If prevChar <> vbCr Then
                    curChar = vbCrLf
                End If

                nextChar = ""
            ElseIf curChar = vbCr Then
                nextChar = vbLf
            End If

            strRet = strRet & curChar
        Next X

        FormatLineEndings = strRet
    End Function


End Class
