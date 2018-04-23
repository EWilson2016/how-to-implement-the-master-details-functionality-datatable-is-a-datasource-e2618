using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MasterDetailInside {
    public class ViewModel {
        public ViewModel() {
            ParentData = new DataTable();
            ParentData.Columns.Add("Id", typeof(int));
            ParentData.Columns.Add("Text", typeof(string));
            ChildData = new DataTable();
            ChildData.Columns.Add("ParentId", typeof(int));
            ChildData.Columns.Add("Text", typeof(string));
            for (int i = 0; i < 100; i++) {
                ParentData.Rows.Add(i, "Master" + i);
                for (int j = 0; j < 50; j++)
                    ChildData.Rows.Add(i, "Detail" + j + " Master" + i);
            }
        }
        public DataTable ParentData { get; set; }
        public DataTable ChildData { get; set; }
    }
}
