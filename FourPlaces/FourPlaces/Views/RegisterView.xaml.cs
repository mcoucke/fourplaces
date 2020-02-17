using FourPlaces.ViewModels;
using Storm.Mvvm.Forms;

namespace FourPlaces.Views
{
    public partial class RegisterView : BaseContentPage
    {
        public RegisterView()
        {
            InitializeComponent();
            BindingContext = new RegisterViewModel();
        }
    }
}