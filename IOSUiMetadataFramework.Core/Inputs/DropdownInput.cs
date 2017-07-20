namespace IOSUiMetadataFramework.Core.Inputs
{
	using System;
	using IOSUiMetadataFramework.Core.Attributes;
	using IOSUiMetadataFramework.Core.Managers;
	using UIKit;

	[Input(Type = "dropdown")]
	public class DropdownInput : IInputManager
	{

	
		public object GetValue()
		{
			return null;
		}

		public void SetValue(object value)
		{
			//this.spinner.SetSelection(value);
		}

		public UIView GetView(UIView viewController)
		{
			throw new NotImplementedException();
		}
	}
}