namespace IOSUiMetadataFramework.Core.Managers
{
    using IOSUiMetadataFramework.Core;
    using UiMetadataFramework.MediatR;

    public interface IEventHandlerManager
    {
        void HandleEvent(object inputEventCustomProperties, MyFormHandler.FormInputManager inputManager, InvokeForm.Response result);
    }
}