using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Business.Models
{
    public class PharmaceuticalFormModel
    {
        public PharmaceuticalFormModel()
        {
            Id = 0;
            Name = null;
            Habilitado = 1;
        }

        public int? Id { get; set; }
        public string Name { get; set; }
        public int? Habilitado { get; set; }

        public string StatusName
        {
            get
            {
                return Habilitado.HasValue ? (Habilitado > 0 ? "Activo" : "Inactivo") : "Inactivo";
            }
        }
    }
}
