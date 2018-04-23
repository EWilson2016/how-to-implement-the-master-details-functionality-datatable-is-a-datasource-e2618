Imports System
Imports System.Collections.Generic
Imports System.Windows
Imports DevExpress.Xpf.Grid
Imports System.Globalization
Imports System.Windows.Data
Imports System.Windows.Markup
Imports System.Data

Namespace MasterDetailInside
    Partial Public Class Window1
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub
    End Class

    Public Class DetailSourceConverter
        Inherits MarkupExtension
        Implements IMultiValueConverter

        Public Function Convert(ByVal values() As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
            Dim masterRowId = values(0)
            Dim childData = DirectCast(values(1), DataTable)
            Dim childView = New DataView(childData)
            childView.RowFilter = String.Format("ParentId = '{0}'", masterRowId.ToString())
            Return childView
        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetTypes() As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function

        Public Overrides Function ProvideValue(ByVal serviceProvider As IServiceProvider) As Object
            Return Me
        End Function
    End Class
End Namespace
