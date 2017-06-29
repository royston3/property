using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Preferences;

namespace property
{
    public class LoanDetailsFragment : Fragment
    {
        private double purchasePrice;
        private double initialAmount;
        private double interestRate;
        private double rentReceived;
        private double loanTaken = 0;
        private double totalFees = 0;

        private const int DAYS_IN_A_YEAR = 52;
        private const double PROPERTY_MANAGEMENT_FEES = 8;
        private const double LAND_RATES = 0.375;
        private const double NUMBER_OF_YEARS = 1;
        private const double HOUSE_PRICE_APPRECIATION = 3;

        private const double RENT_LOSS_INSURANCE = 500;
        private const double HOUSE_INSURANCE = 800;
        private const double MISCELLANEOUS = 2000;
        private const double ACCOUNTANT_FEES = 500;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment            

            string strLoan = ManageKey();

            if (strLoan == "loan")
            {
                return inflater.Inflate(Resource.Layout.LoanInfo, container, false);                
            }
            else if (strLoan == "costs")
            {
                return inflater.Inflate(Resource.Layout.CostsInvolved, container, false);
            }
            else if (strLoan == "sale")
            {
                //var toolbar = container.FindViewById<Toolbar>(Resource.Id.toolbar);
                return inflater.Inflate(Resource.Layout.SaleDetails, container, false);
            }

            // PrintLoanDetails();
            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            string strLoan = ManageKey();

            if (strLoan == "loan")
            {
                PrintLoanDetails();
            } 
            else if (strLoan == "costs")
            {
                PrintCostDetails();
            }
            else if (strLoan == "sale")
            {
                PrintSaleDetails();
            }
        }

        private void PrintSaleDetails()
        {
            getSharedPreferences();

            var amountInvested = View.FindViewById<TextView>(Resource.Id.lblAmountInvested);
            amountInvested.Text = getAmountInvested().ToString("C");

            var housePriceAppreciation = View.FindViewById<TextView>(Resource.Id.lblHousePriceAppreciation);
            housePriceAppreciation.Text = getHousePriceAppreciation().ToString("C");

            var totalMade = View.FindViewById<TextView>(Resource.Id.lblTotalMadeAfterSell);
            totalMade.Text = getTotalMadeAfterSell().ToString("C");
        }

        private double getTotalMadeAfterSell()
        {
            return getHousePriceAppreciation() - getAmountInvested();
        }

        private double getHousePriceAppreciation()
        {
            return purchasePrice * (HOUSE_PRICE_APPRECIATION / 100);
        }

        private double getAmountInvested()
        {
            //this is the amount invested without principal loan repayments
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            totalFees = prefs.GetFloat("total_fees_key", 0);

            if (totalFees !=  0)
            {
                return initialAmount + ((getGainLossPerYear() * -1) * NUMBER_OF_YEARS);
            }
            return 0;
        }

        private void PrintLoanDetails()
        {
            getSharedPreferences();

            var loanTakenText = View.FindViewById<TextView>(Resource.Id.lblLoanTaken);
            loanTakenText.Text = purchasePrice.ToString("C");

            var initialAmountText = View.FindViewById<TextView>(Resource.Id.lblInitialStartupAmount);
            initialAmountText.Text = initialAmount.ToString("C");

            var interestRateText = View.FindViewById<TextView>(Resource.Id.lblInterestTaken);
            interestRateText.Text = string.Format("{0}%", interestRate.ToString());

            var rentReceivedText = View.FindViewById<TextView>(Resource.Id.lblRentReceived);
            rentReceivedText.Text = rentReceived.ToString("C");
        }

        private void PrintCostDetails()
        {
            getSharedPreferences();

            var interestForLoanTaken = View.FindViewById<TextView>(Resource.Id.lblInterestForLoan);
            double result1 = getInterestForLoanCalc();
            interestForLoanTaken.Text = result1.ToString("C");

            double result2 = getPropertyManagementCalcs();
            var propertyManagementFees = View.FindViewById<TextView>(Resource.Id.lblPropertyManagementFees);
            propertyManagementFees.Text = result2.ToString("C");

            double result3 = getLandRates();
            var landRates = View.FindViewById<TextView>(Resource.Id.lblLandRateFees);
            landRates.Text = result3.ToString("C");

            var rentLossInsurance = View.FindViewById<TextView>(Resource.Id.lblRentLossInsurance);
            rentLossInsurance.Text = RENT_LOSS_INSURANCE.ToString("C");

            var houseInsurance = View.FindViewById<TextView>(Resource.Id.lblHouseInsurance);
            houseInsurance.Text = HOUSE_INSURANCE.ToString("C");

            var miscellaneous = View.FindViewById<TextView>(Resource.Id.lblMiscellaneousFees);
            miscellaneous.Text = MISCELLANEOUS.ToString("C");

            var accountantFees = View.FindViewById<TextView>(Resource.Id.lblAccountantFees);
            accountantFees.Text = ACCOUNTANT_FEES.ToString("C");

            totalFees = result1 + result2 + result3 + RENT_LOSS_INSURANCE 
                + HOUSE_INSURANCE + MISCELLANEOUS + ACCOUNTANT_FEES;
            var totalFeesTextBox = View.FindViewById<TextView>(Resource.Id.lblTotalExpenses);
            totalFeesTextBox.Text = totalFees.ToString("C");

            var totalExpenseAfterRent = View.FindViewById<TextView>(Resource.Id.lblTotalExpensesAfterRent);
            totalExpenseAfterRent.Text = getGainLossPerYear().ToString("C");

            AddPreferences(totalFees);
        }

        private double getGainLossPerYear()
        {
            return (rentReceived * DAYS_IN_A_YEAR) - totalFees;
        }

        private double getLandRates()
        {
            return loanTaken * (LAND_RATES / 100);
        }

        private double getPropertyManagementCalcs()
        {
            return rentReceived * DAYS_IN_A_YEAR * (PROPERTY_MANAGEMENT_FEES/100);
        }

        private double getInterestForLoanCalc()
        {
            double initialAmountCalc = purchasePrice * (initialAmount / 100);
            loanTaken = purchasePrice - initialAmountCalc;
            return purchasePrice * (interestRate / 100);
        }

        private string ManageKey()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            string strLoan = prefs.GetString("results_key", "hut");
            return strLoan;
        }

        private void getSharedPreferences()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            purchasePrice = prefs.GetFloat("purchase_price_key", 0);
            initialAmount = prefs.GetFloat("down_payment_key", 0);
            interestRate = prefs.GetFloat("interest_rate_key", 0);
            rentReceived = prefs.GetFloat("rent_received_key", 0);
        }

        private void AddPreferences(double totalFeesValue)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutFloat("total_fees_key", Convert.ToSingle(totalFeesValue));
            editor.Apply();
        }
    }
}