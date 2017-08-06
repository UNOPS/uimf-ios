namespace IOSUiMetadataFramework.Core.Outputs
{
	using System;
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
					var window = UIApplication.SharedApplication.KeyWindow;
					var vc = window.RootViewController;
					var view = new UIView
					{
						Bounds = UIScreen.MainScreen.Bounds,
						BackgroundColor = UIColor.FromWhiteAlpha(1, 0.5f)
					};
					var newVc = new UIViewController
					{
						ModalPresentationStyle = UIModalPresentationStyle.FormSheet,
						ModalTransitionStyle = UIModalTransitionStyle.CoverVertical
					};
					
					var layout = await formView.GetIForm(formLink.Form, formLink.InputFieldValues);
					var btn = new UIButton();
					btn.SetTitle("Close", UIControlState.Normal);
					btn.Frame = new CGRect(new CGPoint(20, UIScreen.MainScreen.Bounds.Height - 40), btnSize);
					btn.SetTitleColor(UIColor.Blue, UIControlState.Normal);
					btn.TouchUpInside += (s, a) =>
					{
						newVc.DismissViewController(true, null);
					};
					
					layout.View.Frame = UIScreen.MainScreen.Bounds;
					layout.View.AddSubview(btn);
					view.AddSubview(layout.View);
					
					newVc.View = view;
					vc.PresentViewController(newVc, true, null);

				
			};

			return this.Button;
		}
	}

}