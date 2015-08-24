using NUnit.Framework;

namespace ORM.UnitTests
{
    public abstract class BaseFixture
    {
        [TestFixtureSetUp]
        public void Startup()
        {
            Arrange();
            Act();
        }

        public abstract void Arrange();

        public abstract void Act();
    }
}
