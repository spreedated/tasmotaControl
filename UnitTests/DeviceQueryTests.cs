using NUnit.Framework;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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

        [SetUp]
        public void SetUp()
        {
            this.mockHttp = new();

            this.stateResponse = LoadJson("State");
            this.statusResponse = LoadJson("Status");
            this.status2firmwareResponse = LoadJson("Firmware");
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
                    Assert.That(d.State.Wifi.Downtime, Is.Not.EqualTo(default(TimeSpan)));
                    Assert.That(d.State.Wifi.Downtime.Minutes, Is.EqualTo(13));
                    Assert.That(d.State.Uptime.Minutes, Is.EqualTo(14));
                    Assert.That(d.State.Uptime.Hours, Is.EqualTo(20));
                    Assert.That(d.State.Uptime.Days, Is.EqualTo(26));
                    Assert.That(d.State.Uptime, Is.Not.EqualTo(default(TimeSpan)));
                    Assert.That(d.State, Is.Not.Null);
                    Assert.That(d.State.Heap, Is.EqualTo(121));
                    Assert.That(d.State.Wifi, Is.Not.Null);
                    Assert.That(d.State.Wifi.Channel, Is.EqualTo(1));
                    Assert.That(d.State.Wifi.Mode, Is.EqualTo("11n"));
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
                    Assert.That(d.State, Is.Null);
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
                Assert.That(d.Status, Is.Null);
                await d.GetStatus();
                Assert.Multiple(() =>
                {
                    Assert.That(d.Status, Is.Not.Null);
                    Assert.That(d.Status.FriendlyNames, Is.Not.Empty);
                    Assert.That(d.Status.FriendlyNames.Count, Is.EqualTo(2));
                    Assert.That(d.Status.FriendlyNames[1], Is.Not.Null);
                    Assert.That(string.IsNullOrEmpty(d.Status.FriendlyNames[1]), Is.True);
                    Assert.That(d.Status.Topic, Does.StartWith("nspanel"));
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
                Assert.That(d.Status, Is.Null);
                await d.GetStatus();
                Assert.Multiple(() =>
                {
                    Assert.That(d.Status, Is.Null);
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
                Assert.That(d.Firmware, Is.Null);
                await d.GetStatus2Firmware();
                Assert.Multiple(() =>
                {
                    Assert.That(d.Firmware, Is.Not.Null);
                    Assert.That(d.Firmware.Core, Is.Not.Null);
                    Assert.That(d.Firmware.Core, Is.EqualTo("2_0_7"));
                    Assert.That(d.Firmware.CpuFrequency, Is.EqualTo(160));
                    Assert.That(d.Firmware.Version, Is.Not.Null);
                    Assert.That(d.Firmware.Version, Does.StartWith("12.5.0"));
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
                Assert.That(d.Firmware, Is.Null);
                await d.GetStatus2Firmware();
                Assert.Multiple(() =>
                {
                    Assert.That(d.Firmware, Is.Null);
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
