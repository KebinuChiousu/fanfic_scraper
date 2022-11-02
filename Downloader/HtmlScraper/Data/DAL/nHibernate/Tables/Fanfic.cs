
namespace HtmlScraper.Data.DAL.nHibernate.Tables
{


    public class Fanfic
    {
        private int _id;
        private string _title;
        private string _author;
        private string _folder;
        private string _chapter;
        private int? _count;
        private string _matchup;
        private string _crossover;
        private string _description;
        private string _internet;
        private string _storyID;
        private bool? _abandoned;
        private bool? _complete;
        private DateTime? _publish_Date;
        private DateTime? _update_Date;
        private DateTime? _last_Checked;
        private int _category_Id;

        public Fanfic() : base()
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
        public virtual string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }
        public virtual string Author
        {
            get
            {
                return _author;
            }
            set
            {
                _author = value;
            }
        }
        public virtual string Folder
        {
            get
            {
                return _folder;
            }
            set
            {
                _folder = value;
            }
        }
        public virtual string Chapter
        {
            get
            {
                return _chapter;
            }
            set
            {
                _chapter = value;
            }
        }
        public virtual int? Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
            }
        }
        public virtual string Matchup
        {
            get
            {
                return _matchup;
            }
            set
            {
                _matchup = value;
            }
        }
        public virtual string Crossover
        {
            get
            {
                return _crossover;
            }
            set
            {
                _crossover = value;
            }
        }
        public virtual string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        public virtual string Internet
        {
            get
            {
                return _internet;
            }
            set
            {
                _internet = value;
            }
        }
        public virtual string StoryID
        {
            get
            {
                return _storyID;
            }
            set
            {
                _storyID = value;
            }
        }
        public virtual bool? Abandoned
        {
            get
            {
                return _abandoned;
            }
            set
            {
                _abandoned = value;
            }
        }
        public virtual bool? Complete
        {
            get
            {
                return _complete;
            }
            set
            {
                _complete = value;
            }
        }
        public virtual DateTime? Publish_Date
        {
            get
            {
                return _publish_Date;
            }
            set
            {
                _publish_Date = value;
            }
        }
        public virtual DateTime? Update_Date
        {
            get
            {
                return _update_Date;
            }
            set
            {
                _update_Date = value;
            }
        }
        public virtual DateTime? Last_Checked
        {
            get
            {
                return _last_Checked;
            }
            set
            {
                _last_Checked = value;
            }
        }
        public virtual int Category_Id
        {
            get
            {
                return _category_Id;
            }
            set
            {
                _category_Id = value;
            }
        }
    }
}