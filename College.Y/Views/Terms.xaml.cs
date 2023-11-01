using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using College.Y.Models;
using College.Y.ViewModels;
using College.Y.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace College.Y.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Terms : ContentPage
    {
        public Terms()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            //self call before page visible
            base.OnAppearing();
            //bind list to collectionview source
            cvTerm.ItemsSource = await App.Database.GetTerms();
        }
        private async void tbiAdd_Clicked(object sender, EventArgs e)
        {
            //goto add new
            await Shell.Current.GoToAsync(nameof(TermsInfo));

        }
        private async void cvTerm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                //bind current to var
                Term term = (Term)e.CurrentSelection.FirstOrDefault();
                //bind page to var
                var edit = new TermsInfo(term);
                //bind 'add' to title if null
                edit.Title = (term.TermTitle == null) ? "Add" : "Edit";
                //top of nav stack
                await Navigation.PushAsync(edit);
            }
        }
    }
}