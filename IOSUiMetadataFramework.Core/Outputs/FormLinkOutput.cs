namespace IOSUiMetadataFramework.Core.Outputs
{
	using CoreGraphics;
	using IOSUiMetadataFramework.Core.Attributes;
	using IOSUiMetadataFramework.Core.Managers;
	using UiMetadataFramework.Basic.Output;
	using UIKit;

	[Output(Type = "formlink")]
	public class FormLinkOutput : IOutputManager
	{
		private UIButton Button { get; set; }

		public UIView GetView(UIView viewController, string name, object value, FormView formView, int yAxis)
		{
			var formLink = (FormLink)value;
			this.Button = new UIButton();
			this.Button.SetTitle(formLink.Label, UIControlState.Normal);
			var btnSize = new CGSize(UIScreen.MainScreen.Bounds.Width - 40, 40);
			this.Button.Frame = new CGRect(new CGPoint(20, yAxis), btnSize);
			this.Button.BackgroundColor = UIColor.Gray;

			this.Button.TouchUpInside += async (sender, args) =>
			{
				await formView.StartIForm(formLink.Form, formLink.InputFieldValues);
			};

			return this.Button;
		}
	}
}