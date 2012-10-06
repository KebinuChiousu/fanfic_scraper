Imports System 
Imports System.Collections.Generic 
Imports System.Text 
Imports FluentNHibernate.Mapping
Imports System.Linq

Public Class CategoryMap
    Inherits ClassMap(Of Category)
    
    Public Sub New()
        MyBase.New
			Table("Category")
			LazyLoad()
			Id(Function(x) x.Id).GeneratedBy.Identity().Column("Id")
			Map(Function(x) x.Name).Column("Name").Length(255)
    End Sub
End Class
