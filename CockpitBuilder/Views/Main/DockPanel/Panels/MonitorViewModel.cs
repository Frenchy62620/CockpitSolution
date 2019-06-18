using Caliburn.Micro;
using CockpitBuilder.Common.CustomControls;
using CockpitBuilder.Common.PropertyEditors;
using CockpitBuilder.Core.Common;
using CockpitBuilder.Events;
using CockpitBuilder.Plugins;
using CockpitBuilder.Views.Main.DockPanel.Models;
using GongSolutions.Wpf.DragDrop;
using Ninject;
using Ninject.Parameters;
using Ninject.Syntax;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using IEventAggregator = CockpitBuilder.Core.Common.Events.IEventAggregator;

namespace CockpitBuilder.Views.Main.DockPanel.Panels
{
    public class MonitorViewModel : PanelViewModel, 
                                    Core.Common.Events.IHandle<BackgroundColorEvent>, 
                                    Core.Common.Events.IHandle<BackgroundImageEvent>,
                                    IDropTarget
    {
        
        public double ZoomFactor;
        private readonly IEventAggregator eventAggregator;
        private readonly IResolutionRoot resolutionRoot;
        private readonly FileSystem fileSystem;

        private MonitorView monitorview;
        private readonly SolidColorBrush color1 = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush color2 = new SolidColorBrush(Colors.LightGray);
        public int Tag = -1;
        private ToolBoxItem tbi;
        public MonitorViewModel(IEventAggregator eventAggregator, IResolutionRoot resolutionRoot, FileSystem fileSystem)
        {
            this.eventAggregator = eventAggregator;
            this.resolutionRoot = resolutionRoot;

            this.fileSystem = fileSystem;
            BackgroundColor1 = color1;
            BackgroundColor2 = color2;
            this.eventAggregator.Publish(new MonitorViewStartedEvent(this));
            this.eventAggregator.Subscribe(this);
            MyCockpitViewModels = new ObservableCollection<PluginModel>();

        }
        private static int indice = 0;
        private static int untitledIndex;
        private int untitledId;

        ~MonitorViewModel()
        {
            MessageBox.Show("MonitorView stop","Important Question", MessageBoxButton.YesNo);
        }

        public MonitorViewModel Configure(string filePath)
        {
            FilePath = filePath;
            if (string.IsNullOrEmpty(filePath))
            {
                untitledId = untitledIndex++;
            }

            return this;
        }
        protected override void OnViewLoaded(object view)
        {
            monitorview = view as MonitorView;
        }

        #region Background Image and Color
        private SolidColorBrush backgroundColor1;
        public SolidColorBrush BackgroundColor1
        {
            get { return backgroundColor1; }
            set
            {
                backgroundColor1 = value;
                NotifyOfPropertyChange(() => BackgroundColor1);
            }
        }
        private SolidColorBrush backgroundColor2;
        public SolidColorBrush BackgroundColor2
        {
            get { return backgroundColor2; }
            set
            {
                backgroundColor2 = value;
                NotifyOfPropertyChange(() => BackgroundColor2);
            }
        }

        private string backgroundimage;
        public string BackgroundImage
        {
            get { return backgroundimage; }
            set
            {
                backgroundimage = value;
                NotifyOfPropertyChange(() => BackgroundImage);
            }
        }
        #endregion

        private string fullimage;
        public string FullImage
        {
            get { return fullimage; }
            set
            {
                fullimage = value;
                NotifyOfPropertyChange(() => FullImage);
            }
        }

        private string script;
        public string Script
        {
            get { return script; }
            set
            {
                script = value;
                NotifyOfPropertyChange(() => Script);
            }
        }
        public override string Filename
        {
            get
            {
                if (!string.IsNullOrEmpty(FilePath))
                    return fileSystem.GetFilename(FilePath);

                var untitledPostfix = untitledId > 0 ? string.Format("-{0}", untitledId) : null;
                
                return string.Format("Untitled{0}.py", untitledPostfix);
            }
        }
        public override string Title
        {
            get { return Filename; }
        }

        public override string ContentId
        {
            get { return FilePath ?? Filename; }
        }

        public void Handle(BackgroundImageEvent message)
        {
            BackgroundImage =  message.FullImageName;
        }

        public void Handle(BackgroundColorEvent message)
        {
            BackgroundColor1 = message.resetcolor ? color1 : message.backgroundcolor;
            BackgroundColor2 = message.resetcolor ? color2 : message.backgroundcolor;
        }

        public void Handle(ToolBoxItem message)
        {
            this.tbi = message;
        }
        public void MouseLeftButtonDownOnMonitorView(IInputElement elem, Point pos, MouseEventArgs e)
        {
            UnselectAll();
            eventAggregator.Publish(new DisplayPropertiesViewEvent(new[] { "CockpitBuilder.Common.PropertyEditors.MonitorPropertyEditorViewModel" }));
        }

        public void ContentControlLoaded(object sender)
        {
            var s = sender as ContentControl;
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(s);

            if (adornerLayer != null)
            {
                MyAdorner myAdorner = new MyAdorner(s);
                adornerLayer.Add(myAdorner);
            }
        }

        public void MouseLeftButtonDownOnContentControl(object sender)
        {
            var s = sender as ContentControl;
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(s);

            if (adornerLayer != null)
            {
                Adorner[] adorners = adornerLayer.GetAdorners(s);
                if (adorners != null)
                {
                    foreach (Adorner adorner in adorners)
                    {
                        if (typeof(MyAdorner).IsAssignableFrom(adorner.GetType()))
                        {
                            adornerLayer.Remove(adorner);
                        }
                    }
                    return;
                }
            }

            MyAdorner myAdorner = new MyAdorner(s);
            adornerLayer.Add(myAdorner);
        }

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is ToolBoxGroup /*&& dropInfo.TargetItem is MonitorViewModel*/)
            {
                var tbg = (dropInfo.Data as ToolBoxGroup).SelectedToolBoxItem;
                FullImage = (dropInfo.Data as ToolBoxGroup).SelectedToolBoxItem.FullImageName;
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Move;
                //System.Diagnostics.Debug.WriteLine($"X, Y = {dropInfo.DropPosition.X}, {dropInfo.DropPosition.Y}  W,H = {tbg.ImageWidth}, {tbg.ImageHeight}");
            }
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            var tbg = dropInfo.Data as ToolBoxGroup;

