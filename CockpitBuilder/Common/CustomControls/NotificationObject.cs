//  Copyright 2014 Craig Courtney
//    
//  Helios is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  Helios is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace CockpitBuilder.Common.CustomControls
{
    using System;
    using System.ComponentModel;

    public class NotificationObject : IPropertyNotification
    {
        #region INotifyPropertyChanged Members

        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyNotificationEventArgs args)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        protected void OnPropertyChanged(string childPropertyName, PropertyNotificationEventArgs args)
        {
            OnPropertyChanged(new PropertyNotificationEventArgs(this, childPropertyName, args));
        }

        protected void OnPropertyChanged(string propertyName, object oldValue, object newValue, bool undoable)
        {
            OnPropertyChanged(new PropertyNotificationEventArgs(this, propertyName, oldValue, newValue, undoable));
        }

        #endregion
    }
}
