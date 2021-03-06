﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Intranet.App.Services;
using Intranet.App.Views;
using Akavache;

namespace Intranet.App
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            //Akavache.Registrations.Start("IntranetApp");
            BlobCache.ApplicationName = "IntranetApp";

            DependencyService.Register<EtiquetasServices>();

            MainPage = new NavigationPage(new TagsListViewPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}
