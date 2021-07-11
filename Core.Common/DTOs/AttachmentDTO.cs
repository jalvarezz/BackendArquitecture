using Core.Common.Base;
using System.Runtime.Serialization;

namespace Core.Common.DTOs
{
    [DataContract]
    public class AttachmentDTO : DTOBase<AttachmentDTO>
    {
        #region Properties

        [DataMember]
        public int DocumentId { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public byte[] Content { get; set; }

        #endregion
    }
}
