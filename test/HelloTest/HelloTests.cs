using System;

namespace Hello.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class Tests
    {

        private Hello subject;

        [SetUp]
        public void Setup() {
            subject = new Hello();
        }

        [Test]
        public void itShouldSayHi()
        {
            Assert.AreEqual("Hello World", subject.SayHi());
        }
    }
}
