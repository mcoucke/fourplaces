using Storm.Mvvm.Forms;
using FourPlaces.ViewModels;

namespace FourPlaces.Views
{
    public partial class PlacesView : BaseContentPage
    {
        public PlacesView(string accessToken)
        {
            InitializeComponent();
            BindingContext = new PlacesViewModel(accessToken);
        }
    }
}