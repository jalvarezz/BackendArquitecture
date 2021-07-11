using Core.Common.Base;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MyBoilerPlate.Business.Entities.DTOs
{
    public class MenuNodeDTO : DTOBase<MenuNodeDTO>
    {
        #region Properties

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Detail { get; set; }

        public bool? IsRoot { get; set; }

        public string NavigationPath { get; set; }

        public string IconClasses { get; set; }

        public int? ParentId { get; set; }

        public int? SortOrder { get; set; }

        public string MainImagePath { get; set; }

        public string MainImageHoverPath { get; set; }

        public bool? NeedSelectedStudent { get; set; }

        #endregion

        #region Relationships

        public MenuNodeDTO Parent { get; set; }

        public List<MenuNodeDTO> Children { get; set; }

        #endregion
    }
}
