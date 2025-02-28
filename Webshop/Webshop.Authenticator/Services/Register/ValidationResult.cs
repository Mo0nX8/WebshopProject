using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Services.Interfaces_For_Services;

namespace Webshop.Services.Services.Register
{
    public class ValidationResult 
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }
        public ValidationResult()
        {
            Errors = new List<string>();
            IsValid = true;
        }
        public void AddError(string error)
        {
            IsValid = false;
            Errors.Add(error);
        }
    }
}
