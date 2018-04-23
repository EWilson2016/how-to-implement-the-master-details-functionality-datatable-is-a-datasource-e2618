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
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace WpfApplication15
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    }
}
