Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Openpay.Entities
    Public Class BankAccount
        Inherits OpenpayResourceObject

        <JsonProperty(PropertyName:="creation_date", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property CreationDate As DateTime?
        <JsonProperty(PropertyName:="alias, NullValueHandling=NullValueHandling.Ignore")>
        Public Property [Alias] As String
        <JsonProperty(PropertyName:="clabe")>
        Public Property CLABE As String
        <JsonProperty(PropertyName:="holder_name")>
        Public Property HolderName As String
        <JsonProperty(PropertyName:="bank_name", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property BankName As String
        <JsonProperty(PropertyName:="bank_code", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property BankCode As String
    End Class
End Namespace
