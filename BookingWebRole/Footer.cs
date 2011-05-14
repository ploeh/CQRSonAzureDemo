using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Ploeh.Samples.Booking.WebUI
{
    public static class Footer
    {
        public static string RoleId
        {
            get
            {
                if (RoleEnvironment.IsAvailable)
                {
                    return RoleEnvironment.CurrentRoleInstance.Id;
                }
                return "(site is running outside of Azure, so no role instance is available.)";
            }
        }
    }
}