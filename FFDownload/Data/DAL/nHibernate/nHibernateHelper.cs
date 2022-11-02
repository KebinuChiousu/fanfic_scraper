using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace HtmlGrabber
{

    public class nHibernateHelper
    {

        public enum Database
        {
            SQLite = 0
        }

        private Database _database;
        private ISessionFactory _sessionFactory;
        private string _connstr;

        /// <summary>
    /// Initialize nHibernate Helper Class
    /// </summary>
    /// <param name="connstr">Connection String to Database</param>
    /// <remarks></remarks>
        public nHibernateHelper(string connstr, Database db = Database.SQLite)
        {
            _connstr = connstr;
            _database = db;
        }

        /// <summary>
    /// Obtain instance of a nHibernate SessionFactory
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
        public ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    InitializeSessionFactory();
                }

                return _sessionFactory;

            }

        }

        /// <summary>
    /// Initialize nHibernate SessionFactory
    /// </summary>
    /// <remarks></remarks>
        private void InitializeSessionFactory()
        {

            switch (_database)
            {
                case Database.SQLite:
                    {
                        InitializeSQLiteSessionFactory();
                        break;
                    }
            }

        }


        /// <summary>
    /// Initialize nHibernate SQLite SessionFactory
    /// </summary>
    /// <remarks></remarks>
        private void InitializeSQLiteSessionFactory()
        {

            _sessionFactory = Fluently.Configure().Database(SQLiteConfiguration.Standard.ConnectionString(_connstr)).Mappings(x => x.FluentMappings.AddFromAssemblyOf<Fanfic>()).BuildSessionFactory();






        }

        /// <summary>
    /// Obtain nHibernate Session
    /// </summary>
    /// <returns></returns>
    /// <remarks></remarks>
        public ISession OpenSession()
        {

            return SessionFactory.OpenSession();

        }


    }
}