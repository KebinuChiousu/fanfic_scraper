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
        Me.txtTitle.Location = New System.Drawing.Point(16, 38)
        Me.txtTitle.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.ReadOnly = True
        Me.txtTitle.Size = New System.Drawing.Size(551, 22)
        Me.txtTitle.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 11)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(35, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Title"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 78)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 17)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Author"
        '
        'lnkAuthor
        '
        Me.lnkAuthor.AutoSize = True
        Me.lnkAuthor.Location = New System.Drawing.Point(12, 105)
        Me.lnkAuthor.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lnkAuthor.Name = "lnkAuthor"
        Me.lnkAuthor.Size = New System.Drawing.Size(80, 17)
        Me.lnkAuthor.TabIndex = 3
        Me.lnkAuthor.TabStop = True
        Me.lnkAuthor.Text = "Author Link"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(268, 78)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(48, 17)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Folder"
        '
        'txtFolder
        '
        Me.txtFolder.Location = New System.Drawing.Point(272, 101)
        Me.txtFolder.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtFolder.Name = "txtFolder"
        Me.txtFolder.ReadOnly = True
        Me.txtFolder.Size = New System.Drawing.Size(176, 22)
        Me.txtFolder.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(473, 78)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(99, 17)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Chapter Count"
        '
        'txtCount
        '
        Me.txtCount.Location = New System.Drawing.Point(477, 101)
        Me.txtCount.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtCount.Name = "txtCount"
        Me.txtCount.ReadOnly = True
        Me.txtCount.Size = New System.Drawing.Size(89, 22)
        Me.txtCount.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 148)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(58, 17)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Story ID"
        '
        'txtStoryID
        '
        Me.txtStoryID.Location = New System.Drawing.Point(16, 180)
        Me.txtStoryID.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtStoryID.Name = "txtStoryID"
        Me.txtStoryID.ReadOnly = True
        Me.txtStoryID.Size = New System.Drawing.Size(115, 22)
        Me.txtStoryID.TabIndex = 9
        '
        'chkAbandoned
        '
        Me.chkAbandoned.AutoCheck = False
        Me.chkAbandoned.AutoSize = True
        Me.chkAbandoned.Location = New System.Drawing.Point(165, 183)
        Me.chkAbandoned.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkAbandoned.Name = "chkAbandoned"
        Me.chkAbandoned.Size = New System.Drawing.Size(18, 17)
        Me.chkAbandoned.TabIndex = 10
        Me.chkAbandoned.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(161, 148)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(81, 17)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Abandoned"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(268, 148)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(67, 17)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Complete"
        '
        'chkComplete
        '
        Me.chkComplete.AutoCheck = False
        Me.chkComplete.AutoSize = True
        Me.chkComplete.Location = New System.Drawing.Point(272, 183)
        Me.chkComplete.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkComplete.Name = "chkComplete"
        Me.chkComplete.Size = New System.Drawing.Size(18, 17)
        Me.chkComplete.TabIndex = 13
        Me.chkComplete.UseVisualStyleBackColor = True
        '
        'txtPublish
        '
        Me.txtPublish.Location = New System.Drawing.Point(359, 180)
        Me.txtPublish.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtPublish.Name = "txtPublish"
        Me.txtPublish.ReadOnly = True
        Me.txtPublish.Size = New System.Drawing.Size(88, 22)
        Me.txtPublish.TabIndex = 14
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(355, 148)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(88, 17)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Publish Date"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(477, 148)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(88, 17)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "Update Date"
        '
        'txtUpdate
        '
        Me.txtUpdate.Location = New System.Drawing.Point(481, 180)
        Me.txtUpdate.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtUpdate.Name = "txtUpdate"
        Me.txtUpdate.ReadOnly = True
        Me.txtUpdate.Size = New System.Drawing.Size(85, 22)
        Me.txtUpdate.TabIndex = 17
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(16, 334)
        Me.txtDescription.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ReadOnly = True
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(551, 150)
        Me.txtDescription.TabIndex = 18
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(12, 298)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(79, 17)
        Me.Label10.TabIndex = 19
        Me.Label10.Text = "Description"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(12, 225)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(62, 17)
        Me.Label11.TabIndex = 20
        Me.Label11.Text = "Matchup"
        '
        'txtMatchup
        '
        Me.txtMatchup.Location = New System.Drawing.Point(16, 256)
        Me.txtMatchup.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtMatchup.Name = "txtMatchup"
        Me.txtMatchup.ReadOnly = True
        Me.txtMatchup.Size = New System.Drawing.Size(255, 22)
        Me.txtMatchup.TabIndex = 21
        '
        'labelCross
        '
        Me.labelCross.AutoSize = True
        Me.labelCross.Location = New System.Drawing.Point(297, 225)
        Me.labelCross.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.labelCross.Name = "labelCross"
        Me.labelCross.Size = New System.Drawing.Size(72, 17)
        Me.labelCross.TabIndex = 22
        Me.labelCross.Text = "Crossover"
        '
        'txtCrossover
        '
        Me.txtCrossover.Location = New System.Drawing.Point(301, 256)
        Me.txtCrossover.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtCrossover.Name = "txtCrossover"
        Me.txtCrossover.ReadOnly = True
        Me.txtCrossover.Size = New System.Drawing.Size(265, 22)
        Me.txtCrossover.TabIndex = 23
        '
        'Story
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(589, 500)
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
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
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
