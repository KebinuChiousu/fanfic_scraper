Imports System.ComponentModel
Imports System.Collections.Generic

<HideModuleName()> _
    Public Module ExtensionModule

    <System.Runtime.CompilerServices.Extension()> _
    Function ToDataTable(Of T)(data As IList(Of T)) As DataTable

        Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(GetType(T))
        Dim table As New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, If(Nullable.GetUnderlyingType(prop.PropertyType), prop.PropertyType))
        Next
        For Each item As T In data
            Dim row As DataRow = table.NewRow()
            For Each prop As PropertyDescriptor In properties
                row(prop.Name) = If(prop.GetValue(item), DBNull.Value)
            Next
            table.Rows.Add(row)
        Next

        table.AcceptChanges()

        Return table
    End Function
End Module
