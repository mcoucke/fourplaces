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

        protected override void OnStart() { }

        protected override void OnSleep() { }

        protected override void OnResume() { }
    }
}
