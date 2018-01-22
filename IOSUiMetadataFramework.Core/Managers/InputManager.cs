namespace IOSUiMetadataFramework.Core.Managers
{
    using System.Collections.Generic;
    using IOSUiMetadataFramework.Core.Model;
    using UIKit;

	public interface IInputManager
	{
		UIView GetView(IDictionary<string, object> inputCustomProperties, MyFormHandler myFormHandler);
		object GetValue();
		void SetValue(object value);
	}
}