# Getting started

Ready to start integrating? This tutorial will help you get started with integrating judo with your Xamarin Mobile Application.

### Step 1 

You can create your judo account in seconds with our [sign-up form](https://www.judopay.com/signup). Once registered, you'll be able to download our SDKs, access development tools, and run test transactions in our Sandbox environment.

### Step 2 

Access your judo dashboard by logging in at https://portal.judopay.com. Once logged in, you'll be able to add and administer your first App by following the on-screen steps.

### Step 3 

To start using the judoPay library you'll first need to configure your SDK to set your API credentials, which are used to authenticate your access with judo.

You can do this with the following code:

### Configuring judoPay APIs


#####Android
``` csharp
protected override void OnCreate( Bundle bundle )
{
	base.OnCreate (bundle);
	
	private const string ApiToken   = "{ApiToken}";
	private const string ApiSecret	= "{ApiSecret}";
	
	JudoSDKManager.Configuration.SetApiTokenAndSecret(
	ApiToken, 
	ApiSecret,
	Environment.Sandbox );
	
	JudoSDKManager.Configuration.IsAVSEnabled = true;
	JudoSDKManager.Configuration.IsFraudMonitoringSignals = true;
	JudoSDKManager.Configuration.IsMaestroEnabled = true;
	
	// your code
}
```

#####iOS
``` csharp
public override bool FinishedLaunching (UIApplication a,NSDictionary o)
{
var configInstance = JudoConfiguration.Instance;

	//setting for SandBox
	configInstance.Environment = Environment.Sandbox;

	configInstance.ApiToken =  "{ApiToken}";
	configInstance.ApiSecret = "{ApiSecret}";
	configInstance.JudoId =    "{JudoID}";

 
    // setting up 3d secure, AVS, Amex and mestro card support
    JudoSDKManager.AVSEnabled = true;
    JudoSDKManager.AmExAccepted = true;
    JudoSDKManager.MaestroAccepted = true;
            
    // this will turn on UI mode which will hand over control to our out of 
    //the box UI solution
    JudoSDKManager.UIMode = true;
            
}
```

**Please note:** You can configure judoPay library to use live environment by changing the third parameter in `SetApiTokenAndSecret ()`from `Environment.Sandbox` to `Environment.live`


### Card payment

Now that you've configured your SDK with your API Tokens and Secrets, you're ready to use the JudoSDKManager to process payments. 

By calling the following with the SDK Manager, you'll invoke judo's UI to enter card data and submit the payment request:

#####Android
```csharp

var intent = JudoSDKManager.UIMethods.Payment ( this, MY_JUDO_ID,
                                                currency, amount,
                                                paymentReference,
                                                consumerRef, metaData );

StartActivityForResult ( intent, ACTION_CARD_PAYMENT );

// your code ...
```

#####iOS
```csharp
//Define a success block
private void SuccessPayment(PaymentReceiptModel receipt)
     {
     // handle receipt
     }
//Define a Failure block
private void FailurePayment(JudoError error, PaymentReceiptModel receipt)
     {
     // handle errors or Declined cards
     }

//Pass through the payment you would like to facilitate
var cardPayment = new PaymentViewModel
     {
      Amount = 4.5m, 
      ConsumerReference = consumerRef,
      PaymentReference = paymentReference,
      Currency = "GBP"
     };

//Let Judo do the rest
JudoSDKManager.Payment(cardPayment, successCallback, failureCallback, this.NavigationController);

```
####Note: 
This callback should be non-blocking

### Register card

You can register a consumer's card with our SDK in order to process future payments. You can invoke this method, with the below:

#####Android
```csharp
var intent = JudoSDKManager.UIMethods.RegisterCard ( this, consumerRef );

StartActivityForResult ( intent, ACTION_REGISTER_CARD );

// your code ...
```

#####iOS
```csharp
JudoSDKManager.RegisterCard(cardPayment, successCallback, failureCallback, this.NavigationController);
// your code ...
```


When you've successfully processed this call, judo's API will return a Consumer Token and a Card Token, which can be stored and used to process future payments with the **Token payment** method.

### Token payment

A Token payment allows you to process future payments on behalf of a consumer without you having to store sensitive card data - this means you don't have to worry about PCI compliance. You can initiate a Token payment with the below:
#####Android
```csharp
var intent = JudoSDKManager.UIMethods.TokenPayment ( this, MY_JUDO_ID,
                                                    currency, amount,
                                                    paymentReference,
                                                    consumerReference,
                                                    token, metadata,
                                                    consumerToken);

StartActivityForResult ( intent, ACTION_TOKEN_PAYMENT );

// your code ...
```
#####iOS
```csharp
JudoSDKManager.TokenPayment(tokenPayment, successCallback, failureCallback, this.NavigationController);
// your code ...
```

# Customizing the judoPay UI

####Android
You can customise our out of the box implementation to match the colour tones, themes and wording of your platform. To customise your themes, you can use the sample custom style sheet below: 
**Android.Xamarin.SampleApp > values > mystyles.xml**

```xml
<?xml version="1.0" encoding="utf-8" ?>

<resources>
  <style name="judo_payments_page">
    <item name="android:layout_width">match_parent</item>
    <item name="android:layout_height">match_parent</item>
    <item name="android:paddingLeft">@dimen/activity_horizontal_margin</item>
    <item name="android:paddingRight">@dimen/activity_horizontal_margin</item>
    <item name="android:paddingTop">@dimen/activity_vertical_margin</item>
    <item name="android:paddingBottom">@dimen/activity_vertical_margin</item>
    <item name="android:orientation">vertical</item>
    <item name="android:background">@color/page_bg</item>
    <item name="android:gravity">center_vertical</item>
  </style>

	// ....
```

You can also customize the messages you present to your user by updating following file:
**Android.Xamarin.SampleApp > values > Strings.xml**

```xml
<?xml version="1.0" encoding="utf-8"?>
<resources>
    <string name="Hello">Hello World, Click Me!</string>
    <string name="ApplicationName">Android.Xamarin.SampleApp</string>
  
    <string name="app_name">judoPay-Xamarin Sample</string>
    <string name="app_name_ui">judoPay-Xamarin Sample With UI</string>

    <!-- messages -->
    <string name="msg_payment_ok">Payment successful: %1$s</string>
    <string name="msg_preauth_ok">PreAuth successful: %1$s</string>

    // ....
```


# Build your own UI

Alternatively, if you want full control over your UI, you can create your own UI for your user to enter their card details. To process payments from your own UI, you can use the following: 

### JudoSDKManager.NonUIMethods 

#####Android
```csharp
var paymentTask = JudoSDKManager.NonUIMethods.Payment ( this, MY_JUDO_ID,currency,amount,paymentReference,consumerRef,metaData, cardNumber,                                                 addressPostCode, startDate,expiryDate, cv2 );

// your code ...
```

#####iOS
```csharp
JudoSDKManager.UIMode = false;

var cardViewModel =new CardViewModel() { CardNumber = cardNumber, CV2 = cv2, ExpireDate = expiryDate, PostCode = addressPostCode, CountryCode = ISO3166CountryCodes.UK}

var cardPayment = new PaymentViewModel
            {
                Amount = 4.5m, 
                ConsumerReference = consumerRef,
                PaymentReference = paymentReference,
                Currency = "GBP",
                // Non-UI API needs to pass card detail
                Card =cardViewModel
            };
 JudoSDKManager.Payment(cardPayment, successCallback, failureCallback, this.NavigationController);            
 
// your code ...
```


Implementing a custom UI can alter your compliancy requirements, so if you're looking to implement a custom UI, let us know so we can help with the process.


# Environments

We have two environments available to you for processing payments:

### Sandbox
Our sandbox environment allows you to process test transactions while developing your app. Please note only sandbox API tokens and test cards will work in the sandbox environment. Test card details are available in your judo dashboard.

### Production
Once you're ready to go live, you can switch to our production environment. Please  note you'll need to change your API token and API secret for a Live token and secret. Only real payment cards will work in this environment.