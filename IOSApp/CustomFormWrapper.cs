namespace IOSApp
{
    using System;
    using CoreGraphics;
    using IOSApp.Controller;
    using IOSUiMetadataFramework.Core.Model;
    using UiMetadataFramework.Basic.Response;
    using UIKit;

    public class CustomFormWrapper : IFormWrapper
    {
        public CustomFormWrapper(SideMenuController sideMenuController, UIViewController navController)
        {
            this.ViewController = navController;
            this.SideMenuController = sideMenuController;
        }

        public SideMenuController SideMenuController { get; set; }

        public UIViewController ViewController { get; set; }

        public void CloseForm()
        {
            var bar = this.ViewController as UINavigationController;
            bar?.PopViewController(false);
        }

        public void ReloadView(MyFormHandler myFormHandler, ReloadResponse reloadResponse)
        {
            var allForms = this.SideMenuController.Reload();
            var metadata = allForms[reloadResponse.Form];
            this.UpdateView(myFormHandler, new FormParameter(metadata, reloadResponse.InputFieldValues));
        }

        public void UpdateView(MyFormHandler myFormHandler, FormParameter formParameter, string submitAction = null)
        {
            try
            {
                var layout = myFormHandler.GetIFormAsync(formParameter.Form, formParameter.Parameters);
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
    }
}