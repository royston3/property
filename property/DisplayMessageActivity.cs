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
using Java.Lang;

namespace property
{
    [Activity(Label = "DisplayMessageActivity")]
    public class DisplayMessageActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Rent);
            //string text = Intent.GetStringExtra("PurchasePrice") ?? "Data not present";

            //EditText textedit = FindViewById<EditText>(Resource.Id.txtTrial);
            //textedit.SetText(text, TextView.BufferType.Editable);
            //ImageButton btnPrevious = FindViewById<ImageButton>(Resource.Id.btnPreviousRentForm);
            ImageButton btnNext = FindViewById<ImageButton>(Resource.Id.btnNextRentForm);

            //btnPrevious.Click += delegate {
            //    var activity2 = new Intent(this, typeof(MainActivity));
            //    StartActivity(activity2);
            //};

            btnNext.Click += delegate
            {
                var activity2 = new Intent(this, typeof(Result));
                StartActivity(activity2);

                SetPreferences();
            };
        }

        private void SetPreferences()
        {
            EditText txtRentReceived = FindViewById<EditText>(Resource.Id.txtRentReceived);
            string sInterestRate = txtRentReceived.Text;

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);

            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutFloat("rent_received_key", Convert.ToSingle(sInterestRate));
            editor.Apply();
        }
    }
}