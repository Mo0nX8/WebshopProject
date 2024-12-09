using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopAndroid.data
{
    
    public class IoCContainer
    {
        
        public static ServiceProvider _serviceProvider { get; set; }
        /// <summary>
        /// Sets ServiceProvider for IoCContainer
        /// </summary>
        public void SetServiceProvider()
        {
            var serviceCollection=new ServiceCollection();
            
            _serviceProvider = serviceCollection.BuildServiceProvider();
            
        }
    }
}
