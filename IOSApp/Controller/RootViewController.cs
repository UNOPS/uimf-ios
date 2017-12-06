using RectangleF = CoreGraphics.CGRect;

namespace IOSApp.Controller
{
    using SidebarNavigation;
    using UIKit;

    public partial class RootViewController : UIViewController
    {
        public RootViewController() : base("RootViewController", null)
        {
        }

        public NavController NavController { get; private set; }
        public SidebarController SidebarController { get; private set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var introController = new IntroController();
            var menuController = new SideMenuController();
            this.NavController = new NavController();

            this.NavController.PushViewController(introController, false);
            this.SidebarController = new SidebarController(this, this.NavController, menuController)
            {
                MenuWidth = 220,
                View = { Frame = new RectangleF(0, 0, this.View.Frame.Width, this.View.Frame.Height) },
                ReopenOnRotate = false,
                MenuLocation = MenuLocations.Left
            };
            // Perform any additional setup after loading the view, typically from a nib.
        }
    }
}