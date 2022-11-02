using System.Diagnostics;

namespace HtmlScraper.Gui
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class Story : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Story));
            txtTitle = new TextBox();
            Label1 = new Label();
            Label2 = new Label();
            lnkAuthor = new LinkLabel();
            lnkAuthor.LinkClicked += new LinkLabelLinkClickedEventHandler(lnkAuthor_LinkClicked);
            Label3 = new Label();
            txtFolder = new TextBox();
            Label4 = new Label();
            txtCount = new TextBox();
            Label5 = new Label();
            txtStoryID = new TextBox();
            chkAbandoned = new CheckBox();
            Label6 = new Label();
            Label7 = new Label();
            chkComplete = new CheckBox();
            txtPublish = new TextBox();
            Label8 = new Label();
            Label9 = new Label();
            txtUpdate = new TextBox();
            txtDescription = new TextBox();
            Label10 = new Label();
            Label11 = new Label();
            txtMatchup = new TextBox();
            labelCross = new Label();
            txtCrossover = new TextBox();
            SuspendLayout();
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(12, 31);
            txtTitle.Name = "txtTitle";
            txtTitle.ReadOnly = true;
            txtTitle.Size = new Size(414, 20);
            txtTitle.TabIndex = 0;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(9, 9);
            Label1.Name = "Label1";
            Label1.Size = new Size(27, 13);
            Label1.TabIndex = 1;
            Label1.Text = "Title";
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Location = new Point(9, 63);
            Label2.Name = "Label2";
            Label2.Size = new Size(38, 13);
            Label2.TabIndex = 2;
            Label2.Text = "Author";
            // 
            // lnkAuthor
            // 
            lnkAuthor.AutoSize = true;
            lnkAuthor.Location = new Point(9, 85);
            lnkAuthor.Name = "lnkAuthor";
            lnkAuthor.Size = new Size(61, 13);
            lnkAuthor.TabIndex = 3;
            lnkAuthor.TabStop = true;
            lnkAuthor.Text = "Author Link";
            // 
            // Label3
            // 
            Label3.AutoSize = true;
            Label3.Location = new Point(201, 63);
            Label3.Name = "Label3";
            Label3.Size = new Size(36, 13);
            Label3.TabIndex = 4;
            Label3.Text = "Folder";
            // 
            // txtFolder
            // 
            txtFolder.Location = new Point(204, 82);
            txtFolder.Name = "txtFolder";
            txtFolder.ReadOnly = true;
            txtFolder.Size = new Size(133, 20);
            txtFolder.TabIndex = 5;
            // 
            // Label4
            // 
            Label4.AutoSize = true;
            Label4.Location = new Point(355, 63);
            Label4.Name = "Label4";
            Label4.Size = new Size(75, 13);
            Label4.TabIndex = 6;
            Label4.Text = "Chapter Count";
            // 
            // txtCount
            // 
            txtCount.Location = new Point(358, 82);
            txtCount.Name = "txtCount";
            txtCount.ReadOnly = true;
            txtCount.Size = new Size(68, 20);
            txtCount.TabIndex = 7;
            // 
            // Label5
            // 
            Label5.AutoSize = true;
            Label5.Location = new Point(9, 120);
            Label5.Name = "Label5";
            Label5.Size = new Size(45, 13);
            Label5.TabIndex = 8;
            Label5.Text = "Story ID";
            // 
            // txtStoryID
            // 
            txtStoryID.Location = new Point(12, 146);
            txtStoryID.Name = "txtStoryID";
            txtStoryID.ReadOnly = true;
            txtStoryID.Size = new Size(87, 20);
            txtStoryID.TabIndex = 9;
            // 
            // chkAbandoned
            // 
            chkAbandoned.AutoCheck = false;
            chkAbandoned.AutoSize = true;
            chkAbandoned.Location = new Point(124, 149);
            chkAbandoned.Name = "chkAbandoned";
            chkAbandoned.Size = new Size(15, 14);
            chkAbandoned.TabIndex = 10;
            chkAbandoned.UseVisualStyleBackColor = true;
            // 
            // Label6
            // 
            Label6.AutoSize = true;
            Label6.Location = new Point(121, 120);
            Label6.Name = "Label6";
            Label6.Size = new Size(62, 13);
            Label6.TabIndex = 11;
            Label6.Text = "Abandoned";
            // 
            // Label7
            // 
            Label7.AutoSize = true;
            Label7.Location = new Point(201, 120);
            Label7.Name = "Label7";
            Label7.Size = new Size(51, 13);
            Label7.TabIndex = 12;
            Label7.Text = "Complete";
            // 
            // chkComplete
            // 
            chkComplete.AutoCheck = false;
            chkComplete.AutoSize = true;
            chkComplete.Location = new Point(204, 149);
            chkComplete.Name = "chkComplete";
            chkComplete.Size = new Size(15, 14);
            chkComplete.TabIndex = 13;
            chkComplete.UseVisualStyleBackColor = true;
            // 
            // txtPublish
            // 
            txtPublish.Location = new Point(269, 146);
            txtPublish.Name = "txtPublish";
            txtPublish.ReadOnly = true;
            txtPublish.Size = new Size(67, 20);
            txtPublish.TabIndex = 14;
            // 
            // Label8
            // 
            Label8.AutoSize = true;
            Label8.Location = new Point(266, 120);
            Label8.Name = "Label8";
            Label8.Size = new Size(67, 13);
            Label8.TabIndex = 15;
            Label8.Text = "Publish Date";
            // 
            // Label9
            // 
            Label9.AutoSize = true;
            Label9.Location = new Point(358, 120);
            Label9.Name = "Label9";
            Label9.Size = new Size(68, 13);
            Label9.TabIndex = 16;
            Label9.Text = "Update Date";
            // 
            // txtUpdate
            // 
            txtUpdate.Location = new Point(361, 146);
            txtUpdate.Name = "txtUpdate";
            txtUpdate.ReadOnly = true;
            txtUpdate.Size = new Size(65, 20);
            txtUpdate.TabIndex = 17;
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(12, 271);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.ReadOnly = true;
            txtDescription.ScrollBars = ScrollBars.Vertical;
            txtDescription.Size = new Size(414, 123);
            txtDescription.TabIndex = 18;
            // 
            // Label10
            // 
            Label10.AutoSize = true;
            Label10.Location = new Point(9, 242);
            Label10.Name = "Label10";
            Label10.Size = new Size(60, 13);
            Label10.TabIndex = 19;
            Label10.Text = "Description";
            // 
            // Label11
            // 
            Label11.AutoSize = true;
            Label11.Location = new Point(9, 183);
            Label11.Name = "Label11";
            Label11.Size = new Size(49, 13);
            Label11.TabIndex = 20;
            Label11.Text = "Matchup";
            // 
            // txtMatchup
            // 
            txtMatchup.Location = new Point(12, 208);
            txtMatchup.Name = "txtMatchup";
            txtMatchup.ReadOnly = true;
            txtMatchup.Size = new Size(192, 20);
            txtMatchup.TabIndex = 21;
            // 
            // labelCross
            // 
            labelCross.AutoSize = true;
            labelCross.Location = new Point(223, 183);
            labelCross.Name = "labelCross";
            labelCross.Size = new Size(54, 13);
            labelCross.TabIndex = 22;
            labelCross.Text = "Crossover";
            // 
            // txtCrossover
            // 
            txtCrossover.Location = new Point(226, 208);
            txtCrossover.Name = "txtCrossover";
            txtCrossover.ReadOnly = true;
            txtCrossover.Size = new Size(200, 20);
            txtCrossover.TabIndex = 23;
            // 
            // Story
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(442, 406);
            Controls.Add(txtCrossover);
            Controls.Add(labelCross);
            Controls.Add(txtMatchup);
            Controls.Add(Label11);
            Controls.Add(Label10);
            Controls.Add(txtDescription);
            Controls.Add(txtUpdate);
            Controls.Add(Label9);
            Controls.Add(Label8);
            Controls.Add(txtPublish);
            Controls.Add(chkComplete);
            Controls.Add(Label7);
            Controls.Add(Label6);
            Controls.Add(chkAbandoned);
            Controls.Add(txtStoryID);
            Controls.Add(Label5);
            Controls.Add(txtCount);
            Controls.Add(Label4);
            Controls.Add(txtFolder);
            Controls.Add(Label3);
            Controls.Add(lnkAuthor);
            Controls.Add(Label2);
            Controls.Add(Label1);
            Controls.Add(txtTitle);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Story";
            Text = "Story Info";
            ResumeLayout(false);
            PerformLayout();

        }
        internal TextBox txtTitle;
        internal Label Label1;
        internal Label Label2;
        internal LinkLabel lnkAuthor;
        internal Label Label3;
        internal TextBox txtFolder;
        internal Label Label4;
        internal TextBox txtCount;
        internal Label Label5;
        internal TextBox txtStoryID;
        internal CheckBox chkAbandoned;
        internal Label Label6;
        internal Label Label7;
        internal CheckBox chkComplete;
        internal TextBox txtPublish;
        internal Label Label8;
        internal Label Label9;
        internal TextBox txtUpdate;
        internal TextBox txtDescription;
        internal Label Label10;
        internal Label Label11;
        internal TextBox txtMatchup;
        internal Label labelCross;
        internal TextBox txtCrossover;
    }
}