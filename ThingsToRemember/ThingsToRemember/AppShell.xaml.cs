using ThingsToRemember.Views;
using Xamarin.Forms;

namespace ThingsToRemember
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(InitialPage),          typeof(InitialPage));
            Routing.RegisterRoute(nameof(AddJournalView),       typeof(AddJournalView));
            Routing.RegisterRoute(nameof(EditJournal),          typeof(EditJournal));
            Routing.RegisterRoute(nameof(EntryListView),        typeof(EntryListView));
            Routing.RegisterRoute(nameof(EntryPage),            typeof(EntryPage));
            Routing.RegisterRoute(nameof(ConfigurationView),    typeof(ConfigurationView));
            Routing.RegisterRoute(nameof(AddMoodView),          typeof(AddMoodView));
            Routing.RegisterRoute(nameof(EditMoodPopUp),        typeof(EditMoodPopUp));
            Routing.RegisterRoute(nameof(EditJournalTypePopUp), typeof(EditJournalTypePopUp));
            Routing.RegisterRoute(nameof(AddJournalTypeView),   typeof(AddJournalTypeView));
            Routing.RegisterRoute(nameof(EntryTextPage),        typeof(EntryTextPage));
            Routing.RegisterRoute(nameof(EntryMediaPage),       typeof(EntryMediaPage));
        }
    }
}
