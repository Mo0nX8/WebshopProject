using Microsoft.AspNetCore.Builder;

namespace DbInitializer
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

            }
        }
    }
}
