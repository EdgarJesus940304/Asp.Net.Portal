using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Business.Models.DataTables
{
    public class RootResult<T>
    {
        public DataTablesResult<T> Data { get; set; }
    }
}
