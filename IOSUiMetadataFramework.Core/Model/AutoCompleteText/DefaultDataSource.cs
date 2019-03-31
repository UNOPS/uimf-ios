namespace IOSUiMetadataFramework.Core.Model.AutoCompleteText
{
    using System.Collections.Generic;
    using System.Linq;
    using Foundation;
    using UIKit;

    public class DefaultDataSource : AutoCompleteViewSource
    {
        private readonly string _cellIdentifier = "DefaultIdentifier";
        private ICollection<string> DefaultSuggestions { get; set; }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(this._cellIdentifier);
            string item = this.DefaultSuggestions.ElementAt(indexPath.Row);

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, this._cellIdentifier);
            }

            cell.BackgroundColor = UIColor.GroupTableViewBackgroundColor;
            cell.TextLabel.Text = item;

            return cell;
        }

        public override void NewSuggestions(ICollection<string> suggestions)
        {
            this.DefaultSuggestions = suggestions;
        }

        //public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        //{
        //    base.RowSelected(tableView, indexPath);
        //}
    }
}