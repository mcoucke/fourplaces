using Storm.Mvvm;
using System.Net.Http;
using FourPlaces.Services;
using FourPlaces.Dtos;
using System.Collections.ObjectModel;
using FourPlaces.Views;

namespace FourPlaces.ViewModels
{
    class PlacesViewModel : ViewModelBase
    {
        private string _token;
        private string _url = "https://td-api.julienmialon.com/places";
        private ApiClient _apiClient;

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
        public PlacesViewModel(string accessToken)
        {
            _apiClient = new ApiClient();
            _token = accessToken;

            PlaceList = new ObservableCollection<PlaceItemSummary>();
            GetPlaces();
        }

        private async void GetPlaces()
        {
            HttpResponseMessage response = await _apiClient.Execute(HttpMethod.Get, _url, null, _token);
            Response<ObservableCollection<PlaceItemSummary>> result =   
                await _apiClient.ReadFromResponse<Response<ObservableCollection<PlaceItemSummary>>>(response);

            if (result.IsSuccess)
            {
                PlaceList = result.Data;
            }
        }
        public async void ShowPlaceAction(PlaceItemSummary place)
        {
            await NavigationService.PushAsync(new DetailsView(_token, place.Id)) ;
        }
    }
}
