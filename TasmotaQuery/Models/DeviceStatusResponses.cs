namespace TasmotaQuery.Models
{
    public sealed class DeviceStatusResponses
    {
        public State State { get; internal set; }
        public Status Status { get; internal set; }
        public Firmware Firmware { get; internal set; }
        public Time Time { get; internal set; }
        public Sensors Sensors { get; internal set; }
    }
}