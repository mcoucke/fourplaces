using FourPlaces.Dtos;
using FourPlaces.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FourPlaces.ViewModels
{
    class UploadImageViewModel : ViewModelBase
    {
        private string _url = "https://td-api.julienmialon.com/images";
        private byte[] _imageAsBytes;
        private ApiClient _apiClient;

        public ICommand UploadImageCommand { get; }
        public ICommand TakePhotoCommand { get; }

        private ImageSource _imageSrc;
        public ImageSource ImageSrc
        {
            get => _imageSrc;
            set => SetProperty(ref _imageSrc, value);
        }

        private EventHandler _takePhotoClicked;
        public EventHandler TakePhotoClicked
        {
            get => _takePhotoClicked;
            set => SetProperty(ref _takePhotoClicked, value);
        }

        public UploadImageViewModel()
        {
            _apiClient = new ApiClient();
            UploadImageCommand = new Command(UploadImageAction);
            TakePhotoCommand = new Command(TakePhotoAction);

        }
        private async void TakePhotoAction()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                //await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                DefaultCamera = CameraDevice.Front
            });

            if (file == null)
                return;

            //await DisplayAlert("File Location", file.Path, "OK");

            ImageSrc = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            _imageAsBytes = null;
            using (var memoryStream = new MemoryStream())
            {
                file.GetStream().CopyTo(memoryStream);
                file.Dispose();
                _imageAsBytes = memoryStream.ToArray();
            }
        }

        public async void UploadImageAction()
        {
            HttpResponseMessage response = await _apiClient.UploadImage(HttpMethod.Post, _url, _imageAsBytes, App.Current.Properties["AccessTokken"].ToString());
            Response<ImageItem> result = await _apiClient.ReadFromResponse<Response<ImageItem>>(response);

            if (result.IsSuccess)
            {
                Console.WriteLine("IMAGEID : " + result.Data.Id);
            }
        }
    }
}
