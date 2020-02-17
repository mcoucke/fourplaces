using Storm.Mvvm;
using System.Net.Http;
using FourPlaces.Services;
using FourPlaces.Dtos;
using System.Collections.ObjectModel;
using FourPlaces.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace FourPlaces.ViewModels
{
    class PlacesViewModel : ViewModelBase
    {
        private string _url = "https://td-api.julienmialon.com/places";
        private ApiClient _apiClient;

        public ICommand ViewProfileCommand { get; }

        private ObservableCollection<PlaceItemSummary> _placeList;
        public ObservableCollection<PlaceItemSummary> PlaceList
        {
            get => _placeList;
            set => SetProperty(ref _placeList, value);
        }

        private PlaceItemSummary _selectedPlace;
        public PlaceItemSummary SelectedPlace
        {
            get => _selectedPlace;
            set
            {
                if (SetProperty(ref _selectedPlace, value) && value != null)
                {
                    ShowPlaceAction(value);
                    SelectedPlace = null;
                }
            }

        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _imageId;
        public string ImageId
        {
            get => _imageId;
            set => SetProperty(ref _imageId, value);
        }
        public PlacesViewModel()
        {
            _apiClient = new ApiClient();

            PlaceList = new ObservableCollection<PlaceItemSummary>();
            ViewProfileCommand = new Command(ViewProfileAction);
            GetPlaces();
        }

        private async void GetPlaces()
        {
            HttpResponseMessage response = 
                await _apiClient.Execute(HttpMethod.Get, _url, null, App.Current.Properties["AccessTokken"].ToString());
            Response<ObservableCollection<PlaceItemSummary>> result =   
                await _apiClient.ReadFromResponse<Response<ObservableCollection<PlaceItemSummary>>>(response);

            if (result.IsSuccess)
            {
                PlaceList = result.Data;
            }
        }
        public async void ShowPlaceAction(PlaceItemSummary place)
        {
            await NavigationService.PushAsync(new DetailsView(place.Id)) ;
        }

        public async void ViewProfileAction()
        {
            await NavigationService.PushAsync(new ProfileView());
        }
    }
}
