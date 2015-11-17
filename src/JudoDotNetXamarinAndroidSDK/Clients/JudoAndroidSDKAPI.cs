using System;
using JudoDotNetXamarin;
using Android.App;

namespace JudoDotNetXamarinAndroidSDK
{
    public interface JudoAndroidSDKAPI
    {
        void Payment (PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure, Activity context);
    }
}

