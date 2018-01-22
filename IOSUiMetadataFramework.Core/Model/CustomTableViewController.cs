namespace IOSUiMetadataFramework.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CoreGraphics;
    using Foundation;
    using Newtonsoft.Json.Linq;
    using UiMetadataFramework.Core;
    using UiMetadataFramework.Core.Binding;
    using UIKit;

    public class CustomTableViewController<T> : UITableViewSource
    {
        public CustomTableViewController(List<T> objectList, IEnumerable<OutputFieldMetadata> outputFieldProperty, MyFormHandler myFormHandler, UIView view)
        {
            this.ObjectList = objectList;
            this.OutputFieldProperty = outputFieldProperty;
            this.MyFormHandler = myFormHandler;
            this.View = view;
        }

        public nfloat CellHeight { get; set; }
        //public nfloat TableHeight { get; set; } = 0;
        public UIView View { get; set; }

        public List<T> ObjectList { get; set; }
        private MyFormHandler MyFormHandler { get; }
        private IEnumerable<OutputFieldMetadata> OutputFieldProperty { get; }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {          
            string CellIdentifier = "TableCell";
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier) ??
                new UITableViewCell(UITableViewCellStyle.Subtitle, CellIdentifier);

            this.CellHeight = 0;

            var orderedOutputs = this.OutputFieldProperty.OrderBy(a => a.OrderIndex);

            foreach (var output in orderedOutputs)
            {
                if (!output.Hidden)
                {
                    object value;
                    if (this.ObjectList[indexPath.Row].GetType() == typeof(JObject))
                    {
                        var jsonObj = this.ObjectList[indexPath.Row] as JObject;
                        value = jsonObj?.GetValue(output.Id.ToLower());
                    }
                    else
                    {
                        var propertyInfo = this.ObjectList[indexPath.Row].GetType().GetProperty(output.Id);
                        value = propertyInfo?.GetValue(this.ObjectList[indexPath.Row], null);
                    }
                    if (value != null)
                    {
                        var manager = this.MyFormHandler.ManagersCollection.OutputManagerCollection.GetManager(output.Type);
                        var outputView = manager.GetView(output, value, this.MyFormHandler, null, null, (int)this.CellHeight);
                        cell.AddSubview(outputView);
                        this.CellHeight += outputView.Frame.Height + 10;
                    }
                }
            }
            //this.TableHeight += this.CellHeight;
            //var size = new CGSize(tableView.Frame.Width, this.TableHeight);
            //tableView.Frame = new CGRect(new CGPoint(tableView.Frame.X, tableView.Frame.Y), size);
            //this.View.Frame = new CGRect(new CGPoint(this.View.Frame.X, this.View.Frame.Y), size);

            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return this.CellHeight;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return this.ObjectList.Count;
        }
    }
}