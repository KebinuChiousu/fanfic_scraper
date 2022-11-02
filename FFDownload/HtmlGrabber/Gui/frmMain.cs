using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace HtmlGrabber
{


    public class HtmlGrabber : Form
    {

        #region  Windows Form Designer generated code 

        public HtmlGrabber() : base()
        {
            Load += HtmlGrabber_Load;
            Closing += HtmlGrabber_Closing;

            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call

        }

        // Form overrides dispose to clean up the component list.
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components is not null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        private Button _btnURL;

        internal virtual Button btnURL
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnURL;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnURL != null)
                {
                    _btnURL.Click -= btnURL_Click;
                }

                _btnURL = value;
                if (_btnURL != null)
                {
                    _btnURL.Click += btnURL_Click;
                }
            }
        }
        private TextBox _txtUrl;

        internal virtual TextBox txtUrl
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtUrl;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _txtUrl = value;
            }
        }
        private TextBox _txtSource;

        internal virtual TextBox txtSource
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtSource;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _txtSource = value;
            }
        }
        private ListBox _ListChapters;

        internal virtual ListBox ListChapters
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ListChapters;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _ListChapters = value;
            }
        }
        private Label _lblPublish;

        internal virtual Label lblPublish
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblPublish;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblPublish = value;
            }
        }
        private Label _lblUpdate;

        internal virtual Label lblUpdate
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblUpdate;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblUpdate = value;
            }
        }
        private TextBox _txtFileMask;

        internal virtual TextBox txtFileMask
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtFileMask;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _txtFileMask = value;
            }
        }
        private Label _Label1;

        internal virtual Label Label1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _Label1 = value;
            }
        }
        private Label _Label2;

        internal virtual Label Label2
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label2;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _Label2 = value;
            }
        }
        private Label _lblChapterCount;

        internal virtual Label lblChapterCount
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblChapterCount;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblChapterCount = value;
            }
        }
        private Label _lblProgress;

        internal virtual Label lblProgress
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblProgress;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblProgress = value;
            }
        }
        private TextBox _txtStart;

        internal virtual TextBox txtStart
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtStart;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _txtStart = value;
            }
        }
        private Button _btnRSS;

        internal virtual Button btnRSS
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnRSS;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnRSS != null)
                {
                    _btnRSS.Click -= btnRSS_Click;
                }

                _btnRSS = value;
                if (_btnRSS != null)
                {
                    _btnRSS.Click += btnRSS_Click;
                }
            }
        }
        private TextBox _urlAtom;

        internal virtual TextBox urlAtom
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _urlAtom;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _urlAtom = value;
            }
        }
        private ComboBox _cmbStory;

        internal virtual ComboBox cmbStory
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbStory;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbStory != null)
                {
                    _cmbStory.SelectedIndexChanged -= Story_Selected;
                }

                _cmbStory = value;
                if (_cmbStory != null)
                {
                    _cmbStory.SelectedIndexChanged += Story_Selected;
                }
            }
        }
        private ListBox _lstStory;

        internal virtual ListBox lstStory
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lstStory;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lstStory = value;
            }
        }
        private ComboBox _cmbType;

        internal virtual ComboBox cmbType
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbType;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbType != null)
                {
                    _cmbType.SelectedIndexChanged -= Source_Changed;
                }

                _cmbType = value;
                if (_cmbType != null)
                {
                    _cmbType.SelectedIndexChanged += Source_Changed;
                }
            }
        }
        private Label _Label6;

        internal virtual Label Label6
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label6;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _Label6 = value;
            }
        }
        private Label _lblStory;

        internal virtual Label lblStory
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblStory;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblStory = value;
            }
        }
        private Label _lblStart;

        internal virtual Label lblStart
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblStart;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblStart = value;
            }
        }
        private Label _lblAnime;

        internal virtual Label lblAnime
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblAnime;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblAnime = value;
            }
        }
        private Button _btnDebug;

        internal virtual Button btnDebug
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnDebug;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnDebug != null)
                {
                    _btnDebug.Click -= ShowDebugWindow;
                }

                _btnDebug = value;
                if (_btnDebug != null)
                {
                    _btnDebug.Click += ShowDebugWindow;
                }
            }
        }
        private TextBox _lblAuthor;

        internal virtual TextBox lblAuthor
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblAuthor;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblAuthor = value;
            }
        }
        private TextBox _lblStoryID;

        internal virtual TextBox lblStoryID
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblStoryID;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblStoryID = value;
            }
        }
        private Label _lblAtom;

        internal virtual Label lblAtom
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblAtom;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblAtom = value;
            }
        }
        private Button _btnHtml;

        internal virtual Button btnHtml
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnHtml;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnHtml != null)
                {
                    _btnHtml.Click -= btnHtml_Click;
                }

                _btnHtml = value;
                if (_btnHtml != null)
                {
                    _btnHtml.Click += btnHtml_Click;
                }
            }
        }
        private TextBox _lblTitle;

        internal virtual TextBox lblTitle
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblTitle;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblTitle = value;
            }
        }





        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(HtmlGrabber));
            _btnURL = new Button();
            _btnURL.Click += new EventHandler(btnURL_Click);
            _txtUrl = new TextBox();
            _txtSource = new TextBox();
            _ListChapters = new ListBox();
            _lblPublish = new Label();
            _lblUpdate = new Label();
            _txtFileMask = new TextBox();
            _Label1 = new Label();
            _Label2 = new Label();
            _lblChapterCount = new Label();
            _lblProgress = new Label();
            _lblStart = new Label();
            _txtStart = new TextBox();
            _btnRSS = new Button();
            _btnRSS.Click += new EventHandler(btnRSS_Click);
            _urlAtom = new TextBox();
            _lblAtom = new Label();
            _cmbStory = new ComboBox();
            _cmbStory.SelectedIndexChanged += new EventHandler(Story_Selected);
            _lblStory = new Label();
            _lstStory = new ListBox();
            _cmbType = new ComboBox();
            _cmbType.SelectedIndexChanged += new EventHandler(Source_Changed);
            _Label6 = new Label();
            _lblAnime = new Label();
            _btnDebug = new Button();
            _btnDebug.Click += new EventHandler(ShowDebugWindow);
            _lblTitle = new TextBox();
            _lblAuthor = new TextBox();
            _lblStoryID = new TextBox();
            _btnHtml = new Button();
            _btnHtml.Click += new EventHandler(btnHtml_Click);
            SuspendLayout();
            // 
            // btnURL
            // 
            _btnURL.Location = new Point(52, 587);
            _btnURL.Name = "_btnURL";
            _btnURL.Size = new Size(266, 78);
            _btnURL.TabIndex = 1;
            _btnURL.Text = "Get Chapters";
            // 
            // txtUrl
            // 
            _txtUrl.Location = new Point(52, 729);
            _txtUrl.Name = "_txtUrl";
            _txtUrl.Size = new Size(520, 31);
            _txtUrl.TabIndex = 2;
            // 
            // txtSource
            // 
            _txtSource.Location = new Point(54, 205);
            _txtSource.MaxLength = 9999999;
            _txtSource.Multiline = true;
            _txtSource.Name = "_txtSource";
            _txtSource.ReadOnly = true;
            _txtSource.ScrollBars = ScrollBars.Vertical;
            _txtSource.Size = new Size(1332, 349);
            _txtSource.TabIndex = 3;
            // 
            // ListChapters
            // 
            _ListChapters.ItemHeight = 25;
            _ListChapters.Location = new Point(320, 486);
            _ListChapters.Name = "_ListChapters";
            _ListChapters.Size = new Size(774, 4);
            _ListChapters.TabIndex = 5;
            _ListChapters.Visible = false;
            // 
            // lblPublish
            // 
            _lblPublish.Location = new Point(1054, 65);
            _lblPublish.Name = "_lblPublish";
            _lblPublish.Size = new Size(332, 49);
            _lblPublish.TabIndex = 6;
            _lblPublish.Text = "Published Date";
            _lblPublish.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblUpdate
            // 
            _lblUpdate.Location = new Point(1054, 114);
            _lblUpdate.Name = "_lblUpdate";
            _lblUpdate.Size = new Size(332, 52);
            _lblUpdate.TabIndex = 7;
            _lblUpdate.Text = "Updated Date";
            _lblUpdate.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtFileMask
            // 
            _txtFileMask.Location = new Point(346, 626);
            _txtFileMask.Name = "_txtFileMask";
            _txtFileMask.Size = new Size(226, 31);
            _txtFileMask.TabIndex = 9;
            // 
            // Label1
            // 
            _Label1.Location = new Point(358, 587);
            _Label1.Name = "_Label1";
            _Label1.Size = new Size(174, 39);
            _Label1.TabIndex = 11;
            _Label1.Text = "File Prefix";
            // 
            // Label2
            // 
            _Label2.Location = new Point(52, 703);
            _Label2.Name = "_Label2";
            _Label2.Size = new Size(254, 26);
            _Label2.TabIndex = 12;
            _Label2.Text = "Fanfiction Story Url";
            // 
            // lblChapterCount
            // 
            _lblChapterCount.Location = new Point(54, 140);
            _lblChapterCount.Name = "_lblChapterCount";
            _lblChapterCount.Size = new Size(186, 41);
            _lblChapterCount.TabIndex = 13;
            // 
            // lblProgress
            // 
            _lblProgress.Location = new Point(586, 127);
            _lblProgress.Name = "_lblProgress";
            _lblProgress.Size = new Size(468, 65);
            _lblProgress.TabIndex = 14;
            _lblProgress.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblStart
            // 
            _lblStart.Location = new Point(266, 140);
            _lblStart.Name = "_lblStart";
            _lblStart.Size = new Size(188, 41);
            _lblStart.TabIndex = 15;
            _lblStart.Text = "Start @ Chapter: ";
            _lblStart.TextAlign = ContentAlignment.MiddleCenter;
            _lblStart.Visible = false;
            // 
            // txtStart
            // 
            _txtStart.Location = new Point(454, 140);
            _txtStart.Name = "_txtStart";
            _txtStart.Size = new Size(120, 31);
            _txtStart.TabIndex = 16;
            _txtStart.Visible = false;
            // 
            // btnRSS
            // 
            _btnRSS.Location = new Point(652, 587);
            _btnRSS.Name = "_btnRSS";
            _btnRSS.Size = new Size(174, 76);
            _btnRSS.TabIndex = 17;
            _btnRSS.Text = "Obtain Feed";
            // 
            // urlAtom
            // 
            _urlAtom.Location = new Point(852, 626);
            _urlAtom.Name = "_urlAtom";
            _urlAtom.Size = new Size(520, 31);
            _urlAtom.TabIndex = 18;
            // 
            // lblAtom
            // 
            _lblAtom.Location = new Point(852, 587);
            _lblAtom.Name = "_lblAtom";
            _lblAtom.Size = new Size(520, 33);
            _lblAtom.TabIndex = 19;
            _lblAtom.Text = "Atom Feed or Author URL";
            // 
            // cmbStory
            // 
            _cmbStory.Location = new Point(652, 729);
            _cmbStory.Name = "_cmbStory";
            _cmbStory.Size = new Size(720, 33);
            _cmbStory.TabIndex = 21;
            // 
            // lblStory
            // 
            _lblStory.Location = new Point(652, 703);
            _lblStory.Name = "_lblStory";
            _lblStory.Size = new Size(254, 26);
            _lblStory.TabIndex = 22;
            _lblStory.Text = "Fanfiction Story List";
            // 
            // lstStory
            // 
            _lstStory.ItemHeight = 25;
            _lstStory.Location = new Point(320, 244);
            _lstStory.Name = "_lstStory";
            _lstStory.Size = new Size(774, 4);
            _lstStory.TabIndex = 23;
            _lstStory.Visible = false;
            // 
            // cmbType
            // 
            _cmbType.Items.AddRange(new object[] { "FFNet", "AFF", "FicWad", "MediaMiner", "HPFFA" });
            _cmbType.Location = new Point(734, 13);
            _cmbType.Name = "_cmbType";
            _cmbType.Size = new Size(252, 33);
            _cmbType.TabIndex = 24;
            // 
            // Label6
            // 
            _Label6.Location = new Point(560, 13);
            _Label6.Name = "_Label6";
            _Label6.Size = new Size(226, 39);
            _Label6.TabIndex = 25;
            _Label6.Text = "Download Site:";
            _Label6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblAnime
            // 
            _lblAnime.Location = new Point(174, 358);
            _lblAnime.Name = "_lblAnime";
            _lblAnime.Size = new Size(1066, 52);
            _lblAnime.TabIndex = 27;
            _lblAnime.Visible = false;
            // 
            // btnDebug
            // 
            _btnDebug.Location = new Point(6, 2);
            _btnDebug.Name = "_btnDebug";
            _btnDebug.Size = new Size(80, 44);
            _btnDebug.TabIndex = 28;
            _btnDebug.Text = "Auto";
            // 
            // lblTitle
            // 
            _lblTitle.Location = new Point(48, 74);
            _lblTitle.MaxLength = 99;
            _lblTitle.Name = "_lblTitle";
            _lblTitle.ReadOnly = true;
            _lblTitle.ScrollBars = ScrollBars.Vertical;
            _lblTitle.Size = new Size(592, 31);
            _lblTitle.TabIndex = 31;
            // 
            // lblAuthor
            // 
            _lblAuthor.Location = new Point(672, 74);
            _lblAuthor.MaxLength = 99;
            _lblAuthor.Name = "_lblAuthor";
            _lblAuthor.ReadOnly = true;
            _lblAuthor.ScrollBars = ScrollBars.Vertical;
            _lblAuthor.Size = new Size(334, 31);
            _lblAuthor.TabIndex = 32;
            // 
            // lblStoryID
            // 
            _lblStoryID.Location = new Point(1154, 15);
            _lblStoryID.MaxLength = 99;
            _lblStoryID.Name = "_lblStoryID";
            _lblStoryID.ReadOnly = true;
            _lblStoryID.ScrollBars = ScrollBars.Vertical;
            _lblStoryID.Size = new Size(232, 31);
            _lblStoryID.TabIndex = 33;
            // 
            // btnHtml
            // 
            _btnHtml.Location = new Point(88, 2);
            _btnHtml.Name = "_btnHtml";
            _btnHtml.Size = new Size(80, 44);
            _btnHtml.TabIndex = 34;
            _btnHtml.Text = "Html";
            // 
            // HtmlGrabber
            // 
            AutoScaleBaseSize = new Size(10, 24);
            ClientSize = new Size(1413, 784);
            Controls.Add(_btnHtml);
            Controls.Add(_lblStoryID);
            Controls.Add(_lblAuthor);
            Controls.Add(_lblTitle);
            Controls.Add(_urlAtom);
            Controls.Add(_txtStart);
            Controls.Add(_txtFileMask);
            Controls.Add(_txtUrl);
            Controls.Add(_txtSource);
            Controls.Add(_btnDebug);
            Controls.Add(_lblAnime);
            Controls.Add(_lblStory);
            Controls.Add(_cmbType);
            Controls.Add(_Label6);
            Controls.Add(_lstStory);
            Controls.Add(_cmbStory);
            Controls.Add(_lblAtom);
            Controls.Add(_btnRSS);
            Controls.Add(_lblStart);
            Controls.Add(_lblProgress);
            Controls.Add(_lblChapterCount);
            Controls.Add(_Label2);
            Controls.Add(_Label1);
            Controls.Add(_lblUpdate);
            Controls.Add(_lblPublish);
            Controls.Add(_ListChapters);
            Controls.Add(_btnURL);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "HtmlGrabber";
            Text = "Fanfiction.net Story Grabber";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        // Declarations used within class
        public string clsname;

        private string Title;
        private DataSet dsRSS;

        private int i;

        #region UI Update Code

        public void ClearUI()
        {

            lblTitle.Text = "";
            lblAuthor.Text = "";
            lblPublish.Text = "";
            lblUpdate.Text = "";
            lblChapterCount.Text = "";
            txtStart.Visible = false;
            lblStart.Visible = false;
            lblProgress.Text = "";
            txtSource.Text = "";
            txtFileMask.Text = "";

            btnURL.Text = "Get Chapters";
            txtUrl.Text = "";

            ListChapters.Items.Clear();
            lstStory.Items.Clear();
            cmbStory.Items.Clear();
            cmbStory.Text = "";
            urlAtom.Text = "";

            Application.DoEvents();

        }

        public void UpdateUI(ref clsFanfic.Story fic, ref string Result, int Start = 1)
        {

            int idx;

            btnURL.Text = "Process Chapters";

            lblChapterCount.Text = fic.ChapterCount;

            var loopTo = Information.UBound(fic.Chapters);
            for (idx = 0; idx <= loopTo; idx++)
                ListChapters.Items.Add(fic.Chapters[idx]);

            lblStoryID.Text = fic.ID;
            lblTitle.Text = fic.Title;
            lblAuthor.Text = fic.Author;
            lblPublish.Text = fic.PublishDate;
            lblUpdate.Text = fic.UpdateDate;

            lblChapterCount.Text = fic.ChapterCount;
            lblProgress.Text = "< -- Enter Starting Chapter";
            txtStart.Text = Start.ToString();

            txtSource.Text = Result;

            lblStart.Visible = true;
            txtStart.Visible = true;

            Application.DoEvents();

        }

        #endregion

        #region Interface Code

        private void btnURL_Click(object sender, EventArgs e)


        {

            string link;
            link = txtUrl.Text;

            if (CheckURL(link))
            {
                DownloadData();
            }
            else
            {
                Interaction.MsgBox("Site: " + link + " is currently not supported.", MsgBoxStyle.Information);
            }

        }

        private void btnRSS_Click(object sender, EventArgs e)


        {

            string link;
            link = urlAtom.Text;

            if (CheckURL(link))
            {
                ObtainFeed(urlAtom.Text);
            }
            else
            {
                Interaction.MsgBox("Site: " + link + " is currently not supported.", MsgBoxStyle.Information);
            }

        }

        private void Story_Selected(object sender, EventArgs e)


        {

            LoadStoryInfo(cmbStory.SelectedIndex);

        }

        private void Source_Changed(object sender, EventArgs e)


        {

            clsname = Conversions.ToString(cmbType.Items[cmbType.SelectedIndex]);

            ClearUI();

            LoadSiteByName(clsname);

        }

        private void HtmlGrabber_Load(object sender, EventArgs e)


        {

            cmbType.SelectedIndex = 0;
            FormManagement.frmMain = this;

        }

        private void HtmlGrabber_Closing(object sender, System.ComponentModel.CancelEventArgs e)



        {
            Application.Exit();
        }

        private void ShowDebugWindow(object sender, EventArgs e)


        {

            FormManagement.Initialize(FormManagement.forms.frmDebug);


            FormManagement.frmDebug.myCaller = this;

            FormManagement.PlaceDebugWindow();
            FormManagement.frmDebug.Show();

        }

        private void btnHtml_Click(object sender, EventArgs e)


        {

            FormManagement.Initialize(FormManagement.forms.frmHtml);


            FormManagement.PlaceHtmlWindow();
            FormManagement.frmHtml.Show();

        }

        #endregion

        #region Business Logic




        public void LoadStoryInfo(int idx)
        {

            btnURL.Text = "Get Chapters";

            clsFanfic.Story fic;

            fic = modMain.BL.GrabStoryInfo(idx);

            // Story Name
            lblTitle.Text = fic.Title;

            // Story Author
            lblAuthor.Text = fic.Author;

            // Story Location
            txtUrl.Text = fic.StoryURL;

            // Story ID
            lblStoryID.Text = fic.ID;

            // Category
            lblProgress.Text = fic.Category;
            lblAnime.Text = lblProgress.Text;

            // Chapter Count
            lblChapterCount.Text = fic.ChapterCount;

            // Last Updated
            lblUpdate.Text = fic.UpdateDate;

            // Published
            lblPublish.Text = fic.PublishDate;

            // Summary

            txtSource.Text = fic.Summary;

        }

        public void DownloadData()
        {

            bool ret;
            clsFanfic.Story fic;

            switch (btnURL.Text ?? "")
            {
                case "Get Chapters":
                    {
                        ret = modMain.BL.GetChapters(txtUrl.Text);
                        if (!ret)
                            goto oops;

                        fic = modMain.BL.FanFic;

                        string argResult = modMain.BL.Result;
                        UpdateUI(ref fic, ref argResult);
                        break;
                    }

                case "Process Chapters":
                    {

                        modMain.BL.ProcessChapters(txtUrl.Text, Conversions.ToInteger(txtStart.Text), Conversions.ToInteger(lblChapterCount.Text), txtFileMask.Text);





                        ClearUI();
                        break;
                    }

            }

            return;

        oops:
            ;

            txtSource.Text = modMain.BL.Result;
            Interaction.MsgBox("Valid URL Must be Entered", MsgBoxStyle.Information);

        }

        public void UpdateProgess(string msg)
        {

            lblProgress.Text = msg;
            Application.DoEvents();

        }

        public void ObtainFeed(string link)
        {
            // Download List of stories for Given Author

            int idx;
            // Dim sumcount As Integer

            clsFanfic.Story fic;

            txtSource.Text = "";
            cmbStory.Items.Clear();
            lstStory.Items.Clear();

            dsRSS = null;

            if (string.IsNullOrEmpty(link))
                goto abort;

            dsRSS = modMain.BL.GrabFeed(link);

            if (dsRSS == null)
                goto abort;

            urlAtom.Text = link;

            btnURL.Text = "Get Chapters";
            ListChapters.Items.Clear();

            // Send Information to Debug Console
            FormManagement.Initialize(FormManagement.forms.frmDebug);


            FormManagement.frmDebug.UpdateRSS(dsRSS);

            var loopTo = dsRSS.Tables[0].Rows.Count - 1;
            for (idx = 0; idx <= loopTo; idx++)
            {

                fic = modMain.BL.GrabStoryInfo(idx);

                // Story Title
                cmbStory.Items.Add(fic.Title);

                // Story Location
                lstStory.Items.Add(fic.StoryURL);

            }

            // Load Info for First Story
            cmbStory.SelectedIndex = 0;

            return;

        abort:
            ;

            urlAtom.Text = "";
            Interaction.MsgBox("Valid Feed Must Be Entered.", MsgBoxStyle.Information, Text);

            return;

        }

        #region Site Logic

        public bool CheckURL(string link)
        {

            bool ret;

            ret = modMain.BL.CheckUrl(ref link);

            if (ret)
            {
                LoadSiteByName(modMain.BL.Name, true);
            }
            else
            {
                ret = LoadSiteByName(cmbType.Text, false);
            }

            return ret;

        }

        public bool LoadSiteByName(string clsname, bool bypass = false)
        {

            bool ret = false;
            string host = "";

            ret = modMain.BL.LoadSiteByName(clsname);

            if (ret)
            {
                Text = modMain.cls.HostName + " - Story Downloader";
                lblAtom.Text = "Valid Author URL";
            }
            else
            {
                Text = "Story Downloader";
            }

            return ret;

        }

        #endregion

        #endregion

    }
}