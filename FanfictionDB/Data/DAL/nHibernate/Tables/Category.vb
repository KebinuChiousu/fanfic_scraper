Option Strict Off
Option Explicit On

Imports System
Imports System.Text
Imports System.Collections.Generic


Public Class Category
    Private _id As Integer
    Private _name As String
    Public Sub New()
        MyBase.New()
    End Sub
    Public Overridable Property Id() As Integer
        Get
            Return Me._id
        End Get
        Set(value As Integer)
            Me._id = value
        End Set
    End Property
    Public Overridable Property Name() As String
        Get
            Return Me._name
        End Get
        Set(value As String)
            Me._name = value
        End Set
    End Property
End Class
