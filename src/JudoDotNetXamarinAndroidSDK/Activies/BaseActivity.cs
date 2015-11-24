using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using JudoDotNetXamarinAndroidSDK.Models;
using JudoDotNetXamarinAndroidSDK.Ui;
using JudoPayDotNet.Models;
using JudoPayDotNet.Errors;

namespace JudoDotNetXamarinAndroidSDK.Activies
{
    public static class BuildConfig
    {
        #if DEBUG
        public const bool DEBUG = true;
        #else
        public const bool DEBUG = false;
#endif
    }

    public abstract class BaseActivity : Activity
    {
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            if (Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.IceCreamSandwich) {
                RequestWindowFeature (WindowFeatures.NoTitle);
            } else {
                SetTheme (Resource.Style.Theme_Judo_payments);

                if (BuildConfig.DEBUG) {
                    Window.SetFlags (WindowManagerFlags.Secure, WindowManagerFlags.Secure);
                }
            }
        }

        public void SetHelpText (int titleId, int messageId)
        {
            SetHelpText (titleId, messageId, Resource.Id.infoButtonID);
        }

        public void SetHelpText (int titleId, int messageId, int helpButtonResId)
        {
            String title = GetString (titleId);
            String message = GetString (messageId);
            SetHelpText (title, message, helpButtonResId);
        }

        public void SetHelpText (string title, string message)
        {
            SetHelpText (title, message, Resource.Id.infoButtonID);
        }

        public void SetHelpText (string title, string message, int helpButtonResId)
        {
            HelpButton infoButton = FindViewById<HelpButton> (helpButtonResId);

            if (infoButton == null) {
                return;
            }

            infoButton.HelpClickListener += help => {
                ShowMessage (title, message);
            };
        }

        protected void ShowMessage (string message)
        {
            ShowMessage (null, message);
        }

        protected void ShowMessage (string title, string message)
        {
            RunOnUiThread (() => ShowConfirmation (title, message));
        }

        public void ShowConfirmation (string title, string message)
        {
            ShowConfirmation (title, message, true, null, null);
        }

        public void ShowConfirmation (string title, string message, bool cancelable, string buttonLabel, Action buttonAction)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder (this);
            if (!String.IsNullOrWhiteSpace (title)) {
                builder.SetTitle (title);
            }

            builder.SetMessage (message);
            builder.SetCancelable (cancelable);

            var dialog = builder.Create ();

            if (buttonLabel == null) {
                builder.SetPositiveButton (Resource.String.ok, (sender, args) => dialog.Dismiss ());
            } else {
                builder.SetPositiveButton (buttonLabel,
                    (sender, args) => {
                        if (buttonAction != null) {
                            Task.Factory.StartNew (buttonAction);
                        }
                        dialog.Dismiss ();
                    });

                if (cancelable) {
                    builder.SetNegativeButton (Resource.String.cancel, (sender, args) => dialog.Dismiss ());
                }

            }

            builder.Show ();
        }

        protected override void OnPostCreate (Bundle savedInstanceState)
        {
            base.OnPostCreate (savedInstanceState);

            View cancelButton = FindViewById<View> (Resource.Id.cancelButton);

            if (cancelButton != null) {
                cancelButton.Click += (sender, args) => {
                    SetResult (JudoSDKManager.JUDO_CANCELLED);
                    Finish ();
                };
            }
        }

        protected abstract void ShowLoadingSpinner (bool show);

        protected void TransactClickHandler (Action DoTransaction)
        {
            try {
                DoTransaction ();
            } catch (Exception e) {
                Log.Error (JudoSDKManager.DEBUG_TAG, "Exception", e);
                SetResult (JudoSDKManager.JUDO_ERROR,
                    JudoSDKManager.CreateErrorIntent (e.Message, e, null));
                Finish ();
            }
        }

        protected void HandleServerResponse (Task<IResult<ITransactionResult>> t)
        {
            var result = t.Result;
            if (result != null && !result.HasError && result.Response.Result != "Declined") {
                var receipt = result.Response;

                if (receipt != null) {
                    Intent intent = new Intent ();
                    intent.PutExtra (JudoSDKManager.JUDO_RECEIPT, new SReceipt (receipt));
                    SetResult (JudoSDKManager.JUDO_SUCCESS, intent);
                    Log.Debug ("com.judopay.android", "SUCCESS: " + receipt.Result);
                    Finish ();
                } else {
                    var threedDSecureReceipt = result.Response as PaymentRequiresThreeDSecureModel;
                    if (threedDSecureReceipt != null) {
                        //, result.Exception,
                        // !result.IsFaulted && result.Result != null ? result.Result.Error : null)
                        SetResult (JudoSDKManager.JUDO_ERROR, JudoSDKManager.CreateErrorIntent ("Account requires 3D Secure but application is not configured to accept it", new Exception ("Account requires 3D Secure but application is not configured to accept it"), new JudoApiErrorModel ()));
                            
                        Finish ();
                    } else {

                        SetResult (JudoSDKManager.JUDO_ERROR,
                            JudoSDKManager.CreateErrorIntent ("JudoXamarinSDK: unable to find the receipt in response.", new Exception ("JudoXamarinSDK: unable to find the receipt in response."), new JudoApiErrorModel ()));
                        Finish ();
                        throw new Exception ("JudoXamarinSDK: unable to find the receipt in response.");
                    }
                }

            } else {
                // Failure callback

                Intent intent = new Intent ();
                var receipt = result.Response;

                if (receipt != null) {
                    intent.PutExtra (JudoSDKManager.JUDO_RECEIPT, new SReceipt (receipt));
                } 
                var error = result.Error as JudoApiErrorModel;
                if (error != null) {
                    intent.PutExtra (JudoSDKManager.JUDO_ERROR_EXCEPTION, new SJudoError (new Exception (error.ErrorMessage), error));
                }
                //intent.PutExtra (JudoSDKManager.JUDO_ERROR_EXCEPTION, new SJudoError (new Exception (result.Response.Message), new JudoApiErrorModel (){ ErrorMessage = result.Response.Message }));
                SetResult (JudoSDKManager.JUDO_ERROR, intent);
                Finish ();

            }


//
//            try {
//                ShowLoadingSpinner (false);
//
//                    if (result.IsFaulted || result.Result == null || result.Result.HasError) {
//                        var errorMessage = !result.IsFaulted && result.Result != null
//                            ? result.Result.Error.ErrorMessage
//                            : result.Exception.ToString ();
//                    Log.Error ("com.judopay.android", "ERROR: " + errorMessage);
//                    SetResult (JudoSDKManager.JUDO_ERROR,
//                            JudoSDKManager.CreateErrorIntent (errorMessage, result.Exception,
//                                !result.IsFaulted && result.Result != null ? result.Result.Error : null));
//                    Finish ();
//                    return;
//                }
//
//                    var receipt = result.Result.Response;
//
//                Intent intent = new Intent ();
//                intent.PutExtra (JudoSDKManager.JUDO_RECEIPT, new Receipt (receipt));
//                SetResult (JudoSDKManager.JUDO_SUCCESS, intent);
//                Log.Debug ("com.judopay.android", "SUCCESS: " + receipt.Result);
//                Finish ();
//            }
//            //Prevent being locked in a payment screen without being notified of an error
//            catch (Exception e) {
//                var errorMessage = e.ToString ();
//                Log.Error ("com.judopay.android", "ERROR: " + errorMessage);
//                SetResult (JudoSDKManager.JUDO_ERROR,
//                JudoSDKManager.CreateErrorIntent (errorMessage, e, null));
//                Finish ();
//                return;
//            }
        }
    }
}