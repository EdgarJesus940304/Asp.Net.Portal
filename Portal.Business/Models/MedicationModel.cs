using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Business.Models
{
    public class MedicationModel
    {
        public MedicationModel()
        {
            Id = 0;
            Name = null;
            Concentration = null;
            Price = null;
            Stock = null;
            Presentation = null;
            Enable = 1;
            PharmaceuticalForm = new PharmaceuticalFormModel();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Concentration { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public string Presentation { get; set; }
        public int? Enable { get; set; }
        public PharmaceuticalFormModel PharmaceuticalForm { get; set; }

        public string StatusName
        {
            get
            {
                return Enable.HasValue ? (Enable > 0 ? "Activo" : "Inactivo") : "Inactivo";
            }
        }
    }
}
