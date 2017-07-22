Imports System.IO
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Text

Public Class frmPath
    Inherits System.Windows.Forms.Form

    Dim conf As DAL

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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents btnPath As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtOutput As System.Windows.Forms.TextBox
    Friend WithEvents btnOutput As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPath))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtPath = New System.Windows.Forms.TextBox()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnPath = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtOutput = New System.Windows.Forms.TextBox()
        Me.btnOutput = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(16, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(240, 29)
        Me.Label1.TabIndex = 0
        Me.Label1.Tag = "FanFic"
        Me.Label1.Text = "FanFic Database Path"
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(16, 44)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(672, 31)
        Me.txtPath.TabIndex = 1
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(258, 162)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(272, 60)
        Me.btnUpdate.TabIndex = 4
        Me.btnUpdate.Text = "Update INI File"
        '
        'btnPath
        '
        Me.btnPath.Location = New System.Drawing.Point(704, 44)
        Me.btnPath.Name = "btnPath"
        Me.btnPath.Size = New System.Drawing.Size(48, 37)
        Me.btnPath.TabIndex = 5
        Me.btnPath.Text = "..."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 85)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(143, 25)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Output Folder"
        '
        'txtOutput
        '
        Me.txtOutput.Location = New System.Drawing.Point(16, 116)
        Me.txtOutput.Name = "txtOutput"
        Me.txtOutput.Size = New System.Drawing.Size(672, 31)
        Me.txtOutput.TabIndex = 7
        '
        'btnOutput
        '
        Me.btnOutput.Location = New System.Drawing.Point(704, 114)
        Me.btnOutput.Name = "btnOutput"
        Me.btnOutput.Size = New System.Drawing.Size(48, 37)
        Me.btnOutput.TabIndex = 8
        Me.btnOutput.Text = "..."
        '
        'frmPath
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(10, 24)
        Me.ClientSize = New System.Drawing.Size(778, 247)
        Me.Controls.Add(Me.btnOutput)
        Me.Controls.Add(Me.txtOutput)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnPath)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmPath"
        Me.Text = "Path to Databases"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub btnBrowse_Click( _
                                 ByVal sender As System.Object, _
                                 ByVal e As System.EventArgs _
                                ) Handles btnPath.Click
        Dim dlg As OpenFileDialog = New OpenFileDialog With {
            .DefaultExt = "db",
            .Filter = "SQL Lite Database|*.db",
            .CheckFileExists = False,
            .CheckPathExists = True,
            .Title = "Select Path to Database."
        }

        Try
            If dlg.ShowDialog() = DialogResult.OK Then
                txtPath.Text = dlg.FileName
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnOutput_Click(sender As System.Object, e As System.EventArgs) Handles btnOutput.Click

        Dim dlg As OpenFileDialog = New OpenFileDialog

        Dim temp As String

        dlg.DefaultExt = "htm"
        dlg.Filter = "HTML File|*.htm|Text File|*.txt"

        dlg.CheckFileExists = False
        dlg.CheckPathExists = True
        dlg.Title = "Select Path to Output Folder."

        Try
            If dlg.ShowDialog() = DialogResult.OK Then
                temp = dlg.FileName
                txtOutput.Text = System.IO.Path.GetDirectoryName(temp)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub frmPath_Load(
                              ByVal sender As Object,
                              ByVal e As System.EventArgs
                            ) Handles MyBase.Load

        'Obtain DAL instance from frmDebug
        conf = frmDebug.DAL

        'Legacy Ini Code
        InitIniFile()

        'App.Config Code
        InitConfigFile()

    End Sub

    Private Sub btnUpdate_Click( _
                                 ByVal sender As System.Object, _
                                 ByVal e As System.EventArgs _
                               ) Handles btnUpdate.Click


        frmDebug.ReloadDAL("Fanfic", txtPath.Text, False)

        'App.Config Code
        UpdateConfigFile()

        'Legacy Ini Code
        UpdateIniFile()

        My.Settings.Reload()

        frmDebug.ReloadDAL("Fanfic", txtPath.Text)

        frmDebug.FillCategories()

        Me.Dispose()

    End Sub

#Region "config.ini Manipulation Routines"

    Dim ifr As IniFileReader

    Sub InitIniFile()

        Dim OutputFolder As String

        OutputFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)

        Dim fi As FileInfo
        Dim sc As StringCollection
        fi = New FileInfo(Application.StartupPath & "\\" & "config.ini")
        If fi.Exists Then
            ifr = New IniFileReader(Application.StartupPath & "\config.ini", True)
            txtPath.Text = ifr.GetIniValue("FanFic", "Path")
            txtOutput.Text = ifr.GetIniValue("Output", "Path")
        Else
            ifr = New IniFileReader(Application.StartupPath & "\config.ini", True)
            sc = ifr.GetIniComments(Nothing)
            sc.Add("Fanfiction Downloader DB Configuration")
            ifr.SetIniComments(Nothing, sc)
            ifr.SetIniValue("FanFic", "Path", "No Path Set")
            ifr.SetIniValue("Output", "Path", OutputFolder)
            ifr.OutputFilename = Application.StartupPath & "\config.ini"
            ifr.SaveAsIniFile()
        End If

    End Sub

    Sub UpdateIniFile()

        Dim OutputFolder As String

        OutputFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)

        If txtPath.Text = "" Then
            ifr.SetIniValue("FanFic", "Path", "No Path Set")
        Else
            ifr.SetIniValue("FanFic", "Path", txtPath.Text)
        End If

        If txtOutput.Text = "" Then
            ifr.SetIniValue("Output", "Path", OutputFolder)
        Else
            ifr.SetIniValue("Output", "Path", txtOutput.Text)
        End If

        ifr.OutputFilename = Application.StartupPath & "\config.ini"
        ifr.SaveAsIniFile()

    End Sub

#End Region

#Region "app.config Manipulation Routines"


    Sub InitConfigFile()

        Dim Path As String = ""
        Dim Output As String = ""

        If Not IsNothing(conf) Then

            Path = conf.GetPath("FanFic")
            Output = conf.GetConfigValue("Output")

            If Path <> "" Then
                txtPath.Text = Path
            End If

            If Output <> "" Then
                txtOutput.Text = Output
            End If

        End If

    End Sub

    Sub UpdateConfigFile()

        Dim OutputFolder As String

        OutputFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)

        If txtPath.Text <> "" Then
            conf.UpdateConnStr("FanFic", txtPath.Text)
        End If

        If txtOutput.Text = "" Then
            conf.SetConfigValue("Output", OutputFolder)
        Else
            conf.SetConfigValue("Output", txtOutput.Text)
        End If


    End Sub

#End Region

   
End Class

