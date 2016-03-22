Public Class Helper

    Public Shared Function GetSQLDataType(Prop As Reflection.PropertyInfo) As String
        Dim SQLType As String = ""

        Dim info As New Attributes.ColumnGenerationAttribute()

        'check for property attributes
        For Each attr In System.Attribute.GetCustomAttributes(Prop)
            If attr.GetType() Is GetType(Attributes.ColumnGenerationAttribute) Then
                info = CType(attr, Attributes.ColumnGenerationAttribute)
            End If
        Next

        If String.IsNullOrEmpty(info.FullDataType) = False Then
            SQLType = info.FullDataType
        ElseIf info.IsMoney Then
            SQLType = "[money]"
        Else
            Select Case Prop.PropertyType.FullName

                Case "System.String", "System.Char"
                    SQLType = String.Format("[varchar]({0})", IIf(info.Length = Integer.MaxValue, "max", info.Length))

                Case "System.Decimal", "System.Double", "System.Single"
                    SQLType = String.Format("decimal({0},{1})", info.Precision, info.Scale)

                Case "System.Boolean"
                    SQLType = "[bit]"

                Case "System.Int16", "System.Int32", "System.UInt16", "System.UInt32"
                    SQLType = "[int]"

                Case "System.Int64", "System.UInt64"
                    SQLType = "[bigint]"

                Case "System.Guid"
                    SQLType = "[uniqueidentifier]"

            End Select
        End If

        Return SQLType
    End Function
End Class
