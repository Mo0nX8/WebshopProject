using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Services.Interfaces;

namespace Webshop.Services.Services.Register
{
    public class ValidationResultModel 
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }
        public ValidationResultModel()
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
