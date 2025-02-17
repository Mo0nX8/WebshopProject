using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Services.Services.ViewModel
{
    /// <summary>
    /// This class helps with the password change. 
    /// </summary>
    public class PasswordChangeViewModel
    {
        /// <summary>
        /// This property stores the new password.
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// This property stores the current password.
        /// </summary>

        public string CurrentPassword { get; set; }
    }
}
