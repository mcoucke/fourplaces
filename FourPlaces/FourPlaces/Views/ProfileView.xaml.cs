using Storm.Mvvm.Forms;
using FourPlaces.ViewModels;

namespace FourPlaces.Views
{
    public partial class ProfileView : BaseContentPage
    {
        public ProfileView()
        {
            InitializeComponent();
            BindingContext = new ProfileViewModel();
        }
    }
}