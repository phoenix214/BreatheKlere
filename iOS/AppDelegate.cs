﻿
using Foundation;
using RoundedBoxView.Forms.Plugin.iOSUnified;
using SegmentedControl.FormsPlugin.iOS;
using UIKit;

namespace BreatheKlere.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            SegmentedControlRenderer.Init();
            Xamarin.FormsGoogleMaps.Init(Config.google_maps_ios_api_key);
            RoundedBoxViewRenderer.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
