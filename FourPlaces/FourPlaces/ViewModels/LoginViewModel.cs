using System;
using System.Windows.Input;
using Storm.Mvvm;
using Xamarin.Forms;
using FourPlaces.Services;
using FourPlaces.Views;
using FourPlaces.Dtos;
using System.Net.Http;

namespace FourPlaces.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string url = "https://td-api.julienmialon.com/auth/login";
        private ApiClient _apiClient;
        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _apiClient = new ApiClient();
            LoginCommand = new Command(LoginAction);
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private async void LoginAction()
        {
            HttpResponseMessage response = await _apiClient.Execute(HttpMethod.Post, url, new LoginRequest() { Email = _username, Password = _password });
            Response<LoginResult> result = await _apiClient.ReadFromResponse<Response<LoginResult>>(response);

            bool success = result.IsSuccess;

            if(success)
            {
                await NavigationService.PushAsync(new PlacesView(result.Data.accessToken));
            }
        }
    }
}
