using System;
using NUnit.Framework;

namespace Funq.Test
{
    [TestFixture]
    public class ServiceKeyFixture
    {
        [Test]
        public void KeyNotEqualsNull()
        {
            var key1 = new ServiceKey(typeof(Func<IDisposable>), null);

            Assert.AreNotEqual(key1, null);
            Assert.IsFalse(key1.Equals(null));
        }

        [Test]
        public void KeyNotEqualsOtherType()
        {
            var key1 = new ServiceKey(typeof(Func<IDisposable>), null);

            Assert.AreNotEqual(key1, new object());
            Assert.IsFalse(key1.Equals(new object()));
        }

        [Test]
        public void KeyEqualsSameReference()
        {
            var key1 = new ServiceKey(typeof(Func<IDisposable>), null);
            var key2 = key1;

            Assert.AreSame(key1, key2);
            Assert.IsTrue(ServiceKey.Equals(key1, key2));
            Assert.IsTrue(key1.Equals(key2));
        }

        [Test]
        public void KeysAreEqualByType()
        {
            var key1 = new ServiceKey(typeof(Func<IDisposable>), null);
            var key2 = new ServiceKey(typeof(Func<IDisposable>), null);

            Assert.AreEqual(key1, key2);
            Assert.AreEqual(key1.GetHashCode(), key2.GetHashCode());
            Assert.IsTrue(ServiceKey.Equals(key1, key2));
            Assert.IsTrue(key1.Equals(key2));
        }

        [Test]
        public void KeysAreEqualByTypeAndName()
        {
            var key1 = new ServiceKey(typeof(Func<IDisposable>), "foo");
            var key2 = new ServiceKey(typeof(Func<IDisposable>), "foo");

            Assert.AreEqual(key1, key2);
            Assert.AreEqual(key1.GetHashCode(), key2.GetHashCode());
            Assert.IsTrue(ServiceKey.Equals(key1, key2));
            Assert.IsTrue(key1.Equals(key2));
        }

        [Test]
        public void KeysNotEqualByName()
        {
            var key1 = new ServiceKey(typeof(Func<IDisposable>), "foo");
            var key2 = new ServiceKey(typeof(Func<IDisposable>), "bar");

            Assert.AreNotEqual(key1, key2);
            Assert.AreNotEqual(key1.GetHashCode(), key2.GetHashCode());
            Assert.IsFalse(ServiceKey.Equals(key1, key2));
        }
    }}

