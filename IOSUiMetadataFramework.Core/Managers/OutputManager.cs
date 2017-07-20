namespace IOSUiMetadataFramework.Core.Managers
{
	using UIKit;

	public interface IOutputManager
	{
		UIView GetView(UIView viewController,string name, object value, FormView formView, int yAxis);
	}
}