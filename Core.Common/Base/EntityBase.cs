using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Common.Base
{
    /// <summary>
    /// Base client applied to the Business Entities
    /// </summary>
    [DataContract]
    public abstract class EntityBase : IExtensibleDataObject
    {
        #region Property Change Notification


        #endregion

        #region IExtensibleDataObject Members

        [IgnoreDataMember]
        [NotMapped]
        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DataContract]
    public class StoredProcedureEntityBase : EntityBase
    {

    }

    [DataContract]
    public class FunctionEntityBase : EntityBase
    {

    }
}
