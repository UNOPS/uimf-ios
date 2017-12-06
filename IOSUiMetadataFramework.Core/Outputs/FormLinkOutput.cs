namespace IOSUiMetadataFramework.Core.Outputs
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using CoreGraphics;
	using Foundation;
	using IOSUiMetadataFramework.Core.Attributes;
	using IOSUiMetadataFramework.Core.Managers;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UIKit;

	[Output(Type = "formlink")]
	public class FormLinkOutput : IOutputManager
	{
	    private UITextView OutputView { get; set; }

        public UIView GetView(OutputFieldMetadata outputField,
		    object value,
		    MyFormHandler myFormHandler,
		    FormMetadata formMetadata,
		    List<MyFormHandler.FormInputManager> inputsManager,
		    int yAxis)
		{
			var formLink = value.CastTObject<FormLink>();
		    var size = new CGSize(UIScreen.MainScreen.Bounds.Width - 40, 30);
            this.OutputView = new UITextView
		    {
		        Text = outputField.Label + ": "+formLink.Label,
		        Frame = new CGRect(new CGPoint(20, yAxis), size),
		        TextColor = UIColor.Blue
		    };

            UITapGestureRecognizer gesture = new UITapGestureRecognizer(); 
            gesture.AddTarget(() => myFormHandler.StartIFormAsyn(formLink.Form, formLink.InputFieldValues));
		    this.OutputView.AddGestureRecognizer(gesture);
            return this.OutputView;
		}

	}

}