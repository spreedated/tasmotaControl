using NUnit.Framework;
using System;
using System.Net;
using TasmotaQuery;

namespace UnitTests
{
    [TestFixture]
    public class ConstructorTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void ConstructorCreateTests()
        {
            Assert.Multiple(() =>
            {
                Assert.DoesNotThrow(() =>
                {
                    _ = new Device("192.168.0.1");
                });
                Assert.DoesNotThrow(() =>
                {
                    _ = new Device(IPAddress.Parse("192.168.0.1"));
                });
                Assert.DoesNotThrow(() =>
                {
                    _ = new Device("192.168.0.1", new());
                });
                Assert.DoesNotThrow(() =>
                {
                    _ = new Device("192.168.0.1", new()
                    {
                        IsShutter = true,
                        HasTemperaturesensor = true
                    });
                });
                Assert.DoesNotThrow(() =>
                {
                    _ = new Device(IPAddress.Parse("192.168.0.1"), new());
                });
            });

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    _ = new Device("192.168.X.1");
                });
                Assert.Throws<ArgumentException>(() =>
                {
                    _ = new Device("192.168.0.1.1");
                });
                Assert.Throws<ArgumentException>(() =>
                {
                    _ = new Device("192");
                });
                Assert.Throws<ArgumentException>(() =>
                {
                    _ = new Device("192.147");
                });
                Assert.Throws<ArgumentException>(() =>
                {
                    _ = new Device("192.168.256.1");
                });
                Assert.Throws<ArgumentException>(() =>
                {
                    _ = new Device("192.168.256.1", new());
                });
                Assert.Throws<ArgumentException>(() =>
                {
                    _ = new Device("192.168.x.1", new());
                });
                Assert.Throws<ArgumentException>(() =>
                {
                    _ = new Device("192", new());
                });
                Assert.Throws<ArgumentException>(() =>
                {
                    _ = new Device("192.147", new());
                });
                Assert.Throws<ArgumentNullException>(() =>
                {
                    _ = new Device("");
                });
                Assert.Throws<ArgumentNullException>(() =>
                {
                    _ = new Device("", new());
                });
                Assert.Throws<ArgumentNullException>(() =>
                {
                    _ = new Device((string)null);
                });
                Assert.Throws<ArgumentNullException>(() =>
                {
                    _ = new Device((string)null, new());
                });
            });
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
