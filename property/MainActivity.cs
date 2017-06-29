using Android.App;
using Android.Widget;
using Android.OS;
using Java.Interop;
using Android.Views;
using Android.Content;
using System;
using Android.Preferences;

namespace property
{
    [Activity(Label = "property", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            //https://www.materialpalette.com/
            SetContentView(Resource.Layout.Main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            //ActionBar.Title = "Hello from Toolbar";

            //Create A Button Object To Set The Event
            ImageButton btnNext = FindViewById<ImageButton>(Resource.Id.btnNext_PropertyForm);

            GetPreferences();

            ////Assign The Event To Button
            //button.Click += delegate {
            //    //Call Your Method When User Clicks The Button
            //    moveToRentalCalc();
            //};
            btnNext.Click += delegate {                

                if (CheckDefaultFields())
                {
                    var activity2 = new Intent(this, typeof(DisplayMessageActivity));
                    StartActivity(activity2);
                }                
            };
        }

        private bool CheckDefaultFields()
        {
            EditText txtPurchasePrice = FindViewById<EditText>(Resource.Id.txtPurchasePrice);
            string sPurchasePrice = txtPurchasePrice.Text;

            EditText txtDownPayment = FindViewById<EditText>(Resource.Id.txtDownPayment);
            string sDownPayment = txtDownPayment.Text;

            EditText txtInterestRate = FindViewById<EditText>(Resource.Id.txtInterestRate);
            string sInterestRate = txtInterestRate.Text;

            if (string.IsNullOrWhiteSpace(sPurchasePrice))
            {
                Toast.MakeText(this, "Please enter your Purchase Price", ToastLength.Short).Show();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(sDownPayment))
            {
                Toast.MakeText(this, "Please enter your Downpayment", ToastLength.Short).Show();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(sInterestRate))
            {
                Toast.MakeText(this, "Please enter your Interest Rate", ToastLength.Short).Show();
                return false;
            }
            else
            {
                SetPreferences();
            }

            return true;
        }

        private void SetPreferences()
        {
            EditText txtPurchasePrice = FindViewById<EditText>(Resource.Id.txtPurchasePrice);
            string sPurchasePrice = txtPurchasePrice.Text;

            EditText txtDownPayment = FindViewById<EditText>(Resource.Id.txtDownPayment);
            string sDownPayment = txtDownPayment.Text;

            EditText txtInterestRate = FindViewById<EditText>(Resource.Id.txtInterestRate);
            string sInterestRate = txtInterestRate.Text;

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            
            ISharedPreferencesEditor editor = prefs.Edit();            
            editor.PutFloat("purchase_price_key", Convert.ToSingle(sPurchasePrice));
            editor.PutFloat("down_payment_key", Convert.ToSingle(sDownPayment));
            editor.PutFloat("interest_rate_key", Convert.ToSingle(sInterestRate));
            editor.Apply();
        }

        private void GetPreferences()
        {
            EditText txtPurchasePrice = FindViewById<EditText>(Resource.Id.txtPurchasePrice);
            EditText txtDownPayment = FindViewById<EditText>(Resource.Id.txtDownPayment);
            EditText txtInterestRate = FindViewById<EditText>(Resource.Id.txtInterestRate);

            //to do only on startup
            PreferenceManager.GetDefaultSharedPreferences(Application.Context).Dispose();
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.Clear();
            editor.Apply();
            //************************************************************************************

            double purchasePrice = prefs.GetFloat("purchase_price_key", 0);
            double deposit = prefs.GetFloat("down_payment_key", 0);
            double interestRate = prefs.GetFloat("interest_rate_key", 0);

            if (purchasePrice != 0)
            {
                txtPurchasePrice.Text = Convert.ToString(purchasePrice);
            }
            txtDownPayment.Text = Convert.ToString(deposit);
            txtInterestRate.Text = Convert.ToString(interestRate);
        }
    }
}
