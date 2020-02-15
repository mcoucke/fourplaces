using Storm.Mvvm.Forms;
using FourPlaces.ViewModels;

namespace FourPlaces.Views
{
    public partial class LoginView : BaseContentPage
    {
        public LoginView()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }
    }
}