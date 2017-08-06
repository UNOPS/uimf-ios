namespace IOSUiMetadataFramework.Core
{
	using System;
	using System.Drawing;
	using UIKit;

	public static class Extensions
	{

		public static void RemoveAllViews(this UIView view)
		{
			foreach (var subview in view.Subviews)
			{
				subview.RemoveFromSuperview();
			}
		}

		public static NSLayoutConstraint[] FillParent(this UIView view, UIView parent, float top = 0, float left = 0 , float right =0, float bottom = 0)
		{
			var list = new NSLayoutConstraint[4];
			view.TranslatesAutoresizingMaskIntoConstraints = false;
			var topC = NSLayoutConstraint.Create(view, NSLayoutAttribute.Top, NSLayoutRelation.Equal,
				parent, NSLayoutAttribute.Top, 1, top);

			var leftC = NSLayoutConstraint.Create(view, NSLayoutAttribute.Left, NSLayoutRelation.Equal, parent, NSLayoutAttribute.Left, 1, left);

			var rightC = NSLayoutConstraint.Create(view, NSLayoutAttribute.Right, NSLayoutRelation.Equal, parent, NSLayoutAttribute.Right, 1, right);
			var bottomC = NSLayoutConstraint.Create(view, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, parent, NSLayoutAttribute.Bottom, 1, bottom);
			list[0] = topC;
			list[1] = leftC;
			list[2] = rightC;
			list[3] = bottomC;
			return list;
		}

		public static void FillWidth(this UIView view, UIView parent, float height, float top = 0, float left = 0, float right = 0)
		{
			view.TranslatesAutoresizingMaskIntoConstraints = false;
			parent.AddConstraint(
				NSLayoutConstraint.Create(view, NSLayoutAttribute.Top, NSLayoutRelation.Equal,
					parent, NSLayoutAttribute.Top, 1, top));
			parent.AddConstraint(
				NSLayoutConstraint.Create(view, NSLayoutAttribute.Left, NSLayoutRelation.Equal, parent, NSLayoutAttribute.Left, 1, left));

			parent.AddConstraint(
				NSLayoutConstraint.Create(view, NSLayoutAttribute.Right, NSLayoutRelation.Equal, parent, NSLayoutAttribute.Right, 1, right));

			parent.AddConstraint(
				NSLayoutConstraint.Create(view, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 0, height)
			);
		}


		public static DateTime NSDateToDateTime(this Foundation.NSDate date)
		{
			DateTime reference = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			var utcDateTime = reference.AddSeconds(date.SecondsSinceReferenceDate);
			return utcDateTime.ToLocalTime();
		}

		public static Foundation.NSDate DateTimeToNSDate(this DateTime date)
		{
			DateTime reference = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			var utcDateTime = date.ToUniversalTime();
			return Foundation.NSDate.FromTimeIntervalSinceReferenceDate((utcDateTime - reference).TotalSeconds);
		}

		public static PointF Rotate(this PointF pt)
		{
			return new PointF(pt.Y, pt.X);
		}

		public static PointF Center(this RectangleF rect)
		{
			return new PointF(
				(rect.Right - rect.Left) / 2.0f,
				(rect.Bottom - rect.Top) / 2.0f
			);
		}
	}
}