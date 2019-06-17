using Caliburn.Micro;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace CockpitBuilder.Views.Main.DockPanel.Models
{
    public class ToolBoxGroup:PropertyChangedBase, IDragSource
    {

        private string _groupName;
        public string GroupName
        {
            get => _groupName;
            set
            {
                _groupName = value;
                NotifyOfPropertyChange(() => GroupName);
            }
        }
        private ToolBoxItem _selectedToolBoxItem;
        public ToolBoxItem SelectedToolBoxItem
        {
            get
            {
                return _selectedToolBoxItem;
            }
            set
            {
                _selectedToolBoxItem = value;
                NotifyOfPropertyChange(() => SelectedToolBoxItem);
            }
        }
        private ObservableCollection<ToolBoxItem> _toolBoxItems;

        public ObservableCollection<ToolBoxItem> ToolBoxItems
        {
            get { return _toolBoxItems; }
            set
            {
                _toolBoxItems = value;
                NotifyOfPropertyChange(() => ToolBoxItems);
            }
        }

        //public void StartDrag(IDragInfo dragInfo)
        //{
        //    var t = dragInfo.Effects;
        //}

        //public bool CanStartDrag(IDragInfo dragInfo)
        //{
        //    return true;
        //}

        //public void Dropped(IDropInfo dropInfo)
        //{
        //    throw new NotImplementedException();
        //}

        //public void DragDropOperationFinished(DragDropEffects operationResult, IDragInfo dragInfo)
        //{
        //    throw new NotImplementedException();
        //}

        //public void DragCancelled()
        //{
        //    throw new NotImplementedException();
        //}

        //public bool TryCatchOccurredException(Exception exception)
        //{
        //    throw new NotImplementedException();
        //}


        void IDragSource.StartDrag(IDragInfo dragInfo)
        {
            var itemCount = dragInfo.SourceItems.Cast<object>().Count();

            if (itemCount == 1)
            {
                dragInfo.Data = dragInfo.SourceItems.Cast<object>().First();
            }
            //else if (itemCount > 1)
            //{
            //    dragInfo.Data = TypeUtilities.CreateDynamicallyTypedList(dragInfo.SourceItems);
            //}

            dragInfo.Effects = (dragInfo.Data != null) ? DragDropEffects.Copy | DragDropEffects.Move : DragDropEffects.None;
        }

        bool IDragSource.CanStartDrag(IDragInfo dragInfo)
        {
            return true;
        }

        void IDragSource.Dropped(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        void IDragSource.DragDropOperationFinished(DragDropEffects operationResult, IDragInfo dragInfo)
        {
            throw new NotImplementedException();
        }

        void IDragSource.DragCancelled()
        {
            throw new NotImplementedException();
        }

        bool IDragSource.TryCatchOccurredException(Exception exception)
        {
            return true;
        }


        //public IDragSourceAdvisor DragAdvisor
        //{
        //    get
        //    {
        //        return _advisor;
        //    }
        //    set
        //    {
        //        if ((_advisor == null && value != null)
        //            || (_advisor != null && !_advisor.Equals(value)))
        //        {
        //            _advisor = value;
        //        }
        //    }
        //}

    }
}
