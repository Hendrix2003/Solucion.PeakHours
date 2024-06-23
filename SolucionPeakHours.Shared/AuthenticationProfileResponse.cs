using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolucionPeakHours.Shared
{
    public class AuthenticationProfileResponse
    {
        public string Profile { get; set; }
        public string Description { get; set; }

        public virtual List<PermissionsResponse> Permissions { get; set; }
    }
}
