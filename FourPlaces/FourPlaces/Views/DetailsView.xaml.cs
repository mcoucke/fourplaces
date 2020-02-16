using FourPlaces.ViewModels;
using Storm.Mvvm.Forms;

namespace FourPlaces.Views
{
    public partial class DetailsView : BaseContentPage
    {
        public DetailsView(string token, int id)
        {
            InitializeComponent();
            BindingContext = new DetailsViewModel(token, id);
        }
    }
}