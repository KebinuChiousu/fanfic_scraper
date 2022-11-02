using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HtmlScraper.BusinessLogic.Sites.Base;
using HtmlScraper.Data.DAL;
using HtmlScraper.Data.DAL.nHibernate.Database;
using HtmlScraper.Data.Ini;
using HtmlScraper.Utility;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace HtmlScraper.Gui
{

    public class Debug : Form
    {

        #region  Windows Form Designer generated code 

        public Debug() : base()
        {
            Load += frmDebug_Load;
            Closing += frmDebug_Closing;

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
        private DataGridView _grdRSS;

        internal virtual DataGridView grdRSS
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _grdRSS;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _grdRSS = value;
            }
        }
        private Button _btnUpdateDB;

        internal virtual Button btnUpdateDB
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnUpdateDB;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnUpdateDB != null)
                {
                    _btnUpdateDB.Click -= UpdateDateBase;
                }

                _btnUpdateDB = value;
                if (_btnUpdateDB != null)
                {
                    _btnUpdateDB.Click += UpdateDateBase;
                }
            }
        }
        private Button _btnUpdateRecord;

        internal virtual Button btnUpdateRecord
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnUpdateRecord;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnUpdateRecord != null)
                {
                    _btnUpdateRecord.Click -= UpdateRec;
                }

                _btnUpdateRecord = value;
                if (_btnUpdateRecord != null)
                {
                    _btnUpdateRecord.Click += UpdateRec;
                }
            }
        }
        private ComboBox _cmbSearch;

        internal virtual ComboBox cmbSearch
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbSearch;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbSearch != null)
                {
                    _cmbSearch.SelectedIndexChanged -= cmbProcess_SelectedIndexChanged;
                }

                _cmbSearch = value;
                if (_cmbSearch != null)
                {
                    _cmbSearch.SelectedIndexChanged += cmbProcess_SelectedIndexChanged;
                }
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
        private Button _btnBatch;

        internal virtual Button btnBatch
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnBatch;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnBatch != null)
                {
                    _btnBatch.Click -= ProcessBatch;
                }

                _btnBatch = value;
                if (_btnBatch != null)
                {
                    _btnBatch.Click += ProcessBatch;
                }
            }
        }
        private Button _btnOpenDB;

        internal virtual Button btnOpenDB
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnOpenDB;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnOpenDB != null)
                {
                    _btnOpenDB.Click -= btnOpenDB_Click;
                }

                _btnOpenDB = value;
                if (_btnOpenDB != null)
                {
                    _btnOpenDB.Click += btnOpenDB_Click;
                }
            }
        }
        private Button _btnPath;

        internal virtual Button btnPath
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnPath;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnPath != null)
                {
                    _btnPath.Click -= btnPath_Click;
                }

                _btnPath = value;
                if (_btnPath != null)
                {
                    _btnPath.Click += btnPath_Click;
                }
            }
        }
        private System.Windows.Forms.Timer _tmrDownload;

        internal virtual System.Windows.Forms.Timer tmrDownload
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _tmrDownload;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_tmrDownload != null)
                {
                    _tmrDownload.Tick -= tmrDownload_Tick;
                }

                _tmrDownload = value;
                if (_tmrDownload != null)
                {
                    _tmrDownload.Tick += tmrDownload_Tick;
                }
            }
        }
        private Button _btnSearch;

        internal virtual Button btnSearch
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnSearch;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnSearch != null)
                {
                    _btnSearch.Click -= btnSearch_Click;
                }

                _btnSearch = value;
                if (_btnSearch != null)
                {
                    _btnSearch.Click += btnSearch_Click;
                }
            }
        }
        private ComboBox _cmbChooseDB;

        internal virtual ComboBox cmbChooseDB
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbChooseDB;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _cmbChooseDB = value;
            }
        }
        private Label _lblStatus;

        internal virtual Label lblStatus
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblStatus;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _lblStatus = value;
            }
        }
        private DataGridView _grdDB;

        internal virtual DataGridView grdDB
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _grdDB;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_grdDB != null)
                {
                    _grdDB.CurrentCellChanged -= grdDB_CurrentCellChanged;
                    _grdDB.DoubleClick -= grdDB_DoubleClick;
                }

                _grdDB = value;
                if (_grdDB != null)
                {
                    _grdDB.CurrentCellChanged += grdDB_CurrentCellChanged;
                    _grdDB.DoubleClick += grdDB_DoubleClick;
                }
            }
        }
        private Button _btnCategory;

        internal virtual Button btnCategory
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnCategory;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnCategory != null)
                {
                    _btnCategory.Click -= btnCategory_Click;
                }

                _btnCategory = value;
                if (_btnCategory != null)
                {
                    _btnCategory.Click += btnCategory_Click;
                }
            }
        }
        private Button _btnSave;

        internal virtual Button btnSave
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnSave;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnSave != null)
                {
                    _btnSave.Click -= btnSave_Click;
                }

                _btnSave = value;
                if (_btnSave != null)
                {
                    _btnSave.Click += btnSave_Click;
                }
            }
        }
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Debug));
            _grdRSS = new DataGridView();
            _btnUpdateDB = new Button();
            _btnUpdateDB.Click += new EventHandler(UpdateDateBase);
            _btnUpdateRecord = new Button();
            _btnUpdateRecord.Click += new EventHandler(UpdateRec);
            _cmbSearch = new ComboBox();
            _cmbSearch.SelectedIndexChanged += new EventHandler(cmbProcess_SelectedIndexChanged);
            _Label1 = new Label();
            _btnBatch = new Button();
            _btnBatch.Click += new EventHandler(ProcessBatch);
            _btnOpenDB = new Button();
            _btnOpenDB.Click += new EventHandler(btnOpenDB_Click);
            _btnPath = new Button();
            _btnPath.Click += new EventHandler(btnPath_Click);
            _btnSave = new Button();
            _btnSave.Click += new EventHandler(btnSave_Click);
            _tmrDownload = new System.Windows.Forms.Timer(components);
            _tmrDownload.Tick += new EventHandler(tmrDownload_Tick);
            _btnSearch = new Button();
            _btnSearch.Click += new EventHandler(btnSearch_Click);
            _cmbChooseDB = new ComboBox();
            _lblStatus = new Label();
            _grdDB = new DataGridView();
            _grdDB.CurrentCellChanged += new EventHandler(grdDB_CurrentCellChanged);
            _grdDB.DoubleClick += new EventHandler(grdDB_DoubleClick);
            _btnCategory = new Button();
            _btnCategory.Click += new EventHandler(btnCategory_Click);
            ((System.ComponentModel.ISupportInitialize)_grdRSS).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_grdDB).BeginInit();
            SuspendLayout();
            // 
            // grdRSS
            // 
            _grdRSS.DataMember = "";
            //_grdRSS.HeaderForeColor = SystemColors.ControlText;
            _grdRSS.Location = new Point(14, 13);
            _grdRSS.Name = "_grdRSS";
            _grdRSS.Size = new Size(1424, 234);
            _grdRSS.TabIndex = 27;
            // 
            // btnUpdateDB
            // 
            _btnUpdateDB.Enabled = false;
            _btnUpdateDB.Location = new Point(1238, 587);
            _btnUpdateDB.Name = "_btnUpdateDB";
            _btnUpdateDB.Size = new Size(200, 39);
            _btnUpdateDB.TabIndex = 31;
            _btnUpdateDB.Text = "Update Database";
            // 
            // btnUpdateRecord
            // 
            _btnUpdateRecord.Enabled = false;
            _btnUpdateRecord.Location = new Point(1238, 635);
            _btnUpdateRecord.Name = "_btnUpdateRecord";
            _btnUpdateRecord.Size = new Size(200, 39);
            _btnUpdateRecord.TabIndex = 32;
            _btnUpdateRecord.Text = "Update Record";
            // 
            // cmbSearch
            // 
            _cmbSearch.Items.AddRange(new object[] { "Title", "Author", "Folder" });
            _cmbSearch.Location = new Point(710, 635);
            _cmbSearch.Name = "_cmbSearch";
            _cmbSearch.Size = new Size(168, 33);
            _cmbSearch.TabIndex = 33;
            // 
            // Label1
            // 
            _Label1.Location = new Point(704, 594);
            _Label1.Name = "_Label1";
            _Label1.Size = new Size(160, 26);
            _Label1.TabIndex = 34;
            _Label1.Text = "Search By";
            // 
            // btnBatch
            // 
            _btnBatch.Enabled = false;
            _btnBatch.Location = new Point(890, 635);
            _btnBatch.Name = "_btnBatch";
            _btnBatch.Size = new Size(336, 39);
            _btnBatch.TabIndex = 35;
            _btnBatch.Text = "Batch Download Updates";
            // 
            // btnOpenDB
            // 
            _btnOpenDB.Location = new Point(296, 633);
            _btnOpenDB.Name = "_btnOpenDB";
            _btnOpenDB.Size = new Size(104, 39);
            _btnOpenDB.TabIndex = 38;
            _btnOpenDB.Text = "Open";
            // 
            // btnPath
            // 
            _btnPath.Location = new Point(14, 596);
            _btnPath.Name = "_btnPath";
            _btnPath.Size = new Size(96, 76);
            _btnPath.TabIndex = 39;
            _btnPath.Text = "Set Paths";
            // 
            // btnSave
            // 
            _btnSave.Enabled = false;
            _btnSave.Location = new Point(412, 600);
            _btnSave.Name = "_btnSave";
            _btnSave.Size = new Size(144, 74);
            _btnSave.TabIndex = 40;
            _btnSave.Text = "Save Story to DB";
            // 
            // tmrDownload
            // 
            _tmrDownload.Interval = 1500;
            // 
            // btnSearch
            // 
            _btnSearch.Enabled = false;
            _btnSearch.Location = new Point(568, 600);
            _btnSearch.Name = "_btnSearch";
            _btnSearch.Size = new Size(124, 74);
            _btnSearch.TabIndex = 41;
            _btnSearch.Text = "Search DB";
            // 
            // cmbChooseDB
            // 
            _cmbChooseDB.FormattingEnabled = true;
            _cmbChooseDB.Location = new Point(122, 596);
            _cmbChooseDB.Name = "_cmbChooseDB";
            _cmbChooseDB.Size = new Size(278, 33);
            _cmbChooseDB.TabIndex = 42;
            // 
            // lblStatus
            // 
            _lblStatus.AutoSize = true;
            _lblStatus.Location = new Point(884, 598);
            _lblStatus.Name = "_lblStatus";
            _lblStatus.Size = new Size(47, 25);
            _lblStatus.TabIndex = 43;
            _lblStatus.Text = "DB:";
            // 
            // grdDB
            // 
            _grdDB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _grdDB.Location = new Point(14, 258);
            _grdDB.Name = "_grdDB";
            _grdDB.Size = new Size(1424, 318);
            _grdDB.TabIndex = 44;
            // 
            // btnCategory
            // 
            _btnCategory.Location = new Point(122, 635);
            _btnCategory.Name = "_btnCategory";
            _btnCategory.Size = new Size(164, 37);
            _btnCategory.TabIndex = 46;
            _btnCategory.Text = "Categories...";
            _btnCategory.UseVisualStyleBackColor = true;
            // 
            // Debug
            // 
            AutoScaleBaseSize = new Size(10, 24);
            ClientSize = new Size(1453, 685);
            Controls.Add(_btnCategory);
            Controls.Add(_grdDB);
            Controls.Add(_lblStatus);
            Controls.Add(_cmbChooseDB);
            Controls.Add(_btnSearch);
            Controls.Add(_btnSave);
            Controls.Add(_btnPath);
            Controls.Add(_btnOpenDB);
            Controls.Add(_btnBatch);
            Controls.Add(_Label1);
            Controls.Add(_cmbSearch);
            Controls.Add(_btnUpdateRecord);
            Controls.Add(_btnUpdateDB);
            Controls.Add(_grdRSS);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Debug";
            Text = "Download Automation";
            ((System.ComponentModel.ISupportInitialize)_grdRSS).EndInit();
            ((System.ComponentModel.ISupportInitialize)_grdDB).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        public enum Process
        {
            AuthorPage = 0,
            StoryPage = 1
        }

        public DAL DAL;
        public HtmlGrabber myCaller;
        public Process Navigate;

        // Declare a new DataGridTableStyle in the
        // declarations area of your form.
        //private DataGridTableStyle ts = new DataGridTableStyle();

        #region Database Routines

        private string DB;

        public DataTable GetData(int Category_ID, bool ALL = false)


        {

            DataTable dt;

            DB = cmbChooseDB.Text;

            dt = DAL.GetData(Category_ID, ALL);

            return dt;

        }

        private int UpdateData(DataTable dt)
        {

            int result = 0;

            result = DAL.UpdateData(ref dt);

            return result;

        }

        private DataTable GetCategories()
        {

            DataTable dt;

            try
            {
                dt = DAL.GetCategories();
            }
            catch
            {
                dt = null;
            }

            return dt;

        }

        public void UpdateRSS(DataSet ds)
        {

            // Initialize Debug Console
            FormManagement.Initialize(FormManagement.forms.frmDebug);


            // Update Data in Debug Console
            try
            {
                grdRSS.DataMember = ds.Tables[0].TableName;
                grdRSS.DataSource = ds;
                grdRSS.Visible = true;
            }
            catch
            {
            }
        }

        private void UpdateDateBase(object sender, EventArgs e)


        {

            int ret;

            DataTable dt;

            dt = (DataTable)grdDB.DataSource;

            string cat_id;

            try
            {
                cat_id = dt.Rows[grdDB.CurrentRow.Index]["Category_Id"].ToString();

                if (string.IsNullOrEmpty(cat_id) | cat_id == "0")
                {
                    cat_id = Conversions.ToString(cmbChooseDB.SelectedValue);
                    dt.Rows[grdDB.CurrentRow.Index]["Category_Id"] = cat_id;
                }
            }
            catch
            {
            }

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

        }

        #endregion

        #region Interface Code

        private void UpdateRec(object sender, EventArgs e)


        {
            UpdateRecord();
        }

        private void btnPath_Click(object sender, EventArgs e)


        {
            var frmpath = new frmPath();
            frmpath.Show();
        }

        private void GotoNextRecord(object sender, EventArgs e)


        {
            MoveNext();
        }

        private void cmbProcess_SelectedIndexChanged(object sender, EventArgs e)


        {

            // Select Case cmbSearch.Text
            // Case "StoryID"
            Navigate = Process.StoryPage;
            // Case "Author"
            // Navigate = Process.AuthorPage
            // End Select

            ResetInfo();

        }

        #endregion

        #region Form Handling Routines

        public void InitConfig()
        {

            const string empty = "No Path Set";

            FileInfo fi;

            IniFileReader ifr;

            string val;

            fi = new FileInfo(Application.StartupPath + @"\\" + "config.ini");

            if (fi.Exists)
            {
                ifr = new IniFileReader(Application.StartupPath + @"\config.ini", true);

                val = ifr.GetIniValue("FanFic", "Path");

                if ((val ?? "") != empty)
                {

                    ReloadDAL("Fanfic", val, false);
                    DAL.UpdateConnStr("FanFic", val);
                    ReloadDAL("Fanfic", val);
                }

                else
                {
                    ReloadDAL("Fanfic", "", false);
                }

            }

            ifr = null;

            // TODO: Handle Setting Reload
            // global::HtmlGrabber.My.MySettingsProperty.Settings.Reload();

        }

        public void ReloadDAL(string csname, string path, bool Init = true)
        {

            FormManagement.frmDebug.DAL = null;

            // Dim fi As FileInfo
            // fi = New FileInfo(path)

            DAL = new SQLite("Fanfic", Init);

            // Select Case fi.Extension
            // Case ".mdb"
            // DAL = New Access
            // Case ".db"
            // DAL = New SQLite("FanFic", Init)
            // End Select

        }

        private void frmDebug_Load(object sender, EventArgs e)


        {



            FormManagement.PlaceDebugWindow();
            cmbSearch.SelectedIndex = 0;
            InitConfig();

            FillCategories();

        }

        public void FillCategories()
        {

            DataTable dt;

            dt = GetCategories();

            cmbChooseDB.DataSource = dt;
            cmbChooseDB.ValueMember = "Id";
            cmbChooseDB.DisplayMember = "Name";

        }


        private void frmDebug_Closing(object sender, System.ComponentModel.CancelEventArgs e)




        {

            e.Cancel = true;
            Hide();

        }

        #endregion

        #region Validation Routines

        public bool isStale(ref DataTable dt, long pos)


        {

            bool ret = false;

            DateTime publish_date;
            DateTime update_date;
            DateTime last_checked;
            TimeSpan tsDelta;

            int wait;


            if (string.IsNullOrEmpty(dt.Rows[(int)pos]["Last_Checked"].ToString()))
            {
                dt.Rows[(int)pos]["Last_checked"] = Conversions.ToDate(DateTime.Today);
                UpdateData(dt);
                ret = false;
                return ret;
            }


            if (string.IsNullOrEmpty(dt.Rows[(int)pos]["Publish_Date"].ToString()))
            {
                ret = false;
                return ret;
            }
            else
            {
                publish_date = Conversions.ToDate(dt.Rows[(int)pos]["Publish_Date"].ToString());
            }

            if (string.IsNullOrEmpty(dt.Rows[(int)pos]["Update_Date"].ToString()))
            {
                update_date = publish_date;
            }
            else
            {
                update_date = Conversions.ToDate(dt.Rows[(int)pos]["Update_Date"].ToString());
            }

            last_checked = Conversions.ToDate(dt.Rows[(int)pos]["Last_Checked"].ToString());

            tsDelta = last_checked.Subtract(update_date);

            switch (tsDelta.Days)
            {
                case var @case when @case > 360:
                    {
                        wait = 180;
                        break;
                    }
                case var case1 when case1 > 180:
                    {
                        wait = 90;
                        break;
                    }
                case var case2 when case2 > 90:
                    {
                        wait = 60;
                        break;
                    }
                case var case3 when case3 > 60:
                    {
                        wait = 30;
                        break;
                    }
                case var case4 when case4 > 30:
                    {
                        wait = 7;
                        break;
                    }

                default:
                    {
                        wait = 0;
                        break;
                    }
            }

            tsDelta = DateTime.Today.Subtract(last_checked);
            if (tsDelta.Days < wait)
            {
                ret = true;
            }

            return ret;

        }

        public bool isValid(ref DataTable dt, long pos)


        {

            bool ret = false;
            string link;

            string[] @params;

            if (ReferenceEquals(dt.Rows[grdDB.CurrentRow.Index]["Internet"].GetType(), typeof(DBNull)))

            {
                ret = false;
            }
            else
            {

                link = Conversions.ToString(dt.Rows[grdDB.CurrentRow.Index]["Internet"]);

                @params = Strings.Split(link, "#");

                if (Information.UBound(@params) > 0)
                {
                    link = @params[1];
                }
                else
                {
                    link = @params[0];
                }

                ret = Program.BL.CheckUrl(ref link);

            }

            return ret;

        }

        #endregion

        public void ProcessStory(DataTable dt, long pos, string link)



        {

            clsFanfic.Story fic;
            string folder;
            bool check;
            int current;
            int start;

            check = Program.BL.GetChapters(link);

            fic = Program.BL.FanFic;


            start = Conversions.ToInteger(Operators.AddObject(dt.Rows[(int)pos]["Count"], 1));

            folder = Conversions.ToString(dt.Rows[(int)pos]["Folder"]);

            if (check)
            {

                string argResult = Program.BL.Result;
                FormManagement.frmMain.UpdateUI(ref fic, ref argResult, start);

                Application.DoEvents();

                FormManagement.frmMain.txtFileMask.Text = folder + "-";

                if (string.IsNullOrEmpty(dt.Rows[(int)pos]["Publish_Date"].ToString()))
                {
                    dt.Rows[(int)pos]["Publish_Date"] = Conversions.ToDate(fic.PublishDate);
                    UpdateData(dt);
                }

                current = Conversions.ToInteger(dt.Rows[(int)pos]["Count"]);

                if (Conversions.ToInteger(fic.ChapterCount) > current)
                {

                    dt.Rows[(int)pos]["Count"] = Conversions.ToInteger(fic.ChapterCount);

                    Program.BL.ProcessChapters(link, start, Conversions.ToInteger(fic.ChapterCount), folder, cmbChooseDB.Text);

                    fic = Program.BL.FanFic;

                    dt.Rows[(int)pos]["Update_Date"] = Conversions.ToDate(fic.UpdateDate);
                }

                else if (Conversions.ToInteger(fic.ChapterCount) < current)
                {
                    dt.Rows[(int)pos]["Count"] = Conversions.ToInteger(fic.ChapterCount);
                }

                dt.Rows[(int)pos]["Last_Checked"] = Conversions.ToDate(DateTime.Today);

                UpdateData(dt);
            }

            else
            {

                Program.BL.ProcessError(link, start, folder, cmbChooseDB.Text);

            }

        }

        public int MoveNext(bool initial = false)

        {

            long pos;
            DataTable dt;
            string url = "";
            string data = "";
            bool abort;

            dt = (DataTable)grdDB.DataSource;

        bypass:
            ;


            if (initial)
            {
                Application.DoEvents();
                SetCurrentRow(0);
                pos = 0L;
            }
            else
            {
                pos = grdDB.CurrentRow.Index;
                pos += 1L;
            }



            if (pos <= dt.Rows.Count - 1)
            {
                if (initial == false)
                {
                    SetCurrentRow((int)pos);
                    Application.DoEvents();
                }
            }
            else
            {
                return -1;
            }

            abort = false;

            if (string.IsNullOrEmpty(GetStoryID()))
            {
                abort = true;
            }
            else if (!isValid(ref dt, pos))
            {
                abort = true;
            }
            else if (!isStale(ref dt, pos))
            {

                switch (Navigate)
                {
                    case Process.AuthorPage:
                        {
                            url = GetAtom(dt);
                            break;
                        }
                    case Process.StoryPage:
                        {
                            url = GetStory(dt);
                            break;
                        }
                }
            }
            else
            {
                abort = true;
            }



            if (abort)
            {
                initial = false;
                goto bypass;
            }
            else
            {
                switch (Navigate)
                {
                    case Process.AuthorPage:
                        {
                            UpdateAtom(url);
                            break;
                        }
                    case Process.StoryPage:
                        {
                            ProcessStory(dt, pos, url);
                            break;
                        }
                }
            }

            return (int)pos;

        }

        private void ProcessBatch(object sender, EventArgs e)


        {

            var dt = new DataTable();
            int start;

            dt = GetData(Conversions.ToInteger(cmbChooseDB.SelectedValue));

            if (dt.Rows.Count == 0)
            {
                Interaction.MsgBox("No new stories to download!", MsgBoxStyle.Exclamation);
                return;
            }

            grdDB.DataSource = dt;

            Application.DoEvents();

            start = MoveNext(true);

            tmrDownload.Enabled = true;

        }

        private void RefreshDB()
        {

            DataTable dt;

            int cat_id;

            cat_id = Conversions.ToInteger(cmbChooseDB.SelectedValue.ToString());

            dt = GetData(cat_id, true);

            btnSave.Enabled = true;
            btnSearch.Enabled = true;

            btnBatch.Enabled = true;

            btnUpdateDB.Enabled = true;
            btnUpdateRecord.Enabled = true;

            grdDB.DataSource = dt;
            grdDB.Columns["Id"].Visible = false;
            grdDB.Columns["Category_Id"].Visible = false;
            SetCurrentRow(0);

            if (dt.Rows.Count > 0)
            {

                lblStatus.Text = "(" + (grdDB.CurrentRow.Index + 1) + " of " + dt.Rows.Count + ")";

            }
            else
            {
                lblStatus.Text = "(0 of 0)";
            }

        }

        public void SetCurrentRow(int Index)
        {

            grdDB.CurrentCell = grdDB.Rows[Index].Cells[1];

        }

        #region Interface Code

        private void btnOpenDB_Click(object sender, EventArgs e)


        {

            RefreshDB();

        }

        private void btnSave_Click(object sender, EventArgs e)


        {

            clsFanfic.Story fic;
            string id;
            DataTable dt;
            int NewRow;
            int iYesNo;
            string link;
            string folder;
            int cat_id;

            dt = (DataTable)grdDB.DataSource;

            NewRow = dt.Rows.Count;

            iYesNo = (int)Interaction.MsgBox("Add New Record?", MsgBoxStyle.YesNo);
            cat_id = Conversions.ToInteger(cmbChooseDB.SelectedValue);

            if (iYesNo == (int)Constants.vbYes)
            {

                folder = Interaction.InputBox("Enter File Name");

                if (!DAL.RecordExists(folder, cat_id))
                {

                    var dr = dt.NewRow();

                    link = myCaller.txtUrl.Text;
                    id = Program.BL.GetStoryID(link);
                    fic = Program.BL.GetStoryInfoByID(id);

                    dr["Title"] = fic.Title;
                    dr["Author"] = fic.Author;
                    dr["Folder"] = folder; // Folder Name

                    // dr("Chapter") = "" 'Current Chapter

                    dr["Count"] = 0; // Chapter Count

                    // dr("Matchup") = "" ' Matchup

                    dr["Description"] = fic.Summary;
                    dr["Internet"] = fic.Author + "#" + fic.AuthorURL + "#";
                    dr["StoryId"] = fic.ID;
                    dr["Complete"] = false;
                    dr["Publish_Date"] = fic.PublishDate;
                    dr["Category_Id"] = cat_id;

                    dt.Rows.Add(dr);

                    grdDB.DataSource = dt;

                    SetCurrentRow(NewRow);
                }

                else
                {
                    Interaction.MsgBox("Record already exists!", Constants.vbExclamation);
                }
            }

            dt = null;

        }

        private void tmrDownload_Tick(object sender, EventArgs e)


        {

            long pos;

            tmrDownload.Enabled = false;
            pos = MoveNext();

            if (pos != -1)
            {
                tmrDownload.Enabled = true;
            }
            else
            {
                RefreshDB();
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)


        {

            if (string.IsNullOrEmpty(cmbSearch.Text))
            {
                Interaction.MsgBox("Please select field to search by!", MsgBoxStyle.Exclamation);
                return;
            }

            string title = "";
            string folder = "";
            string author = "";

            string search = "";
            string result = "";

            string value = "";


            search = Interaction.InputBox("Enter Value to Search For.", "Fanfiction DB");
            search = Strings.UCase(search);

            if (string.IsNullOrEmpty(search))
                return;

            int count;
            int start;

            start = grdDB.CurrentRow.Index;

            DataTable dt;
            dt = (DataTable)grdDB.DataSource;

            if (start < dt.Rows.Count - 1)
            {
                start = start + 1;
            }
            else
            {
                start = 0;
            }

            var loopTo = dt.Rows.Count - 1;
            for (count = start; count <= loopTo; count++)
            {

                SetCurrentRow(count);

                Application.DoEvents();

                folder = Conversions.ToString(grdDB[3, grdDB.CurrentRow.Index].Value);
                title = Conversions.ToString(grdDB[1, grdDB.CurrentRow.Index].Value);
                author = Conversions.ToString(grdDB[2, grdDB.CurrentRow.Index].Value);

                switch (cmbSearch.Text ?? "")
                {
                    case "Title":
                        {
                            value = Strings.UCase(title);
                            break;
                        }
                    case "Author":
                        {
                            value = Strings.UCase(author);
                            break;
                        }
                    case "Folder":
                        {
                            value = Strings.UCase(folder);
                            break;
                        }
                }

                switch (cmbSearch.Text ?? "")
                {
                    case "Folder":
                        {
                            if (Strings.InStr(value, search) == 1)
                            {
                                result = value;
                            }

                            break;
                        }

                    default:
                        {
                            if (Strings.InStr(value, search) != 0)
                            {
                                result = value;
                            }

                            break;
                        }
                }


                if (!string.IsNullOrEmpty(result))
                {
                    break;
                }

            }

            if (string.IsNullOrEmpty(result))
            {

                var loopTo1 = start;
                for (count = 0; count <= loopTo1; count++)
                {

                    SetCurrentRow(count);
                    Application.DoEvents();

                    folder = Conversions.ToString(grdDB[3, grdDB.CurrentRow.Index].Value);
                    title = Conversions.ToString(grdDB[1, grdDB.CurrentRow.Index].Value);
                    author = Conversions.ToString(grdDB[2, grdDB.CurrentRow.Index].Value);

                    switch (cmbSearch.Text ?? "")
                    {
                        case "Title":
                            {
                                value = Strings.UCase(title);
                                break;
                            }
                        case "Author":
                            {
                                value = Strings.UCase(author);
                                break;
                            }
                        case "Folder":
                            {
                                value = Strings.UCase(folder);
                                break;
                            }
                    }

                    switch (cmbSearch.Text ?? "")
                    {
                        case "Folder":
                            {
                                if (Strings.InStr(value, search) == 1)
                                {
                                    result = value;
                                }

                                break;
                            }

                        default:
                            {
                                if (Strings.InStr(value, search) != 0)
                                {
                                    result = value;
                                }

                                break;
                            }
                    }

                    if (!string.IsNullOrEmpty(result))
                    {
                        break;
                    }

                }

            }

            if (string.IsNullOrEmpty(result))
            {
                Interaction.MsgBox("Story Does Not Exist in Database");
            }
            else
            {
                SetCurrentRow(count);
                Interaction.MsgBox("Folder: " + folder + Constants.vbCrLf + "Author: " + author + Constants.vbCrLf + "Title: " + title + Constants.vbCrLf);




            }

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

        private void grdDB_DoubleClick(object sender, EventArgs e)


        {

            FormManagement.Initialize(FormManagement.forms.frmStory);


            FormManagement.frmStory.myCaller = this;

            FormManagement.frmStory.Show();

            FormManagement.frmStory.TopMost = true;
            FormManagement.frmStory.RefreshData();
            FormManagement.frmStory.TopMost = false;

        }

        #endregion

        #region Retrieval Functions

        public string GetStory(DataTable dt)

        {

            string link;
            string StoryID;

            if (ReferenceEquals(dt.Rows[grdDB.CurrentRow.Index]["StoryId"].GetType(), typeof(DBNull)))

            {
                link = "";
            }
            else
            {

                StoryID = Conversions.ToString(dt.Rows[grdDB.CurrentRow.Index]["StoryID"]);

                if (string.IsNullOrEmpty(StoryID))
                {
                    link = "";
                }
                else
                {
                    link = Program.BL.GetStoryURL(StoryID);
                }

            }

            return link;

        }

        public string GetAtom(DataTable dt)

        {
            string GetAtomRet = default;

            if (ReferenceEquals(dt.Rows[grdDB.CurrentRow.Index]["Internet"].GetType(), typeof(DBNull)))

            {
                GetAtomRet = "";
            }
            else
            {
                GetAtomRet = Strings.Replace(Conversions.ToString(dt.Rows[grdDB.CurrentRow.Index]["Internet"]), "#", "");





            }

            return GetAtomRet;

        }

        #endregion

        #region Utility Routines

        private string GetStoryID()
        {

            string ret;

            DataTable dt;
            dt = (DataTable)grdDB.DataSource;

            ret = dt.Rows[grdDB.CurrentRow.Index]["StoryID"].ToString();


            FormManagement.frmMain.lblStoryID.Text = ret;

            return ret;

        }

        private void UpdateRecord()
        {

            clsFanfic.Story fic;
            string id;
            string link;
            DataTable dt;
            DateTime dte;

            dt = (DataTable)grdDB.DataSource;

            link = myCaller.txtUrl.Text;
            id = Program.BL.GetStoryID(link);
            fic = Program.BL.GetStoryInfoByID(id);

            dt.Rows[grdDB.CurrentRow.Index]["Title"] = fic.Title;

            dt.Rows[grdDB.CurrentRow.Index]["Author"] = fic.Author;

            dt.Rows[grdDB.CurrentRow.Index]["Description"] = fic.Summary;

            dt.Rows[grdDB.CurrentRow.Index]["Internet"] = fic.Author + "#" + fic.AuthorURL + "#";

            dt.Rows[grdDB.CurrentRow.Index]["StoryId"] = id;

            dt.Rows[grdDB.CurrentRow.Index]["Complete"] = false;
            dt.Rows[grdDB.CurrentRow.Index]["Abandoned"] = false;

            dte = Conversions.ToDate(fic.PublishDate);

            dt.Rows[grdDB.CurrentRow.Index]["Publish_Date"] = dte;

            dt.Rows[grdDB.CurrentRow.Index]["Last_Checked"] = dte;

            dt.Rows[grdDB.CurrentRow.Index]["Category_Id"] = Conversions.ToInteger(cmbChooseDB.SelectedValue);

        }

        public void UpdateAtom(string url)
        {
            // update urlAtom with AuthorPage
            FormManagement.frmMain.urlAtom.Text = url;
            // Obtain Feed From Site
            FormManagement.frmMain.ObtainFeed(url);
        }



        public void ResetInfo()
        {
            // Clear Information from source
            FormManagement.frmMain.ListChapters.Items.Clear();
            FormManagement.frmMain.lblChapterCount.Text = "";
            FormManagement.frmMain.lblProgress.Text = "";
            FormManagement.frmMain.lblStart.Visible = false;
            FormManagement.frmMain.txtStart.Text = "1";
            FormManagement.frmMain.txtStart.Visible = false;
            FormManagement.frmMain.txtSource.Text = "";
        }

        #endregion

        private void btnCategory_Click(object sender, EventArgs e)


        {

            var frmCat = new frmCategory();
            frmCat.ShowDialog();

        }


    }
}