            int left = (int)dropInfo.DropPosition.X - (int)(tbg.SelectedToolBoxItem.ImageWidth / 2);
            int top = (int)dropInfo.DropPosition.Y - (int)(tbg.SelectedToolBoxItem.ImageHeight / 2);
            FullImage = (dropInfo.Data as ToolBoxGroup).SelectedToolBoxItem.FullImageName;
            var num = MyCockpitViewModels.Count;
            var nameUC = tbg.SelectedToolBoxItem.ShortImageName;

            if (MyCockpitViewModels.ToList().Select(t => t.NameUC.Equals(nameUC)) != null)
            {
                var r  = MyCockpitViewModels.ToList().Select(t => t.NameUC.Contains(nameUC)).Count();
                nameUC = $"{nameUC}_{r}"; 
            }

            Ninject.Parameters.Parameter[] param;

            Tag = Tag + 1;

            Ninject.Parameters.Parameter[] paramproperties;
            string[] properties;
            string model;
            if (FullImage.Contains("mfd"))
            {
                var FullImage1 = FullImage.Replace("_0.png", "_1.png");
                param = new Ninject.Parameters.Parameter[]
                {
                new ConstructorArgument("settings", new object[]{
                            $"{tbg.SelectedToolBoxItem.ShortImageName}", left, top, 0,
                             FullImage,
                             FullImage1,
                            tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight,
                            0, 1d, 0, 3 }, true)
                };

                model = "CockpitBuilder.Plugins.General.PushButton_ViewModel";
                properties = new string[] { "CockpitBuilder.Common.PropertyEditors.LayoutPropertyEditorViewModel",
                                            "CockpitBuilder.Common.PropertyEditors.PushButtonAppearanceEditorViewModel",
                                            "CockpitBuilder.Common.PropertyEditors.PushButtonBehaviorEditorViewModel"};


                paramproperties = new Ninject.Parameters.Parameter[]
                {
                    // Layout
                    new ConstructorArgument("settings", new object[]{
                        $"{tbg.SelectedToolBoxItem.ShortImageName}",                                                            // name of UC
                        new double[]{ left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight }, 0,  // Left, Top, Width, Height, Angle
                        tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, 0,                             
                        0, 1d, 0, 3 }, true),
                    // Appearance
                    new ConstructorArgument("settings", new object[]{
                        $"{tbg.SelectedToolBoxItem.ShortImageName}",                                                            // name of UC
                        new string[]{ FullImage, FullImage1 }, 0,                                                               // images, start image position
                        new TextFormat() }, true),
                    // Behavior
                    new ConstructorArgument("settings", new object[]{
                        $"{tbg.SelectedToolBoxItem.ShortImageName}",                                                            // name of UC

                        0, 1d, 0, 3 }, true)
                };


            }
            else
            {
                var FullImage1 = FullImage.Replace("_0.png", "_1.png");
                var FullImage2 = FullImage.Replace("_0.png", "_2.png");

                param = new Ninject.Parameters.Parameter[]
                {
                new ConstructorArgument("settings", new object[]{
                            $"{tbg.SelectedToolBoxItem.ShortImageName}", left, top, Tag * 90,
                             FullImage,
                             FullImage1,
                             FullImage2,
                            tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight,
                            2, 1d, 2, 3 }, true)
                };

                model = "CockpitBuilder.Plugins.General.Switch1_ViewModel";
                properties = new string[] { "", "", "" };
            }

