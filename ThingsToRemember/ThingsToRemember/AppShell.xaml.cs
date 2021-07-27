using System;
using System.Collections.Generic;
using ThingsToRemember.ViewModels;
using ThingsToRemember.Views;
using Xamarin.Forms;

namespace ThingsToRemember
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(InitialPage), typeof(InitialPage));
            Routing.RegisterRoute(nameof(AddJournal),  typeof(AddJournal));
            Routing.RegisterRoute(nameof(EditJournal), typeof(EditJournal));
            Routing.RegisterRoute(nameof(EntryList),   typeof(EntryList));
            Routing.RegisterRoute(nameof(EntryPage),   typeof(EntryPage));
        }

    }
}
