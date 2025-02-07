using System.Collections.Generic;
using AppFramework.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace AppFramework.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
