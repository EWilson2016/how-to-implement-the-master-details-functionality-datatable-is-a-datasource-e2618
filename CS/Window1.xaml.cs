using System;
using System.Collections.Generic;
using System.Windows;
using DevExpress.Xpf.Grid;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Data;

namespace MasterDetailInside {
    public partial class Window1 : Window {
        public Window1() {
            InitializeComponent();
        }
    }

    public class DetailSourceConverter : MarkupExtension, IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            var masterRowId = values[0];
            var childData = (DataTable)values[1];
            var childView = new DataView(childData);
            childView.RowFilter = string.Format("ParentId = '{0}'", masterRowId.ToString());
            return childView;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }
    }
}
