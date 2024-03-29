﻿using Caliburn.Micro;
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using IEventAggregator = CockpitBuilder.Core.Common.Events.IEventAggregator;

namespace CockpitBuilder.Views.Main.DockPanel.Panels
{
    public class MonitorViewModel : PanelViewModel,
                                    //Core.Common.Events.IHandle<BackgroundColorEvent>,
                                    //Core.Common.Events.IHandle<BackgroundImageEvent>,
                                    IDropTarget, IDropInfo
    {
        
        public double ZoomFactorFromMonitorViewModel;
        public Dictionary<ContentControl, bool > DictContentcontrol = new Dictionary<ContentControl, bool>();

        private readonly IEventAggregator eventAggregator;
        private readonly IResolutionRoot resolutionRoot;
        private readonly FileSystem fileSystem;
        private readonly DisplayManager DisplayManager;
        public  Monitor Monitor { get; private set; }

        private MonitorView monitorview;

        public MonitorPropertyEditorViewModel LayoutMonitor { get; }
        public ContentControl FirstSelected { get; set; } = null;
        public int Tag = -1;
        private ToolBoxItem tbi;
        public MonitorViewModel(IEventAggregator eventAggregator, IResolutionRoot resolutionRoot, FileSystem fileSystem, DisplayManager displayManager)
        {
            this.eventAggregator = eventAggregator;
            this.resolutionRoot = resolutionRoot;

            this.DisplayManager = displayManager;
            MonitorCollection mc = DisplayManager.Displays;
            Monitor = mc[0];

            LayoutMonitor = new MonitorPropertyEditorViewModel(eventAggregator);

            this.fileSystem = fileSystem;

            this.eventAggregator.Publish(new MonitorViewStartedEvent(this));
            eventAggregator.Publish(new DisplayPropertiesView1Event(new[] { LayoutMonitor }));
            this.eventAggregator.Subscribe(this);
            MyCockpitViewModels = new ObservableCollection<PluginModel>();

            NbrSelected = 0;
        }

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


        private string fullimage;
        public string FullImage
        {
            get { return fullimage; }
            set
            {
                fullimage = value;
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

        public void Handle(ToolBoxItem message)
        {
            this.tbi = message;
        }


        public void ContentControlLoaded(object sender)
        {
            var s = sender as ContentControl;
            s.Focus();
            RemoveAdorners();
            AddNewAdorner(s, true);
            FirstSelected = s;
            NbrSelected = 1;

            eventAggregator.Publish(new DisplayPropertiesView1Event((s.DataContext as PluginModel).GetProperties()));
        }

        private void RemoveAdorner(ContentControl s)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(s);
            if (adornerLayer != null)
            {
                Adorner[] adorners = adornerLayer.GetAdorners(s);
                if (adorners != null)
                    foreach (var adorner in adorners)
                        if (typeof(MyAdorner).IsAssignableFrom(adorner.GetType()))
                            adornerLayer.Remove(adorner);
            }
            DictContentcontrol[s] = false;
        }

        private void RemoveAdorners(ContentControl s = null)
        {
            foreach (var item in DictContentcontrol.Keys.ToList())
            {
                if (!(DictContentcontrol[item]) || (s != null && s == item)) continue;

                DictContentcontrol[item] = false;
                var adornerLayer = AdornerLayer.GetAdornerLayer(item);
                if (adornerLayer != null)
                {
                    Adorner[] adorners = adornerLayer.GetAdorners(item);
                    if (adorners != null)
                        foreach (var adorner in adorners)
                            if (typeof(MyAdorner).IsAssignableFrom(adorner.GetType()))
                                adornerLayer.Remove(adorner);
                }
            }
        }

        public void AddNewAdorner(ContentControl s, bool first = false)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(s);
            if (adornerLayer != null)
            {
                MyAdorner myAdorner = new MyAdorner(s, first);
                adornerLayer.Add(myAdorner);
                DictContentcontrol[s] = true;
            }
        }

        private bool IsAlreadySelected(ContentControl s)
        {
            return DictContentcontrol[s];
        }

        private int nbrSelected;
        public int NbrSelected
        {
            get => nbrSelected;
            set
            {
                if (NbrSelected != value)
                {
                    nbrSelected = value;
                    EnableIcons = value > 1;
                }
            }
        }

        private bool nbrSelectedsupa2;
        public bool EnableIcons
        {
            get => nbrSelectedsupa2;
            set
            {
                if (nbrSelectedsupa2 == value) return;
                nbrSelectedsupa2 = value;
                NotifyOfPropertyChange(() => EnableIcons);
            }
        }

        //public bool InsideContentcontrol;
        //public void MouseEnterInContentControl()
        //{
        //    InsideContentcontrol = true;
        //}
        //public void MouseLeaveFromContentControl()
        //{
        //    InsideContentcontrol = false;
        //}
        public void MouseWheelOnContentControl(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            var match = Keyboard.IsKeyDown(key: Key.LeftCtrl) || Keyboard.IsKeyDown(key: Key.RightCtrl);
            var step = (match ? 5 : 1) * (e.Delta > 0 ? 1 : -1);
            var list = DictContentcontrol.Where(item => item.Value).Select(item => item.Key.DataContext as PluginModel);
            foreach (var item in list)
            {
                item.Width += step;
            }
        }

        public void MouseLeftButtonDownOnMonitorView(IInputElement elem, Point pos, MouseEventArgs e)
        {
            RemoveAdorners();
            eventAggregator.Publish(new DisplayPropertiesView1Event(new[] { LayoutMonitor }));
        }

        public void PreviewMouseLeftButtonDownOnContentControl(object sender, MouseEventArgs e)
        {
            var s = sender as ContentControl;
            var CtrlDown = (Keyboard.Modifiers & ModifierKeys.Control) != 0;

            if (!IsAlreadySelected(s) || CtrlDown)
                e.Handled = true;

            if (CtrlDown)
            {
                if (DictContentcontrol[s])
                {
                    if (FirstSelected != null && s == FirstSelected)
                    {
                        RemoveAdorners();
                        FirstSelected = null;
                        NbrSelected = 0;
                    }
                    else
                    {
                        if (NbrSelected > 0)
                        {
                            RemoveAdorner(s);
                            NbrSelected = DictContentcontrol.Where(item => item.Value).Count();
                        }
                    }
                }
                else
                {
                    if (NbrSelected == 0)
                        FirstSelected = s;
                    AddNewAdorner(s, NbrSelected++ == 0);
                }
            }
            else
            {
                if (!DictContentcontrol[s])
                {
                    RemoveAdorners();
                    AddNewAdorner(s, true);
                    FirstSelected = s;
                    NbrSelected = 1;
                }
            }


            if (NbrSelected== 0)
                eventAggregator.Publish(new DisplayPropertiesView1Event(new[] { LayoutMonitor }));
            else
                eventAggregator.Publish(new DisplayPropertiesView1Event((FirstSelected.DataContext as PluginModel).GetProperties()));
        }


        public void KeyTest(object sender, KeyEventArgs e)
        {
            var key = e.Key;
            e.Handled = true;
            //ModifierKeys.Alt 1
            //ModifierKeys.Control 2
            //ModifierKeys.Shift 4
            //ModifierKeys.Windows 8

            var step = (Keyboard.Modifiers & ModifierKeys.Control) != 0 ? 200 : 1;
            switch (key)
            {
                case Key.Left:
                    {
                        var list = DictContentcontrol.Where(item => item.Value).Select(item => item.Key.DataContext as PluginModel);
                        foreach (var k in list)
                        {
                            k.Left = k.Left - step;
                        }
                        //eventAggregator.Publish(new TranslateScaleEvent(deltaletf: step));
                    }
                    break;
                case Key.Right:
                    {
                        var list = DictContentcontrol.Where(item => item.Value).Select(item => item.Key.DataContext as PluginModel);
                        foreach (var k in list)
                        {
                            k.Left = k.Left + step;
                        }
                        //eventAggregator.Publish(new TranslateScaleEvent(deltaletf: step));
                    }

                    break;
                case Key.Up:
                    {
                        var list = DictContentcontrol.Where(item => item.Value).Select(item => item.Key.DataContext as PluginModel);
                        foreach (var k in list)
                        {
                            k.Top = k.Top - step;
                        }
                        //eventAggregator.Publish(new TranslateScaleEvent(deltaletf: step));
                    }
                    break;
                case Key.Down:
                    {
                        var list = DictContentcontrol.Where(item => item.Value).Select(item => item.Key.DataContext as PluginModel);
                        foreach (var k in list)
                        {
                            k.Top = k.Top + step;
                        }
                        //eventAggregator.Publish(new TranslateScaleEvent(deltaletf: step));
                    }
                    break;
            }
        }



        public int nbrfois = 0;
        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is ToolBoxGroup /*&& dropInfo.TargetItem is MonitorViewModel*/)
            {
                
                var tbg = dropInfo.Data as ToolBoxGroup;
                FullImage = tbg.SelectedToolBoxItem.FullImageName;
                tbg.AnchorMouse = new Point(0.0, 0.0);
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Move;
            }
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            var tbg = dropInfo.Data as ToolBoxGroup;
            var selected = tbg.SelectedToolBoxItem;
            int left = (int)dropInfo.DropPosition.X ;
            int top = (int)dropInfo.DropPosition.Y ;
            FullImage = (dropInfo.Data as ToolBoxGroup).SelectedToolBoxItem.FullImageName;
            var num = MyCockpitViewModels.Count;
            var nameUC = tbg.SelectedToolBoxItem.ShortImageName;

