using Intranet.App.Models;
using Intranet.App.Services;
using Intranet.App.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Intranet.App.ViewModels
{
    public class TagsPageViewModel : INotifyPropertyChanged
    {

        private IEtiquetaService _tagService = null;
        public Command LoadItemsCommand { get; set; }
        public Command NuevaTagCommand { get; set; }


        public TagsPageViewModel()
        {
            Items = new ObservableCollection<EtiquetaModel>();

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            

            _tagService = DependencyService.Get<IEtiquetaService>();
            LoadItemsCommand = new Command( async () => {

                if (Connectivity.NetworkAccess != NetworkAccess.None)
                {
                    try
                    {
                        IsBusy = true;
                        Items.Clear();
                        var data = await _tagService.GetAll();
                        foreach (EtiquetaModel t in data) { Items.Add(t); }
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert(
                                                        "Error desconocido",
                                                        ex.Message,
                                                        "OK");
                    }
                    finally {
                        IsBusy = false;
                    }       
                }
            });

            this.NuevaTagCommand = new Command(async () => {
                await Application.Current.MainPage.Navigation.PushAsync(
                    new EtiquetaView { BindingContext = new EtiquetaViewModel(this.Items)});
            });



            LoadItemsCommand.Execute(null);
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            InternetKO = e.NetworkAccess != NetworkAccess.Internet;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<EtiquetaModel> Items { get;}

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set {
                var args = new PropertyChangedEventArgs(nameof(IsBusy));

                if (_isBusy == value) return;
                _isBusy = value;
                PropertyChanged?.Invoke(this, args);
            }
        }

        private bool _internetKO = false;

        public bool InternetKO
        {
            get { return _internetKO; }
            set { 
                var args = new PropertyChangedEventArgs(nameof(InternetKO));

                if (_internetKO == value) return;
                _internetKO = value;
                PropertyChanged?.Invoke(this, args);
            }
        }


    }
}
