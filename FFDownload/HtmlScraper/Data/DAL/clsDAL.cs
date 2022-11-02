using System.Configuration;
using System.Data;

namespace HtmlScraper.Data.DAL
{

    public abstract class DAL
    {

        /// <summary>
    /// Obatains Story Metadata
    /// </summary>
    /// <param name="Category_ID">Category Id of Metadata to Retrieve</param>
    /// <param name="ALL">If set to true obtains all Stories regardless of Status.</param>
    /// <returns>Returns DataTable of Story MetaData</returns>
    /// <remarks></remarks>
        public abstract DataTable GetData(int Category_ID, bool ALL = false);






        /// <summary>
    /// Checks to see if the record already exists in the database.
    /// </summary>
    /// <param name="category_id">Cateory Id to check for folder</param>
    /// <returns>Retuurns true if record exists</returns>
    /// <remarks></remarks>
        public abstract bool RecordExists(int category_id);

        /// <summary>
    /// Checks to see if the record already exists in the database.
    /// </summary>
    /// <param name="folder">Folder Nmae to check for.</param>
    /// <param name="category_id">Cateory Id to check for folder</param>
    /// <returns>Retuurns true if record exists</returns>
    /// <remarks></remarks>
        public abstract bool RecordExists(string folder, int category_id);

        /// <summary>
    /// Checks to see if the record already exists in the database.
    /// </summary>
    /// <param name="category">Category Name to check for.</param>
    /// <returns>Returns true if record exists</returns>
    /// <remarks></remarks>
        public abstract bool CategoryExists(string category);

        /// <summary>
    /// Update Categories in Database based on Metadata from DataTable
    /// </summary>
    /// <param name="dt">DataTable of Category MetaData</param>
    /// <returns></returns>
    /// <remarks></remarks>
        public abstract int UpdateCategories(ref DataTable dt);

        /// <summary>
    /// Update Database based on Metadata from DataTable
    /// </summary>
    /// <param name="dt">DataTable of Story MetaData</param>
    /// <returns></returns>
    /// <remarks></remarks>
        public abstract int UpdateData(ref DataTable dt);

        /// <summary>
    /// Retrieves Category Name and ID of Story Data
    /// </summary>
    /// <returns></returns>
    /// <remarks></remarks>
        public abstract DataTable GetCategories();

        /// <summary>
    /// Obtain Path to Database from ConnectionString
    /// </summary>
    /// <param name="ConnStr">Database Connection String</param>
    /// <returns>Path to Database</returns>
    /// <remarks></remarks>
        public abstract string GetPath(string ConnStr);

        /// <summary>
    /// Build Connection String
    /// </summary>
    /// <param name="csName">Name of Connection String Key</param>
    /// <param name="csValue">Path to Database</param>
    /// <returns></returns>
    /// <remarks></remarks>
        protected abstract ConnectionStringSettings SetConnectionString(string csName, string csValue);




        #region .NET Configuration Management

        /// <summary>
    /// Retrieve full Conig Name Key Path from XML File
    /// </summary>
    /// <param name="Key">Configuration Key Name</param>
    /// <returns></returns>
    /// <remarks></remarks>
        private string GetConfigName(string Key)
        {

            string ns = HtmlGrabber.My.MyProject.Application.GetType().Namespace;
            string settings = "MySettings";
            string name = "";

            name = ns;
            name += ".";
            name += settings;
            name += ".";
            name += Key;

            return name;

        }

        /// <summary>
    /// Retrieve full Connection String Key Path from XML File
    /// </summary>
    /// <param name="csName">Connection String Key Name</param>
    /// <returns></returns>
    /// <remarks></remarks>
        private string GetConnStrName(string csName)
        {

            string name;

            name = GetConfigName(csName);

            return name;

        }

        /// <summary>
    /// Retrieve Connection String Name from App.Config
    /// </summary>
    /// <param name="csName">Name of Connection String Key</param>
    /// <returns></returns>
    /// <remarks></remarks>
        public string GetConnStr(string csName)

        {


            Configuration conf;
            string ConnStr = "";

            conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);




            var csSection = conf.ConnectionStrings;


            csName = GetConnStrName(csName);

            string path = "";

            try
            {
                ConnStr = csSection.ConnectionStrings[csName].ConnectionString;
            }
            catch
            {
                ConnStr = "";
            }

            return ConnStr;

        }

        /// <summary>
    /// Update Connection String in App.Config
    /// </summary>
    /// <param name="csName">Name of Connection String Key</param>
    /// <param name="csValue">Connection String</param>
    /// <remarks></remarks>
        public void UpdateConnStr(string csName, string csValue)


        {

            Configuration conf;
            var csSettings = new ConnectionStringSettings();


            conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);





            // Get the connection strings section.
            var csSection = conf.ConnectionStrings;


            // Get the current connection strings count.
            int connStrCnt = ConfigurationManager.ConnectionStrings.Count;

            csName = GetConnStrName(csName);

            // Create the connection string name. 
            // Dim csName As String = "ConnStr" + connStrCnt.ToString()

            csSettings = SetConnectionString(csName, csValue);

            try
            {
                csSection.ConnectionStrings.Remove(csName);
            }
            catch
            {
            }

            csSection.ConnectionStrings.Add(csSettings);

            // Save the configuration file.
            conf.Save(ConfigurationSaveMode.Modified, true);

            ConfigurationManager.RefreshSection("connectionStrings");

        }

        #region Output Folder Logic

        public void SetConfigValue(string KeyName, string Value)
        {

            Configuration conf;

            KeyValueConfigurationElement Setting;

            conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);




            var Section = conf.AppSettings;

            // Get the current AppSettings count.
            int Cnt = ConfigurationManager.AppSettings.Count;

            KeyName = GetConfigName(KeyName);

            Setting = Section.Settings[KeyName];

            if (Setting == null)
            {
                Section.Settings.Add(KeyName, Value);
            }
            else
            {
                Section.Settings[KeyName].Value = Value;
            }

            // Save the configuration file.
            conf.Save(ConfigurationSaveMode.Modified, true);

            ConfigurationManager.RefreshSection("appSettings");

        }

        /// <summary>
    /// Retrieve Key from App.Config
    /// </summary>
    /// <param name="KeyName">Name of Key</param>
    /// <returns></returns>
    /// <remarks></remarks>
        public string GetConfigValue(string KeyName)

        {


            Configuration conf;
            string Value = "";

            conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);




            var Section = conf.AppSettings;

            KeyName = GetConfigName(KeyName);

            try
            {
                Value = Section.Settings[KeyName].Value;
            }
            catch
            {
                Value = "";
            }

            return Value;

        }

        #endregion

        #endregion

    }
}