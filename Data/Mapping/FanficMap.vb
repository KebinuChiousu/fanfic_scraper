Imports System 
Imports System.Collections.Generic 
Imports System.Text 
Imports FluentNHibernate.Mapping
Imports System.Linq

Public Class FanficMap
    Inherits ClassMap(Of Fanfic)
    
    Public Sub New()
        MyBase.New
			Table("Fanfic")
			LazyLoad()
			Id(Function(x) x.Id).GeneratedBy.Identity().Column("Id")
			Map(Function(x) x.Title).Column("Title").Length(50)
			Map(Function(x) x.Author).Column("Author").Length(50)
			Map(Function(x) x.Folder).Column("Folder").Length(50)
			Map(Function(x) x.Chapter).Column("Chapter").Length(50)
			Map(Function(x) x.Count).Column("Count").Length(8)
			Map(Function(x) x.Matchup).Column("Matchup").Length(50)
			Map(Function(x) x.Crossover).Column("Crossover").Length(255)
			Map(Function(x) x.Description).Column("Description").Length(65536)
			Map(Function(x) x.Internet).Column("Internet").Length(65536)
			Map(Function(x) x.StoryID).Column("StoryID").Length(255)
			Map(Function(x) x.Abandoned).Column("Abandoned").Length(1)
			Map(Function(x) x.Complete).Column("Complete").Length(1)
			Map(Function(x) x.Publish_Date).Column("Publish_Date").Length(8)
			Map(Function(x) x.Update_Date).Column("Update_Date").Length(8)
			Map(Function(x) x.Last_Checked).Column("Last_Checked").Length(8)
			Map(Function(x) x.Category_Id).Column("Category_Id").Not.Nullable().Length(8)
    End Sub
End Class
