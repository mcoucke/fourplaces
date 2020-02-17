using FourPlaces.Dtos;
using FourPlaces.Services;
using FourPlaces.Views;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FourPlaces.ViewModels
{
    class RegisterViewModel : ViewModelBase
    {
        private string _url = "https://td-api.julienmialon.com/auth/register";
        private ApiClient _apiClient;
        public ICommand RegisterCommand { get; }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        public RegisterViewModel()
        {
            _apiClient = new ApiClient();
            RegisterCommand = new Command(RegisterAction);
        }

        private async void RegisterAction()
        {
            var data = new RegisterRequest() { Email = _email, FirstName = _firstName, LastName = _lastName, Password = _password };
            HttpResponseMessage response = await _apiClient.Execute(HttpMethod.Post, _url, data);
            Response<LoginResult> result = await _apiClient.ReadFromResponse<Response<LoginResult>>(response);

            if (result.IsSuccess)
            {
                App.Current.Properties["AccessTokken"] = result.Data.AccessToken;
                await NavigationService.PushAsync(new PlacesView());
            }
        }
    }
}
