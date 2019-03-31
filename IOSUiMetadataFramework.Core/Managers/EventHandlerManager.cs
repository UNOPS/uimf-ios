namespace IOSUiMetadataFramework.Core.Managers
{
    using IOSUiMetadataFramework.Core;
    using IOSUiMetadataFramework.Core.Model;
    using UiMetadataFramework.MediatR;

    public interface IEventHandlerManager
    {
        void HandleEvent(object inputEventCustomProperties, FormInputManager inputManager, InvokeForm.Response result);
    }
}