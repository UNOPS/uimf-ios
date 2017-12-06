namespace IOSApp.Controller
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading.Tasks;
    using IOSUiMetadataFramework.Core;
    using IOSUiMetadataFramework.Core.EventHandlers;
    using IOSUiMetadataFramework.Core.Inputs;
    using IOSUiMetadataFramework.Core.Managers;
    using IOSUiMetadataFramework.Core.Model;
    using IOSUiMetadataFramework.Core.Outputs;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using UiMetadataFramework.Core;
    using UIKit;

    public partial class SideMenuController : BaseController
    {
        public SideMenuController() : base("SideMenuController", null)
        {
        }

        public Dictionary<string, FormMetadata> AllForms { get; set; }

        public UiMetadataWebApi UiMetadataWebApi { get; set; }
        public CustomFormWrapper Wrapper { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var contentController = new ViewController();
            this.View.BackgroundColor = UIColor.FromRGBA(.9f, .9f, .9f, .9f);

            this.UiMetadataWebApi = new UiMetadataWebApi
            {
                FormMetadataUrl = "http://10.70.0.245:801/api/form/metadata",
                MetadataUrl = "http://10.70.0.245:801/api/form/metadata",
                RunFormUrl = "http://10.70.0.245:801/api/form/run"
            };
            this.RegisterManagers();
            this.Wrapper = new CustomFormWrapper(this.NavController);
            this.MyFormHandler = new MyFormHandler(this.Wrapper, this.UiMetadataWebApi, this.InputManager, this.OutputManager, this.EventManager,this.AllForms);
            this.GetAllFormsMetadata();

            //introButton.TouchUpInside += (sender, e) =>
            //{
           // SidebarController.ChangeContentView(new IntroController());
            //};

        }

        public MyFormHandler MyFormHandler { get; set; }
        private InputManagerCollection InputManager { get; set; }
        private OutputManagerCollection OutputManager { get; set; }
        public EventHandlerManagerCollection EventManager { get; set; }

        private void GetAllFormsMetadata()
        {
            this.AllForms = new Dictionary<string, FormMetadata>();
            //var appPreference = new AppSharedPreference(Application.Context);
            var result = Task.Run(
                () => UiMetadataHttpRequestHelper.GetAllFormsMetadata(this.UiMetadataWebApi.MetadataUrl, ""));
            var metadata = JsonConvert.DeserializeObject<MyForms>(result.Result);

            //this.MenuItems = new List<object>();
            // this.MenuTitles = new List<string>();
            var y = 50;
            foreach (var menuItem in metadata.Menus)
            {
                if (!string.IsNullOrEmpty(menuItem.Name))
                {
                    var existingForms = false;
                    foreach (var form in metadata.Forms)
                    {
                        if (form.CustomProperties != null)
                        {
                            var customeProperties = (JObject)form.CustomProperties;
                            var menuName = customeProperties.GetValue("menu").ToString();
                            if (menuName.Equals(menuItem.Name))
                            {
                                if (!existingForms)
                                {
                                    var menuTitle = new UILabel(new RectangleF(15, y, 270, 20))
                                    {
                                        Font = UIFont.SystemFontOfSize(18.0f),
                                        TextAlignment = UITextAlignment.Left,
                                        TextColor = UIColor.Black,
                                        Text = menuItem.Name
                                    };
                                    this.View.Add(menuTitle);
                                    y += 55;
                                }
                                existingForms = true;
                               
                                var contentButton = new UIButton(UIButtonType.System) { Frame = new RectangleF(30, y, 260, 20) };
                                contentButton.SetTitle(form.Label, UIControlState.Normal);
                                contentButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;

                                //introButton.TouchUpInside += (sender, e) =>
                                //{
                                 //SidebarController.ChangeContentView(new IntroController());
                                //};

                                contentButton.TouchUpInside += async (o, e) =>
                                {
                                    await this.Wrapper.UpdateViewAsync(this.MyFormHandler, form);
                                   
                                    this.SidebarController.CloseMenu();
                                   // this.SidebarController.ChangeContentView(new IntroController());
                                };

                                this.View.Add(contentButton);
                                y += 50;
                            }
                        }
                        if (!this.AllForms.ContainsKey(form.Id))
                        {
                            this.AllForms.Add(form.Id, form);
                        }
                    }
                }
            }
        }

        private void RegisterManagers()
        {
            this.InputManager = new InputManagerCollection();
            this.InputManager.RegisterAssembly(typeof(TextInput).Assembly);

            this.OutputManager = new OutputManagerCollection();
            this.OutputManager.RegisterAssembly(typeof(TextOutput).Assembly);
            this.OutputManager.RegisterAssembly(typeof(InstallmentList).Assembly);

            this.EventManager = new EventHandlerManagerCollection();
            this.EventManager.RegisterAssembly(typeof(BindToOutputEventHandler).Assembly);
        }
    }
}