﻿namespace IOSUiMetadataFramework.Core.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using IOSUiMetadataFramework.Core;
    using IOSUiMetadataFramework.Core.Model;
    using UiMetadataFramework.Core;
    using UiMetadataFramework.MediatR;

    public static class EventsManager
    {
        public static void OnFormLoadedEvent(FormParameter formParameters)
        {
        }

        public static void OnFormPostingEvent(FormMetadata formMetadata, List<FormInputManager> inputsManager)
        {
        }

        public static void OnResponseHandledEvent(MyFormHandler myFormHandler,FormMetadata formMetadata, List<FormInputManager> inputsManager, InvokeForm.Response result)
        {
           var inputsWithEvent = inputsManager.Where(a => a.Input.EventHandlers.Any(e => e.RunAt.Equals(FormEvents.ResponseHandled))).ToList();
           //var formEvent = formMetadata.EventHandlers.Where(e => e.RunAt.Equals(FormEvents.ResponseHandled)).ToList();

            foreach (var input in inputsWithEvent)
            {
                var inputEvents = input.Input.EventHandlers.Where(e => e.RunAt.Equals(FormEvents.ResponseHandled));
                foreach (var inputEvent in inputEvents)
                {
                    var manager = myFormHandler.ManagersCollection.EventHandlerManagerCollection.GetManager(inputEvent.Id);
                    manager.HandleEvent(inputEvent.CustomProperties, input, result);
                }
               
            }
        }

        public static void OnResponseReceivedEvent(FormMetadata formMetadata, List<FormInputManager> inputsManager, object resultData)
        {
        }
    }
}