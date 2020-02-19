using FourPlaces.ViewModels;
using Storm.Mvvm.Forms;

namespace FourPlaces.Views
{
    public partial class AddPlaceView : BaseContentPage
    {
        public AddPlaceView()
        {
            InitializeComponent();
            BindingContext = new AddPlaceViewModel();
        }
    }
}