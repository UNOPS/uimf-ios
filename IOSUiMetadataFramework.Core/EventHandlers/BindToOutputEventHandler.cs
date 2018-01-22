namespace IOSUiMetadataFramework.Core.EventHandlers
{
    using System;
    using IOSUiMetadataFramework.Core;
    using IOSUiMetadataFramework.Core.Attributes;
    using IOSUiMetadataFramework.Core.Managers;
    using IOSUiMetadataFramework.Core.Model;
    using Newtonsoft.Json.Linq;
    using UiMetadataFramework.Basic.EventHandlers;
    using UiMetadataFramework.MediatR;

    [EventHandler(Type = "bind-to-output")]
    public class BindToOutputEventHandler : IEventHandlerManager
    {
        public void HandleEvent(object inputEventCustomProperties, FormInputManager inputManager, InvokeForm.Response result)
        {
            string outputField;
            if (inputEventCustomProperties.GetType() == typeof(JObject))
            {
                var jsonObj = (JObject)inputEventCustomProperties;
                
                outputField = jsonObj.GetValue(nameof(BindToOutputAttribute.OutputFieldId), StringComparison.OrdinalIgnoreCase).ToString();
            }
            else
            {
                var data = (CustomProperty)inputEventCustomProperties;
                outputField = data.OutputFieldId;
            }

            object value;
            if (result.Data.GetType() == typeof(JObject))
            {
                var jsonObj = (JObject)result.Data;
                value = jsonObj.GetValue(outputField, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                var propertyInfo = result.Data.GetType().GetProperty(outputField);
                value = propertyInfo?.GetValue(result.Data, null);
            }

            inputManager.Manager.SetValue(value);
        }

        internal class CustomProperty
        {
            public string OutputFieldId { get; set; }
        }

    }
}