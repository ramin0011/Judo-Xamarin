<p>
  <img  align="right" src="https://github.com/JudoPay/Judo-Xamarin/blob/master/resources/judo_logo.png?raw=true" alt="Judo"/>
</p>
#Judo-Xamarin

Secure in-app payments SDK for Xamarin.


The judoPay library lets you integrate card payments into your Xamarin project. It is built to be mobile first with ease of integration in mind. Judo's SDK enables a faster, simpler and more secure payment experience within your app. Build trust and user loyalty in your app with our secure and intuitive UX.

####Sell more. Risk less.

## Requirements

Xamarin developer licence

[JudoPay Account]( https://www.judopay.com/docs/ "JudoPay") (click get started)

Thats it!

## Installation

####[Xamarin Component Store]( https://components.xamarin.com/view/judopay-xamarin-sdk "Component Store")
-   Within Xamarin Studio- Project/Get More Components - Search "JudoPay" and add to app
-   Click download from the above link and unzip the .Xam file and run the install command using the xamarin-component.exe above
  -  mono xamarin-component.exe install /path/to/your.xam (mac)
  -  xamarin-component.exe install /path/to/your.xam (windows)
  -  You can also download the component .xam from this page component/judopay-xamarin-sdk-2.0.xam

####Here!
-  Fork the SDK and build it yourself. It won't bite.
-  Again download the component .xam from this page component/judopay-xamarin-sdk-2.0.xam

####Nuget
-  Coming Soon

####Getting Started

Ready to start integrating? This tutorial will help you get started with integrating judo with your Xamarin Mobile Application.

### Step 1 

You can create your judo account by clicking “Get Started” here: [https://www.judopay.com/docs/](https://www.judopay.com/docs/)

### Step 2 

Once registered, you will receive an email providing you with temporary login details and will be asked to reset your password to a memorable, strong password. Once logged in, you'll be able to download our SDKs, access development tools, run test transactions in our Sandbox environment and be able to add and administer your first App by following the on-screen steps.

### Step 3 

Add the following code snippet to your project upon start up or before making your transaction call. This is best done within the appDelegates FinishedLaunching method (iOS) or within the apps initial activity's OnCreate() method (Android). Specifying your ApiToken, ApiSecret and Judo ID which you can find in the Dashboard (this code snippet is configured to operate in Sandbox environment. If you are planning to integrate in Live, please change the necessary information listed in the going live document):

## Configuration

``` csharp
var configInstance = JudoConfiguration.Instance;

    //setting for SandBox
    configInstance.Environment = JudoEnvironment.Sandbox;

    configInstance.ApiToken =  "{ApiToken}";
    configInstance.ApiSecret = "{ApiSecret}";
    configInstance.JudoId =    "{JudoID}";


    // setting up 3d secure, AVS, Amex and mestro card support (optional configuration)
    JudoSDKManager.AVSEnabled = true;
    JudoSDKManager.AmExAccepted = true;
    JudoSDKManager.MaestroAccepted = true;

    // this will turn on UI mode which will hand over control to our out of 
    //the box UI solution
    JudoSDKManager.UIMode = true;
```

**Please note:** You can configure judoPay library to use live environment by changing the third parameter in `SetApiTokenAndSecret ()`from `Environment.Sandbox` to `Environment.live`


### Card payment

Now that you've configured your SDK with your API Tokens and Secrets, you're ready to use the JudoSDKManager to process payments. 

By calling the following with the SDK Manager, you'll invoke judo's UI to enter card data and submit the payment request:

```csharp
//Define a success block
private void JudoSuccessPayment(PaymentReceiptModel receipt)
     {
     // handle receipt
     }
//Define a Failure block
private void JudoFailurePayment(JudoError error, PaymentReceiptModel receipt)
     {
     // handle errors or Declined cards
     }

//Pass through the payment you would like to facilitate
var paymentViewModel = new PaymentViewModel
     {
      Amount = 4.5m, 
      ConsumerReference = consumerRef,
      PaymentReference = paymentReference,
      Currency = "GBP",
      Card = new CardViewModel()
     };

//Let Judo do the rest
 //iOS
JudoSDKManager.Instance.Payment (PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure)

//Android
JudoSDKManager.Instance.Payment (PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure, Activity context) 

```
####Note: 
This callback should be non-blocking

### Apple Pay

The iOS Implementation supports Apple Pay on payments and PreAuthorisations.

To integrate ApplePay with your app, you must set it up at the Judo Account level ([guide here](https://www.judopay.com/docs/v4_6/apple-pay/quickstart/)), as well as make some changes to your applications settings.

Add this block of code in your applications Entitlements.Plist:
```
<dict>
<key>com.apple.developer.in-app-payments</key> 
<array> 
<string>INSERT-MERCHANT-NAME-HERE</string> 
</array>
</dict>
```

After you are all set up making an Apple Pay transaction is just as easy as before

#### Payment

ApplePay
```

//Construct the viewModel
var summaryItems = new PKPaymentSummaryItem[] {
				new PKPaymentSummaryItem () {
					Amount = new NSDecimalNumber ("0.90"),
					Label = @"Judo Burrito"

				},
				new PKPaymentSummaryItem () {
					Amount = new NSDecimalNumber ("0.10"),
					Label = @"Extra Guac"

				}
			};
			
			var applePayViewModel = new ApplePayViewModel {
				
				CurrencyCode = new NSString ("GBP"),
				CountryCode = new NSString (@"GB"),
				SupportedNetworks = new NSString[3]{ new NSString ("Visa"),
				new NSString ("MasterCard"), new NSString ("Amex") },
				SummaryItems = summaryItems,
				TotalSummaryItem = new PKPaymentSummaryItem () {
					Amount = new NSDecimalNumber ("1.00"),//total of the previous items
					Label = @"El Judorito" // who the transaction is made out to pay

				},
				ConsumerRef=new NSString (@"GenerateYourOwnCustomerRefHere"),
				MerchantIdentifier = new NSString ("INSERT_MERCHANT_ID_HERE")

			};

JudoSDKManager.Instance.MakeApplePayment (applePayViewModel payment, SuccessCallback success, FailureCallback failure)

```

#### PreAuthorise
ApplePay
```
JudoSDKManager.Instance.MakeApplePreAuth (applePayViewModel payment, SuccessCallback success, FailureCallback failure)
```

## TroubleShooting

-  For further SDK reference please visit our SDK [docs]( https://www.judopay.com/docs/ "Docs")
-  Raise an issue here - if you're having problems let us know! This will be an active and evolving SDK, and helping you helps us.

## FAQ

-  [Check our FAQ page]( http://help.judopayments.com/ "Help")
-  [Email our support team]( http://help.judopayments.com/customer/portal/emails/new "Support")
-  Contact one of our Maintainers

## Maintainers

[Luke Fieldsend]( https://github.com/LukeFieldsend "ItsYourBoi!")  : Luke.Fieldsend@judopayments.com

## Contributing

Have your say, If you want a feature maybe we can work together on it?

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D

## Release Notes

####V2.2.0
- Unification of Android SDK with Core Library
- Unified SDK Configuration and Library calls
- Jailbroken/Rooted device detection
- Improved Android UI
- Smaller SDK footprint
- Xamarin 4 support

####V2.1.0
- Apple Pay Support for Judo Xamarin iOS: Payments and preauthorisations.
- JudoShield Support.
- Device DNA.
- Improved Sample application UI.
- Improved SDK Configuration

####V2.0 iOS support
-  Full suite of judo services are now available to Xamarin iOS developers
  -  Payments
  -  pre-Auth
  -  Token Payments
  -  Card Registry
-  3D Secure support for Xamarin iOS
-  64bit component support
-  iPad form factor support

## Known Issues

-  Some implementations of 3D Secure require subsequent reloads of judoPay 3D Secure viewer, your application may block this,
add the key below to the info.plist of your app if 3D Secure view does not load properly.
```
<key>NSAppTransportSecurity</key> 
<dict> 
<key>NSAllowsArbitraryLoads</key><true/> 
</dict> 
```


-  The Xamarin Forum is an invaluable [resource]( http://forums.xamarin.com/ "Xamarin Forum")  

## License

The MIT License (MIT)

Copyright (c) 2015 Alternative Payments Ltd

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
