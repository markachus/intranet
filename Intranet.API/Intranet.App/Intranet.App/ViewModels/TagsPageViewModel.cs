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

        private EtiquetasServices _tagService = null;
        public Command LoadItemsCommand { get; set; }
        public Command NuevaTagCommand { get; set; }


        public TagsPageViewModel()
        {
            Items = new ObservableCollection<EtiquetaModel>();

            _tagService = new EtiquetasServices();
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
                        //TODO add message
                        throw;
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


    }
}
