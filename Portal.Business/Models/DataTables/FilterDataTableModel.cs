using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Business.Models.DataTables
{
    public class FilterDataTableModel
    {
        public string SearchBy { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public string SortBy { get; set; }
        public bool SortDir { get; set; }
    }
}
