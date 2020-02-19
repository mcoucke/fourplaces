using FourPlaces.Dtos;
using FourPlaces.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Storm.Mvvm;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FourPlaces.ViewModels
{
    class AddPlaceViewModel : ViewModelBase
    {
        private string _urlPlaces = "https://td-api.julienmialon.com/places";
        private string _urlImage = "https://td-api.julienmialon.com/images";
        private ApiClient _apiClient;
        private byte[] _imageAsBytes;

        public ICommand AddPlaceCommand { get; }
        public ICommand GetCurrentLocationCommand { get; }
        public ICommand PickPictureCommand { get; }
        public ICommand TakePictureCommand { get; }

        private ImageSource _imageSrc;
        public ImageSource ImageSrc
        {
            get => _imageSrc;
            set => SetProperty(ref _imageSrc, value);
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

        private int _imageId = 0;

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

        public AddPlaceViewModel()
        {
            _apiClient = new ApiClient();
            AddPlaceCommand = new Command(AddPlaceAction);
            GetCurrentLocationCommand = new Command(GetCurrentLocationAction);
            PickPictureCommand = new Command(PickPictureAction);
            TakePictureCommand = new Command(TakePictureAction);
        }

        public async void AddPlaceAction()
        {
            await UploadImage();
            if (_imageId == 0)
            {
                return;
            }
            var place = new CreatePlaceRequest()
            {
                Title = Title,
                Description = Description,
                ImageId = _imageId,
                Latitude = Latitude,
                Longitude = Longitude
            };

            HttpResponseMessage response = await _apiClient.Execute(HttpMethod.Post, _urlPlaces, place, App.Current.Properties["AccessTokken"].ToString());
            Response result = await _apiClient.ReadFromResponse<Response>(response);

            if (result.IsSuccess)
            {
                await NavigationService.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Error while adding place", "OK");
            }

        }

        public async void GetCurrentLocationAction()
        {
            try
            {
                var location = await Geolocation.GetLocationAsync();

                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async void PickPictureAction()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", "No camera available.", "OK");
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

        public async void TakePictureAction()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", "No camera available.", "OK");
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

        public async Task UploadImage()
        {
            if (_imageAsBytes != null)
            {
                HttpResponseMessage response_upload = await _apiClient.UploadImage(HttpMethod.Post, _urlImage, _imageAsBytes, App.Current.Properties["AccessTokken"].ToString());
                Response<ImageItem> result_upload = await _apiClient.ReadFromResponse<Response<ImageItem>>(response_upload);
                if (result_upload.IsSuccess)
                {
                    _imageId = result_upload.Data.Id;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Error while uploading image", "OK");
                }
            } else
            {
                await Application.Current.MainPage.DisplayAlert("Warning", "Please select an image", "OK");
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
