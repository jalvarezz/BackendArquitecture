using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Base
{
    public class DocumentBase
    {
        public byte[] Content { get; set; }
        public bool HasErrors { get; set; }
        public int PageCount { get; set; }
    }
}
