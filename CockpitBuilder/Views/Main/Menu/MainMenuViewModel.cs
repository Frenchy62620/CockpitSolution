using Caliburn.Micro;
using CockpitBuilder.Common.Strategies;
using CockpitBuilder.Core.Common;
using CockpitBuilder.Core.Persistence;
using CockpitBuilder.Events;
using CockpitBuilder.Result;
using CockpitBuilder.Shells;
using CockpitBuilder.Views.Main.DockPanel;
using CockpitBuilder.Views.Main.DockPanel.Panels;
using System;
using System.Collections.Generic;
using System.Linq;
using IEventAggregator = CockpitBuilder.Core.Common.Events.IEventAggregator;

namespace CockpitBuilder.Views.Main.Menu
{
    public class MainMenuViewModel : PropertyChangedBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IFileSystem fileSystem;
        private readonly Func<MonitorViewModel> scriptEditorFactory;
        private readonly ProfileDialogStrategy profileDialogStrategy;
        private readonly ISettingsManager settingsManager;
        private readonly IResultFactory resultFactory;
        public MainMenuViewModel(IResultFactory resultFactory, 
                                 IEventAggregator eventAggregator, 
                                 IFileSystem fileSystem, 
                                 Func<MonitorViewModel> scriptEditorFactory,
                                 ProfileDialogStrategy profileDialogStrategy,
                                 ISettingsManager settings)
        {
            this.resultFactory = resultFactory;
            this.eventAggregator = eventAggregator;
            this.scriptEditorFactory = scriptEditorFactory;
            this.fileSystem = fileSystem;
            this.profileDialogStrategy = profileDialogStrategy;
        }

        private PanelViewModel activeDocument;
        private PanelViewModel ActiveDocument
        {
            get { return activeDocument; }
            set
            {
                activeDocument = value;
                NotifyOfPropertyChange(() => CanQuickSaveProfile);
                NotifyOfPropertyChange(() => CanSaveProfile);
                //PublishProfileStateChange();
            }
        }

        public void NewProfile()
        {
            CreateProfileViewModel(null);
        }
        public IEnumerable<IResult> OpenProfile()
        {
            return profileDialogStrategy.Open(CreateProfileViewModel);
        }
        public void CreateProfileViewModel(string filePath)
        {
            if (filePath != null && !fileSystem.Exists(filePath)) return;

            var document = scriptEditorFactory()
                .Configure(filePath);

            if (!string.IsNullOrEmpty(filePath))
            {
                //document.LoadFileContent(fileSystem.ReadAllText(filePath));
                AddRecentProfile(filePath);
            }

            eventAggregator.Publish(new ScriptDocumentAddedEvent(document));
        }

        public IEnumerable<IResult> SaveProfile()
        {
            return SaveProfile(ActiveDocument);
        }

        public IEnumerable<IResult> SaveProfile(PanelViewModel document)
        {
            return profileDialogStrategy.SaveAs(document, false, path => Save(document, path));
        }

        private void Save(PanelViewModel document)
        {
            Save(document, document.FilePath);
        }

        private void Save(PanelViewModel document, string filePath)
        {
            document.FilePath = filePath;
            fileSystem.WriteAllText(filePath, document.FileContent);
            document.Saved();

            AddRecentProfile(filePath);
        }

        public bool CanOpenRecentProfile
        {
            get { return RecentProfiles.Any(); }
        }

        public void OpenRecentProfile(RecentFileViewModel model)
        {
            CreateProfileViewModel(model.File);
        }

        private void AddRecentProfile(string filePath)
        {
            settingsManager.Settings.AddRecentProfile(filePath);
            RecentProfiles.Clear();
            RecentProfiles.AddRange(ListRecentFiles());
            NotifyOfPropertyChange(() => CanOpenRecentProfile);
        }

        private IEnumerable<RecentFileViewModel> ListRecentFiles()
        {
            return settingsManager.Settings.RecentProfiles.Select((file, index) => new RecentFileViewModel(file, index));
        }

        public IEnumerable<IResult> QuickSaveProfile()
        {
            if (PathSet)
            {
                Save(activeDocument);
                return null;
            }

            return SaveProfile();
        }

        public bool CanQuickSaveProfile
        {
            get { return CanSaveProfile; }
        }

        public bool PathSet
        {
            get { return !string.IsNullOrEmpty(activeDocument.FilePath); }
        }

        public bool CanSaveProfile
        {
            get { return activeDocument != null; }
        }

        //public void RunScript()
        //{
        //    scriptRunning = true;
        //    PublishScriptStateChange();

        //    currentScriptEngine = scriptEngineFactory();
        //    currentScriptEngine.Start(activeDocument.FileContent);
        //}

        //public void StopScript()
        //{
        //    scriptRunning = false;
        //    currentScriptEngine.Stop();
        //    PublishScriptStateChange();
        //}

        public IEnumerable<IResult> ShowAbout()
        {
            yield return resultFactory.ShowDialog<AboutViewModel>();
        }

        //private void PublishScriptStateChange()
        //{
        //    NotifyOfPropertyChange(() => CanRunScript);
        //    NotifyOfPropertyChange(() => CanStopScript);
        //    if (activeDocument != null)
        //        eventAggregator.Publish(new ScriptStateChangedEvent(scriptRunning, activeDocument.Filename));
        //    else
        //        eventAggregator.Publish(new ScriptStateChangedEvent(scriptRunning, null));
        //}

        public void Handle(ActiveScriptDocumentChangedEvent message)
        {
            ActiveDocument = message.Document;
        }


        public IObservableCollection<RecentFileViewModel> RecentProfiles { get; set; }

    }
}
