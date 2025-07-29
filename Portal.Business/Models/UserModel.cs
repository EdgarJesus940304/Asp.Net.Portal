using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Business.Models
{
    public class UserModel
    {
        public UserModel()
        {
            Id = 0;
            Name = null;
            CreationDate = DateTime.Now;
            UserName = null;
            Password = null;
            Perfil = null;
            Status = null;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreationDate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? Perfil { get; set; }
        public int? Status { get; set; }
        public string StatusName
        {
            get
            {
                return Status.HasValue ? (Status > 0 ? "Activo" : "Inactivo") : "Inactivo";
            }
        }
    }
}
