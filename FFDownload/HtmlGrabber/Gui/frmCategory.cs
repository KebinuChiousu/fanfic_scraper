using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace HtmlGrabber
{
    public partial class frmCategory
    {

        private DAL DAL;

        public frmCategory()
        {
            InitializeComponent();
        }

        private void grdDB_CurrentCellChanged(object sender, EventArgs e)


        {

            DataGridView grdDB = (DataGridView)sender;
            DataTable dt;

            dt = (DataTable)grdDB.DataSource;

            if (dt.Rows.Count > 0)
            {

                if (!(grdDB.CurrentRow == null))
                {

                    lblStatus.Text = "(" + (grdDB.CurrentRow.Index + 1) + " of " + dt.Rows.Count + ")";

                }
            }

            else
            {
                lblStatus.Text = "(0 of 0)";
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)


        {

            string Category;
            DataTable dt = (DataTable)grdDB.DataSource;
            DataRow dr;
            int NewRow = dt.Rows.Count;

            Category = Interaction.InputBox("Enter Category Name");

            if (!string.IsNullOrEmpty(Category))
            {
                if (!DAL.CategoryExists(Category))
                {

                    dr = dt.NewRow();

                    dr["Name"] = Category;

                    dt.Rows.Add(dr);

                    grdDB.DataSource = dt;

                    SetCurrentRow(NewRow);
                }

                else
                {
                    Interaction.MsgBox("Record already exists!", Constants.vbExclamation);
                }
            }

        }

        private void btnEdit_Click(object sender, EventArgs e)


        {

            string oldCategory;
            string Category;
            DataTable dt = (DataTable)grdDB.DataSource;
            int idx = grdDB.CurrentRow.Index;
            DataRow dr;

            oldCategory = Conversions.ToString(grdDB.CurrentCell.Value);

            Category = Interaction.InputBox("Enter Category Name", DefaultResponse: oldCategory);

            if (!string.IsNullOrEmpty(Category))
            {
                if (!DAL.CategoryExists(Category))
                {

                    dr = dt.Rows[idx];

                    dr["Name"] = Category;

                    grdDB.DataSource = dt;

                    SetCurrentRow(idx);
                }

                else
                {
                    Interaction.MsgBox("Record already exists!", Constants.vbExclamation);
                }
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)


        {

            string Category;
            int CategoryIdx;
            int iYesNo;
            DataTable dt = (DataTable)grdDB.DataSource;
            int idx = grdDB.CurrentRow.Index;
            DataRow dr;

            Category = Conversions.ToString(grdDB.CurrentCell.Value);

            dr = dt.Rows[idx];

            CategoryIdx = Conversions.ToInteger(dr["id"]);

            iYesNo = (int)Interaction.MsgBox("Delete Record: " + Category + "?", MsgBoxStyle.YesNo);

            if (iYesNo == (int)Constants.vbYes)
            {
                if (!DAL.RecordExists(CategoryIdx))
                {

                    dt.Rows.Remove(dr);

                    grdDB.DataSource = dt;
                }

                else
                {
                    Interaction.MsgBox("Unable to delete record. Stories exist under this category!", Constants.vbExclamation);
                }
            }


        }

        private void btnUpdate_Click(object sender, EventArgs e)


        {

            int ret;

            DataTable dt;

            dt = (DataTable)grdDB.DataSource;

            if (!(dt.GetChanges() == null))
            {
                ret = UpdateData(dt);
                if (ret > 0)
                {
                    Interaction.MsgBox("Database updated sucessfully!");
                }
                else
                {
                    Interaction.MsgBox("No changes detected...");
                }
            }

            if (dt.Rows.Count > 0)
            {
                FormManagement.frmDebug.FillCategories();
            }

            RefreshDB();

        }

        #region Form Handling Routines

        private void frmCategory_Load(object sender, EventArgs e)


        {

            DAL = FormManagement.frmDebug.DAL;

            RefreshDB();

        }

        public void SetCurrentRow(int Index)
        {

            grdDB.CurrentCell = grdDB.Rows[Index].Cells[1];

        }

        #endregion

        #region Data Routines

        private DataTable GetCategories()
        {

            DataTable dt;

            if (!(DAL == null))
            {
                dt = DAL.GetCategories();
            }
            else
            {
                dt = null;
            }

            return dt;

        }

        private int UpdateData(DataTable dt)
        {

            int result = 0;

            result = DAL.UpdateCategories(ref dt);

            return result;

        }

        #endregion

        private void RefreshDB()
        {

            DataTable dt;

            dt = GetCategories();

            grdDB.DataSource = dt;
            grdDB.Columns["Id"].Visible = false;

            if (dt.Rows.Count == 0)
            {
                lblStatus.Text = "(0 of 0)";
            }
            else
            {
                SetCurrentRow(0);
            }

        }



    }
}