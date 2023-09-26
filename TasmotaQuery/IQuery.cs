using System.Threading.Tasks;

namespace TasmotaQuery
{
    public interface IQuery
    {
        public IDevice Device { get; set; }
        public Task IsAvailable();
        public Task GetState();
    }
}
