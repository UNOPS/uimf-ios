namespace IOSUiMetadataFramework.Core.Managers
{
    using System.Collections.Generic;
    using IOSUiMetadataFramework.Core.Model;
    using UiMetadataFramework.Core;
    using UIKit;

	public interface IOutputManager
	{
		UIView GetView(OutputFieldMetadata outputField,
		    object value,
		    MyFormHandler myFormHandler,
		    FormMetadata formMetadata,
		    List<FormInputManager> inputsManager, int yAxis);
	}
}