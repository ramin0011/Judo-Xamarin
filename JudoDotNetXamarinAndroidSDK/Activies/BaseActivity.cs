using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using JudoDotNetXamarinSDK.Ui;

namespace JudoDotNetXamarinSDK.Activies
{
    /// <summary>
    /// TODO This needs to be addressed because doesn't make sense on the native android sdk
    /// </summary>
    public static class BuildConfig
    {
        public static readonly bool DEBUG = true;
    }

    public class BaseActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if (Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.IceCreamSandwich)
            {
                RequestWindowFeature(WindowFeatures.NoTitle);
            }
            else
            {
                SetTheme(Resource.Style.Theme_Judo_payments);

                if (BuildConfig.DEBUG)
                {
                    Window.SetFlags(WindowManagerFlags.Secure, WindowManagerFlags.Secure);
                }
            }
        }

        public void SetHelpText(int titleId, int messageId)
        {
            SetHelpText(titleId, messageId, Resource.Id.infoButtonID);
        }

        public void SetHelpText(int titleId, int messageId, int helpButtonResId)
        {
            String title = GetString(titleId);
            String message = GetString(messageId);
            SetHelpText(title, message, helpButtonResId);
        }

        public void SetHelpText(string title, string message)
        {
            SetHelpText(title, message, Resource.Id.infoButtonID);
        }

        public void SetHelpText(string title, string message, int helpButtonResId)
        {
            HelpButton infoButton = FindViewById<HelpButton>(helpButtonResId);

            if (infoButton == null)
            {
                return;
            }

            infoButton.HelpClickListener += help =>
            {
                ShowMessage(title, message);
            };
        }

        protected void ShowMessage(string message)
        {
            ShowMessage(null, message);
        }

        protected void ShowMessage(string title, string message)
        {
            RunOnUiThread(() => ShowConfirmation(title, message));
        }

        public void ShowConfirmation(string title, string message)
        {
            ShowConfirmation(title, message, true, null, null);
        }

        public void ShowConfirmation(string title, string message, bool cancelable, string buttonLabel, Action buttonAction)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            if (!String.IsNullOrWhiteSpace(title))
            {
                builder.SetTitle(title);
            }

            builder.SetMessage(message);
            builder.SetCancelable(cancelable);

            var dialog = builder.Create();

            if (buttonLabel == null)
            {
                builder.SetPositiveButton(Resource.String.ok, (sender, args) => dialog.Dismiss());
            }
            else
            {
                builder.SetPositiveButton(buttonLabel,
                    (sender, args) =>
                    {
                        if (buttonAction != null)
                        {
                            Task.Factory.StartNew(buttonAction);
                        }
                        dialog.Dismiss();
                    });

                if (cancelable)
                {
                    builder.SetNegativeButton(Resource.String.cancel, (sender, args) => dialog.Dismiss());
                }

            }

            builder.Show();
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            View cancelButton = FindViewById<View>(Resource.Id.cancelButton);

            if (cancelButton != null)
            {
                cancelButton.Click += (sender, args) =>
                {
                    SetResult(JudoSDKManager.JUDO_CANCELLED);
                    Finish();
                };
            }
        }
    }
}