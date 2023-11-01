using College.Y.Models;
using College.Y.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace College.Y.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Assessments : ContentPage
    {
        public Assessments()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            //self call before page visible
            base.OnAppearing();
            //bind list to collectionview source
            cvAssessment.ItemsSource = await App.Database.GetAssessments();

        }
        private async void tbiAdd_Clicked(object sender, EventArgs e)
        {
            //goto add new
            await Shell.Current.GoToAsync(nameof(AssessmentsInfo));

        }
        private async void cvAssessment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                //bind current to var
                Assessment assess = (Assessment)e.CurrentSelection.FirstOrDefault();
                //bind page to var
                var edit = new AssessmentsInfo(assess);
                //bind 'add' to title if null
                edit.Title = (assess.AssessmentName == null) ? "Add" : "Edit";
                //top of nav stack
                await Navigation.PushAsync(edit);
            }

        }
    }
}