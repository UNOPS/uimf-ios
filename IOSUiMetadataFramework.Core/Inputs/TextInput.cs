namespace IOSUiMetadataFramework.Core.Inputs
{
	using CoreGraphics;
	using IOSUiMetadataFramework.Core.Attributes;
	using IOSUiMetadataFramework.Core.Managers;
	using UIKit;

	[Input(Type = "text")]
	public class TextInput : IInputManager
	{
		private UITextField InputText { get; set; }

		public UIView GetView(object inputCustomProperties, MyFormHandler myFormHandler)
		{
			this.InputText = new UITextField();
		    this.InputText.SetTextBorders();
            this.InputText.ClearButtonMode = UITextFieldViewMode.WhileEditing;
            UIView paddingView = new UIView(new CGRect(0, 0, 10, 20));
			this.InputText.LeftView = paddingView;
			this.InputText.LeftViewMode = UITextFieldViewMode.Always;
			return this.InputText;
		}

		public object GetValue()
		{
			return this.InputText.Text;
		}

		public void SetValue(object value)
		{
			this.InputText.Text = value?.ToString();
		}
	}
}