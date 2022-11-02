using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;
using NHibernate.Mapping;
using Array = System.Array;

namespace HtmlGrabber
{

    public partial class Html
    {

        private clsBL BL = new clsBL();

        public Html()
        {
            InitializeComponent();
        }

        #region Drag and Drop / File Browsing

        private void txtFile_DragDrop(object sender, DragEventArgs e)
        {

            var data = e.Data.GetData(DataFormats.FileDrop);
            List<string> ret = new();

            if (data is Array)
                ret = ((string[])data).ToList();
            
            txtFile.Text = ret[0].ToString();
        }

        private void txtFile_DragEnter(object sender, DragEventArgs e)



        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {

            OpenFile.FileName = "";
            OpenFile.Filter = "HTML (*.htm;*.html)|*.htm;*.html";
            OpenFile.ShowDialog();

            txtFile.Text = OpenFile.FileName;

        }

        #endregion

        private void btnClean_Click(object sender, EventArgs e)
        {

            FileStream fs;
            string html;

            fs = File.OpenRead(txtFile.Text);

            StreamReader sr;

            sr = new StreamReader(fs, System.Text.Encoding.UTF8);



            html = sr.ReadToEnd();

            sr.Close();
            sr.Dispose();

            modHTML.CleanHTML(ref html);

            BL.ProcessChapter(ref html, txtPrefix.Text, Conversions.ToInteger(txtChapter.Text));

        }


    }
}