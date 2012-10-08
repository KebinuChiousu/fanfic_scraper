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

    Private _sessionFactory As ISessionFactory
    Private _connstr As String

    Sub New(connstr As String)
        _connstr = connstr
    End Sub

    Public ReadOnly Property SessionFactory As ISessionFactory
        Get
            If IsNothing(_sessionFactory) Then
                IntializeSessionFactory()
            End If

            Return _sessionFactory

        End Get

    End Property

    Private Sub IntializeSessionFactory()

        _sessionFactory = Fluently.Configure() _
         .Database( _
                    SQLiteConfiguration.Standard _
                    .ConnectionString(_connstr) _
                  ) _
         .Mappings(Function(x) x.FluentMappings.AddFromAssemblyOf(Of Fanfic)()) _
         .BuildSessionFactory()

    End Sub

    Public Function OpenSession() As ISession

        Return SessionFactory.OpenSession

    End Function

    
End Class
