using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Domain.Entities;

namespace _3d_pasatiempos_backend.Application.Interfaces.PaymentInterfaces
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment pPayment);
        Task<Payment> GetPaymentByIdAsync(int pPaymentId);
        Task<(List<Payment> Data, int TotalCount)> GetPagedPaymentAsync(QueryParams pQuery);
    }
}
