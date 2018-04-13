﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.CommonPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageMaster : ContentPage
    {
        public ListView ListView;

        public MainPageMaster()
        {
            InitializeComponent();

            BindingContext = new MainPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MainPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainPageMenuItem> MenuItems { get; set; }
            
            public MainPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MainPageMenuItem>(new[]
                {
                    new MainPageMenuItem { Id = 0, Title = "My Study Groups", Icon = "mystudygroups.png",TargetType = typeof(Views.CommonPages.MyStudyGroupsPage) },
                    new MainPageMenuItem { Id = 1, Title = "Study Groups Invitations", Icon = "invitationicon.png",TargetType = typeof(Views.CommonPages.ViewStudyGroupInvitations) },
                    new MainPageMenuItem { Id = 2, Title = "My Favorite Notebook", Icon = "menufavorite.png",TargetType = typeof(Views.CommonPages.ViewFavoriteNotebooks) },
                    new MainPageMenuItem { Id = 3, Title = "My Help Requests", Icon = "helpmenuitem.png",TargetType = typeof(Views.CommonPages.MyHelpRequests) },
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}