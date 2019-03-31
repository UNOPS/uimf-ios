namespace IOSApp.Controller
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;
    using Foundation;
    using IOSUiMetadataFramework.Core;
    using IOSUiMetadataFramework.Core.EventHandlers;
    using IOSUiMetadataFramework.Core.Inputs;
    using IOSUiMetadataFramework.Core.Managers;
    using IOSUiMetadataFramework.Core.Model;
    using IOSUiMetadataFramework.Core.Outputs;
    using Newtonsoft.Json;
    using UiMetadataFramework.Core;
    using UIKit;

    public partial class SideMenuController : BaseController
    {
        public SideMenuController() : base("SideMenuController", null)
        {
        }

        public Dictionary<string, FormMetadata> AllForms { get; set; } = new Dictionary<string, FormMetadata>();
        public EventHandlerManagerCollection EventManager { get; set; }

        public List<FormMetadata> MenuItems { get; set; }

        public MyFormHandler MyFormHandler { get; set; }
        public UIScrollView ScrollView { get; set; }
        public UiMetadataWebApi UiMetadataWebApi { get; set; }
        public CustomFormWrapper Wrapper { get; set; }
        private InputManagerCollection InputManager { get; set; }
        private OutputManagerCollection OutputManager { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.View.BackgroundColor = UIColor.FromRGBA(.9f, .9f, .9f, .9f);
            this.ScrollView = new UIScrollView ();
            this.View = this.ScrollView;
            this.UiMetadataWebApi = new UiMetadataWebApi
            {
                FormMetadataUrl = "http://grants-management-system.azurewebsites.net/api/form/metadata",
                MetadataUrl = "http://grants-management-system.azurewebsites.net/api/form/metadata",
                RunFormUrl = "http://grants-management-system.azurewebsites.net/api/form/run"
            };
            this.RegisterManagers();
            this.Wrapper = new CustomFormWrapper(this, this.NavController);
            var managers = new ManagersCollection
            {
                InputManagerCollection = this.InputManager,
                OutputManagerCollection = this.OutputManager,
                EventHandlerManagerCollection = this.EventManager
            };
            this.MyFormHandler = new MyFormHandler(this.Wrapper, this.UiMetadataWebApi, managers, this.AllForms);
            this.GetAllFormsMetadata();

            //introButton.TouchUpInside += (sender, e) =>
            //{
            // SidebarController.ChangeContentView(new IntroController());
            //};
        }

        private void GetAllFormsMetadata()
        {
            this.AllForms = new Dictionary<string, FormMetadata>();
            var userDefaults = NSUserDefaults.StandardUserDefaults;
            this.MenuItems = new List<FormMetadata>();

            try
            {
                var result = Task.Run(
              () => UiMetadataHttpRequestHelper.GetAllFormsMetadata(this.UiMetadataWebApi.MetadataUrl, userDefaults.StringForKey("Cookies")));
                var metadata = JsonConvert.DeserializeObject<MyForms>(result.Result);
                var orderedMenu = metadata.Menus.OrderBy(a => a.OrderIndex);
                var orderedForms = metadata.Forms
                    .OrderBy(a => a.CustomProperties != null ? a.CustomProperties?.GetCustomProperty<long>("menuOrderIndex") : 0)
                    .ToList();

                var y = 50;

                foreach (var menuItem in orderedMenu)
                {
                    var existingForms = false;
                    foreach (var form in orderedForms)
                    {
                        if (form.CustomProperties != null && this.MenuItems.All(a => a != form))
                        {
                            var menuName = form.CustomProperties.GetCustomProperty<string>("menu");
                            if (!string.IsNullOrEmpty(menuItem.Name))
                            {
                                if (menuName != null && (menuName.Equals(menuItem.Name) || menuName == ""))
                                {
                                    if (!existingForms && menuName != "")
                                    {
                                        var menuTitle = new UILabel(new RectangleF(15, y, 270, 20))
                                        {
                                            Font = UIFont.SystemFontOfSize(18.0f),
                                            TextAlignment = UITextAlignment.Left,
                                            TextColor = UIColor.Black,
                                            Text = menuItem.Name
                                        };
                                        this.ScrollView.Add(menuTitle);
                                        y += 55;
                                    }
                                    existingForms = true;
                                    this.MenuItems.Add(form);
                                    var contentButton = new UIButton(UIButtonType.System) { Frame = new RectangleF(30, y, 260, 20) };
                                    contentButton.SetTitle(form.Label, UIControlState.Normal);
                                    contentButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;

                                    contentButton.TouchUpInside += (o, e) =>
                                    {

                                        this.Wrapper.UpdateView(this.MyFormHandler, new FormParameter(form));

                                        this.SidebarController.CloseMenu();
                                        // this.SidebarController.ChangeContentView(new IntroController());
                                    };

                                    this.ScrollView.Add(contentButton);
                                    y += 50;
                                }
                            }
                        }
                        if (!this.AllForms.ContainsKey(form.Id))
                        {
                            this.AllForms.Add(form.Id, form);
                        }

                        if (!this.MyFormHandler.AllFormsMetadata.ContainsKey(form.Id))
                        {
                            this.MyFormHandler.AllFormsMetadata.Add(form.Id, form);
                        }
                    }
                }
                var frame = this.ScrollView.Frame;
                frame.Height = y + 50;
                this.ScrollView.ContentSize = frame.Size;
            }
            catch (System.Exception)
            {
                this.MyFormHandler.ShowToast("Server is not available in this moment");
            }
          
        }

        public Dictionary<string, FormMetadata> Reload()
        {
            this.ScrollView.RemoveAllViews();
            this.GetAllFormsMetadata();          
            return this.AllForms;
        }

        private void RegisterManagers()
        {
            this.InputManager = new InputManagerCollection();
            this.InputManager.RegisterAssembly(typeof(TextInput).Assembly);

            this.OutputManager = new OutputManagerCollection();
            this.OutputManager.RegisterAssembly(typeof(TextOutput).Assembly);

            this.EventManager = new EventHandlerManagerCollection();
            this.EventManager.RegisterAssembly(typeof(BindToOutputEventHandler).Assembly);
        }
    }
}