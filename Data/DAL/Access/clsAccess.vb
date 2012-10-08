Imports System.Configuration

Public Class Access
    Inherits DAL

    Public Overrides _
    Function GetCategories() As System.Data.DataTable

        Dim dt As DataTable

        Dim taCat As New dsFanFicTableAdapters.CategoryTableAdapter

        dt = taCat.GetData()

        Return dt

    End Function

    Public Overrides _
    Function GetData(Category_ID As Integer, Optional ALL As Boolean = False) As System.Data.DataTable

        Dim dt As DataTable
        Dim taFF As New dsFanFicTableAdapters.FanficTableAdapter

        Try
            If ALL Then
                dt = taFF.GetDataByCat(Category_ID)
            Else
                dt = taFF.GetDataByStatus(False, False, Category_ID)
            End If

        Catch
            dt = Nothing
        Finally
            taFF.Dispose()
        End Try

        GetData = dt

        dt.Dispose()

    End Function

    Public Overrides _
    Function UpdateData(ByRef dt As System.Data.DataTable) As Integer

        Dim result As Integer = 0

        Dim taFF As New dsFanFicTableAdapters.FanficTableAdapter

        Try
            result = taFF.Update(dt)
        Catch
            result = -1
        Finally
            taFF.Dispose()
        End Try

        Return result

    End Function

    Public Overrides _
    Function GetPath(ConnStr As String) As String

        Dim Conn() As String
        Dim path As String = ""
        Dim index As Integer

        Try
            Conn = Split(ConnStr, ";")

            For index = 0 To UBound(Conn)
                If InStr(Conn(index), "Data Source") Then
                    Conn = Split(Conn(index), "=")
                    path = Conn(1)
                    path = Replace(path, """", "")
                    Exit For
                End If
            Next

        Catch
            path = ""
        End Try

        Return path

    End Function

    Protected Overrides _
    Function SetConnectionString( _
                                  csName As String, _
                                  Path As String _
                                ) As ConnectionStringSettings

        Dim connstr As String = ""

        connstr = "Provider=Microsoft.Jet.OLEDB.4.0"
        connstr += ";"
        connstr += "Data Source="
        connstr += """" & Path & """"
        connstr += ";"
        connstr += "Persist Security Info=True"

        ' Create a connection string element and
        ' save it to the configuration file.
        ' Create a connection string element.
        Dim csSettings As New  _
        ConnectionStringSettings( _
                                  csName, _
                                  connstr, _
                                  "System.Data.OleDb" _
                                )

        Return csSettings

    End Function

End Class
