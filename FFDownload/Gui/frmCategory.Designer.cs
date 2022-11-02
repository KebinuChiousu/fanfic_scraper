using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace HtmlGrabber
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class frmCategory : Form
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCategory));
            btnAdd = new Button();
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnDelete = new Button();
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnUpdate = new Button();
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            grdDB = new DataGridView();
            grdDB.CurrentCellChanged += new EventHandler(grdDB_CurrentCellChanged);
            btnEdit = new Button();
            btnEdit.Click += new EventHandler(btnEdit_Click);
            lblStatus = new Label();
            ((System.ComponentModel.ISupportInitialize)grdDB).BeginInit();
            SuspendLayout();
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(14, 193);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(66, 23);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(158, 193);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(66, 23);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(112, 222);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(112, 23);
            btnUpdate.TabIndex = 4;
            btnUpdate.Text = "Update Database";
            btnUpdate.UseVisualStyleBackColor = true;
            // 
            // grdDB
            // 
            grdDB.AllowUserToAddRows = false;
            grdDB.AllowUserToDeleteRows = false;
            grdDB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grdDB.EditMode = DataGridViewEditMode.EditProgrammatically;
            grdDB.Location = new Point(14, 12);
            grdDB.Name = "grdDB";
            grdDB.Size = new Size(210, 175);
            grdDB.TabIndex = 5;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(86, 193);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(66, 23);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(18, 227);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 13);
            lblStatus.TabIndex = 6;
            // 
            // frmCategory
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(236, 257);
            Controls.Add(lblStatus);
            Controls.Add(grdDB);
            Controls.Add(btnUpdate);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);
            Controls.Add(btnAdd);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmCategory";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Edit Categories";
            ((System.ComponentModel.ISupportInitialize)grdDB).EndInit();
            Load += new EventHandler(frmCategory_Load);
            ResumeLayout(false);
            PerformLayout();

        }
        internal Button btnAdd;
        internal Button btnDelete;
        internal Button btnUpdate;
        internal DataGridView grdDB;
        internal Button btnEdit;
        internal Label lblStatus;
    }
}