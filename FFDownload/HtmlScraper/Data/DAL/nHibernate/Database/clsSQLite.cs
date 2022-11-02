using System.Configuration;
using System.Data;
using HtmlScraper.Data.DAL.nHibernate.Tables;
using HtmlScraper.Utility;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using NHibernate.Criterion;

namespace HtmlScraper.Data.DAL.nHibernate.Database
{


    public class SQLite : DAL
    {

        private nHibernateHelper orm;

        /// <summary>
    /// Initilizes SQLite Database connection settings
    /// </summary>
    /// <param name="ConnectionStringName">Name of ConnectionString Key in app.config</param>
    /// <param name="Init">If set to true initialize nHibernate.</param>
    /// <remarks></remarks>
        public SQLite(string ConnectionStringName, bool Init = true)
        {

            string connstr;

            if (Init)
            {

                // Read Connection String from App Config XML File
                connstr = GetConnStr(ConnectionStringName);

                // Create an instance of the nHibernate Helper class using the connstr obtained from the config file.
                orm = new nHibernateHelper(connstr);
            }

        }

        /// <summary>
    /// Obtain List of Fanfiction Categories from SQLlite Database
    /// </summary>
    /// <returns>DataTable of ID and Names of Categories</returns>
    /// <remarks></remarks>
        public override DataTable GetCategories()
        {

            DataTable dt;
            var cat = new List<Category>();

            using (var s = orm.OpenSession())
            {

                s.CreateCriteria<Category>().List(cat);

            }

            dt = cat.ToDataTable();

            return dt;

        }

        /// <summary>
    /// Obtains DataTable of Metadata from SQLite Database
    /// </summary>
    /// <param name="Category_ID">ID number of Fiction Category.</param>
    /// <param name="ALL">If set to true obtains all Stories regardless of Status.</param>
    /// <returns></returns>
    /// <remarks></remarks>
        public override DataTable GetData(int Category_ID, bool ALL = false)
        {

            DataTable dt;
            var fic = new List<Fanfic>();

            using (var s = orm.OpenSession())
            {

                if (ALL)
                {
                    s.CreateCriteria<Fanfic>().Add(Restrictions.Eq("Category_Id", Category_ID)).AddOrder(Order.Asc("Folder")).List(fic);


                }

                else
                {

                    s.CreateCriteria<Fanfic>().Add(Restrictions.Eq("Abandoned", false)).Add(Restrictions.Eq("Complete", false)).Add(Restrictions.Eq("Category_Id", Category_ID)).AddOrder(Order.Asc("Folder")).List(fic);




                }

            }

            dt = fic.ToDataTable();

            fic = null;

            return dt;


        }

        public override bool CategoryExists(string category)
        {

            bool ret;

            DataTable dt;
            var fic = new List<Category>();

            using (var s = orm.OpenSession())
            {

                s.CreateCriteria<Category>().Add(Restrictions.Eq("Name", category)).List(fic);


            }

            dt = fic.ToDataTable();

            if (dt.Rows.Count > 0)
            {
                ret = true;
            }
            else
            {
                ret = false;
            }

            fic = null;
            dt = null;

            return ret;

        }

        public override bool RecordExists(int category_id)
        {

            bool ret;

            DataTable dt;
            var fic = new List<Fanfic>();

            using (var s = orm.OpenSession())
            {

                s.CreateCriteria<Fanfic>().Add(Restrictions.Eq("Category_Id", category_id)).List(fic);


            }

            dt = fic.ToDataTable();

            if (dt.Rows.Count > 0)
            {
                ret = true;
            }
            else
            {
                ret = false;
            }

            fic = null;
            dt = null;

            return ret;

        }

        public override bool RecordExists(string folder, int category_id)
        {

            bool ret;

            DataTable dt;
            var fic = new List<Fanfic>();

            using (var s = orm.OpenSession())
            {

                s.CreateCriteria<Fanfic>().Add(Restrictions.Eq("Category_Id", category_id)).Add(Restrictions.Eq("Folder", folder)).List(fic);



            }

            dt = fic.ToDataTable();

            if (dt.Rows.Count > 0)
            {
                ret = true;
            }
            else
            {
                ret = false;
            }

            fic = null;
            dt = null;

            return ret;

        }

