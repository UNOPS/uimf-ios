namespace IOSUiMetadataFramework.Core.Inputs
{
    using System;
    using System.Collections.Generic;
    using IOSUiMetadataFramework.Core.Attributes;
    using IOSUiMetadataFramework.Core.Managers;
    using IOSUiMetadataFramework.Core.Model;
    using UiMetadataFramework.Basic.Input;
    using UIKit;

    [Input(Type = "paginator")]
    public class PaginatorInput : IInputManager
    {
        private UITextField Ascending { get; set; }
        private UIView InputPaginator { get; set; }
        private UITextField OrderBy { get; set; }
        private UITextField PageIndex { get; set; }
        private UITextField PageSize { get; set; }

        public UIView GetView(IDictionary<string, object> inputCustomProperties, MyFormHandler myFormHandler)
        {
            this.Ascending = new UITextField();
            this.OrderBy = new UITextField();
            this.PageIndex = new UITextField();
            this.PageSize = new UITextField();
            this.InputPaginator = new UIView();
            this.InputPaginator.AddSubviews(this.Ascending, this.OrderBy, this.PageIndex, this.PageSize);
            return this.InputPaginator;
        }

        public object GetValue()
        {
            return new Paginator
            {
                Ascending = !string.IsNullOrEmpty(this.Ascending.Text) && Convert.ToBoolean(this.Ascending.Text),
                OrderBy = this.OrderBy.Text,
                PageIndex = !string.IsNullOrEmpty(this.PageIndex.Text) ? Convert.ToInt32(this.PageIndex.Text) : 1,
                PageSize = !string.IsNullOrEmpty(this.PageSize.Text) ? Convert.ToInt32(this.PageSize.Text) : 10
            };
        }

        public void SetValue(object value)
        {
            Paginator paginator = value.CastTObject<Paginator>();

            this.Ascending.Text = paginator.Ascending?.ToString();
            this.OrderBy.Text = paginator.OrderBy;
            this.PageIndex.Text = paginator.PageIndex?.ToString();
            this.PageSize.Text = paginator.PageSize?.ToString();
        }
    }
}