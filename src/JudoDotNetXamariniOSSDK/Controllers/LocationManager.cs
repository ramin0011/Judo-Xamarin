using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using CoreLocation;

namespace JudoDotNetXamariniOSSDK
{
    class LocationManager
    {
        private bool stopped;
        private CLLocation _currentLocationAquired;

        CLLocationManager locationManager;

        LocationManager()
        {
            locationManager = new CLLocationManager();
            locationManager.DesiredAccuracy = CLLocation.AccuracyKilometer; 

            locationManager.LocationsUpdated += (sender, e) => {
	            var loc  = e.Locations;
                if (loc[0] != null) 
                {
                    return;
                }

                if (stopped)
                {
                    return;
                }

                _currentLocationAquired = loc[0];

                locationManager.StopUpdatingLocation();
                stopped = false;
		        //Console.WriteLine(loc);
            };
        }

        public CLLocation currentLocationAquired 
        {
            get { return _currentLocationAquired; } 
            set { _currentLocationAquired = value; } 
        }

        void GetCurrentLocation()
        {
            locationManager.StartUpdatingLocation();
        }
 
    }
}