        /// <summary>
    /// Updates Fanfiction Metadata using the provided DataTable as Input
    /// </summary>
    /// <param name="dt">DataTable containing fanfiction metadata.</param>
    /// <returns>Number of rows affected.</returns>
    /// <remarks></remarks>
        public override int UpdateData(ref DataTable dt)
        {

            Fanfic fic;
            int ret;

            string temp;

            dt = dt.GetChanges();

            using (var s = orm.OpenSession())
            {

                using (var t = s.BeginTransaction())
                {

                    for (int i = 0, loopTo = dt.Rows.Count - 1; i <= loopTo; i++)
                    {

                        fic = new Fanfic();

                        switch (dt.Rows[i].RowState)
                        {
                            case DataRowState.Added:
                            case DataRowState.Modified:
                                {

                                    temp = dt.Rows[i]["Id"].ToString();

                                    if (string.IsNullOrEmpty(temp))
                                        temp = 0.ToString();

                                    fic.Id = Conversions.ToInteger(temp);
                                    fic.Title = Conversions.ToString(dt.Rows[i]["Title"]);
                                    fic.Author = Conversions.ToString(dt.Rows[i]["Author"]);
                                    fic.Folder = Conversions.ToString(dt.Rows[i]["Folder"]);
                                    fic.Chapter = dt.Rows[i]["Chapter"].ToString();
                                    fic.Count = (int?)dt.Rows[i]["Count"];
                                    fic.Matchup = dt.Rows[i]["Matchup"].ToString();
                                    fic.Crossover = dt.Rows[i]["Crossover"].ToString();
                                    fic.Description = dt.Rows[i]["Description"].ToString();
                                    fic.Internet = dt.Rows[i]["Internet"].ToString();
                                    fic.StoryID = dt.Rows[i]["StoryID"].ToString();

                                    temp = dt.Rows[i]["Abandoned"].ToString();

                                    if (string.IsNullOrEmpty(temp))
                                        temp = Conversions.ToString(false);

                                    fic.Abandoned = Conversions.ToBoolean(temp);

                                    temp = dt.Rows[i]["Complete"].ToString();

                                    if (string.IsNullOrEmpty(temp))
                                        temp = Conversions.ToString(false);

                                    fic.Complete = Conversions.ToBoolean(temp);

                                    if (Information.IsDate(dt.Rows[i]["Publish_Date"]))
                                    {
                                        fic.Publish_Date = (DateTime?)dt.Rows[i]["Publish_Date"];
                                    }

                                    if (Information.IsDate(dt.Rows[i]["Update_Date"]))
                                    {
                                        fic.Update_Date = (DateTime?)dt.Rows[i]["Update_Date"];
                                    }
                                    else if (!((object)fic.Publish_Date == null))
                                    {
                                        fic.Update_Date = fic.Publish_Date;
                                    }

                                    if (Information.IsDate(dt.Rows[i]["Last_Checked"]))
                                    {
                                        fic.Last_Checked = (DateTime?)dt.Rows[i]["Last_Checked"];
                                    }

                                    fic.Category_Id = Conversions.ToInteger(dt.Rows[i]["Category_Id"]);

                                    if (dt.Rows[i].RowState == DataRowState.Added)
                                    {
                                        if (!RecordExists(fic.Folder, fic.Category_Id))
                                        {
                                            s.SaveOrUpdate(fic);
                                        }
                                        else
                                        {
                                            dt.Rows[i].Delete();
                                        }
                                    }
                                    else
                                    {
                                        s.SaveOrUpdate(fic);
                                    }

                                    break;
                                }

                            case DataRowState.Deleted:
                                {

                                    temp = dt.Rows[i]["Id", DataRowVersion.Original].ToString();

                                    fic.Id = Conversions.ToInteger(temp);

                                    s.Delete(fic);
                                    break;
                                }

                        }

                        fic = null;


                    }

                    t.Commit();

                }
            }

            ret = dt.Rows.Count;
            dt.AcceptChanges();

            fic = null;

            return ret;

        }

