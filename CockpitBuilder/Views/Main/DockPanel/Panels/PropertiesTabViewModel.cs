using Caliburn.Micro;
using System.Windows.Media;
using CockpitBuilder.Events;
using IEventAggregator = CockpitBuilder.Core.Common.Events.IEventAggregator;
using Ninject.Syntax;
using CockpitBuilder.Common.PropertyEditors;
using Ninject.Parameters;
using System.Collections.ObjectModel;
using System;
using Ninject;
using System.Windows;
using System.Collections.Generic;

namespace CockpitBuilder.Views.Main.DockPanel.Panels
{
    public class PropertiesTabViewModel : PanelViewModel, Core.Common.Events.IHandle<DisplayPropertiesViewEvent>, Core.Common.Events.IHandle<DisplayPropertiesView1Event>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IResolutionRoot resolutionRoot;
        private Dictionary<string, PropertyEditorModel> ViewModels;
        public PropertiesTabViewModel(IEventAggregator eventAggregator, IResolutionRoot resolutionRoot)
        {
            this.eventAggregator = eventAggregator;
            this.resolutionRoot = resolutionRoot;
            ViewModels = new Dictionary<string, PropertyEditorModel>();
            Title = "Properties";



            //CreateNewInstancePropertyModel("CockpitBuilder.Common.PropertyEditors.MonitorPropertyEditorViewModel", true);
            //CreateNewInstancePropertyModel("CockpitBuilder.Common.PropertyEditors.LayoutPropertyEditorViewModel");

            //var typeClass = Type.GetType("CockpitBuilder.Common.PropertyEditors.MonitorPropertyEditorViewModel");           
            ////Ninject.Parameters.Parameter[] param = { new ConstructorArgument("tag", 0, true) };
            //var viewmodel = resolutionRoot.TryGet(typeClass);
            //var view = ViewLocator.LocateForModel(viewmodel, null, null);
            //ViewModelBinder.Bind(viewmodel, view, null);
            //PropertyViewModels.Add((PropertyEditorModel)viewmodel);



            //typeClass = Type.GetType("CockpitBuilder.Common.PropertyEditors.LayoutPropertyEditorViewModel");
            ////Ninject.Parameters.Parameter[] param = { new ConstructorArgument("tag", 0, true) };
            //viewmodel = resolutionRoot.TryGet(typeClass);
            //view = ViewLocator.LocateForModel(viewmodel, null, null);
            //ViewModelBinder.Bind(viewmodel, view, null);
            //PropertyViewModels.Add((PropertyEditorModel)viewmodel);


            //typeClass = Type.GetType("CockpitBuilder.Common.PropertyEditors.PushButtonAppearanceEditorViewModel");
            //Ninject.Parameters.Parameter[] param1 = { new ConstructorArgument("tag", 0, true) };
            //viewmodel = resolutionRoot.TryGet(typeClass, param1);
            //view = ViewLocator.LocateForModel(viewmodel, null, null);
            //ViewModelBinder.Bind(viewmodel, view, null);
            //PropertyViewModels.Add((PropertyEditorModel)viewmodel);

            //typeClass = Type.GetType("CockpitBuilder.Common.PropertyEditors.PushButtonBehaviorEditorViewModel");
            //Ninject.Parameters.Parameter[] param2 = { new ConstructorArgument("tag", 1, true) };
            //viewmodel = resolutionRoot.TryGet(typeClass, param2);
            //view = ViewLocator.LocateForModel(viewmodel, null, null);
            //ViewModelBinder.Bind(viewmodel, view, null);
            //PropertyViewModels.Add((PropertyEditorModel)viewmodel);

            //typeClass = Type.GetType("CockpitBuilder.Common.PropertyEditors.ThreeWayToggleSwitchAppearanceEditorViewModel");
            //Ninject.Parameters.Parameter[] param1 = { new ConstructorArgument("tag", 0, true) };
            //viewmodel = resolutionRoot.TryGet(typeClass, param1);
            //view = ViewLocator.LocateForModel(viewmodel, null, null);
            //ViewModelBinder.Bind(viewmodel, view, null);
            //PropertyViewModels.Add((PropertyEditorModel)viewmodel);

            //typeClass = Type.GetType("CockpitBuilder.Views.Main.DockPanel.Panels.UserViewModel");
            //Ninject.Parameters.Parameter[] param1 = { new ConstructorArgument("tag", 0, true) };
            //viewmodel = resolutionRoot.TryGet(typeClass, param1);
            //view = ViewLocator.LocateForModel(viewmodel, null, null);
            //ViewModelBinder.Bind(viewmodel, view, null);
            //PropertyViewModels.Add((PropertyEditorModel)viewmodel);

            //typeClass = Type.GetType("CockpitBuilder.Common.PropertyEditors.ThreeWayToggleSwitchBehaviorEditorViewModel");
            //Ninject.Parameters.Parameter[] param2 = { new ConstructorArgument("tag", 1, true) };
            //viewmodel = resolutionRoot.TryGet(typeClass, param2);
            //view = ViewLocator.LocateForModel(viewmodel, null, null);
            //ViewModelBinder.Bind(viewmodel, view, null);
            //PropertyViewModels.Add((PropertyEditorModel)viewmodel);

            //typeClass = Type.GetType("CockpitBuilder.Common.PropertyEditors.MonitorPropertyEditorViewModel");
            //Ninject.Parameters.Parameter[] param2 = { new ConstructorArgument("tag", 1, true) };
            //viewmodel = resolutionRoot.TryGet(typeClass, param2);
            //view = ViewLocator.LocateForModel(viewmodel, null, null);
            //ViewModelBinder.Bind(viewmodel, view, null);
            //PropertyViewModels.Add((PropertyEditorModel)viewmodel);
            //PropertyViewModels = new ObservableCollection<AboutViewModel> { "&é", "12", "azert" };

            this.eventAggregator.Subscribe(this);
        }


