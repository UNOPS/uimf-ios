namespace IOSUiMetadataFramework.Core.Managers
{
    using System.Collections.Generic;
    using UiMetadataFramework.Core;
    using UIKit;

	public interface IOutputManager
	{
		UIView GetView(OutputFieldMetadata outputField,
		    object value,
		    MyFormHandler myFormHandler,
		    FormMetadata formMetadata,
		    List<MyFormHandler.FormInputManager> inputsManager, int yAxis);
	}
}