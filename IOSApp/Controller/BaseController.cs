namespace IOSApp.Controller
{
    using Foundation;
    using UIKit;

    public partial class BaseController : UIViewController
    {
        public BaseController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        // provide access to the navigation controller to all inheriting controllers
        protected NavController NavController
        {
            get
            {
                var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
                return appDelegate?.RootViewController.NavController;
            }
        }

        // provide access to the sidebar controller to all inheriting controllers
        protected SidebarNavigation.SidebarController SidebarController
        {
            get
            {
                var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
                return appDelegate?.RootViewController.SidebarController;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.NavigationItem.SetLeftBarButtonItem(
                new UIBarButtonItem(UIImage.FromBundle("threelines")
                    , UIBarButtonItemStyle.Plain
                    , (sender, args) => { this.SidebarController.ToggleMenu(); }), true);
        }
    }
}