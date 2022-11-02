using FluentNHibernate.Mapping;

namespace HtmlGrabber
{

    public class CategoryMap : ClassMap<Category>
    {

        public CategoryMap() : base()
        {
            Table("Category");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Name).Column("Name").Length(255);
        }
    }
}