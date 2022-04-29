using System.Threading.Tasks;

namespace CashRegister.Services
{
    public interface IQrScanningService
    {
        Task<string> ScanAsync();
    }
}
