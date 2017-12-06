namespace IOSApp
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CoreGraphics;
    using IOSApp.Controller;
    using IOSUiMetadataFramework.Core;
    using IOSUiMetadataFramework.Core.Model;
    using SidebarNavigation;
    using UiMetadataFramework.Core;
    using UIKit;

    public class CustomFormWrapper : IFormWrapper
    {
        public CustomFormWrapper(UIViewController navController)
        {
            this.ViewController = navController;
        }

        public UIViewController ViewController { get; set; }

        public async Task UpdateViewAsync(MyFormHandler myFormHandler, FormMetadata metadata, IDictionary<string, object> inputFieldValues = null)
        {
            try
            {
                var layout = await myFormHandler.GetIFormAsync(metadata, inputFieldValues);
                var contentController = new ViewController();
                var size = new CGSize(UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
                layout.View.Frame = new CGRect(new CGPoint(0, 0), size);
                // layout.View.FillParent(contentController.View);
                contentController.View = layout.View;
                var bar = this.ViewController as UINavigationController;
                bar?.PushViewController(contentController, false);
            }
            catch (Exception ex)
            {
                myFormHandler.ShowToast(ex.Message);
            }
           
        }

        public void UpdateView(MyFormHandler myFormHandler, FormMetadata metadata, IDictionary<string, object> inputFieldValues = null)
        {
            throw new NotImplementedException();
        }
    }
}