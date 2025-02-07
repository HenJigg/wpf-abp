using System.Collections.Generic;
using AppFramework.Authorization.Users.Importing.Dto;
using AppFramework.Dto;

namespace AppFramework.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
