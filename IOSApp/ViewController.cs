namespace IOSApp
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using App.Core;
	using Autofac;
	using IOSUiMetadataFramework.Core;
	using IOSUiMetadataFramework.Core.Inputs;
	using IOSUiMetadataFramework.Core.Managers;
	using IOSUiMetadataFramework.Core.Outputs;
	using MediatR;
	using MediatR.Pipeline;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;
	using UiMetadataFramework.MediatR;
	using UIKit;

	public partial class ViewController : UIViewController
	{
		public List<Layout> AppLayouts = new List<Layout>();

		public ViewController(IntPtr handle) : base(handle)
		{
		}

		private IContainer Container { get; set; }
		private FormRegister FormRegister { get; set; }
		private FormView FormView { get; set; }
		private InputManagerCollection InputManager { get; set; }
		private OutputManagerCollection OutputManager { get; set; }

		public FormRegister GetRegisterForms()
		{
			try
			{
				var binder = new MetadataBinder();
				binder.RegisterAssembly(typeof(StringOutputFieldBinding).GetTypeInfo().Assembly);
				binder.RegisterAssembly(typeof(OutputFieldBinding).GetTypeInfo().Assembly);
				this.FormRegister = new FormRegister(binder);
				this.FormRegister.RegisterAssembly(typeof(DoMagic).GetTypeInfo().Assembly);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return this.FormRegister;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.RegisterInputOutputManagers();
			this.ConfigureContainer();
			this.FormRegister = this.Container.Resolve<FormRegister>();
			UIView formsView = new UIView();

			this.DrawMainScreen(formsView);			

			this.FormView = new FormView(formsView, this.Container.Resolve<IMediator>(), this.FormRegister, this.InputManager, this.OutputManager,
				this.AppLayouts);
			
			var backButton = new UIBarButtonItem("Back", UIBarButtonItemStyle.Plain, (sender, args) =>
			{
				if (this.AppLayouts.Count > 1)
				{
					this.AppLayouts.RemoveAt(this.AppLayouts.Count - 1);
					var layout = this.AppLayouts[this.AppLayouts.Count - 1];
					formsView.RemoveAllViews();
					formsView.AddSubview(layout.View);
					if (layout.Constraints != null)
					{
						formsView.AddConstraints(layout.Constraints.ToArray());
					}
				}
			});

			this.NavBar.LeftBarButtonItem = backButton;
		}

		private void DrawMainScreen(UIView formsView)
		{
			UIView mainView = new UIView();
			var btn = new UIButton();
			btn.SetTitle("Do Magic", UIControlState.Normal);
			btn.BackgroundColor = UIColor.Gray;
			btn.TouchUpInside += async (sender, args) => { await this.FormView.StartIForm(typeof(DoMagic)); };
			this.View.AddSubview(formsView);
			formsView.AddSubview(mainView);
			mainView.AddSubview(btn);

			this.View.AddConstraints(formsView.FillParent(this.View, 70).ToArray());
			var constraints = mainView.FillParent(formsView);
			formsView.AddConstraints(constraints);
			btn.FillWidth(mainView, 40, 10, 10, -10);
			this.AppLayouts.Add(new Layout(mainView, constraints));
		}

		private void ConfigureContainer()
		{
			var builder = new ContainerBuilder();

			builder.Register<SingleInstanceFactory>(ctx =>
			{
				var c = ctx.Resolve<IComponentContext>();
				return t => c.Resolve(t);
			}).InstancePerLifetimeScope();

			builder.Register<MultiInstanceFactory>(ctx =>
			{
				var c = ctx.Resolve<IComponentContext>();
				return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
			}).InstancePerLifetimeScope();

			builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();

			builder
				.Register(context => this.GetRegisterForms())
				.InstancePerLifetimeScope();

			var openHandlersTypes = new[]
			{
				typeof(IRequestHandler<,>),
				typeof(IAsyncRequestHandler<,>),
				typeof(ICancellableAsyncRequestHandler<,>),
				typeof(INotificationHandler<>),
				typeof(IAsyncNotificationHandler<>),
				typeof(ICancellableAsyncNotificationHandler<>)
			};

			var myAssemblies = new[]
			{
				typeof(DoMagic).Assembly,
				typeof(InvokeForm).Assembly
			};

			foreach (var openHandlerType in openHandlersTypes)
			{
				foreach (var myAssembly in myAssemblies)
				{
					builder
						.RegisterAssemblyTypes(myAssembly)
						.AsClosedTypesOf(openHandlerType)
						.InstancePerDependency();
				}
			}

			builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
			builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

			this.Container = builder.Build();
		}

		private void RegisterInputOutputManagers()
		{
			this.InputManager = new InputManagerCollection();
			this.InputManager.RegisterAssembly(typeof(TextInput).Assembly);
			this.InputManager.RegisterAssembly(typeof(NumericInput).Assembly);
			this.InputManager.RegisterAssembly(typeof(DateInput).Assembly);

			this.OutputManager = new OutputManagerCollection();
			this.OutputManager.RegisterAssembly(typeof(TextOutput).Assembly);
			this.OutputManager.RegisterAssembly(typeof(NumericOutput).Assembly);
			this.OutputManager.RegisterAssembly(typeof(DateOutput).Assembly);
			this.OutputManager.RegisterAssembly(typeof(TableOutput).Assembly);
		}
	}
}