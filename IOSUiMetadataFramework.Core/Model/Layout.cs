namespace IOSUiMetadataFramework.Core
{
	using UIKit;

	public class Layout
	{
		public Layout(UIView view, NSLayoutConstraint[] constraints)
		{
			this.View = view;
			this.Constraints = constraints;
		}

		public NSLayoutConstraint[] Constraints { get; }
		public UIView View { get; }
	}
}