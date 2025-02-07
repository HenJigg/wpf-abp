using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using AppFramework.Dto;

namespace AppFramework.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
