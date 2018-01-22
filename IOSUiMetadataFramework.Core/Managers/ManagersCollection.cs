namespace IOSUiMetadataFramework.Core.Managers
{
    public class ManagersCollection
    {
        public InputManagerCollection InputManagerCollection { get; set; } = new InputManagerCollection();
        public OutputManagerCollection OutputManagerCollection { get; set; } = new OutputManagerCollection();
        public EventHandlerManagerCollection EventHandlerManagerCollection { get; set; } = new EventHandlerManagerCollection();
    }
}