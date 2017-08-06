namespace IOSUiMetadataFramework.Core.Outputs
{
	using CoreGraphics;
	using IOSUiMetadataFramework.Core.Attributes;
	using IOSUiMetadataFramework.Core.Managers;
	using UIKit;

	[Output(Type = "tabstrip")]
	public class TabstripOutput : IOutputManager
	{
		private UITextView OutputText { get; set; }

		public UIView GetView(UIView viewController, string name, object value, FormView formView, int yAxis)
		{
			this.OutputText = new UITextView();
			return this.OutputText;
		}

	}
}