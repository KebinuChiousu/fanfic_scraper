using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace HtmlGrabber
{

    public class frmPath : Form
    {

        private DAL conf;

        #region  Windows Form Designer generated code 

        public frmPath() : base()
        {
            Load += frmPath_Load;

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
        private TextBox _txtPath;

        internal virtual TextBox txtPath
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtPath;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _txtPath = value;
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
                    _btnPath.Click -= btnBrowse_Click;
                }

                _btnPath = value;
                if (_btnPath != null)
                {
                    _btnPath.Click += btnBrowse_Click;
                }
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
        private TextBox _txtOutput;

        internal virtual TextBox txtOutput
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtOutput;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _txtOutput = value;
            }
        }
        private Button _btnOutput;

        internal virtual Button btnOutput
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnOutput;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnOutput != null)
                {
                    _btnOutput.Click -= btnOutput_Click;
                }

                _btnOutput = value;
                if (_btnOutput != null)
                {
                    _btnOutput.Click += btnOutput_Click;
                }
            }
        }
        private Button _btnUpdate;

        internal virtual Button btnUpdate
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnUpdate;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnUpdate != null)
                {
                    _btnUpdate.Click -= btnUpdate_Click;
                }

                _btnUpdate = value;
                if (_btnUpdate != null)
                {
                    _btnUpdate.Click += btnUpdate_Click;
                }
            }
        }
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPath));
            _Label1 = new Label();
            _txtPath = new TextBox();
            _btnUpdate = new Button();
            _btnUpdate.Click += new EventHandler(btnUpdate_Click);
            _btnPath = new Button();
            _btnPath.Click += new EventHandler(btnBrowse_Click);
            _Label2 = new Label();
            _txtOutput = new TextBox();
            _btnOutput = new Button();
            _btnOutput.Click += new EventHandler(btnOutput_Click);
            SuspendLayout();
            // 
            // Label1
            // 
            _Label1.Location = new Point(16, 15);
            _Label1.Name = "_Label1";
            _Label1.Size = new Size(240, 29);
            _Label1.TabIndex = 0;
            _Label1.Tag = "FanFic";
            _Label1.Text = "FanFic Database Path";
            // 
            // txtPath
            // 
            _txtPath.Location = new Point(16, 44);
            _txtPath.Name = "_txtPath";
            _txtPath.Size = new Size(672, 31);
            _txtPath.TabIndex = 1;
            // 
            // btnUpdate
            // 
            _btnUpdate.Location = new Point(258, 162);
            _btnUpdate.Name = "_btnUpdate";
            _btnUpdate.Size = new Size(272, 60);
            _btnUpdate.TabIndex = 4;
            _btnUpdate.Text = "Update INI File";
            // 
            // btnPath
            // 
            _btnPath.Location = new Point(704, 44);
            _btnPath.Name = "_btnPath";
            _btnPath.Size = new Size(48, 37);
            _btnPath.TabIndex = 5;
            _btnPath.Text = "...";
            // 
            // Label2
            // 
            _Label2.AutoSize = true;
            _Label2.Location = new Point(16, 85);
            _Label2.Name = "_Label2";
            _Label2.Size = new Size(143, 25);
            _Label2.TabIndex = 6;
            _Label2.Text = "Output Folder";
            // 
            // txtOutput
            // 
            _txtOutput.Location = new Point(16, 116);
            _txtOutput.Name = "_txtOutput";
            _txtOutput.Size = new Size(672, 31);
            _txtOutput.TabIndex = 7;
            // 
            // btnOutput
            // 
            _btnOutput.Location = new Point(704, 114);
            _btnOutput.Name = "_btnOutput";
            _btnOutput.Size = new Size(48, 37);
            _btnOutput.TabIndex = 8;
            _btnOutput.Text = "...";
            // 
            // frmPath
            // 
            AutoScaleBaseSize = new Size(10, 24);
            ClientSize = new Size(778, 247);
            Controls.Add(_btnOutput);
            Controls.Add(_txtOutput);
            Controls.Add(_Label2);
            Controls.Add(_btnPath);
            Controls.Add(_btnUpdate);
            Controls.Add(_txtPath);
            Controls.Add(_Label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmPath";
            Text = "Path to Databases";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private void btnBrowse_Click(object sender, EventArgs e)


        {
            var dlg = new OpenFileDialog()
            {
                DefaultExt = "db",
                Filter = "SQL Lite Database|*.db",
                CheckFileExists = false,
                CheckPathExists = true,
                Title = "Select Path to Database."
            };

            try
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = dlg.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {

            var dlg = new OpenFileDialog();

            string temp;

            dlg.DefaultExt = "htm";
            dlg.Filter = "HTML File|*.htm|Text File|*.txt";

            dlg.CheckFileExists = false;
            dlg.CheckPathExists = true;
            dlg.Title = "Select Path to Output Folder.";

            try
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    temp = dlg.FileName;
                    txtOutput.Text = Path.GetDirectoryName(temp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void frmPath_Load(object sender, EventArgs e)
        {

            // Obtain DAL instance from frmDebug
            conf = FormManagement.frmDebug.DAL;

            // Legacy Ini Code
            InitIniFile();

            // App.Config Code
            InitConfigFile();

        }

        private void btnUpdate_Click(object sender, EventArgs e)


        {


            FormManagement.frmDebug.ReloadDAL("Fanfic", txtPath.Text, false);

            // App.Config Code
            UpdateConfigFile();

            // Legacy Ini Code
            UpdateIniFile();

            My.MySettingsProperty.Settings.Reload();

            FormManagement.frmDebug.ReloadDAL("Fanfic", txtPath.Text);

            FormManagement.frmDebug.FillCategories();

            Dispose();

        }

        #region config.ini Manipulation Routines

        private IniFileReader ifr;

        public void InitIniFile()
        {

            string OutputFolder;

            OutputFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            FileInfo fi;
            StringCollection sc;
            fi = new FileInfo(Application.StartupPath + @"\\" + "config.ini");
            if (fi.Exists)
            {
                ifr = new IniFileReader(Application.StartupPath + @"\config.ini", true);
                txtPath.Text = ifr.GetIniValue("FanFic", "Path");
                txtOutput.Text = ifr.GetIniValue("Output", "Path");
            }
            else
            {
                ifr = new IniFileReader(Application.StartupPath + @"\config.ini", true);
                sc = ifr.GetIniComments(null);
                sc.Add("Fanfiction Downloader DB Configuration");
                ifr.SetIniComments(null, sc);
                ifr.SetIniValue("FanFic", "Path", "No Path Set");
                ifr.SetIniValue("Output", "Path", OutputFolder);
                ifr.OutputFilename = Application.StartupPath + @"\config.ini";
                ifr.SaveAsIniFile();
            }

        }

        public void UpdateIniFile()
        {

            string OutputFolder;

            OutputFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (string.IsNullOrEmpty(txtPath.Text))
            {
                ifr.SetIniValue("FanFic", "Path", "No Path Set");
            }
            else
            {
                ifr.SetIniValue("FanFic", "Path", txtPath.Text);
            }

            if (string.IsNullOrEmpty(txtOutput.Text))
            {
                ifr.SetIniValue("Output", "Path", OutputFolder);
            }
            else
            {
                ifr.SetIniValue("Output", "Path", txtOutput.Text);
            }

            ifr.OutputFilename = Application.StartupPath + @"\config.ini";
            ifr.SaveAsIniFile();

        }

        #endregion

        #region app.config Manipulation Routines


        public void InitConfigFile()
        {

            string Path = "";
            string Output = "";

            if (!(conf == null))
            {

                Path = conf.GetPath("FanFic");
                Output = conf.GetConfigValue("Output");

                if (!string.IsNullOrEmpty(Path))
                {
                    txtPath.Text = Path;
                }

                if (!string.IsNullOrEmpty(Output))
                {
                    txtOutput.Text = Output;
                }

            }

        }

        public void UpdateConfigFile()
        {

            string OutputFolder;

            OutputFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (!string.IsNullOrEmpty(txtPath.Text))
            {
                conf.UpdateConnStr("FanFic", txtPath.Text);
            }

            if (string.IsNullOrEmpty(txtOutput.Text))
            {
                conf.SetConfigValue("Output", OutputFolder);
            }
            else
            {
                conf.SetConfigValue("Output", txtOutput.Text);
            }


        }

        #endregion


    }
}