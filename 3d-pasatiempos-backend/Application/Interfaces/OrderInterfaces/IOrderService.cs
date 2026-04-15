namespace _3d_pasatiempos_backend.Application.Interfaces.OrderInterfaces
{
    public interface IOrderService
    {
        Task StartOrderAsync(int pOrderId);
    }
}
