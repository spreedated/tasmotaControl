using NUnit.Framework;
using RichardSzalay.MockHttp;
using System;
using System.Linq;
using System.Net;
using TasmotaQuery;
using static UnitTests.HelperFunctions;

namespace UnitTests
{
    [TestFixture]
    public class DeviceQueryTests
    {
        private MockHttpMessageHandler mockHttp;
        private string stateResponse;
        private string statusResponse;
        private string status2firmwareResponse;
        private string status7timeResponse;
        private string status10sensorsResponse;

        [SetUp]
        public void SetUp()
        {
            this.mockHttp = new();

            this.stateResponse = LoadJson("State");
            this.statusResponse = LoadJson("Status");
            this.status2firmwareResponse = LoadJson("Firmware");
            this.status7timeResponse = LoadJson("Time");
            this.status10sensorsResponse = LoadJson("Sensors");
        }

        [Test]
        public void StateQueryTests()
        {
            this.mockHttp.When("http://*/cm?cmnd=state")
                .Respond("application/json", stateResponse);

            this.mockHttp.When("http://*/cm")
                .Respond("application/json", "{ \"WARNING\": \"Enter command cmnd=\" }");

            Device d = new(IPAddress.Parse("192.168.0.0"))
            {
                httpHandler = this.mockHttp
            };

            Assert.DoesNotThrowAsync(async () =>
            {
                await d.IsAvailable();
                Assert.That(d.IsAvaiable, Is.True);
                await d.GetState();
                Assert.Multiple(() =>
                {
                    Assert.That(d.DeviceStatusResponses.State.Wifi.Downtime, Is.Not.EqualTo(default(TimeSpan)));
                    Assert.That(d.DeviceStatusResponses.State.Wifi.Downtime.Minutes, Is.EqualTo(13));
                    Assert.That(d.DeviceStatusResponses.State.Uptime.Minutes, Is.EqualTo(14));
                    Assert.That(d.DeviceStatusResponses.State.Uptime.Hours, Is.EqualTo(20));
                    Assert.That(d.DeviceStatusResponses.State.Uptime.Days, Is.EqualTo(26));
                    Assert.That(d.DeviceStatusResponses.State.Uptime, Is.Not.EqualTo(default(TimeSpan)));
                    Assert.That(d.DeviceStatusResponses.State, Is.Not.Null);
                    Assert.That(d.DeviceStatusResponses.State.Heap, Is.EqualTo(121));
                    Assert.That(d.DeviceStatusResponses.State.Wifi, Is.Not.Null);
                    Assert.That(d.DeviceStatusResponses.State.Wifi.Channel, Is.EqualTo(1));
                    Assert.That(d.DeviceStatusResponses.State.Wifi.Mode, Is.EqualTo("11n"));
                    Assert.That(d.DeviceStatusResponses.State.QueryTime, Is.Not.EqualTo(default(DateTime)));
                });
            });
        }

        [Test]
        public void StateQueryFailTests()
        {
            this.mockHttp.When("http://*/cm?cmnd=state")
                .Respond("application/json", new string('.', 15));

            Device d = new(IPAddress.Parse("192.168.0.0"))
            {
                httpHandler = this.mockHttp
            };

            Assert.DoesNotThrowAsync(async () =>
            {
                await d.IsAvailable();
                Assert.That(d.IsAvaiable, Is.False);
                await d.GetState();
                Assert.Multiple(() =>
                {
                    Assert.That(d.DeviceStatusResponses.State, Is.Null);
                });
            });
        }

        [Test]
        public void StatusQueryTests()
        {
            this.mockHttp.When("http://*/cm?cmnd=status")
                .Respond("application/json", this.statusResponse);

            Device d = new(IPAddress.Parse("192.168.0.0"))
            {
                httpHandler = this.mockHttp
            };

            Assert.DoesNotThrowAsync(async () =>
            {
                Assert.That(d.DeviceStatusResponses.Status, Is.Null);
                await d.GetStatus();
                Assert.Multiple(() =>
                {
                    Assert.That(d.DeviceStatusResponses.Status, Is.Not.Null);
                    Assert.That(d.DeviceStatusResponses.Status.FriendlyNames, Is.Not.Empty);
                    Assert.That(d.DeviceStatusResponses.Status.FriendlyNames.Count, Is.EqualTo(2));
                    Assert.That(d.DeviceStatusResponses.Status.FriendlyNames[1], Is.Not.Null);
                    Assert.That(string.IsNullOrEmpty(d.DeviceStatusResponses.Status.FriendlyNames[1]), Is.True);
                    Assert.That(d.DeviceStatusResponses.Status.Topic, Does.StartWith("nspanel"));
                    Assert.That(d.DeviceStatusResponses.Status.QueryTime, Is.Not.EqualTo(default(DateTime)));
                });
            });
        }

        [Test]
        public void StatusQueryFailTests()
        {
            this.mockHttp.When("http://*/cm?cmnd=status")
                .Respond("application/json", new string('.', 15));

            Device d = new(IPAddress.Parse("192.168.0.0"))
            {
                httpHandler = this.mockHttp
            };

            Assert.DoesNotThrowAsync(async () =>
            {
                Assert.That(d.DeviceStatusResponses.Status, Is.Null);
                await d.GetStatus();
                Assert.Multiple(() =>
                {
                    Assert.That(d.DeviceStatusResponses.Status, Is.Null);
                });
            });
        }

