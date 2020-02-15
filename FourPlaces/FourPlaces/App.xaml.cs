using Storm.Mvvm;
using FourPlaces.Views;


namespace FourPlaces
{
    public partial class App : MvvmApplication
    {
        public App() : base(() => new LoginView())
        {
            InitializeComponent();
        }
    }
}
