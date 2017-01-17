<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Story
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Story))
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lnkAuthor = New System.Windows.Forms.LinkLabel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtFolder = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtCount = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtStoryID = New System.Windows.Forms.TextBox()
        Me.chkAbandoned = New System.Windows.Forms.CheckBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.chkComplete = New System.Windows.Forms.CheckBox()
        Me.txtPublish = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtUpdate = New System.Windows.Forms.TextBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtMatchup = New System.Windows.Forms.TextBox()
        Me.labelCross = New System.Windows.Forms.Label()
        Me.txtCrossover = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtTitle
        '
        Me.txtTitle.Location = New System.Drawing.Point(12, 31)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.ReadOnly = True
        Me.txtTitle.Size = New System.Drawing.Size(414, 20)
        Me.txtTitle.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(27, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Title"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 63)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(38, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Author"
        '
        'lnkAuthor
        '
        Me.lnkAuthor.AutoSize = True
        Me.lnkAuthor.Location = New System.Drawing.Point(9, 85)
        Me.lnkAuthor.Name = "lnkAuthor"
        Me.lnkAuthor.Size = New System.Drawing.Size(61, 13)
        Me.lnkAuthor.TabIndex = 3
        Me.lnkAuthor.TabStop = True
        Me.lnkAuthor.Text = "Author Link"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(201, 63)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(36, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Folder"
        '
        'txtFolder
        '
        Me.txtFolder.Location = New System.Drawing.Point(204, 82)
        Me.txtFolder.Name = "txtFolder"
        Me.txtFolder.ReadOnly = True
        Me.txtFolder.Size = New System.Drawing.Size(133, 20)
        Me.txtFolder.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(355, 63)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(75, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Chapter Count"
        '
        'txtCount
        '
        Me.txtCount.Location = New System.Drawing.Point(358, 82)
        Me.txtCount.Name = "txtCount"
        Me.txtCount.ReadOnly = True
        Me.txtCount.Size = New System.Drawing.Size(68, 20)
        Me.txtCount.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(9, 120)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Story ID"
        '
        'txtStoryID
        '
        Me.txtStoryID.Location = New System.Drawing.Point(12, 146)
        Me.txtStoryID.Name = "txtStoryID"
        Me.txtStoryID.ReadOnly = True
        Me.txtStoryID.Size = New System.Drawing.Size(87, 20)
        Me.txtStoryID.TabIndex = 9
        '
        'chkAbandoned
        '
        Me.chkAbandoned.AutoCheck = False
        Me.chkAbandoned.AutoSize = True
        Me.chkAbandoned.Location = New System.Drawing.Point(124, 149)
        Me.chkAbandoned.Name = "chkAbandoned"
        Me.chkAbandoned.Size = New System.Drawing.Size(15, 14)
        Me.chkAbandoned.TabIndex = 10
        Me.chkAbandoned.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(121, 120)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(62, 13)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Abandoned"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(201, 120)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(51, 13)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Complete"
        '
        'chkComplete
        '
        Me.chkComplete.AutoCheck = False
        Me.chkComplete.AutoSize = True
        Me.chkComplete.Location = New System.Drawing.Point(204, 149)
        Me.chkComplete.Name = "chkComplete"
        Me.chkComplete.Size = New System.Drawing.Size(15, 14)
        Me.chkComplete.TabIndex = 13
        Me.chkComplete.UseVisualStyleBackColor = True
        '
        'txtPublish
        '
        Me.txtPublish.Location = New System.Drawing.Point(269, 146)
        Me.txtPublish.Name = "txtPublish"
        Me.txtPublish.ReadOnly = True
        Me.txtPublish.Size = New System.Drawing.Size(67, 20)
        Me.txtPublish.TabIndex = 14
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(266, 120)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(67, 13)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Publish Date"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(358, 120)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(68, 13)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "Update Date"
        '
        'txtUpdate
        '
        Me.txtUpdate.Location = New System.Drawing.Point(361, 146)
        Me.txtUpdate.Name = "txtUpdate"
        Me.txtUpdate.ReadOnly = True
        Me.txtUpdate.Size = New System.Drawing.Size(65, 20)
        Me.txtUpdate.TabIndex = 17
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(12, 271)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ReadOnly = True
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(414, 123)
        Me.txtDescription.TabIndex = 18
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(9, 242)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(60, 13)
        Me.Label10.TabIndex = 19
        Me.Label10.Text = "Description"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(9, 183)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(49, 13)
        Me.Label11.TabIndex = 20
        Me.Label11.Text = "Matchup"
        '
        'txtMatchup
        '
        Me.txtMatchup.Location = New System.Drawing.Point(12, 208)
        Me.txtMatchup.Name = "txtMatchup"
        Me.txtMatchup.ReadOnly = True
        Me.txtMatchup.Size = New System.Drawing.Size(192, 20)
        Me.txtMatchup.TabIndex = 21
        '
        'labelCross
        '
        Me.labelCross.AutoSize = True
        Me.labelCross.Location = New System.Drawing.Point(223, 183)
        Me.labelCross.Name = "labelCross"
        Me.labelCross.Size = New System.Drawing.Size(54, 13)
        Me.labelCross.TabIndex = 22
        Me.labelCross.Text = "Crossover"
        '
        'txtCrossover
        '
        Me.txtCrossover.Location = New System.Drawing.Point(226, 208)
        Me.txtCrossover.Name = "txtCrossover"
        Me.txtCrossover.ReadOnly = True
        Me.txtCrossover.Size = New System.Drawing.Size(200, 20)
        Me.txtCrossover.TabIndex = 23
        '
        'Story
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(442, 406)
        Me.Controls.Add(Me.txtCrossover)
        Me.Controls.Add(Me.labelCross)
        Me.Controls.Add(Me.txtMatchup)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.txtUpdate)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtPublish)
        Me.Controls.Add(Me.chkComplete)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.chkAbandoned)
        Me.Controls.Add(Me.txtStoryID)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtCount)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtFolder)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lnkAuthor)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtTitle)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Story"
        Me.Text = "Story Info"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtTitle As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lnkAuthor As System.Windows.Forms.LinkLabel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtFolder As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtCount As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtStoryID As System.Windows.Forms.TextBox
    Friend WithEvents chkAbandoned As System.Windows.Forms.CheckBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents chkComplete As System.Windows.Forms.CheckBox
    Friend WithEvents txtPublish As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtUpdate As System.Windows.Forms.TextBox
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtMatchup As System.Windows.Forms.TextBox
    Friend WithEvents labelCross As System.Windows.Forms.Label
    Friend WithEvents txtCrossover As System.Windows.Forms.TextBox
End Class
