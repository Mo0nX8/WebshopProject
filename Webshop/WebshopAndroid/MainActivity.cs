using Microsoft.Extensions.DependencyInjection;
using WebshopAndroid.data;

namespace WebshopAndroid
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private static ServiceProvider _serviceProvider { get; set; }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            _serviceProvider = IoCContainer._serviceProvider;
        }
    }
}