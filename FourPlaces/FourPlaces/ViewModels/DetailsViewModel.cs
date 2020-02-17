using FourPlaces.Dtos;
using FourPlaces.Services;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FourPlaces.ViewModels
{
    class DetailsViewModel : ViewModelBase
    {
        private string _url = "https://td-api.julienmialon.com/places/";
        private ApiClient _apiClient;

        public ICommand OpenLocationCommand { get; }

        private PlaceItem _place;
        public PlaceItem Place
        {
            get => _place;
            set => SetProperty(ref _place, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private int _imageId;
        public int ImageId
        {
            get => _imageId;
            set => SetProperty(ref _imageId, value);
        }

        private double _latitude;
        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        private double _longitude;
        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        private List<CommentItem> _comments;
        public List<CommentItem> Comments
        {
            get => _comments;
            set => SetProperty(ref _comments, value);
        }


        public DetailsViewModel(int id)
        {
            _apiClient = new ApiClient();
            OpenLocationCommand = new Command(OpenLocationAction);
            GetPlace(id);
        }


        private async void GetPlace(int id)
        {
            HttpResponseMessage response = 
                await _apiClient.Execute(HttpMethod.Get, _url + id, null, App.Current.Properties["AccessTokken"].ToString());
            Response<PlaceItem> result = await _apiClient.ReadFromResponse<Response<PlaceItem>>(response);

            if (result.IsSuccess)
            {
                Place = result.Data;
                Title = _place.Title;
                Description = _place.Description;
                ImageId = _place.ImageId;
                Longitude = _place.Longitude;
                Latitude = _place.Latitude;
                Comments = _place.Comments;
            }
            else
            {
                Title = "Error";
            }
        }

        private async void OpenLocationAction()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                await Launcher.OpenAsync("http://maps.apple.com/?q=394+Pacific+Ave+San+Francisco+CA");
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                // opens the Maps app directly
                await Launcher.OpenAsync("geo:" + Latitude + ',' + Longitude);
            }
        }
    }
}
