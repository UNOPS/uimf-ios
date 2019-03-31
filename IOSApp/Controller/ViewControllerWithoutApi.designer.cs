// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//

namespace IOSApp.Controller
{
    using System.CodeDom.Compiler;
    using Foundation;

    [Register ("ViewControllerWithoutApi")]
    partial class ViewControllerWithoutApi
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UINavigationItem NavBar { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (this.NavBar != null) {
                this.NavBar.Dispose ();
                this.NavBar = null;
            }
        }
    }
}