            var typeClass = Type.GetType(model);
            var viewmodel = resolutionRoot.TryGet(typeClass, param);
            var view = ViewLocator.LocateForModel(viewmodel, null, null);
            ViewModelBinder.Bind(viewmodel, view, null);
            var v = viewmodel as PluginModel;
            v.ZoomFactor = ZoomFactor;
            v.IsUCSelected = true;
            UnselectAll();
            MyCockpitViewModels.Add((PluginModel)viewmodel);
            eventAggregator.Publish(new DragSelectedItemEvent(tbg.SelectedToolBoxItem));

            foreach(var p in properties)
            {

            }


            //eventAggregator.Publish(new DisplayPropertiesView1Event(new[] { (PropertyEditorModel)layout, appearance, behavior }));
        }


        //private ObservableCollection<PluginModel> _myCockpitViewModels;
        public ObservableCollection<PluginModel> MyCockpitViewModels { get; set; }
        //{
        //    get { return _myCockpitViewModels; }
        //    set
        //    {
        //        _myCockpitViewModels = value;
        //        NotifyOfPropertyChange(() => MyCockpitViewModels);
        //    }
        //}

        public void MouseEnter()
        {
            foreach(var p in MyCockpitViewModels)
            {
                p.IsClickCommingFromMonitorViewModel = true;
            }
        }
        public void MouseLeave()
        {
            foreach (var p in MyCockpitViewModels)
            {
                p.IsClickCommingFromMonitorViewModel = false;
            }
        }

        public void UnselectAll()
        {
            foreach (var p in MyCockpitViewModels)
            {
                p.IsUCSelected = false;
            }

            ProcessElement((DependencyObject)GetView());
            void ProcessElement(DependencyObject element)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
                {
                    Visual childVisual = (Visual)VisualTreeHelper.GetChild(element, i);
                    var t = childVisual.GetType().Name;
                    if (t.Contains("ContentControl"))
                    {
                        var s = childVisual as ContentControl;
                        AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(s);

                        if (adornerLayer != null)
                        {
                            Adorner[] adorners = adornerLayer.GetAdorners(s);
                            if (adorners != null)
                            {
                                foreach (Adorner adorner in adorners)
                                {
                                    if (typeof(MyAdorner).IsAssignableFrom(adorner.GetType()))
                                    {
                                        adornerLayer.Remove(adorner);
                                    }
                                }
                                return;
                            }
                        }
                    }

                    //var childContentVisual = childVisual as ContentControl;
                    //if (childContentVisual != null)
                    //{
                    //    var content = childContentVisual.Content;
                    //}
                    ProcessElement(childVisual);
                }
            }
        }

        public Type DataType => typeof(MonitorViewModel);

        //public void Handle(DragSelectedItemEvent message)
        //{
        //    ProcessElement((DependencyObject)GetView());
        //    void ProcessElement(DependencyObject element)
        //    {
        //        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
        //        {

        //            Visual childVisual = (Visual)VisualTreeHelper.GetChild(element, i);
        //            var t = childVisual.GetType().Name;
        //            if (t.Contains("ContentControl"))
        //            {

        //                var s = childVisual as ContentControl;
        //                System.Diagnostics.Debug.WriteLine($"{i} -> {t}  {s.Content}");
        //                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(s);

        //                if (adornerLayer != null)
        //                {
        //                    Adorner[] adorners = adornerLayer.GetAdorners(s);
        //                    if (adorners != null)
        //                    {
        //                        foreach (Adorner adorner in adorners)
        //                        {
        //                            if (typeof(MyAdorner).IsAssignableFrom(adorner.GetType()))
        //                            {
        //                                adornerLayer.Remove(adorner);
        //                            }
        //                        }
        //                        return;
        //                    }
        //                }

        //            }

        //            //System.Diagnostics.Debug.WriteLine($"{i} -> {t}");
        //            var childContentVisual = childVisual as ContentControl;
        //            if (childContentVisual != null)
        //            {
        //                var content = childContentVisual.Content;
        //            }
        //            ProcessElement(childVisual);
        //        }
        //    }
        //}
    }
}
