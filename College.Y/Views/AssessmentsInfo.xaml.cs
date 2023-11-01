using College.Y.Models;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace College.Y.Views
{
    [QueryProperty(nameof(AssessmentId), nameof(AssessmentId))]
    public partial class AssessmentsInfo : ContentPage
    {
        //initiation
        public AssessmentsInfo()
        {
            InitializeComponent();
            BindingContext = new Assessment();
            dpStart.Date = DateTime.Now;
            dpEnd.Date = DateTime.Now;

        }
        public AssessmentsInfo(Assessment nassess)
        {
            InitializeComponent();
            BindingContext = nassess;
        }
        //generation
        public string AssessmentId { set { LoadAssessment(value); } }
        private async void LoadAssessment(string assessid)
        {
            try
            {
                int id = Convert.ToInt32(assessid);
                //retrieve + set as BC
                Assessment assess = await App.Database.GetNAssessment(id);
                BindingContext = assess;
            }
            catch (Exception)
            {

            }
        }
        //function
        Func<StackLayout, bool> CheckNullEmpty = sl =>
        {
            bool IsNullEmpty = false;

            foreach (var ctl in sl.Children)
            {
                switch (ctl)
                {
                    case Label lb:
                    case DatePicker dp:
                        continue;
                    case Entry entry:
                        if (string.IsNullOrEmpty(entry.Text))
                        {
                            entry.BackgroundColor = Color.PaleVioletRed;
                            IsNullEmpty = true;

                        }
                        else
                        {
                            entry.BackgroundColor = Color.White;

                        }
                        break;
                    default:
                        break;
                }
            }
            return IsNullEmpty;
        };
        //validation
        private async Task IsSwitchToggled()
        {
            if (swStart.IsToggled && dpStart.Date >= DateTime.Today)
            {
                await NotifyAssessmentStart();

            }
            else if (swEnd.IsToggled && dpEnd.Date >= DateTime.Today)
            {
                await NotifyAssessmentEnd();
            }

        }
        //action
        public async Task AlertError()
        {
            await DisplayAlert("Error", "Blank or Whitespace is invalid", "OK");

        }
        private async Task NotifyAssessmentStart()
        {

            var notify = new NotificationRequest
            {
                Title = "Assessent Start",
                Description = dpStart.Date.ToString("D"),
                Schedule =
                    {
                        NotifyTime = DateTime.Now.AddSeconds(5)

                    }
            };

            await LocalNotificationCenter.Current.Show(notify);

        }
        private async Task NotifyAssessmentEnd()
        {
            var notify = new NotificationRequest
            {
                Title = "Assessent End",
                Description = dpEnd.Date.ToString("D"),
                Schedule =
                    {
                        NotifyTime = DateTime.Now.AddSeconds(5)

                    }
            };

            await LocalNotificationCenter.Current.Show(notify);

        }
        //reaction
        private async void swStart_Toggled(object sender, ToggledEventArgs e)
        {
            await IsSwitchToggled();

        }
        private async void swEnd_Toggled(object sender, ToggledEventArgs e)
        {
            await IsSwitchToggled();
        }
        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            var assess = (Assessment)BindingContext;
            if (!CheckNullEmpty(slAssessmentsInfo))
            {
                //save
                await App.Database.AddAssessment(assess);
                //backto
                await Navigation.PopToRootAsync();

            }
            else
            {
                await AlertError();
            }


        }
        private async void btnDelete_Clicked_1(object sender, EventArgs e)
        {
            var assess = (Assessment)BindingContext;
            await App.Database.DeleteAssessment(assess);
            await Shell.Current.GoToAsync("..");

        }
        //selection

    }
}