            var nbr = MyCockpitViewModels.ToList().Select(t => t.NameUC.Equals(nameUC)).Count();
            if (nbr > 0)
            {
                nameUC = $"{nameUC}_{nbr}"; 
            }

            Ninject.Parameters.Parameter[] param;

            Tag = Tag + 1;

            Ninject.Parameters.Parameter[][] paramproperties = null;
            string[] properties;
            string model;
            var AngleSwitch = 90; 
            if (FullImage.Contains("mfd"))
            {
                var FullImage1 = FullImage.Replace("_0.png", "_1.png");

                param = new Ninject.Parameters.Parameter[]
                {
                        new ConstructorArgument("settings", new object[]{                                                   //PushButton
                            true,                                                                                               //0  is in Mode Editor?
                            $"{nameUC}",                                                                                        //1  name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, 0 },//2  [Left, Top, Width, Height, Angle]

                            new string[]{ FullImage, FullImage1 }, 0,                                                           //3  [images] & startimageposition
                            2d, 0.8d, (PushButtonGlyph)0, Colors.White,                                                         //5  Glyph: Thickness, Scale, Type, Color
                            "Hello", "1,1", "Franklin Gothic", "Normal", "Normal",                                              //9  Text, TextPushOffset, Family, Style, Weight
                            12d, new double[] { 0d, 0d, 0d, 0d },                                                               //14 Size, [padding L,T,R,B]
                            new int[] { 1, 1 },  Colors.White,                                                                  //16 [TextAlign H,V], TextColor

                            1                                                                                                   //18 Button Type
                                                                        }, true)
                };

                model = "CockpitBuilder.Plugins.General.PushButton_ViewModel";
                properties = new string[] { "CockpitBuilder.Common.PropertyEditors.LayoutPropertyEditorViewModel",
                                            "CockpitBuilder.Common.PropertyEditors.PushButtonAppearanceEditorViewModel",
                                            "CockpitBuilder.Common.PropertyEditors.PushButtonBehaviorEditorViewModel"};


                paramproperties = new Ninject.Parameters.Parameter[][]
                {
                    new Ninject.Parameters.Parameter[]
                    {
                        // Layout
                        new ConstructorArgument("settings", new object[]{
                            true,                                                                                               //0  In Mode Editor?
                            $"{nameUC}",                                                                                        //1  name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, 0 } //2  [Left, Top, Width, Height, Angle]
                        }, true)
                    },
                        // Appearance
                    new Ninject.Parameters.Parameter[] 
                    {
                        new ConstructorArgument("settings", new object[]{
                            true,                                                                                               //0  In Mode Editor?
                            $"{nameUC}",                                                                                        //1  name of UC
                            new string[]{ FullImage, FullImage1 }, 0,                                                           //2  images, start image position
                            2d, 0.8d, (PushButtonGlyph)0, Colors.White,                                                         //4  Glyph: Thickness, Scale, Type, Color
                            "Hello", "1,1", "Franklin Gothic", "Normal", "Normal",                                              //8  Text, TextPushOffset, Family, Style, Weight
                            12d, new double[] { 0d, 0d, 0d, 0d },                                                               //13 Size, [padding L,T,R,B]
                            new int[] { 1, 1 },  Colors.White                                                                   //15 [TextAlign H,V], TextColor
                        }, true)
                    },
                        // Behavior
                    new Ninject.Parameters.Parameter[]
                    { 
                        new ConstructorArgument("settings", new object[]{
                            true,                                                                                               //0  In Mode Editor?
                            $"{nameUC}",                                                                                        //1  name of UC
                            1                                                                                                   //2 Button Type
                        }, true)}
                    };


            }
            else
            {
                var FullImage1 = FullImage.Replace("_0.png", "_1.png");
                var FullImage2 = FullImage.Replace("_0.png", "_2.png");

                param = new Ninject.Parameters.Parameter[]
                {
                        new ConstructorArgument("settings", new object[]{                                                   //Switch Button
                            true,                                                                                               //0 is in Mode Editor?
                            $"{nameUC}",                                                                                        //1 name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, AngleSwitch },//2 [Left, Top, Width, Height, Angle]

                            new string[]{ FullImage, FullImage1, FullImage2 , "", "", "" }, 0,                                  //3 [images] & startimageposition

                            2, 1d, 2, 3 }, true)
                };

                model = "CockpitBuilder.Plugins.General.Switch1_ViewModel";
                properties = new string[] { "", "", "" };
            }

