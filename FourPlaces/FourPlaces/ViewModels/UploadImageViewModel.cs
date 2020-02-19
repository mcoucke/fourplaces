using FourPlaces.Dtos;
using FourPlaces.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Storm.Mvvm;
using System;
using System.IO;
using System.Net.Http;
using System.Windows.Input;
using Xamarin.Forms;

namespace FourPlaces.ViewModels
{
    class UploadImageViewModel : ViewModelBase
    {
        private string _urlImage = "https://td-api.julienmialon.com/images";
        private string _urlUser = "https://td-api.julienmialon.com/me";
        private string _firstName;
        private string _lastName;
        private byte[] _imageAsBytes;
        private ApiClient _apiClient;

        public ICommand UploadImageCommand { get; }
        public ICommand TakePhotoCommand { get; }
        public ICommand PickPhotoCommand { get; }

        private ImageSource _imageSrc;
        public ImageSource ImageSrc
        {
            get => _imageSrc;
            set => SetProperty(ref _imageSrc, value);
        }
        public UploadImageViewModel(string firstname, string lastname)
        {
            _apiClient = new ApiClient();
            UploadImageCommand = new Command(UploadImageAction);
            TakePhotoCommand = new Command(TakePhotoAction);
            PickPhotoCommand = new Command(PickPhotoAction);
            _firstName = firstname;
            _lastName = lastname;

        }
        private async void TakePhotoAction()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                DefaultCamera = CameraDevice.Front,
                PhotoSize = PhotoSize.Small
            });

            if (file == null)
                return;

            await Application.Current.MainPage.DisplayAlert("File Location", file.Path, "OK");

            ImageSrc = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            _imageAsBytes = FromImageToBinary(file.Path);
        }

        private async void PickPhotoAction()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            await Application.Current.MainPage.DisplayAlert("File Location", file.Path, "OK");

            ImageSrc = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            _imageAsBytes = FromImageToBinary(file.Path);
        }

        public async void UploadImageAction()
        {
            HttpResponseMessage response_upload = await _apiClient.UploadImage(HttpMethod.Post, _urlImage, _imageAsBytes, App.Current.Properties["AccessTokken"].ToString());
            Response<ImageItem> result_upload = await _apiClient.ReadFromResponse<Response<ImageItem>>(response_upload);
            if (result_upload.IsSuccess)
            {
                HttpResponseMessage response = await _apiClient.Execute(new HttpMethod("PATCH"), _urlUser, 
                    new UpdateProfileRequest() { FirstName = _firstName, LastName = _lastName, ImageId = result_upload.Data.Id },
                    App.Current.Properties["AccessTokken"].ToString());
                Response<UserItem> result = await _apiClient.ReadFromResponse<Response<UserItem>>(response);
                if (result.IsSuccess)
                {
                    Console.WriteLine("image uploaded");
                    await NavigationService.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Error while updating profile", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Error while updating profile", "OK");
            }
        }

        public byte[] FromImageToBinary(string imagePath)
        {
            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, (int)fileStream.Length);
            fileStream.Close();
            return buffer;
        }
    }
}
