using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace HtmlGrabber
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class Html : Form
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Html));
            btnBrowse = new Button();
            btnBrowse.Click += new EventHandler(btnBrowse_Click);
            Label1 = new Label();
            txtPrefix = new TextBox();
            btnClean = new Button();
            btnClean.Click += new EventHandler(btnClean_Click);
            txtFile = new TextBox();
            txtFile.DragDrop += new DragEventHandler(txtFile_DragDrop);
            txtFile.DragEnter += new DragEventHandler(txtFile_DragEnter);
            Label2 = new Label();
            txtChapter = new TextBox();
            OpenFile = new OpenFileDialog();
            SuspendLayout();
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(265, 9);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(25, 23);
            btnBrowse.TabIndex = 2;
            btnBrowse.Text = "...";
            btnBrowse.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 44);
            Label1.Name = "Label1";
            Label1.Size = new Size(55, 13);
            Label1.TabIndex = 3;
            Label1.Text = "File Prefix:";
            // 
            // txtPrefix
            // 
            txtPrefix.Location = new Point(73, 41);
            txtPrefix.Name = "txtPrefix";
            txtPrefix.Size = new Size(52, 20);
            txtPrefix.TabIndex = 4;
            // 
            // btnClean
            // 
            btnClean.Location = new Point(117, 70);
            btnClean.Name = "btnClean";
            btnClean.Size = new Size(69, 23);
            btnClean.TabIndex = 7;
            btnClean.Text = "Clean Html";
            btnClean.UseVisualStyleBackColor = true;
            // 
            // txtFile
            // 
            txtFile.AllowDrop = true;
            txtFile.ForeColor = SystemColors.WindowText;
            txtFile.Location = new Point(3, 12);
            txtFile.Name = "txtFile";
            txtFile.ReadOnly = true;
            txtFile.Size = new Size(256, 20);
            txtFile.TabIndex = 1;
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Location = new Point(142, 44);
            Label2.Name = "Label2";
            Label2.Size = new Size(44, 13);
            Label2.TabIndex = 5;
            Label2.Text = "Chapter";
            // 
            // txtChapter
            // 
            txtChapter.Location = new Point(201, 41);
            txtChapter.Name = "txtChapter";
            txtChapter.Size = new Size(37, 20);
            txtChapter.TabIndex = 6;
            // 
            // Html
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(295, 104);
            Controls.Add(txtChapter);
            Controls.Add(Label2);
            Controls.Add(txtFile);
            Controls.Add(btnClean);
            Controls.Add(txtPrefix);
            Controls.Add(Label1);
            Controls.Add(btnBrowse);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Html";
            Text = "frmHtml";
            ResumeLayout(false);
            PerformLayout();

        }
        internal Button btnBrowse;
        internal Label Label1;
        internal TextBox txtPrefix;
        internal Button btnClean;
        internal TextBox txtFile;
        internal Label Label2;
        internal TextBox txtChapter;
        internal OpenFileDialog OpenFile;
    }
}