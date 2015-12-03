using System;
using Foundation;
using UIKit;
using JudoDotNetXamariniOSSDK;
using JudoPayDotNet.Enums;

namespace JudoPayiOSXamarinSampleApp
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register ("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        public override UIWindow Window {
            get;
            set;
        }


        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {  
            Window = new UIWindow (UIScreen.MainScreen.Bounds); 
            RootView root = new RootView ();
            Window.RootViewController = new UINavigationController (root);
            Window.TintColor = UIColor.Black;
            Window.MakeKeyAndVisible ();
            ConfigureJudoSettings ();
            return true;   
        }
			
        // This method is invoked when the application is about to move from active to inactive state.
        // OpenGL applications should use this method to pause.
        public override void OnResignActivation (UIApplication application)
        {
        }
        // This method should be used to release shared resources and it should store the application state.
        // If your application supports background exection this method is called instead of WillTerminate
        // when the user quits.
        public override void DidEnterBackground (UIApplication application)
        {
        }
        // This method is called as part of the transiton from background to active state.
        public override void WillEnterForeground (UIApplication application)
        {
        }
        // This method is called when the application is about to terminate. Save data, if needed.
        public override void WillTerminate (UIApplication application)
        {
        }

        void ConfigureJudoSettings ()
        {
            //Configure JudoPay app here
            var configInstance = JudoConfiguration.Instance;

            //setting for Sandnox
            configInstance.Environment = JudoEnvironment.Live;


            /*
			configInstance.ApiToken = "[Application ApiToken]"; //retrieve from JudoPortal
			configInstance.ApiSecret = "[Application ApiSecret]"; //retrieve from JudoPortal
			configInstance.JudoId = "[Judo ID]"; //Received when registering an account with Judo
           
			*/


            configInstance.ApiToken = "bqqAHqFsrqbyPVtr"; //retrieve from JudoPortal
            configInstance.ApiSecret = "146f554a0fb11f44e6e38ad1cb4681907c138bb31d5d15dab0ba46036ab5c3f9"; //retrieve from JudoPortal
            configInstance.JudoId = "100224351"; //Received when registering an account with Judo

//            configInstance.ApiToken = "LDTQMks1Ou6pBIru"; //retrieve from JudoPortal
//            configInstance.ApiSecret = "6468663c2f20ed14e8f378c9e586c8af9c2a69ba223e13a16a61bea9de2c6101"; //retrieve from JudoPortal
//            configInstance.JudoId = "990788"; //Received when registering an account with Judo

            if (configInstance.ApiToken == null) {
                throw(new Exception ("Judo Configuration settings have not been set on the config Instance.i.e JudoID Token,Secret"));
            }
				
            /*
            // setting up 3d secure, AVS, Amex and mestro card support
		    JudoSDKManager.AVSEnabled = true;
		    JudoSDKManager.AmExAccepted = true;
		    JudoSDKManager.MaestroAccepted = true;


            
            // this will turn off UI mode and you can use same judo APIs to link with your own UI
		    //JudoSDKManager.UIMode = false;
            */
        }
    }


}