        /// <summary>
    /// Updates Category Metadata using the provided DataTable as Input
    /// </summary>
    /// <param name="dt">DataTable containing Category metadata.</param>
    /// <returns>Number of rows affected.</returns>
    /// <remarks></remarks>
        public override int UpdateCategories(ref DataTable dt)
        {

            Category fic;
            int ret;

            string temp;

            dt = dt.GetChanges();

            using (var s = orm.OpenSession())
            {

                using (var t = s.BeginTransaction())
                {

                    for (int i = 0, loopTo = dt.Rows.Count - 1; i <= loopTo; i++)
                    {

                        fic = new Category();

                        switch (dt.Rows[i].RowState)
                        {
                            case DataRowState.Added:
                            case DataRowState.Modified:
                                {

                                    temp = dt.Rows[i]["Id"].ToString();

                                    if (string.IsNullOrEmpty(temp))
                                        temp = 0.ToString();

                                    fic.Id = Conversions.ToInteger(temp);
                                    fic.Name = dt.Rows[i]["Name"].ToString();


                                    if (dt.Rows[i].RowState == DataRowState.Added)
                                    {
                                        if (!CategoryExists(fic.Name))
                                        {
                                            s.SaveOrUpdate(fic);
                                        }
                                        else
                                        {
                                            dt.Rows[i].Delete();
                                        }
                                    }
                                    else
                                    {
                                        s.SaveOrUpdate(fic);
                                    }

                                    break;
                                }

                            case DataRowState.Deleted:
                                {

                                    temp = dt.Rows[i]["Id", DataRowVersion.Original].ToString();

                                    fic.Id = Conversions.ToInteger(temp);

                                    s.Delete(fic);
                                    break;
                                }

                        }

                        fic = null;


                    }

                    t.Commit();

                }
            }

            ret = dt.Rows.Count;
            dt.AcceptChanges();

            fic = null;

            return ret;

        }

        /// <summary>
    /// Extracts path to SQLite Database from Connection String
    /// </summary>
    /// <param name="csName">SQLite Connection String Name</param>
    /// <returns>Path to SQLite Database</returns>
    /// <remarks></remarks>
        public override string GetPath(string csName)
        {

            string[] Conn;
            string path = "";
            int index;

            string ConnStr;

            ConnStr = GetConnStr(csName);

            try
            {
                Conn = Strings.Split(ConnStr, ";");

                var loopTo = Information.UBound(Conn);
                for (index = 0; index <= loopTo; index++)
                {
                    if (Conversions.ToBoolean(Strings.InStr(Conn[index], "Data Source")))
                    {
                        Conn = Strings.Split(Conn[index], "=");
                        path = Conn[1];
                        path = Strings.Replace(path, "\"", "");
                        break;
                    }
                }
            }

            catch
            {
                path = "";
            }

            return path;

        }

        /// <summary>
    /// Generates Connection String Settings from Key String and Path to SQLite Database
    /// </summary>
    /// <param name="csName">Name of Connection String Key</param>
    /// <param name="Path">Path to SQLite Database</param>
    /// <returns>ConnectionStringSettings for app.config file</returns>
    /// <remarks></remarks>
        protected override ConnectionStringSettings SetConnectionString(string csName, string Path)



        {

            string connstr = "";

            connstr = "Data Source=";
            connstr += "\"" + Path + "\"";
            connstr += ";";
            connstr += "Version=3";
            connstr += ";";
            connstr += "New=False";
            connstr += ";";
            connstr += "Compress=True";
            connstr += ";";

            // Create a connection string element and
            // save it to the configuration file.
            // Create a connection string element.
            var csSettings = new ConnectionStringSettings(csName, connstr, "System.Data.SQLite");





            return csSettings;

        }


    }
}