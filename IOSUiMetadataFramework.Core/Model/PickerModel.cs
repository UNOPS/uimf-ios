namespace IOSUiMetadataFramework.Core.Model
{
    using System;
    using System.Collections.Generic;
    using UIKit;

    public class PickerModel : UIPickerViewModel
    {
        private readonly IList<string> values;

        public PickerModel(IList<string> values)
        {
            this.values = values;
        }

        public override nint GetComponentCount(UIPickerView picker)
        {
            return 1;
        }

        public override nfloat GetRowHeight(UIPickerView picker, nint component)
        {
            return 40f;
        }

        public override nint GetRowsInComponent(UIPickerView picker, nint component)
        {
            return this.values.Count;
        }

        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {
            return this.values[Convert.ToInt32(row)];
        }

        public event EventHandler<PickerChangedEventArgs> PickerChanged;

        public override void Selected(UIPickerView picker, nint row, nint component)
        {
            this.PickerChanged?.Invoke(this, new PickerChangedEventArgs { SelectedValue = this.values[Convert.ToInt32(row)] });
        }
    }

    public class PickerChangedEventArgs : EventArgs
    {
        public string SelectedValue { get; set; }
    }
}