using Core.Common.Base;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MyBoilerPlate.Business.Entities.DTOs
{
    [DataContract]
    public class MenuNodeDTO : DTOBase<MenuNodeDTO>
    {
        #region Properties

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Detail { get; set; }

        [DataMember]
        public bool? IsRoot { get; set; }

        [DataMember]
        public string NavigationPath { get; set; }

        [DataMember]
        public string IconClasses { get; set; }

        [DataMember]
        public int? ParentId { get; set; }

        [DataMember]
        public int? SortOrder { get; set; }

        [DataMember]
        public string MainImagePath { get; set; }

        [DataMember]
        public string MainImageHoverPath { get; set; }

        [DataMember]
        public bool? NeedSelectedStudent { get; set; }

        #endregion

        #region Relationships

        public MenuNodeDTO Parent { get; set; }

        public List<MenuNodeDTO> Children { get; set; }

        #endregion
    }
}
