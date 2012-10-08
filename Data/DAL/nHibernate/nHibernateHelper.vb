Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports NHibernate
Imports NHibernate.Tool.hbm2ddl
Imports FluentNHibernate
Imports FluentNHibernate.Cfg
Imports FluentNHibernate.Cfg.Db

Public Class nHibernateHelper
	
	Private Shared _sessionFactory As ISessionFactory
	
	Private Shared readonly Property SessionFactory As ISessionFactory
		Get 
			If isnothing(_sessionFactory) Then
                IntializeSessionFactory()
			End If
		End Get
			
		
	End Property
	
	Private Shared Sub IntializeSessionFactory
		
		_sessionFactory = Fluently.Configure() _
			.Database( _
			           SQLiteConfiguration.Standard _
			.UsingFile("C:\Users\Kevin\Desktop\Stories\FanfictionDB.db").ShowSql _
			         ) _
			.Mappings( Function(x) x.FluentMappings.AddFromAssemblyOf(Of Fanfic)) _
			.BuildSessionFactory()		
		
	End Sub	
	
	Public Shared Function OpenSession As ISession
		
		Return SessionFactory.OpenSession
		
	End Function
	
End Class
