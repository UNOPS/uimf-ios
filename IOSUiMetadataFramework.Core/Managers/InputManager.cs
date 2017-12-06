namespace IOSUiMetadataFramework.Core.Managers
{
	using UIKit;

	public interface IInputManager
	{
		UIView GetView(object inputCustomProperties, MyFormHandler myFormHandler);
		object GetValue();
		void SetValue(object value);
	}
}