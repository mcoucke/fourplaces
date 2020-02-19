

using FourPlaces.ViewModels;
using Storm.Mvvm.Forms;

namespace FourPlaces.Views
{
    public partial class EditProfileView : BaseContentPage
    {
        public EditProfileView(int imageId)
        {
            InitializeComponent();
            BindingContext = new EditProfileViewModel(imageId);
        }
    }
}