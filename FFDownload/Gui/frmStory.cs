using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace HtmlGrabber
{
    public partial class Story
    {

        public Debug myCaller;

        public Story()
        {
            InitializeComponent();
        }

        public void RefreshData()
        {

            int row_idx;
            DataTable dt;
            string temp;

            string Title;
            string Author;
            string AuthorLink;
            string Folder;
            string Count;
            string StoryID;
            bool Abandoned;
            bool Complete;
            DateTime PubDate;
            DateTime UpdDate;
            string Matchup;
            string Crossover;
            string Description;

            LinkLabel.Link link;

            row_idx = myCaller.grdDB.CurrentRow.Index;
            dt = (DataTable)myCaller.grdDB.DataSource;

            Title = dt.Rows[row_idx]["Title"].ToString();
            Author = dt.Rows[row_idx]["Author"].ToString();
            Folder = dt.Rows[row_idx]["Folder"].ToString();
            Count = dt.Rows[row_idx]["Count"].ToString();
            StoryID = dt.Rows[row_idx]["StoryID"].ToString();

            temp = dt.Rows[row_idx]["Abandoned"].ToString();

            if (string.IsNullOrEmpty(temp))
            {
                Abandoned = false;
            }
            else
            {
                Abandoned = Conversions.ToBoolean(dt.Rows[row_idx]["Abandoned"]);
            }

            temp = dt.Rows[row_idx]["Complete"].ToString();

            if (string.IsNullOrEmpty(temp))
            {
                Complete = false;
            }
            else
            {
                Complete = Conversions.ToBoolean(dt.Rows[row_idx]["Complete"]);
            }

            temp = dt.Rows[row_idx]["Publish_Date"].ToString();

            if (string.IsNullOrEmpty(temp))
            {
                PubDate = default;
            }
            else
            {
                PubDate = Conversions.ToDate(dt.Rows[row_idx]["Publish_Date"]);
            }

            temp = dt.Rows[row_idx]["Update_Date"].ToString();

            if (string.IsNullOrEmpty(temp))
            {
                UpdDate = default;
            }
            else
            {
                UpdDate = Conversions.ToDate(dt.Rows[row_idx]["Update_Date"]);
            }

            Matchup = dt.Rows[row_idx]["Matchup"].ToString();

            Crossover = dt.Rows[row_idx]["Crossover"].ToString();

            Description = dt.Rows[row_idx]["Description"].ToString();

            AuthorLink = dt.Rows[row_idx]["Internet"].ToString();

            if (!string.IsNullOrEmpty(AuthorLink))
            {
                AuthorLink = Strings.Split(AuthorLink, "#")[1];
            }

            txtTitle.Text = Title;

            link = new LinkLabel.Link();

            lnkAuthor.Text = Author;

            link.Start = 0;
            link.Length = Author.Length;
            link.LinkData = AuthorLink;

            lnkAuthor.Links.Clear();
            lnkAuthor.Links.Add(link);

            txtFolder.Text = Folder;
            txtCount.Text = Count;
            txtStoryID.Text = StoryID;
            chkAbandoned.Checked = Abandoned;
            chkComplete.Checked = Complete;
            txtPublish.Text = PubDate.ToString("MM/dd/yyyy");
            txtUpdate.Text = UpdDate.ToString("MM/dd/yyyy");
            txtMatchup.Text = Matchup;
            txtCrossover.Text = Crossover;
            txtDescription.Text = Description;



        }

        private void lnkAuthor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)



        {

            string target = Conversions.ToString(e.Link.LinkData);
            Process.Start(target);

        }

    }
}