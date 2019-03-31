namespace IOSUiMetadataFramework.Core.Outputs
{
    using System.Collections.Generic;
    using CoreGraphics;
    using IOSUiMetadataFramework.Core;
    using IOSUiMetadataFramework.Core.Attributes;
    using IOSUiMetadataFramework.Core.Managers;
    using IOSUiMetadataFramework.Core.Model;
    using UiMetadataFramework.Core;
    using UIKit;

    [Output(Type = "list")]
	public class ListOutput : IOutputManager
	{
	    private UITextView OutputText { get; set; }

	    public UIView GetView(OutputFieldMetadata outputField,
	        object value,
	        MyFormHandler myFormHandler,
	        FormMetadata formMetadata,
	        List<FormInputManager> inputsManager,
	        int yAxis)
	    {
	        this.OutputText = new UITextView();
	        if (value != null)
	        {
	            this.OutputText.Text = outputField.Label + ": " + value;
	            var labelSize = new CGSize(UIScreen.MainScreen.Bounds.Width - 40, 30);
	            this.OutputText.Frame = new CGRect(new CGPoint(20, yAxis), labelSize);
	        }
	        return this.OutputText;
	    }
    }

}