            var typeClass = Type.GetType(model);
            //var viewmodel = Activator.CreateInstance(typeClass);
            var viewmodel = resolutionRoot.TryGet(typeClass, param);
            var view = ViewLocator.LocateForModel(viewmodel, null, null);
            ViewModelBinder.Bind(viewmodel, view, null);
            var v = viewmodel as PluginModel;
            v.ZoomFactorFromPluginModel = ZoomFactorFromMonitorViewModel;
            //v.AngleRot = model.Contains("Switch") ? AngleSwitch : 0;
            //RemoveAdorners();
            MyCockpitViewModels.Add((PluginModel)viewmodel);
            eventAggregator.Publish(new DragSelectedItemEvent(tbg.SelectedToolBoxItem));

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

        public void MouseOver(object sender, MouseEventArgs e)
        {

        }

        public void PreviewMouseMoveOnMonitorView(object sender, Point MousePoint, MouseEventArgs e)
        {
            if (nbrfois <= 2)
            {
                System.Diagnostics.Debug.WriteLine($"Preview MouseMove {e.GetPosition((IInputElement)sender)}   sender = {sender}  point={ MousePoint}");
                nbrfois++;
            }
        }

        //public void MouseEnter(object sender, Point MousePoint, MouseEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine($"{e.GetPosition((IInputElement)sender)}   sender = {sender}");
        //    if (sender.ToString().Contains("Rectangle"))
        //    {
                
