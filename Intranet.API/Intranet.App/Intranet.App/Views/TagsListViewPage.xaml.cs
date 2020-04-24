using Intranet.App.Models;
using Intranet.App.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Intranet.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TagsListViewPage : ContentPage
    {
        public TagsListViewPage()
        {
            InitializeComponent();
        }


        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await this.Navigation.PushAsync(
                new EtiquetaView { BindingContext = new EtiquetaViewModel((EtiquetaModel)e.Item) });

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
