using Caliburn.Micro;
using CockpitBuilder.Shells;
using Ninject;
using System;
using System.Collections.Generic;
using System.Windows;
using CockpitBuilder.Core.Services;
using CockpitBuilder.Views.Main.DockPanel;
using CockpitBuilder.Result;
using CockpitBuilder.Views.Main.DockPanel.Panels;
using CockpitBuilder.Views.Main;
using CockpitBuilder.Common.AvalonDock;
using System.Windows.Input;
using CockpitBuilder.Common.Input;

namespace CockpitBuilder.Bootstrapper
{
    public class Bootstrapper : BootstrapperBase
    {
        private IKernel kernel;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            kernel = ServiceBootstrapper.Create();

            kernel.Bind<IWindowManager>().To<WindowManager>().InSingletonScope();
            kernel.Bind<IResultFactory>().To<ResultFactory>();
            kernel.Bind<MainShellViewModel>().ToSelf().InSingletonScope();

            ConfigurePanels();

            SetupCustomMessageBindings();
        }

        private void ConfigurePanels()
        {
            kernel.Bind<PanelViewModel>().To<PreviewTabViewModel>();
            kernel.Bind<PanelViewModel>().To<ProfileTabViewModel>();

            kernel.Bind<PanelViewModel>().To<ToolBoxViewModel>();

            kernel.Bind<PanelViewModel>().To<PropertiesTabViewModel>();
            kernel.Bind<PanelViewModel>().To<LayersTabViewModel>();

            //kernel.Bind<PanelViewModel>().To<MonitorViewModel>();

            kernel.Bind<PanelViewModel>().To<BindingsViewModel>();

        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            Coroutine.BeginExecute(kernel
                .Get<SettingsLoaderViewModel>()
                .Load(OnSettingsLoaded)
                .GetEnumerator());
        }

        protected override object GetInstance(Type service, string key)
        {
            return kernel.Get(service);
        }

        private void OnSettingsLoaded()
        {
            //DisplayRootViewFor<TrayIconViewModel>();
            DisplayRootViewFor<MainShellViewModel>();
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return kernel.GetAll(service);
        }
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            kernel.Get<ILog>().Error(e.ExceptionObject as Exception);
        }

        private void SetupCustomMessageBindings()
        {
            DocumentContext.Init();
            MessageBinder.SpecialValues.Add("$orignalsourcecontext", context =>
            {
                var args = context.EventArgs as RoutedEventArgs;
                if (args == null)
                {
                    return null;
                }

                var fe = args.OriginalSource as FrameworkElement;
                if (fe == null)
                {
                    return null;
                }

                return fe.DataContext;
            });


            MessageBinder.SpecialValues.Add("$mousepoint", ctx =>
            {
                var e = ctx.EventArgs as MouseEventArgs;
                if (e == null)
                    return null;

                return e.GetPosition(ctx.Source);
            });

            MessageBinder.SpecialValues.Add("$pressedkey", (context) =>
            {
                // NOTE: IMPORTANT - you MUST add the dictionary key as lowercase as CM
                // does a ToLower on the param string you add in the action message, in fact ideally
                // all your param messages should be lowercase just in case. I don't really like this
                // behaviour but that's how it is!
                var keyArgs = context.EventArgs as KeyEventArgs;

                if (keyArgs != null)
                    return keyArgs.Key;
                return null;
            });

            MessageBinder.SpecialValues.Add("$pressedmodifierkey", (context) =>
            {
                var keyArgs = context.EventArgs as KeyEventArgs;

                if (keyArgs != null)
                    return keyArgs.KeyboardDevice.Modifiers;
                return null;
            });

        }
        //see https://github.com/Caliburn-Micro/Caliburn.Micro/tree/master/samples/scenarios/Scenario.KeyBinding
        private void SetupCustomTriggerBindings()
        {
            var defaultCreateTrigger = Parser.CreateTrigger;

            Parser.CreateTrigger = (target, triggerText) =>
            {
                if (triggerText == null)
                {
                    return defaultCreateTrigger(target, null);
                }

                var triggerDetail = triggerText
                    .Replace("[", string.Empty)
                    .Replace("]", string.Empty);

                var splits = triggerDetail.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

                switch (splits[0])
                {
                    case "Key":
                        var key = (Key)Enum.Parse(typeof(Key), splits[1], true);
                        return new KeyTrigger { Key = key };

                    case "Gesture":
                        var mkg = (MultiKeyGesture)(new MultiKeyGestureConverter()).ConvertFrom(splits[1]);
                        return new KeyTrigger { Modifiers = mkg.KeySequences[0].Modifiers, Key = mkg.KeySequences[0].Keys[0] };
                }

                return defaultCreateTrigger(target, triggerText);
            };
        }

    }
}

