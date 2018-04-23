// Developer Express Code Central Example:
// How to implement the Master-Details functionality (DataTable is a ItemsSource)
// 
// DataRow does not have a property that includes all child rows. So, it is
// impossible to bind the child GridControl ItemsSource to child rows collection via
// xaml directly. A solution is to implement a multi binding converter. In this
// converter, pass the RowHandle and the ActiveView. In the Convert method, create
// a DataView passing a child table to the DataView constructor, filter this
// DataView based by the current value of the RowHandle, return the filtered
// DataView.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E2618

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Windows.Markup;
using DevExpress.Xpf.Grid;

namespace WpfApplication15
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region ExpandedProperty
        public static readonly DependencyProperty ExpandedProperty;

        public static void SetExpanded(DependencyObject element, bool value) {
            element.SetValue(ExpandedProperty, value);
        }

        public static bool GetExpanded(DependencyObject element) {
            return (bool)element.GetValue(ExpandedProperty);
        }

        static MainWindow()
        {
            ExpandedProperty = DependencyProperty.RegisterAttached("Expanded", typeof(bool),
                typeof(MainWindow), new PropertyMetadata(false));
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        public DataSet CreateData()
        {
            DataTable mdt = new DataTable("Company");
            mdt.Columns.Add(new DataColumn("Name", typeof(string)));
            mdt.Columns.Add(new DataColumn("ID", typeof(int)));
            mdt.Rows.Add("Ford", 4);
            mdt.Rows.Add("Nissan", 5);
            mdt.Rows.Add("Mazda", 6);

            DataTable ddt = new DataTable("Models");
            ddt.Columns.Add(new DataColumn("Name", typeof(string)));
            ddt.Columns.Add(new DataColumn("MaxSpeed", typeof(int)));
            ddt.Columns.Add(new DataColumn("CompanyName", typeof(string)));
            ddt.Rows.Add("FordFocus", 400, "Ford");
            ddt.Rows.Add("FordST", 400, "Ford");
            ddt.Rows.Add("Note", 1000, "Nissan");
            //ddt.Rows.Add("Mazda3", 1000, "Mazda");

            ds = new DataSet("CM");
            ds.Tables.Add(mdt);
            ds.Tables.Add(ddt);
            DataRelation dr = new DataRelation("CompanyModels", mdt.Columns["Name"], ddt.Columns["CompanyName"]);
            ds.Relations.Add(dr);

            return ds;
         }

        DataSet ds;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ds = this.CreateData();
            gridControl1.ItemsSource = ds.Tables["Company"];
            
        }
    }

    public class MyConverter : MarkupExtension, IMultiValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TableView view = values[0] as TableView;
            if (view != null) {
                GridControl gridControl1 = view.Grid;
                DataTable company = gridControl1.ItemsSource as DataTable;
                DataTable dt = company.DataSet.Tables["Models"];
                if (dt == null) return null;
                int rowIndex = (int)values[1];
                DataRowView drv = gridControl1.GetRow(rowIndex) as DataRowView;
                DataView dv = new DataView(dt);
                dv.RowFilter = String.Format("CompanyName = '{0}'", drv["Name"].ToString());
                return dv;
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class MyConverterExpanderState : MarkupExtension, IMultiValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            TableView view = values[0] as TableView;
            if (view == null) return false;
            GridControl gridControl1 = view.Grid;
            DataTable company = gridControl1.ItemsSource as DataTable;
            DataTable dt = company.DataSet.Tables["Models"] as DataTable;
            if (dt == null) return false;
            int rowIndex = (int)values[1];
            DataRowView drv = gridControl1.GetRow(rowIndex) as DataRowView;
            DataView dv = new DataView(dt);
            dv.RowFilter = String.Format("CompanyName = '{0}'", drv["Name"].ToString());
            if (dv.Count == 0) return false;
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
