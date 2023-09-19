using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TasmotaQuery;

namespace UnitTests
{
    [TestFixture]
    public class DeviceQueryTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void StateQueryTests()
        {
            IDevice d = new Device(IPAddress.Parse("192.168.10.30"));

            Assert.DoesNotThrowAsync(async () =>
            {
                
                await d.IsAvailable();
                Assert.That(d.IsAvaiable, Is.True);
                await d.GetState();
                Assert.That(d.State, Is.Not.Null);
            });
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
