using Syncfusion.ListView.XForms.UWP;

namespace ThingsToRemember.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
this.InitializeComponent();
SfListViewRenderer.Init();

            LoadApplication(new ThingsToRemember.App());
        }
    }
}
