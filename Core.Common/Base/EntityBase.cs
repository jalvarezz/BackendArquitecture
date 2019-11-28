using Core.Common.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Extensions;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Common.Attributes;

namespace Core.Common.Base
{
    /// <summary>
    /// Base client applied to the Business Entities
    /// </summary>
    [DataContract]
    public abstract class EntityBase : NotificationObject, IExtensibleDataObject
    {
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

        #region IExtensibleDataObject Members

        [IgnoreDataMember]
        [NotMapped]
        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }
}
