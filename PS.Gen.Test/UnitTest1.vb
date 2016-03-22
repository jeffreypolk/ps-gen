Imports PS.Gen.Attributes

<TableGeneration(Name:="tblPerson", Schema:="Util")>
Public Class Person

    <ColumnGeneration(IsPrimaryKey:=False, IdentitySeed:=1000, IdentityIncrement:=10)>
    Property PersonId As Integer

    <ColumnGeneration(IsPrimaryKey:=False, PrimaryKeyOrdinal:=0)>
    Property SolutionId As Integer

    <ColumnGeneration(IsPrimaryKey:=False, PrimaryKeyOrdinal:=1)>
    Property ParameterCode As String

    <ColumnGeneration(Name:="FullName", Length:=200)>
    Property Name As String

    <ColumnGeneration(IsMoney:=True)>
    Property Salary As Decimal

    Property Address As String

End Class

<TestClass()> Public Class UnitTest1

    <TestMethod()> Public Sub TestMethod1()
        Dim tgen As New PS.Gen.TableGenerator()
        Dim code As String = tgen.Generate(GetType(Person))
    End Sub

End Class