        //    }
        //        if (sender.ToString().Contains("MonitorView"))
        //    {
        //        foreach (var p in MyCockpitViewModels)
        //        {
        //            //p.IsClickCommingFromMonitorViewModel = true;
        //        }
        //    }
        //}
        //public void MouseLeave()
        //{
        //    foreach (var p in MyCockpitViewModels)
        //    {
        //        //p.IsClickCommingFromMonitorViewModel = false;
        //    }
        //}

        //public void UnselectAll()
        //{
        //    foreach (var p in MyCockpitViewModels)
        //    {
        //        p.IsUCSelected = false;
        //    }

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

        //            //var childContentVisual = childVisual as ContentControl;
        //            //if (childContentVisual != null)
        //            //{
        //            //    var content = childContentVisual.Content;
        //            //}
        //            ProcessElement(childVisual);
        //        }
        //    }
        //}

        public Type DataType => typeof(MonitorViewModel);




        public object Data => throw new NotImplementedException();

        public IDragInfo DragInfo => throw new NotImplementedException();

        public Point DropPosition => throw new NotImplementedException();

        public Type DropTargetAdorner { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DragDropEffects Effects { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int InsertIndex => throw new NotImplementedException();

        public int UnfilteredInsertIndex => throw new NotImplementedException();

        public IEnumerable TargetCollection => throw new NotImplementedException();

        public object TargetItem => throw new NotImplementedException();

        public CollectionViewGroup TargetGroup => throw new NotImplementedException();

        public UIElement VisualTarget => throw new NotImplementedException();

        public UIElement VisualTargetItem => throw new NotImplementedException();

        public Orientation VisualTargetOrientation => throw new NotImplementedException();

        public FlowDirection VisualTargetFlowDirection => throw new NotImplementedException();

        public string DestinationText { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string EffectText { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public RelativeInsertPosition InsertPosition => throw new NotImplementedException();

        public DragDropKeyStates KeyStates => throw new NotImplementedException();

        public bool NotHandled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsSameDragDropContextAsSource => throw new NotImplementedException();

        public EventType EventType => throw new NotImplementedException();


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
