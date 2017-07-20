namespace IOSUiMetadataFramework.Core.Outputs
{
	using System;
	using CoreGraphics;
	using IOSUiMetadataFramework.Core;
	using IOSUiMetadataFramework.Core.Attributes;
	using IOSUiMetadataFramework.Core.Managers;
	using UIKit;

	[Output(Type = "datetime")]
	public class DateOutput : IOutputManager
	{
		private UITextView OutputText { get; set; }

		public UIView GetView(UIView viewController, string name, object value, FormView formView, int yAxis)
		{
			this.OutputText = new UITextView();
			if (value != null)
			{
				DateTime datetime = (DateTime)value;
				this.OutputText.Text = name + ": " + datetime.ToShortDateString();

				var labelSize = new CGSize(viewController.Frame.Width - 40, 30);
				this.OutputText.Frame = new CGRect(new CGPoint(20, yAxis), labelSize);
			}
			return this.OutputText;
		}
	}
}