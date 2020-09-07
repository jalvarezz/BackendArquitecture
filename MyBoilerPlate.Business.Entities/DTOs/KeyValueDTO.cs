using Core.Common.Base;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace MyBoilerPlate.Business.Entities.DTOs
{
    [DataContract]
    public abstract class KeyValueBase : DTOBase<KeyValueBase>
    {
        #region Members

        private string _Name;

        #endregion

        [DataMember]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = !string.IsNullOrEmpty(value) ? value : null;
            }
        }

        [IgnoreDataMember]
        public int SortOrder { get; set; }


    }

    [DataContract]
    public class KeyValueDTO : KeyValueBase
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public bool Selected { get; set; }
        [DataMember]
        public bool Disabled { get; set; }

        public bool IsDirty { get; set; }
    }

    [DataContract]
    public class KeyValueDTO<TKey> : KeyValueBase
    {
        [DataMember]
        public TKey Id { get; set; }
    }

    [DataContract]
    public class KeyValueDTO<TKey, TValue> : KeyValueBase
    {
        [DataMember]
        public TKey Id { get; set; }

        [DataMember]
        public TValue Value { get; set; }
    }
}
