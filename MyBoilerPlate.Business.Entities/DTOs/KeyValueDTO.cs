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
    public class KeyValueDTO : DTOBase<KeyValueDTO>
    {
        private string _Name;

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

        public int SortOrder { get; set; }

        public long Id { get; set; }

        public bool Selected { get; set; }
 
        public bool Disabled { get; set; }

        public bool IsDirty { get; set; }
    }

    public class KeyValueDTO<TKey> : DTOBase<KeyValueDTO<TKey>>
    {
        private string _Name;

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

        public int SortOrder { get; set; }

        public TKey Id { get; set; }
    }

    public class KeyValueDTO<TKey, TValue> : DTOBase<KeyValueDTO<TKey, TValue>>
    {
        private string _Name;

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

        public int SortOrder { get; set; }

        public TKey Id { get; set; }

        public TValue Value { get; set; }
    }
}