        private void CreateNewInstancePropertyModel(string propertymodel, bool AddToPropertyCollection = false)
        {
            if (!ViewModels.ContainsKey(propertymodel))
            {
                var typeClass = Type.GetType(propertymodel);
                //Ninject.Parameters.Parameter[] param = { new ConstructorArgument("tag", 0, true) };
                var viewmodel = resolutionRoot.TryGet(typeClass);
                var view = ViewLocator.LocateForModel(viewmodel, null, null);
                ViewModelBinder.Bind(viewmodel, view, null);
                ViewModels[propertymodel] = (PropertyEditorModel)viewmodel;
            }
            if (AddToPropertyCollection) PropertyViewModels.Add(ViewModels[propertymodel]);
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                NotifyOfPropertyChange(() => Name);
                name = value;
            }
        }


        private ObservableCollection<PropertyEditorModel> _propertyViewModels = new ObservableCollection<PropertyEditorModel>();
        public ObservableCollection<PropertyEditorModel> PropertyViewModels
        {
            get { return _propertyViewModels; }
            set
            {
                _propertyViewModels = value;
                NotifyOfPropertyChange(() => PropertyViewModels);
            }
        }

        public void Handle(DisplayPropertiesViewEvent message)
        {
            var viewmodels = message.Views;
            PropertyViewModels.Clear();
            foreach (var v in viewmodels)
                CreateNewInstancePropertyModel(v, true);
        }

        public void Handle(DisplayPropertiesView1Event message)
        {
            if (message.Clear)
            {
                PropertyViewModels.Clear();
                return;
            }

            var viewmodels = message.Views;
            if (PropertyViewModels.Count > 0 &&  viewmodels[0] == PropertyViewModels[0])
                return;

            PropertyViewModels.Clear();
            foreach (var v in viewmodels)
                PropertyViewModels.Add(v);
        }

        ~PropertiesTabViewModel()
        {
            MessageBox.Show("PropertiesTabViewModel", "Important Question", MessageBoxButton.YesNo);
        }


        //public enum ImageAlignment
        //{
        //    Centered,
        //    Stretched,
        //    Tiled
        //}

        //public BindableCollection<ImageAlignment> ImageStyle
        //{
        //    get
        //    {
        //        return  new BindableCollection<ImageAlignment>(new[]{ImageAlignment.Centered, ImageAlignment.Stretched, ImageAlignment.Tiled});
        //    }
        //}

        //private bool isProperties;
        //public bool IsProperties
        //{
        //    get { return isProperties; }
        //    set
        //    {
        //        isProperties = value;
        //        NotifyOfPropertyChange(() => IsProperties);
        //    }
        //}



        //private ImageAlignment selectedImageStyle;
        //public ImageAlignment SelectedImageStyle
        //{
        //    get { return selectedImageStyle; }
        //    set
        //    {
        //        //Set(ref selectedImageStyle, value);
        //        selectedImageStyle = value;
        //        NotifyOfPropertyChange(() => SelectedImageStyle);
        //    }
        //}

        //private bool fillBackground;
        //public bool FillBackground
        //{
        //    get { return fillBackground; }
        //    set
        //    {
        //        fillBackground = value;
        //        NotifyOfPropertyChange(() => FillBackground);
        //        eventAggregator.Publish(new BackgroundColorEvent(BackgroundColor, !value));
        //    }
        //}
        //private Color backgroundColor;
        //public Color BackgroundColor
        //{
        //    get { return backgroundColor; }
        //    set
        //    {
        //        backgroundColor = value;
        //        NotifyOfPropertyChange(() => BackgroundColor);
        //        eventAggregator.Publish( new BackgroundColorEvent(value));
        //    }
        //}


        //private string backgroundImage;
        //public string BackgroundImage
        //{
        //    get { return backgroundImage; }
        //    set
        //    {
        //        backgroundImage = value;
        //        NotifyOfPropertyChange(() => BackgroundImage);
        //        eventAggregator.Publish(new BackgroundImageEvent(value));
        //    }
        //}

        //private string folder;
        //public string Folder
        //{
        //    get { return folder; }
        //    set
        //    {
        //        folder = value;
        //        NotifyOfPropertyChange(() => Folder);
        //        eventAggregator.Publish(new FolderNameEvent(value));
        //    }
        //}

        //private string deviceid;
        //public string DeviceId
        //{
        //    get { return deviceid; }
        //    set
        //    {
        //        deviceid = value;
        //        NotifyOfPropertyChange(() => DeviceId);
        //    }
        //}

        //private int x;
        //public int X
        //{
        //    get { return x; }
        //    set
        //    {
        //        x = value;
        //        NotifyOfPropertyChange(() => X);
        //        eventAggregator.Publish(new TransformEvent(deltaletf: value, deltatop: Y));
        //    }
        //}

        //private int y;
        //public int Y
        //{
        //    get { return y; }
        //    set
        //    {
        //        y = value;
        //        NotifyOfPropertyChange(() => Y);
        //        eventAggregator.Publish(new TransformEvent(deltaletf: X, deltatop: value));
        //    }
        //}

        //private int width;
        //public int Width
        //{
        //    get { return width; }
        //    set
        //    {
        //        width = value;
        //        NotifyOfPropertyChange(() => Width);
        //    }
        //}

        //private int height;
        //public int Height
        //{
        //    get { return height; }
        //    set
        //    {
        //        height = value;
        //        NotifyOfPropertyChange(() => Height);
        //    }
        //}

        //public void Handle(MonitorEvent message)
        //{
        //    IsProperties = true;
        //}

        //public void Handle(DevicePropertiesEvent message)
        //{
        //    X = message.X;
        //    Y = message.Y;
        //    Width = message.Width;
        //    Height = message.Height;
        //    DeviceId = message.Tag;
        //}

    }
}
