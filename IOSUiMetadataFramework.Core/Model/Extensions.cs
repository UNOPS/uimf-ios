namespace IOSUiMetadataFramework.Core
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;
	using Foundation;
	using IOSUiMetadataFramework.Core.Model;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.MediatR;
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

	    public static T CastTObject<T>(this object obj)
	    {
	        if (obj.GetType() == typeof(JObject))
	        {
	            return JsonConvert.DeserializeObject<T>(obj.ToString());
	        }
	        if (obj.GetType() == typeof(JValue))
	        {
	            return ((JValue)obj).ToObject<T>();
	        }
	        if (obj.GetType() == typeof(JArray))
	        {
	            return ((JArray)obj).ToObject<T>();
	        }
	        return (T)obj;
	    }

	    public static void AdjustLabelSize(this UITextView label,nfloat width, float maxHeight = 100f)
	    {
	        var size = ((NSString)label.Text).StringSize(label.Font, constrainedToSize: new SizeF((float)width, maxHeight),
	            lineBreakMode: UILineBreakMode.WordWrap);
	        var labelFrame = label.Frame;
	        labelFrame.Size = new SizeF((float)width, (float)size.Height+15);
	        label.Frame = labelFrame;
	    }

        public static IEnumerable<object> GetTypeaheadSource<T>(this object source,
            MyFormHandler myFormHandler,
            TypeaheadRequest<T> request = null)
        {
            if (source is IEnumerable<object>)
            {
                return source.CastTObject<IEnumerable<object>>();
            }
            if (request != null)
            {
                var list = new Dictionary<string, object> { { "query", request.Query }, { "ids", request.Ids } };
                var obj = JsonConvert.SerializeObject(list);

                var dataSource = source.ToString();
                var formRequest = new InvokeForm.Request
                {
                    Form = dataSource,
                    InputFieldValues = obj
                };

                try
                {
                    var result = Task.Run(
                        () => myFormHandler.InvokeFormAsync(new[] { formRequest }));

                    var response = result.Result;

                    var typeahead = response[0].Data.CastTObject<TypeaheadResponse<object>>();

                    if (typeahead != null)
                    {
                        return typeahead.Items;
                    }
                }
                catch (AggregateException ex)
                {
                    ex.ThrowInnerException();
                }
            }
            return new List<object>();
        }
        public static void ThrowInnerException(this AggregateException exception)
        {
            var innerException = exception.InnerExceptions?.FirstOrDefault();
            if (innerException != null)
            {
                throw innerException;
            }
            throw exception;
        }
        public static T GetCustomProperty<T>(this IDictionary<string, object> customProperties, string property)
        {
            var dictionary = new Dictionary<string, object>(customProperties, StringComparer.OrdinalIgnoreCase);
            dictionary.TryGetValue(property, out var value);
            return value != null ? value.CastTObject<T>() : default(T);
        }

        public static void SetTextBorders(this UITextField textField)
	    {
	        textField.Layer.CornerRadius = 8;
	        textField.Layer.BorderColor = UIColor.LightGray.CGColor;
	        textField.Layer.BorderWidth = 1;
        }
    }
}