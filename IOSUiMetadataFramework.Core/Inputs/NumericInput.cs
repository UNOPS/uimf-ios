namespace IOSUiMetadataFramework.Core.Inputs
{
	using System;
	using CoreGraphics;
	using IOSUiMetadataFramework.Core.Attributes;
	using IOSUiMetadataFramework.Core.Managers;
	using UIKit;

	[Input(Type = "number")]
	public class NumericInput : IInputManager
	{
		private UITextField InputText { get; set; }

		public UIView GetView(UIView viewController)
		{
			this.InputText = new UITextField { KeyboardType = UIKeyboardType.NumberPad };
			this.InputText.Layer.CornerRadius = 8;
			this.InputText.Layer.BorderColor = UIColor.LightGray.CGColor;
			this.InputText.Layer.BorderWidth = 1;
			UIView paddingView = new UIView(new CGRect(0, 0, 10, 20));
			this.InputText.LeftView = paddingView;
			this.InputText.LeftViewMode = UITextFieldViewMode.Always;

			return this.InputText;
		}

		public object GetValue()
		{
			if (!string.IsNullOrEmpty(this.InputText.Text))
			{
				return Convert.ToInt32(this.InputText.Text);
			}
			return null;
		}

		public void SetValue(object value)
		{
			this.InputText.Text = value?.ToString();
		}
	}
}