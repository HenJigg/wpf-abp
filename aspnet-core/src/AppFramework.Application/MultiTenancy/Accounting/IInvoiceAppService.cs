using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using AppFramework.MultiTenancy.Accounting.Dto;

namespace AppFramework.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