        [Test]
        public void StatusQuery2FirmwareTests()
        {
            this.mockHttp.When("http://*/cm?cmnd=status%202")
                .Respond("application/json", this.status2firmwareResponse);

            Device d = new(IPAddress.Parse("192.168.0.0"))
            {
                httpHandler = this.mockHttp
            };

            Assert.DoesNotThrowAsync(async () =>
            {
                Assert.That(d.DeviceStatusResponses.Firmware, Is.Null);
                await d.GetStatus2Firmware();
                Assert.Multiple(() =>
                {
                    Assert.That(d.DeviceStatusResponses.Firmware, Is.Not.Null);
                    Assert.That(d.DeviceStatusResponses.Firmware.Core, Is.Not.Null);
                    Assert.That(d.DeviceStatusResponses.Firmware.Core, Is.EqualTo("2_0_7"));
                    Assert.That(d.DeviceStatusResponses.Firmware.CpuFrequency, Is.EqualTo(160));
                    Assert.That(d.DeviceStatusResponses.Firmware.Version, Is.Not.Null);
                    Assert.That(d.DeviceStatusResponses.Firmware.Version, Does.StartWith("12.5.0"));
                    Assert.That(d.DeviceStatusResponses.Firmware.QueryTime, Is.Not.EqualTo(default(DateTime)));
                });
            });
        }

        [Test]
        public void StatusQuery2FirmwareFailTests()
        {
            this.mockHttp.When("http://*/cm?cmnd=status%202")
                .Respond("application/json", new string('.', 15));

            Device d = new(IPAddress.Parse("192.168.0.0"))
            {
                httpHandler = this.mockHttp
            };

            Assert.DoesNotThrowAsync(async () =>
            {
                Assert.That(d.DeviceStatusResponses.Firmware, Is.Null);
                await d.GetStatus2Firmware();
                Assert.Multiple(() =>
                {
                    Assert.That(d.DeviceStatusResponses.Firmware, Is.Null);
                });
            });
        }

        [Test]
        public void StatusQuery7TimeTests()
        {
            this.mockHttp.When("http://*/cm?cmnd=status%207")
                .Respond("application/json", this.status7timeResponse);

            Device d = new(IPAddress.Parse("192.168.0.0"))
            {
                httpHandler = this.mockHttp
            };

            Assert.DoesNotThrowAsync(async () =>
            {
                Assert.That(d.DeviceStatusResponses.Time, Is.Null);
                await d.GetStatus7Time();
                Assert.Multiple(() =>
                {
                    Assert.That(d.DeviceStatusResponses.Time, Is.Not.Null);
                    Assert.That(d.DeviceStatusResponses.Time.Timezone, Is.EqualTo(99));
                    Assert.That(d.DeviceStatusResponses.Time.Utc, Is.Not.EqualTo(default(DateTime)));
                    Assert.That(d.DeviceStatusResponses.Time.Local, Is.Not.EqualTo(default(DateTime)));
                    Assert.That(d.DeviceStatusResponses.Time.StartDst, Is.Not.EqualTo(default(DateTime)));
                    Assert.That(d.DeviceStatusResponses.Time.EndDst, Is.Not.EqualTo(default(DateTime)));
                    Assert.That(d.DeviceStatusResponses.Time.Sunrise, Is.EqualTo(new TimeOnly(7, 42)));
                    Assert.That(d.DeviceStatusResponses.Time.Sunset, Is.EqualTo(new TimeOnly(19, 40)));
                    Assert.That(d.DeviceStatusResponses.Time.QueryTime, Is.Not.EqualTo(default(DateTime)));
                });
            });
        }

        [Test]
        public void StatusQuery7TimeFailTests()
        {
            this.mockHttp.When("http://*/cm?cmnd=status%207")
                .Respond("application/json", new string('.', 15));

            Device d = new(IPAddress.Parse("192.168.0.0"))
            {
                httpHandler = this.mockHttp
            };

            Assert.DoesNotThrowAsync(async () =>
            {
                Assert.That(d.DeviceStatusResponses.Time, Is.Null);
                await d.GetStatus7Time();
                Assert.Multiple(() =>
                {
                    Assert.That(d.DeviceStatusResponses.Time, Is.Null);
                });
            });
        }

        [Test]
        public void StatusQuery10SensorsTests()
        {
            this.mockHttp.When("http://*/cm?cmnd=status%2010")
                .Respond("application/json", this.status10sensorsResponse);

            Device d = new(IPAddress.Parse("192.168.0.0"))
            {
                httpHandler = this.mockHttp
            };

            Assert.DoesNotThrowAsync(async () =>
            {
                Assert.That(d.DeviceStatusResponses.Sensors, Is.Null);
                await d.GetStatus10Sensors();
                Assert.Multiple(() =>
                {
                    Assert.That(d.DeviceStatusResponses.Sensors, Is.Not.Null);
                    Assert.That(d.DeviceStatusResponses.Sensors.Temperature1, Is.EqualTo(24.4f));
                    Assert.That(d.DeviceStatusResponses.Sensors.TempUnit, Is.EqualTo('C'));
                    Assert.That(d.DeviceStatusResponses.Sensors.Time, Is.Not.EqualTo(default(DateTime)));
                    Assert.That(d.DeviceStatusResponses.Sensors.QueryTime, Is.Not.EqualTo(default(DateTime)));
                });
            });
        }

        [Test]
        public void StatusQuery10SensorsFailTests()
        {
            this.mockHttp.When("http://*/cm?cmnd=status%2010")
                .Respond("application/json", new string('.', 15));

            Device d = new(IPAddress.Parse("192.168.0.0"))
            {
                httpHandler = this.mockHttp
            };

            Assert.DoesNotThrowAsync(async () =>
            {
                Assert.That(d.DeviceStatusResponses.Sensors, Is.Null);
                await d.GetStatus7Time();
                Assert.Multiple(() =>
                {
                    Assert.That(d.DeviceStatusResponses.Sensors, Is.Null);
                });
            });
        }

        [TearDown]
        public void TearDown()
        {
            this.mockHttp?.Dispose();
        }
    }
}
