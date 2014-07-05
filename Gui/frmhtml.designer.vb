<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Html
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtPrefix = New System.Windows.Forms.TextBox()
        Me.btnClean = New System.Windows.Forms.Button()
        Me.txtFile = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtChapter = New System.Windows.Forms.TextBox()
        Me.OpenFile = New System.Windows.Forms.OpenFileDialog()
        Me.SuspendLayout()
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(353, 11)
        Me.btnBrowse.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(33, 28)
        Me.btnBrowse.TabIndex = 2
        Me.btnBrowse.Text = "..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 54)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 17)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "File Prefix:"
        '
        'txtPrefix
        '
        Me.txtPrefix.Location = New System.Drawing.Point(97, 50)
        Me.txtPrefix.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtPrefix.Name = "txtPrefix"
        Me.txtPrefix.Size = New System.Drawing.Size(68, 22)
        Me.txtPrefix.TabIndex = 4
        '
        'btnClean
        '
        Me.btnClean.Location = New System.Drawing.Point(156, 86)
        Me.btnClean.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnClean.Name = "btnClean"
        Me.btnClean.Size = New System.Drawing.Size(92, 28)
        Me.btnClean.TabIndex = 7
        Me.btnClean.Text = "Clean Html"
        Me.btnClean.UseVisualStyleBackColor = True
        '
        'txtFile
        '
        Me.txtFile.AllowDrop = True
        Me.txtFile.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFile.Location = New System.Drawing.Point(4, 15)
        Me.txtFile.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtFile.Name = "txtFile"
        Me.txtFile.ReadOnly = True
        Me.txtFile.Size = New System.Drawing.Size(340, 22)
        Me.txtFile.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(189, 54)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 17)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Chapter"
        '
        'txtChapter
        '
        Me.txtChapter.Location = New System.Drawing.Point(268, 50)
        Me.txtChapter.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtChapter.Name = "txtChapter"
        Me.txtChapter.Size = New System.Drawing.Size(48, 22)
        Me.txtChapter.TabIndex = 6
        '
        'Html
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(393, 128)
        Me.Controls.Add(Me.txtChapter)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtFile)
        Me.Controls.Add(Me.btnClean)
        Me.Controls.Add(Me.txtPrefix)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnBrowse)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "Html"
        Me.Text = "frmHtml"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtPrefix As System.Windows.Forms.TextBox
    Friend WithEvents btnClean As System.Windows.Forms.Button
    Friend WithEvents txtFile As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtChapter As System.Windows.Forms.TextBox
    Friend WithEvents OpenFile As System.Windows.Forms.OpenFileDialog
End Class
