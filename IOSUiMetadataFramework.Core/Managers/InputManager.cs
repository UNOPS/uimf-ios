namespace IOSUiMetadataFramework.Core.Managers
{
	using UIKit;

	public interface IInputManager
	{
		UIView GetView(object inputCustomProperties);
		object GetValue();
		void SetValue(object value);
	}
}