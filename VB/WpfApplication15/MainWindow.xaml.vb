' Developer Express Code Central Example:
' How to implement the Master-Details functionality (DataTable is a DataSource)
' 
' DataRow does not have a property that includes all child rows. So, it is
' impossible to bind the child GridControl DataSource to child rows collection via
' xaml directly. A solution is to implement a multi binding converter. In this
' converter, pass the RowHandle and the ActiveView. In the Convert method, create
' a DataView passing a child table to the DataView constructor, filter this
' DataView based by the current value of the RowHandle, return the filtered
' DataView.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E2618


Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Data
Imports System.Windows.Markup
Imports DevExpress.Xpf.Grid

Namespace WpfApplication15
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		#Region "ExpandedProperty"
		Public Shared ReadOnly ExpandedProperty As DependencyProperty

		Public Shared Sub SetExpanded(ByVal element As DependencyObject, ByVal value As Boolean)
			element.SetValue(ExpandedProperty, value)
		End Sub

		Public Shared Function GetExpanded(ByVal element As DependencyObject) As Boolean
			Return CBool(element.GetValue(ExpandedProperty))
		End Function

		Shared Sub New()
			ExpandedProperty = DependencyProperty.RegisterAttached("Expanded", GetType(Boolean), GetType(MainWindow), New PropertyMetadata(False))
		End Sub
		#End Region

		Public Sub New()
			InitializeComponent()
		End Sub

		Public Function CreateData() As DataSet
			Dim mdt As New DataTable("Company")
			mdt.Columns.Add(New DataColumn("Name", GetType(String)))
			mdt.Columns.Add(New DataColumn("ID", GetType(Integer)))
			mdt.Rows.Add("Ford", 4)
			mdt.Rows.Add("Nissan", 5)
			mdt.Rows.Add("Mazda", 6)

			Dim ddt As New DataTable("Models")
			ddt.Columns.Add(New DataColumn("Name", GetType(String)))
			ddt.Columns.Add(New DataColumn("MaxSpeed", GetType(Integer)))
			ddt.Columns.Add(New DataColumn("CompanyName", GetType(String)))
			ddt.Rows.Add("FordFocus", 400, "Ford")
			ddt.Rows.Add("FordST", 400, "Ford")
			ddt.Rows.Add("Note", 1000, "Nissan")
			'ddt.Rows.Add("Mazda3", 1000, "Mazda");

			ds = New DataSet("CM")
			ds.Tables.Add(mdt)
			ds.Tables.Add(ddt)
			Dim dr As New DataRelation("CompanyModels", mdt.Columns("Name"), ddt.Columns("CompanyName"))
			ds.Relations.Add(dr)

			Return ds
		End Function

		Private ds As DataSet
		Private Sub Window_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
			ds = Me.CreateData()
			gridControl1.DataSource = ds.Tables("Company")

		End Sub
	End Class

	Public Class MyConverter
		Inherits MarkupExtension
		Implements IMultiValueConverter
		Public Overrides Function ProvideValue(ByVal serviceProvider As IServiceProvider) As Object
			Return Me
		End Function

		#Region "IMultiValueConverter Members"

		Public Function Convert(ByVal values() As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IMultiValueConverter.Convert
			Dim view As TableView = TryCast(values(0), TableView)
			If view IsNot Nothing Then
				Dim gridControl1 As GridControl = view.Grid
				Dim company As DataTable = TryCast(gridControl1.DataSource, DataTable)
				Dim dt As DataTable = company.DataSet.Tables("Models")
				If dt Is Nothing Then
					Return Nothing
				End If
				Dim rowIndex As Integer = CInt(Fix(values(1)))
				Dim drv As DataRowView = TryCast(gridControl1.GetRow(rowIndex), DataRowView)
				Dim dv As New DataView(dt)
				dv.RowFilter = String.Format("CompanyName = '{0}'", drv("Name").ToString())
				Return dv
			End If
			Return Nothing
		End Function

		Public Function ConvertBack(ByVal value As Object, ByVal targetTypes() As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
			Throw New NotImplementedException()
		End Function

		#End Region
	End Class

	Public Class MyConverterExpanderState
		Inherits MarkupExtension
		Implements IMultiValueConverter
		Public Overrides Function ProvideValue(ByVal serviceProvider As IServiceProvider) As Object
			Return Me
		End Function

		#Region "IMultiValueConverter Members"

		Public Function Convert(ByVal values() As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IMultiValueConverter.Convert

			Dim view As TableView = TryCast(values(0), TableView)
			If view Is Nothing Then
				Return False
			End If
			Dim gridControl1 As GridControl = view.Grid
			Dim company As DataTable = TryCast(gridControl1.DataSource, DataTable)
			Dim dt As DataTable = TryCast(company.DataSet.Tables("Models"), DataTable)
			If dt Is Nothing Then
				Return False
			End If
			Dim rowIndex As Integer = CInt(Fix(values(1)))
			Dim drv As DataRowView = TryCast(gridControl1.GetRow(rowIndex), DataRowView)
			Dim dv As New DataView(dt)
			dv.RowFilter = String.Format("CompanyName = '{0}'", drv("Name").ToString())
			If dv.Count = 0 Then
				Return False
			End If
			Return True
		End Function

		Public Function ConvertBack(ByVal value As Object, ByVal targetTypes() As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
			Throw New NotImplementedException()
		End Function

		#End Region
	End Class

End Namespace
