using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Preferences;

namespace property
{
    [Activity(Label = "Result")]
    public class Result : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Result);
            var editToolbar = FindViewById<Toolbar>(Resource.Id.edit_toolbar);
            //editToolbar.Title = "Editing";
            editToolbar.InflateMenu(Resource.Menu.results);
            editToolbar.MenuItemClick += (sender, e) =>
            {
                Toast.MakeText(this, "Bottom toolbar tapped: " + e.Item.TitleFormatted, ToastLength.Short).Show();
                string labelInfo = e.Item.TitleFormatted.ToString();
                FragmentTransaction transaction = this.FragmentManager.BeginTransaction();

                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
                ISharedPreferencesEditor editor = prefs.Edit();

                if (labelInfo.ToLower().Contains("loan"))
                {
                    editor.PutString("results_key", "loan");
                }
                else if (labelInfo.ToLower().Contains("costs"))
                {
                    editor.PutString("results_key", "costs");
                }
                else if (labelInfo.ToLower().Contains("sale"))
                {
                    editor.PutString("results_key", "sale");
                }

                editor.Apply();
                LoanDetailsFragment loanFragment = new LoanDetailsFragment();
                transaction.Replace(Resource.Id.main_content, loanFragment);
                transaction.Commit();
                //PrintLoanDetails(loanFragment.Name);
            };
        }        
    }
}