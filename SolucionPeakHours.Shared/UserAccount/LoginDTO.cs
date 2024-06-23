using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucionPeakHours.Shared.UserAccount
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Ingrese correo")]
        public string? UserName { get; set; }


        [Required(ErrorMessage = "Ingrese contraseña")]
        public string? Pwd { get; set; }
    }
}
