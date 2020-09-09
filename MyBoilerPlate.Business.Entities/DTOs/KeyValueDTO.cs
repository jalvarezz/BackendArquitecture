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
    public class KeyValueDTO : DTOBase<KeyValueDTO>
    {
        private string _Name;

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

        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public bool Selected { get; set; }
        [DataMember]
        public bool Disabled { get; set; }

        public bool IsDirty { get; set; }
    }

    [DataContract]
    public class KeyValueDTO<TKey> : DTOBase<KeyValueDTO<TKey>>
    {
        private string _Name;

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

        [DataMember]
        public TKey Id { get; set; }
    }

    [DataContract]
    public class KeyValueDTO<TKey, TValue> : DTOBase<KeyValueDTO<TKey, TValue>>
    {
        private string _Name;

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

        [DataMember]
        public TKey Id { get; set; }

        [DataMember]
        public TValue Value { get; set; }
    }
}
