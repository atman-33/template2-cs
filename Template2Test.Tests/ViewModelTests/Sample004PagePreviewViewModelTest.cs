using Template2.WPF.ViewModels;

namespace Template2Test.Tests.ViewModelTests
{
    [TestClass]
    public class Sample004PagePreviewViewModelTest
    {
        [TestMethod]
        public void シナリオ()
        {
            var vm = new Sample004PagePreviewViewModel();

            Assert.AreEqual(false, vm.ShowPreviewImmediately);
        }
    }
}
