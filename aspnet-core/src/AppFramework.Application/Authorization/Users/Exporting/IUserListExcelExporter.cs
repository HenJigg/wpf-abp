using System.Collections.Generic;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Dto;

namespace AppFramework.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}