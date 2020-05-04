using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using Syncfusion.SfChart.XForms.iOS.Renderers;

using Syncfusion.SfDataGrid.XForms.iOS;

using Syncfusion.XForms.iOS.ProgressBar; 

using Syncfusion.ListView.XForms.iOS;

using Syncfusion.SfDiagram.XForms.iOS;

using Syncfusion.XForms.iOS.TabView;

using Syncfusion.XForms.iOS.Buttons;

using Syncfusion.XForms.iOS.ComboBox;

using Syncfusion.XForms.iOS.Border;

using Syncfusion.XForms.iOS.Expander;

using Syncfusion.XForms.iOS.Cards;

using Syncfusion.XForms.Pickers.iOS;

namespace Consumption.Moblie.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            
			SfChartRenderer.Init();
			
			
			SfDataGridRenderer.Init();
			
			
			SfLinearProgressBarRenderer.Init(); 
			
			
			SfCircularProgressBarRenderer.Init(); 
			
			
			SfListViewRenderer.Init();
			
			
			SfDiagramRenderer.Init();
			
			
			SfTabViewRenderer.Init();
			
			
			SfCheckBoxRenderer.Init();
			
			
			SfRadioButtonRenderer.Init();
			
			
			SfComboBoxRenderer.Init();
			
			
			SfButtonRenderer.Init();
			
			
			SfBorderRenderer.Init();
			
			
			SfExpanderRenderer.Init();
			
			
			SfCardViewRenderer.Init();
			
			
			SfCardLayoutRenderer.Init();
			
			
			SfSwitchRenderer.Init();
			
			
			SfTimePickerRenderer.Init();
			
			
			SfDatePickerRenderer.Init();
			
			LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
