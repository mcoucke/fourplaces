
using FourPlaces.Dtos;
using FourPlaces.Services;
using Storm.Mvvm;
using System;
using System.Net.Http;
using System.Windows.Input;
using Xamarin.Forms;

namespace FourPlaces.ViewModels
{
    class EditProfileViewModel : ViewModelBase
    {
        private string _url = "https://td-api.julienmialon.com/me";
        private ApiClient _apiClient;
        private int _imageId;

        public ICommand EditProfileCommand { get; }

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

        public EditProfileViewModel(int imageId)
        {
            _imageId = imageId;
            _apiClient = new ApiClient();
            EditProfileCommand = new Command(EditProfileAction);
        }

        public async void EditProfileAction()
        {
            HttpResponseMessage response = await _apiClient.Execute(new HttpMethod("PATCH"), _url,
                    new UpdateProfileRequest() { FirstName = _firstName, LastName = _lastName, ImageId = _imageId },
                    App.Current.Properties["AccessTokken"].ToString());
            Response<UserItem> result = await _apiClient.ReadFromResponse<Response<UserItem>>(response);

            if (result.IsSuccess)
            {
                Console.WriteLine("profile updated");
                await NavigationService.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Error while updating profile", "OK");
            }
        }
    }
}
