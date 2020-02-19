using FourPlaces.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Storm.Mvvm.Forms;
using System;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;

namespace FourPlaces.Views
{
    public partial class UploadImageView : BaseContentPage
    {

        public UploadImageView(string firstname, string lastname)
        {
            InitializeComponent();
            BindingContext = new UploadImageViewModel(firstname, lastname);
        }
    }
}