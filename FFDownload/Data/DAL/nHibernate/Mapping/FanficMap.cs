using FluentNHibernate.Mapping;

namespace HtmlGrabber
{

    public class FanficMap : ClassMap<Fanfic>
    {

        public FanficMap() : base()
        {
            Table("Fanfic");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Title).Column("Title").Length(50);
            Map(x => x.Author).Column("Author").Length(50);
            Map(x => x.Folder).Column("Folder").Length(50);
            Map(x => x.Chapter).Column("Chapter").Length(50);
            Map(x => x.Count).Column("Count").Length(8);
            Map(x => x.Matchup).Column("Matchup").Length(50);
            Map(x => x.Crossover).Column("Crossover").Length(255);
            Map(x => x.Description).Column("Description").Length(65536);
            Map(x => x.Internet).Column("Internet").Length(65536);
            Map(x => x.StoryID).Column("StoryID").Length(255);
            Map(x => x.Abandoned).Column("Abandoned").Length(1);
            Map(x => x.Complete).Column("Complete").Length(1);
            Map(x => x.Publish_Date).Column("Publish_Date").Length(8);
            Map(x => x.Update_Date).Column("Update_Date").Length(8);
            Map(x => x.Last_Checked).Column("Last_Checked").Length(8);
            Map(x => x.Category_Id).Column("Category_Id").Not.Nullable().Length(8);
        }
    }
}