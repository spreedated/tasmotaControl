using System.Threading.Tasks;

namespace TasmotaQuery
{
    public interface IQuery
    {
        public Task<IQuery> IsAvailable();
        public Task<IQuery> GetState();
        public Task<IQuery> GetStatus();
        public Task<IQuery> GetStatus2Firmware();
        public Task<IQuery> GetStatus7Time();
        public Task<IQuery> GetStatus10Sensors();
    }
}
