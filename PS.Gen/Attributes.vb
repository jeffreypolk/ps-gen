
Namespace Attributes

    <AttributeUsage(AttributeTargets.Class)>
    Public Class TableGenerationAttribute
        Inherits Attribute
        Property Name As String = String.Empty
        Property Schema As String = "dbo"
    End Class

    <AttributeUsage(AttributeTargets.Property)>
    Public Class ColumnGenerationAttribute
        Inherits Attribute
        Property Name As String = String.Empty
        Property IsPrimaryKey As Boolean = False
        Property PrimaryKeyOrdinal As Integer = 1
        Property Length As Integer = 50
        Property Precision As Integer = 10
        Property Scale As Integer = 2
        Property FullDataType As String = String.Empty
        Property IsMoney As Boolean = False
        Property IsNullable As Boolean = True
        Property IdentitySeed As Integer = 1
        Property IdentityIncrement As Integer = 0
        Property DefaultValue As String
    End Class

End Namespace
