using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using JudoDotNetXamarinAndroidSDK.Ui;
using JudoPayDotNet.Models;
using JudoPayDotNet.Errors;
using JudoDotNetXamarin;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace JudoDotNetXamarinAndroidSDK.Activities
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
                    SetResult (Judo.JUDO_CANCELLED);
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
                Log.Error (Judo.DEBUG_TAG, "Exception", e);
                SetResult (Judo.JUDO_ERROR,
                    Judo.CreateErrorIntent (e.Message, e, null));
                Finish ();
            }
        }

        protected void HandleServerResponse (Task<IResult<ITransactionResult>> t)
        {
            if (t.Exception != null) {      
                var judoError = t.Exception.FlattenToJudoError ();
                SetResult (Judo.JUDO_ERROR, Judo.CreateErrorIntent (judoError.Message, judoError.Exception, judoError.ApiError));
                Finish ();
            } else {
                
                var result = t.Result;
                if (result != null && !result.HasError && result.Response.Result != "Declined") {
                    var receipt = result.Response;

                    if (receipt != null) {
                        var threedDSecureReceipt = result.Response as PaymentRequiresThreeDSecureModel;
                        if (threedDSecureReceipt != null) {
                            var message = "Account requires 3D Secure but application is not configured to accept it";
                            SetResult (Judo.JUDO_ERROR, Judo.CreateErrorIntent (message, new Exception (message), new ModelError () {
                                Message = message,
                                Code = (int)JudoApiError.AuthenticationFailure
                            }));

                            Finish ();
                        } else {

                            Intent intent = new Intent ();
                            intent.PutExtra (Judo.JUDO_RECEIPT, JsonConvert.SerializeObject (receipt));
                            SetResult (Judo.JUDO_SUCCESS, intent);
                            Log.Debug ("com.judopay.android", "SUCCESS: " + receipt.Result);
                            Finish ();
                        }

                    } else {
                        var message = "JudoXamarinSDK: unable to find the receipt in response.";
                        SetResult (Judo.JUDO_ERROR,
                            Judo.CreateErrorIntent ("message", new Exception (message), new ModelError () {
                                Message = message,
                                Code = (int)JudoApiError.AuthenticationFailure
                            }));
                        Finish ();
                        throw new Exception (message);
                    }
                } else {

                    Intent intent = new Intent ();
                    var receipt = result.Response;

                    if (receipt != null) {
                        intent.PutExtra (Judo.JUDO_RECEIPT, JsonConvert.SerializeObject (receipt));
                    } 
                    var error = result.Error;
                    if (error != null) {
                        intent.PutExtra (Judo.JUDO_ERROR_EXCEPTION, JsonConvert.SerializeObject (new JudoError (new Exception (error.Message), error)));
                    }
            
                    SetResult (Judo.JUDO_ERROR, intent);
                    Finish ();

                }
            }

        }
    }
}