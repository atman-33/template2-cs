using Template2.Domain.Entities;

namespace Template2Test.Tests
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestMethod1()
        {
            var entities = new List<WorkingTimePlanMstEntity>();
            entities.Add(new WorkingTimePlanMstEntity("A‚³‚ñ", 0, 1.2f));
            entities.Add(new WorkingTimePlanMstEntity("B‚³‚ñ", 1, 1.4f));
            entities.Add(new WorkingTimePlanMstEntity("C‚³‚ñ", 2, 1.6f));

        }
    }
}