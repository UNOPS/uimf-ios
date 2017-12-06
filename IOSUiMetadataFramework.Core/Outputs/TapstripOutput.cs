namespace IOSUiMetadataFramework.Core.Outputs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CoreAnimation;
    using CoreGraphics;
	using IOSUiMetadataFramework.Core.Attributes;
	using IOSUiMetadataFramework.Core.Managers;
    using UiMetadataFramework.Basic.Output;
    using UiMetadataFramework.Core;
	using UIKit;

	[Output(Type = "tabstrip")]
	public class TabstripOutput : IOutputManager
	{
		private UIView OutputView { get; set; }

		public UIView GetView(OutputFieldMetadata outputField,
		    object value,
		    MyFormHandler myFormHandler,
		    FormMetadata formMetadata,
		    List<MyFormHandler.FormInputManager> inputsManager,
		    int yAxis)
		{
			this.OutputView = new UIView();
		    var labelSize = new CGSize(UIScreen.MainScreen.Bounds.Width - 40, 30);
		    this.OutputView.Frame = new CGRect(new CGPoint(20, yAxis), labelSize);

            var tabstrip = value.CastTObject<Tabstrip>();
		    var currentTab = tabstrip.Tabs.SingleOrDefault(a => a.Form == tabstrip.CurrentTab);

		    var x = 0;
		    var width = UIScreen.MainScreen.Bounds.Width - 60;
            foreach (var tab in tabstrip.Tabs)
		    {
		        var size = new CGSize(width / tabstrip.Tabs.Count, 30);		        
                if (tab != null)
		        {		           
                    var tv = new UITextView
		            {
		                Text = tab.Label,
		                Frame = new CGRect(new CGPoint(x, 0), size),
		                TextColor = UIColor.LightGray
		            };
		            nfloat borderWidth = 2;
                    var border = new CALayer
		            {
		                BorderColor = UIColor.LightGray.CGColor,
		                Frame = new CGRect(0, tv.Frame.Size.Height - borderWidth, tv.Frame.Size.Width - 20, tv.Frame.Size.Height),
		                BorderWidth = width
		            };
		           
                    x += (int)tv.Frame.Width;
                    UITapGestureRecognizer gesture = new UITapGestureRecognizer();
		            gesture.AddTarget(() => myFormHandler.StartIFormAsyn(tab.Form, tab.InputFieldValues)); 
		            tv.AddGestureRecognizer(gesture);
                    if (tab == currentTab)
		            {
		                tv.TextColor = UIColor.Black;
		                border.BorderColor = UIColor.Black.CGColor;

		            }
		            tv.Layer.AddSublayer(border);
		            tv.Layer.MasksToBounds = true;
                    this.OutputView.AddSubview(tv);

                }

		    }
            return this.OutputView;
		}


	}
}