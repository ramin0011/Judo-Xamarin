<p>
  <img  align="right" src="https://github.com/JudoPay/Judo-Xamarin/blob/master/resources/judo_logo.png?raw=true" alt="Judo"/>
</p>
#Judo-Xamarin

Mobile Payments Xamarin SDK


The JudoPay library lets you integrate card payments into your Xamarin project. It is built to be mobile first with ease of integration in mind. Judo's SDK enables a faster, simpler and more secure payment experience within your app. Build trust and user loyalty in your app with our secure and intuitive UX.

####Sell more Risk less

## Requirments

Xamarin developer Licence

[JudoPay Account]( https://www.judopay.com/docs/ "JudoPay")

Thats it!

## Installation

####[Xamarin Component Store]( https://components.xamarin.com/view/judopay-xamarin-sdk "Component Store")
-   Within Xamaring Studio- Project/Get More Components- Search "JudoPay" and add to app
-   Click download from the above link and unzip the .Xam file and run the install command using the xamarin-component.exe above
  -  mono xamarin-component.exe install /path/to/your.xam (mac)
  -  xamarin-component.exe install /path/to/your.xam (windows)
-   You can also download the component .xam from this page component/judopay-xamarin-sdk-2.0.xam

####Here!
-  Fork the SDK and build it yorself. It won't bite
-  Again download the component .xam from this page component/judopay-xamarin-sdk-2.0.xam

####Nuget
-  Coming Soon

## Configuration

#####iOS
``` csharp
public override bool FinishedLaunching (UIApplication a,NSDictionary o)
{
var configInstance = JudoConfiguration.Instance;

	//setting for SandBox
	configInstance.Environment = JudoPayDotNet.Enums.Environment.Sandbox;

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
	JudoPayDotNet.Enums.Environment.Sandbox );
	
	JudoSDKManager.Configuration.IsAVSEnabled = true;
	JudoSDKManager.Configuration.IsFraudMonitoringSignals = true;
	JudoSDKManager.Configuration.IsMaestroEnabled = true;
	
	// your code
}
```
**Please note:** You can configure judoPay library to use live environment by changing the third parameter in `SetApiTokenAndSecret ()`from `Environment.Sandbox` to `Environment.live`


### Card payment

Now that you've configured your SDK with your API Tokens and Secrets, you're ready to use the JudoSDKManager to process payments. 

By calling the following with the SDK Manager, you'll invoke judo's UI to enter card data and submit the payment request:

#####Android
```csharp

var intent = JudoSDKManager.UIMethods.Payment ( Context context,  	judoId, currency, amount, paymentReference, consumerRef, metaData );

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
var paymentViewModel = new PaymentViewModel
     {
      Amount = 4.5m, 
      ConsumerReference = consumerRef,
      PaymentReference = paymentReference,
      Currency = "GBP",
      Card = new CardViewModel()
     };

//Let Judo do the rest
JudoSDKManager.Payment(paymentViewModel, successCallback, failureCallback, this.NavigationController);

```
####Note: 
This callback should be non-blocking

## TroubleShooting

-  For further SDK reference please visit our SDK [docs]( https://www.judopay.com/docs/ "Docs")
-  Raise an issue here- if your having problems lets us know! This will be an active and evolving SDK and helping you helps us

## FAQ

-  [Maybe somebody got their answer]( http://help.judopayments.com/ "Help")
-  [Email our support Team]( http://help.judopayments.com/customer/portal/emails/new "Support")
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

####V2.0 iOS support
-  Full Suite of Judo services are now available to Xamarin iOS Developers
  -  Payments
  -  preAuth
  -  Token Payments
  -  Card Registry
-  3DSecure support for Xamarion iOS
-  64bit component support
-  iPad form factor support

## Known Issues

-  Some Implementations of 3dSecure require subsequent reloads of JudoPay 3dSecure viewer, your application may block this
add the key below to the info.plist of your app if 3dSecure view does not load properly
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
