using ThingsToRemember.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ThingsToRemember.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutViewModel ViewModel { get; set; }
        public AboutPage()
        {
            InitializeComponent();

            ViewModel = new AboutViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            VersionNumberSpan.Text = $"{VersionTracking.CurrentVersion}";
            BuildNumberSpan.Text   = $".{VersionTracking.CurrentBuild}";
            DatabaseSizeSpan.Text  = $"{ViewModel.DatabaseSizeForDisplay}";
        }
    }
}