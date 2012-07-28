

Public Class Debug
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
    Friend WithEvents grdData As System.Windows.Forms.DataGrid
    Friend WithEvents grdDB As System.Windows.Forms.DataGrid
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents cmbProcess As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents btnPath As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.grdData = New System.Windows.Forms.DataGrid
        Me.grdDB = New System.Windows.Forms.DataGrid
        Me.Button3 = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.cmbProcess = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button5 = New System.Windows.Forms.Button
        Me.Button6 = New System.Windows.Forms.Button
        Me.Button7 = New System.Windows.Forms.Button
        Me.btnPath = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        CType(Me.grdData, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grdDB, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grdData
        '
        Me.grdData.DataMember = ""
        Me.grdData.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdData.Location = New System.Drawing.Point(7, 7)
        Me.grdData.Name = "grdData"
        Me.grdData.Size = New System.Drawing.Size(656, 177)
        Me.grdData.TabIndex = 27
        '
        'grdDB
        '
        Me.grdDB.DataMember = ""
        Me.grdDB.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdDB.Location = New System.Drawing.Point(7, 187)
        Me.grdDB.Name = "grdDB"
        Me.grdDB.Size = New System.Drawing.Size(655, 176)
        Me.grdDB.TabIndex = 29
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(560, 367)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(100, 21)
        Me.Button3.TabIndex = 31
        Me.Button3.Text = "Update Database"
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(560, 388)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(100, 21)
        Me.Button4.TabIndex = 32
        Me.Button4.Text = "Update StoryID"
        '
        'cmbProcess
        '
        Me.cmbProcess.DisplayMember = "StoryPage"
        Me.cmbProcess.Items.AddRange(New Object() {"StoryID", "Author"})
        Me.cmbProcess.Location = New System.Drawing.Point(467, 388)
        Me.cmbProcess.Name = "cmbProcess"
        Me.cmbProcess.Size = New System.Drawing.Size(86, 21)
        Me.cmbProcess.TabIndex = 33
        Me.cmbProcess.Text = "StoryID"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(467, 367)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 14)
        Me.Label1.TabIndex = 34
        Me.Label1.Text = "Navigate By"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(328, 368)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(64, 42)
        Me.Button1.TabIndex = 35
        Me.Button1.Text = "Automate HP"
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(392, 368)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(64, 42)
        Me.Button5.TabIndex = 36
        Me.Button5.Text = "Automate Ranma"
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(64, 368)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(106, 21)
        Me.Button6.TabIndex = 37
        Me.Button6.Text = "Open HP DB"
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(64, 388)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(106, 21)
        Me.Button7.TabIndex = 38
        Me.Button7.Text = "Open Ranma DB"
        '
        'btnPath
        '
        Me.btnPath.Location = New System.Drawing.Point(8, 368)
        Me.btnPath.Name = "btnPath"
        Me.btnPath.Size = New System.Drawing.Size(48, 40)
        Me.btnPath.TabIndex = 39
        Me.btnPath.Text = "Set Paths"
        '
        'btnSave
        '
        Me.btnSave.Enabled = False
        Me.btnSave.Location = New System.Drawing.Point(184, 368)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(72, 40)
        Me.btnSave.TabIndex = 40
        Me.btnSave.Text = "Save Story to DB"
        '
        'Debug
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(665, 416)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnPath)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbProcess)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.grdDB)
        Me.Controls.Add(Me.grdData)
        Me.Name = "Debug"
        Me.Text = "frmDebug"
        CType(Me.grdData, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grdDB, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Enum Process
        AuthorPage = 0
        StoryPage = 1
    End Enum

    Public myCaller As HtmlGrabber
    Dim DB As String

    Public Navigate As Process

#Region "Interface Code"

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Public Sub UpdateDBData(ByVal ds As DataSet, Optional ByVal DataMember As String = "Table")
        grdDB.DataMember = DataMember
        grdDB.DataSource = ds
        grdDB.Visible = True
    End Sub

    Public Sub UpdateData(ByVal ds As DataSet)
        grdData.DataMember = "entry"
        grdData.DataSource = ds
        grdData.Visible = True
    End Sub

    Private Sub frmDebug_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        e.Cancel = True
        Me.Hide()
    End Sub

#End Region

    Private Sub UpdateDateBase(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim ds As New DataSet

        ds = grdDB.DataSource

        CreateConnection(ModOleDB.DatabaseType.Access)
        SynchData(ds)

    End Sub

    Private Sub GetStoryID(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        StoryID()
    End Sub

    Private Sub GotoNextRecord(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MoveNext()
    End Sub

    Private Sub frmDebug_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        PlaceDebugWindow()
        cmbProcess.SelectedIndex = 0

    End Sub

    Sub UpdateAtom(ByVal url As String)
        ' update urlAtom with AuthorPage
        frmMain.urlAtom.Text = url
        ' Obtain Feed From Site
        frmMain.ObtainFeed()
    End Sub


    Sub UpdateURL(ByVal url As String)
        'Update txtUrl with story Page
        frmMain.txtUrl.Text = url
        'Reset frmMain
        ResetInfo()
        'Update btnURL Caption for Correct Processing
        frmMain.btnURL.Text = "Get Chapters"
        'Obtain Story Info From Story Page
        frmMain.DownloadData()
    End Sub

    Sub ProcessStory(ByVal ds As DataSet, ByVal pos As Long)

        If InStr(LCase(frmMain.txtSource.Text), "story not found") <> 0 Then
            frmMain.lblChapterCount.Text = _
                 CInt(ds.Tables(0).Rows(pos).Item("Count")) + 1
        End If


        If frmMain.lblChapterCount.Text > _
           ds.Tables(0).Rows(pos). _
           Item("Count") _
        Then
            frmMain.txtStart.Text = _
                                    ds.Tables(0). _
                                    Rows(pos). _
                                    Item("Count") + 1

            frmMain.txtFileMask.Text = _
                                       ds.Tables(0). _
                                       Rows(pos). _
                                       Item("Folder") & "-"

            ds.Tables(0).Rows(pos).Item("Count") = CInt(frmMain.lblChapterCount.Text)

            ds.Tables(0).Rows(pos).Item("Update_Date") = CDate(frmMain.lblUpdate.Text)

            frmMain.DownloadData()

            CreateConnection(DatabaseType.Access)
            SynchData(ds)
        End If
    End Sub

    Function MoveNext(Optional ByVal initial As Boolean = False)

        Dim pos As Long
        Dim ds As DataSet
        Dim url As String
        Dim data As String

        ds = grdDB.DataSource

        If initial Then
            grdDB.CurrentRowIndex = 0
            initial = False
        End If

bypass:

        pos = grdDB.CurrentRowIndex

        If pos < (ds.Tables(0).Rows.Count - 1) Then
            grdDB.CurrentRowIndex = grdDB.CurrentRowIndex + 1
            pos = grdDB.CurrentRowIndex
        Else
            Return -1
        End If



        Select Case Navigate
            Case Process.AuthorPage
                url = GetAtom(ds)
            Case Process.StoryPage
                url = GetStory(ds)
        End Select


        If (InStr(url, "fanfiction") = 0 And _
           pos < ds.Tables(0).Rows.Count - 1) _
        Or _
              (ds.Tables(0).Rows(pos). _
                Item("Complete") = True) _
        Then
            GoTo bypass
        Else
            Select Case Navigate
                Case Process.AuthorPage
                    UpdateAtom(url)
                Case Process.StoryPage
                    UpdateURL(url)
                    Application.DoEvents()
                    ProcessStory(ds, pos)
            End Select
        End If

        Return pos

    End Function

    Function GetStory(ByVal ds As DataSet) As String
        Dim StoryID As String

        If ds.Tables(0). _
              Rows(grdDB.CurrentRowIndex). _
              Item("StoryId").GetType Is GetType(DBNull) _
        Then
            GetStory = ""
        Else

            StoryID = ds.Tables(0). _
                         Rows(grdDB.CurrentRowIndex). _
                         Item("StoryID")
            GetStory = "http://www.fanfiction.net/s/" & _
                       StoryID & _
                       "/1/"
        End If

    End Function

    Function GetAtom(ByVal ds As DataSet) As String

        If ds.Tables(0). _
              Rows(grdDB.CurrentRowIndex). _
              Item("Internet").GetType Is GetType(DBNull) _
        Then
            GetAtom = ""
        Else

            GetAtom = Replace( _
                                   ds.Tables(0). _
                                   Rows(grdDB.CurrentRowIndex). _
                                   Item("Internet") _
                                   , "#", "")
        End If

    End Function

    Private Sub StoryID()
        Dim ds As New DataSet

        ds = grdDB.DataSource

        ds.Tables(0). _
        Rows(grdDB.CurrentRowIndex). _
        Item("StoryID") _
        = _
        frmMain.lblStoryID.Text

    End Sub

    Sub ResetInfo()
        'Clear Information from source
        frmMain.ListChapters.Items.Clear()
        frmMain.lblChapterCount.Text = ""
        frmMain.lblProgress.Text = ""
        frmMain.lblStart.Visible = False
        frmMain.txtStart.Text = "1"
        frmMain.txtStart.Visible = False
        frmMain.txtSource.Text = ""
    End Sub


    Private Sub cmbProcess_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbProcess.SelectedIndexChanged

        Select Case cmbProcess.Text
            Case "StoryID"
                Navigate = Process.StoryPage
            Case "Author"
                Navigate = Process.AuthorPage
        End Select

        ResetInfo()

    End Sub

    Private Sub AutomateHP( _
                            ByVal sender As System.Object, _
                            ByVal e As System.EventArgs _
                          ) Handles Button1.Click
        Dim ds As New DataSet
        Dim pos As Long

        Dim start As Integer

        GetHPData()

        ds = grdDB.DataSource

        start = MoveNext(True)

        For pos = start To (ds.Tables(0).Rows.Count - 1)
            pos = MoveNext()
            If pos = -1 Then Exit For
        Next

    End Sub

    Private Sub AutomateRanma(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim ds As New DataSet
        Dim pos As Long
        Dim start As Integer

        GetRanmaData()

        ds = grdDB.DataSource

        start = MoveNext(True)

        For pos = start To (ds.Tables(0).Rows.Count - 1)
            pos = MoveNext()
            If pos = -1 Then Exit For
        Next

    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        GetHPData()
        DB = "HP"

        btnSave.Enabled = True

        MoveNext(True)
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        GetRanmaData()

        DB = "Ranma"

        btnSave.Enabled = True


        MoveNext(True)
    End Sub

    Private Sub btnPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPath.Click
        Dim frmpath As New frmPath
        frmpath.Show()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim ds As DataSet

        ds = grdDB.DataSource

        Dim NewRow As Integer = ds.Tables(0).Rows.Count


        Select Case DB
            Case "HP"
                Dim iYesNo As Integer = MsgBox("Add New Record?", MsgBoxStyle.YesNo)

                If iYesNo = vbYes Then
                    Dim dr As DataRow = ds.Tables(0).NewRow

                    dr("Title") = myCaller.lblTitle.Text
                    dr("Author") = myCaller.lblAuthor.Text
                    dr("Folder") = InputBox("Enter File Name") 'Folder Name
                    'dr("Chapter") = "" 'Current Chapter
                    dr("Count") = 0 'Chapter Count
                    'dr("Matchup") = "" ' Matchup
                    dr("Description") = Trim(myCaller.txtSource.Text)
                    dr("Internet") = myCaller.lblAuthor.Text & _
                                     "#" & Replace(myCaller.urlAtom.Text, "atom/", "") & "#"
                    dr("StoryId") = myCaller.lblStoryID.Text
                    dr("Complete") = False
                    dr("Publish_Date") = CDate( _
                                                Replace( _
                                                         frmMain.lblPublish.Text, _
                                                         "Published:", _
                                                         "" _
                                                       ) _
                                              )

                    ds.Tables(0).Rows.Add(dr)

                    grdDB.DataSource = ds
                    grdDB.CurrentRowIndex = NewRow

                    'CreateConnection(ModOleDB.DatabaseType.Access)
                    'SynchData(ds)

                End If

            Case "Ranma"
                Dim iYesNo As Integer = MsgBox("Add New Record?", MsgBoxStyle.YesNo)

                If iYesNo = vbYes Then
                    Dim dr As DataRow = ds.Tables(0).NewRow

                    dr("Title") = myCaller.lblTitle.Text
                    dr("Author") = myCaller.lblAuthor.Text
                    dr("Folder") = InputBox("Enter File Name") 'Folder Name
                    'dr("Chapter") = "" 'Current Chapter
                    dr("Count") = 0 'Chapter Count
                    'dr("Crossover") = "" ' Matchup
                    dr("Description") = Trim(myCaller.txtSource.Text)
                    dr("Internet") = myCaller.lblAuthor.Text & _
                                     "#" & Replace(myCaller.urlAtom.Text, "atom/", "") & "#"
                    dr("StoryId") = myCaller.lblStoryID.Text
                    dr("Complete") = False


                    ds.Tables(0).Rows.Add(dr)

                    grdDB.DataSource = ds
                    grdDB.CurrentRowIndex = NewRow

                End If
        End Select

        ds = Nothing

    End Sub
End Class
