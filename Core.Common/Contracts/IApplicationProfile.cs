using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace Core.Common.Contracts
{
    public interface IApplicationProfile
    {
        int? ApplicationId { get; set; }
    }
}
