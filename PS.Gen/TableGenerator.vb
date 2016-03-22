Imports PS.Gen.Attributes

Public Class TableGenerator

    Private Class PrimaryKeyInfo
        Property ColumnName As String
        Property Ordinal As Integer
    End Class

    Public Function Generate(Item As Type) As String
        Dim Code As New Text.StringBuilder

        Dim PrimaryKeys As New List(Of PrimaryKeyInfo)
        Dim TableInfo As New TableGenerationAttribute

        'check for class attributes
        For Each attr In System.Attribute.GetCustomAttributes(Item)
            If attr.GetType() Is GetType(TableGenerationAttribute) Then
                TableInfo = CType(attr, TableGenerationAttribute)
            End If
        Next

        Code.AppendFormat("CREATE TABLE [{0}].[{1}] (", TableInfo.Schema, TableInfo.Name).AppendLine()

        For Each prop In Item.GetProperties()
            If IsSupportedType(prop.PropertyType) Then

                Dim ColumnInfo As New ColumnGenerationAttribute
                Dim SQLDataType As String = Helper.GetSQLDataType(prop)

                'check for property attributes
                For Each attr In System.Attribute.GetCustomAttributes(prop)
                    If attr.GetType() Is GetType(ColumnGenerationAttribute) Then
                        ColumnInfo = CType(attr, ColumnGenerationAttribute)
                    End If
                Next

                'default the name if missing
                If String.IsNullOrEmpty(ColumnInfo.Name) Then
                    ColumnInfo.Name = prop.Name
                End If

                If ColumnInfo.IsPrimaryKey Then
                    PrimaryKeys.Add(New PrimaryKeyInfo With {.ColumnName = ColumnInfo.Name, .Ordinal = ColumnInfo.PrimaryKeyOrdinal})
                    'PKs can't be null
                    ColumnInfo.IsNullable = False
                End If

                Code.AppendFormat("[{0}] {1} {2} {3},", ColumnInfo.Name, SQLDataType, IIf(ColumnInfo.IdentityIncrement > 0, String.Format("IDENTITY({0}, {1})", ColumnInfo.IdentitySeed, ColumnInfo.IdentityIncrement), ""), IIf(ColumnInfo.IsNullable, "NULL", "NOT NULL")).AppendLine()
            End If
        Next
        If PrimaryKeys.Count = 0 Then
            'stip off the last line break and comma and then add a line break
            Code.Length = Code.Length - 3
            Code.AppendLine()
        Else
            Code.AppendFormat("CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED", TableInfo.Name).AppendLine()
            Code.AppendLine("(")
            For Each key In PrimaryKeys.OrderBy(Function(pk) pk.Ordinal).ToList
                Code.AppendFormat("[{0}] ASC,", key.ColumnName).AppendLine()
            Next
            'remove the last line break and comma and then add a line break
            Code.Length = Code.Length - 3
            Code.AppendLine()
            Code.AppendLine(")")
        End If

        Code.Append(")")

        Return Code.ToString
    End Function

    Private Function IsSupportedType(Item As Type) As Boolean
        If Item.IsPrimitive OrElse Item Is GetType(String) OrElse Item Is GetType(Decimal) OrElse Item Is GetType(Guid) Then
            Return True
        Else
            Return False
        End If

    End Function
End Class
