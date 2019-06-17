using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Caliburn.Micro;
using CockpitBuilder.Common.AvalonDock;
using CockpitBuilder.Core.Common;
using CockpitBuilder.Core.Persistence;
using CockpitBuilder.Core.Persistence.Paths;
using CockpitBuilder.Events;
using CockpitBuilder.Result;
using CockpitBuilder.Views.Main.DockPanel;
using CockpitBuilder.Views.Main.DockPanel.Panels;
using CockpitBuilder.Views.Main.Menu;
using CockpitBuilder.Views.Main.ToolBarTray;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using IEventAggregator = CockpitBuilder.Core.Common.Events.IEventAggregator;

namespace CockpitBuilder.Shells
{
    public class MainShellViewModel : ShellPresentationModel, Core.Common.Events.IHandle<ExitingEvent>, Core.Common.Events.IHandle<ScriptDocumentAddedEvent>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IPersistanceManager persistanceManager;
        private readonly ISettingsManager settingsManager;
        private readonly IFileSystem fileSystem;
        private readonly IPaths paths;

        private const string dockingConfig = "layout.config";
        public MainShellViewModel(IResultFactory resultFactory,
                                  IEventAggregator eventAggregator,
                                  MainMenuViewModel mainMenuViewModel,
                                  MainToolBarTrayViewModel mainToolBarViewModel,
                                  IEnumerable<PanelViewModel> panels,
                                  ISettingsManager settingsManager,
                                  IFileSystem fileSystem,
                                  IPersistanceManager persistanceManager,
                                  IPaths paths,
                                  IPortable portable
            )
            : base(resultFactory)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);

            this.persistanceManager = persistanceManager;
            this.settingsManager = settingsManager;
            this.fileSystem = fileSystem;
            this.paths = paths;

            Menu = mainMenuViewModel;
            ToolBarTray = mainToolBarViewModel;

            Scripts = new BindableCollection<MonitorViewModel>();
            Tools = new BindableCollection<PanelViewModel>(panels);

           mainMenuViewModel.NewProfile();
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            InitDocking();
            //parser.ParseAndExecute();
            //eventAggregator.Publish(new StartedEvent());
        }

        private void InitDocking()
        {
            var path = paths.GetDataPath(dockingConfig);

            if (!fileSystem.Exists(path)) return;
            try
            {
                var layoutSerializer = new XmlLayoutSerializer(DockingManager);
                layoutSerializer.Deserialize(path);
            }
            catch
            {
                fileSystem.Delete(path);
            }
        }

        //public IEnumerable<IResult> DocumentClosing(ScriptEditorViewModel document, DocumentClosingEventArgs e)
        //{
        //    return Result.Coroutinify(HandleScriptClosing(document), () => e.Cancel = true);
        //}

        //public void DocumentClosed(ScriptEditorViewModel document)
        //{
        //    Scripts.Remove(document);
        //}

        private IEnumerable<IResult> HandleScriptClosing(bool v = false)
        {
            if (v)
            {
                var message = Result.ShowMessageBox("", "", MessageBoxButton.YesNoCancel);
                yield return message;
            }
        }
        protected override IEnumerable<IResult> CanClose()
        {
            var r = HandleScriptClosing(false);
            foreach (var result in r)
            {
                yield return result;
            }

            eventAggregator.Publish(new ExitingEvent());


        }
        private DockingManager DockingManager
        {
            get { return (this.GetView() as IDockingManagerSource).DockingManager; }
        }

        private PanelViewModel activeDocument;
        public PanelViewModel ActiveDocument
        {
            get { return activeDocument; }
            set
            {

                if (value == null || value.IsFileContent)
                {
                    activeDocument = value;
                    NotifyOfPropertyChange(() => ActiveDocument);
                    eventAggregator.Publish(new ActiveScriptDocumentChangedEvent(value));
                }
            }
        }
        public MainMenuViewModel Menu { get; set; }
        public MainToolBarTrayViewModel ToolBarTray { get; set; }

        public BindableCollection<MonitorViewModel> Scripts { get; set; }
        public BindableCollection<PanelViewModel> Tools { get; set; }


        public void Handle(ScriptDocumentAddedEvent message)
        {
            if (message.toDelete)
            {
                Scripts.Remove(message.Document);
                return;
            }

            var script = Scripts.FirstOrDefault(s => s.ContentId == message.Document.ContentId);

            if (script == null)
            {
                script = message.Document;
                Scripts.Add(script);
            }

            script.IsActive = true;

            ActiveDocument = message.Document;
        }

        public void Handle(ExitingEvent message)
        {
            persistanceManager.Save();
            var layoutSerializer = new XmlLayoutSerializer(DockingManager);
            layoutSerializer.Serialize(paths.GetDataPath(dockingConfig));
        }
    }
}
