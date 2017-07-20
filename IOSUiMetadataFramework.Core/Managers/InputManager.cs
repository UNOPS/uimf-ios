namespace IOSUiMetadataFramework.Core.Managers
{
	using UIKit;

	public interface IInputManager
	{
		UIView GetView(UIView viewController);
		object GetValue();
		void SetValue(object value);
	}
}