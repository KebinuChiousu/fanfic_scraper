using System;

namespace web_scraper.Models.Base
{
    public struct Story
    {
        public string ID;
        public string Title;
        public string Author;
        public string AuthorURL;
        public string StoryURL;
        public string Category;
        public string[] Chapters;
        public int ChapterCount;
        public DateTime? PublishDate;
        public DateTime? UpdateDate;
        public string Summary;
    }

}