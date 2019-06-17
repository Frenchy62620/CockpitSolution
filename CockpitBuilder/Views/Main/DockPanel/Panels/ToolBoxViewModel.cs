using Caliburn.Micro;
using CockpitBuilder.Events;
using CockpitBuilder.Views.Main.DockPanel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IEventAggregator = CockpitBuilder.Core.Common.Events.IEventAggregator;

namespace CockpitBuilder.Views.Main.DockPanel.Panels
{
    public class ToolBoxViewModel : PanelViewModel, 
                                    Core.Common.Events.IHandle<FolderNameEvent>
    {
        private readonly IEventAggregator eventAggregator;

        public ToolBoxViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            Title = "ToolBox - Select the DeviceType";

            //var t = GetNamespacesInAssembly(Assembly.GetExecutingAssembly());
            //Type[] typelist = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "CockpitBuilder.Plugins.");
            //List<string> finallist = typelist.Select(x => x.ToString().Replace("CockpitBuilder.Plugins.", "").Replace("_ViewModel", "")).ToList();

            //DeviceTypes = new BindableCollection<string>(finallist  );
            //selectedDeviceType = DeviceTypes[0];

            //Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
            //{
            //    double f = 0;

            //    return
            //        assembly.GetTypes()
            //            .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal) && t.FullName.EndsWith("_ViewModel"))
            //            .ToArray();
            //}

            LoadImage(@"J:\heliosDevices\Images");

        }

        private static IEnumerable<string> GetNamespacesInAssembly(Assembly asm)
        {
            Type[] types = asm.GetTypes();

            return types.Select(t => t.Namespace)
                        .Distinct()
                        .Where(n => n != null);
        }

        private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            //var xt = assembly.GetTypes()
            //        .Where(t => t.Namespace.Contains(nameSpace) && t.FullName.EndsWith("ViewModel"))
            //        .ToArray();

            //return xt;

