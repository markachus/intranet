using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Auth;

namespace Intranet.App.Droid
{
    [Activity(Label = "Intranet.App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            var authenticator = new OAuth2Authenticator(
            "native_implicit",
            "read",
            new Uri("https://markachusidsrv.azurewebsites.net/identity/connect/authorize"),
            new Uri("https://markachusidsrv.azurewebsites.net/identity"));

            authenticator.Completed += Authenticator_Completed;

            StartActivity(authenticator.GetUI(this));
        }

        private void Authenticator_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        protected override void OnDestroy()
        {
            Akavache.BlobCache.Shutdown();
        }
    }
}