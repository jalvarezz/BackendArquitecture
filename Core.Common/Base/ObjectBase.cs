using Core.Common.Contracts;
using Core.Common.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Base
{
    /// <summary>
    /// Base client applied to the Client Entities
    /// </summary>
    public abstract class ObjectBase : NotificationObject
    {

        public ObjectBase()
        {
        }
        
        //public static CompositionContainer Container { get; set; }

        #region Property Change Notification

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(true, propertyName);
        }

        protected virtual void OnPropertyChanged(bool makeDirty, [CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }

        #endregion
    }
}