            return
                assembly.GetTypes()
                    .Where(t => t.Namespace.Contains(nameSpace) && t.FullName.EndsWith("ViewModel"))
                    .ToArray();
        }
        public void LoadImage(string parentDir)
        {
            ToolBoxGroups = new BindableCollection<ToolBoxGroup>();
            foreach (string subdir in Directory.GetDirectories(parentDir))
            {
                var toolBoxItems = new BindableCollection<ToolBoxItem>();
                var groupname = Path.GetFileName(subdir);

                bool flag = subdir.Contains("Switch") || subdir.Contains("Rockers") || subdir.Contains("Buttons");
                foreach (string file in Directory.GetFiles(subdir))
                {
                    if (flag && (file.Contains("_0.")))
                    {
                        var shortImageName = file.Split('\\').Last().Replace("_0.png", "");
                        //System.Drawing.Image img = System.Drawing.Image.FromStream;

                        getSizeOfImage(file, out int width, out int height);

                        toolBoxItems.Add(new ToolBoxItem
                        {
                            FullImageName = file,
                            ShortImageName = shortImageName,
                            ImageHeight = height,
                            ImageWidth = width,
                        });

                    }
                    else if (!flag)
                    {
                        var shortImageName = file.Split('\\').Last().Split('_').First().Split('.').First();
                        getSizeOfImage(file, out int width, out int height);

                        toolBoxItems.Add(new ToolBoxItem
                        {
                            FullImageName = file,
                            ShortImageName = shortImageName,
                            ImageHeight = height,
                            ImageWidth = width,
                        });
                    }

                    void getSizeOfImage(string filename, out int w, out int h)
                    {
                        //using (var imageStream = File.OpenRead(file))
                        //{
                        //    var decoder = BitmapDecoder.Create(imageStream, BitmapCreateOptions.None, BitmapCacheOption.None);
                        //    h = decoder.Frames[0].PixelHeight;
                        //    w = decoder.Frames[0].PixelWidth;
                        //}

                        using (BinaryReader b = new BinaryReader(File.Open(file, FileMode.Open)))
                        {
                            b.BaseStream.Seek(1, SeekOrigin.Begin);
                            var p = b.ReadBytes(3);
                            string bytesAsString = Encoding.UTF8.GetString(p);
                            b.BaseStream.Seek(16, SeekOrigin.Begin);
                            w = (b.ReadByte() << 24) + (b.ReadByte() << 16) + (b.ReadByte() << 8) + b.ReadByte();
                            h = (b.ReadByte() << 24) + (b.ReadByte() << 16) + (b.ReadByte() << 8) + b.ReadByte();
                        }

                    }

                }

                ToolBoxGroups.Add(new ToolBoxGroup
                {
                    GroupName = groupname,
                    ToolBoxItems = toolBoxItems
                });

            }
        }

        public BindableCollection<string> DeviceTypes{ get; private set; }

        private string selectedDeviceType;
        public string SelectedDeviceType
        {
            get => selectedDeviceType;

            set
            {
                selectedDeviceType = value;
                NotifyOfPropertyChange(() => SelectedDeviceType);
            }
        }

        private BindableCollection<ToolBoxGroup> _toolBoxGroups;

        public BindableCollection<ToolBoxGroup> ToolBoxGroups
        {
            get { return _toolBoxGroups; }
            set
            {
                _toolBoxGroups = value;
                NotifyOfPropertyChange(() => ToolBoxGroups);
            }
        }


        private void GetSubFolders(List<string> folders, string parentDir)
        {
            foreach (string d in Directory.GetDirectories(parentDir))
            {
                folders.Add(d);
                GetSubFolders(folders, d);
            }
        }

        private string[] GetDirectories(string parentDir)
        {
            List<string> dirs = new List<string>();
            GetSubFolders(dirs, parentDir);
            return dirs.ToArray();
        }

        //private string _imageSelected;
        //private string _imageSelected1;

        //public string ImageSelected
        //{
        //    get
        //    {
        //        System.Diagnostics.Debug.WriteLine($"get isi {_imageSelected}");
        //        return @"E:\Téléchargement\C#Samples\helios-master\Helios\Images\A-10\adj-down.png";
        //        if (String.IsNullOrEmpty(_imageSelected))
        //            return _imageSelected1;
        //        return _imageSelected;
        //    }
        //    set
        //    {
        //        _imageSelected = value;
        //        System.Diagnostics.Debug.WriteLine($"set is {value}");
        //        NotifyOfPropertyChange(() => ImageSelected);
        //    }
        //}



        //public void SelectImage(object datacontext, MouseButtonEventArgs e, object source)
        //{
        //    var img = (datacontext as ToolBoxItem).ShortImageName;
        //    if (!string.IsNullOrEmpty(img))
        //    {
        //        System.Diagnostics.Debug.WriteLine($"sva is {ImageSelected} - {img}");
        //        ImageSelected = img;
        //        _imageSelected1 = img;
        //    }

        //}

        //public void Save(object datacontext)
        //{
        //    var img = (datacontext as ToolBoxItem).ShortImageName;
        //    if (!string.IsNullOrEmpty(img))
        //    {
        //        System.Diagnostics.Debug.WriteLine($"sva is {ImageSelected} - {img}");
        //        ImageSelected = img;
        //        _imageSelected1 = img;
        //    }

        //    //ToolBoxItem item = null;
        //    //ItemsControl _itemsControl = GetAncestor((UIElement)source, typeof(ItemsControl)) as ItemsControl;
        //    //ContentPresenter itemPresenter = (GetAncestor((UIElement)source, typeof(ItemsControl)) as ItemsControl).ContainerFromElement(dragElement) as ContentPresenter;
        //    //if (itemPresenter != null)
        //    //{
        //    //    item = itemPresenter.Content as ToolBoxItem;
        //    //}

        //}

        //public void PreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        //{
        //    e.Handled = true;
        //}

        private DependencyObject GetAncestor(UIElement element, Type parentType)
        {
            DependencyObject item = element;
            while (item != null && item.GetType() != parentType)
            {
                item = VisualTreeHelper.GetParent(item);
            }

            return item;
        }

        public void Handle(FolderNameEvent message)
        {
            LoadImage(message.FolderName);
        }
    }
}

