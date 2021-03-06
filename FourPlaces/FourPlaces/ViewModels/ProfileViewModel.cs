﻿using FourPlaces.Dtos;
using FourPlaces.Services;
using FourPlaces.Views;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FourPlaces.ViewModels
{
    class ProfileViewModel : ViewModelBase
    {
        private string _url = "https://td-api.julienmialon.com/me";
        private ApiClient _apiClient;
        public ICommand EditImageCommand { get; }
        public ICommand EditProfileCommand { get; }

        private UserItem _user;
        public UserItem User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

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

        private int _imageId;
        public int ImageId
        {
            get => _imageId;
            set => SetProperty(ref _imageId, value);
        }

        public ProfileViewModel()
        {
            _apiClient = new ApiClient();
            EditImageCommand = new Command(EditImageAction);
            EditProfileCommand = new Command(EditProfileAction);
            GetProfile();
        }
        private async void GetProfile()
        {
            HttpResponseMessage response =
                await _apiClient.Execute(HttpMethod.Get, _url, null, App.Current.Properties["AccessTokken"].ToString());
            Response<UserItem> result = await _apiClient.ReadFromResponse<Response<UserItem>>(response);

            if (result.IsSuccess)
            {
                User = result.Data;
                Email = _user.Email;
                FirstName = _user.FirstName;
                LastName = _user.LastName;
                ImageId = _user.ImageId == null ? 0 : _user.ImageId.Value;
            }
        }

        private async void EditImageAction()
        {
            await NavigationService.PushAsync(new UploadImageView(FirstName, LastName));
        }

        private async void EditProfileAction()
        {
            await NavigationService.PushAsync(new EditProfileView(ImageId));
        }

        public override Task OnResume()
        {
            GetProfile();
            return base.OnResume();
        }
    }
}
