

namespace HtmlScraper.Data.DAL.nHibernate.Tables
{


    public class Category
    {
        private int _id;
        private string _name;
        public Category() : base()
        {
        }
        public virtual int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
    }
}