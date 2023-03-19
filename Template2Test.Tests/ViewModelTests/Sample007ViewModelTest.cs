using Moq;
using Prism.Events;
using Prism.Services.Dialogs;
using System.Reflection;
using Template2.WPF.Events;
using Template2.WPF.ViewModels;

namespace Template2Test.Tests.ViewModelTests
{
    [TestClass]
    public class Sample007ViewModelTest
    {
        [TestMethod]
        public void シナリオ()
        {
            var dialogServiceMock = new Mock<IDialogService>();

            var eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock.Setup(
                x => x.GetEvent<MainWindowSetSubTitleEvent>().Publish(It.IsAny<string>())).Callback<string>(
                    subTitle =>
                    {
                        //Assert.AreEqual("サブタイトル名称", subTitle);
                    });

            var vm = new Sample007ViewModel(dialogServiceMock.Object,eventAggregatorMock.Object);

            var _eventAggregatorInfo = typeof(Sample007ViewModel).GetField("_eventAggregator", BindingFlags.NonPublic | BindingFlags.Instance);
            var _eventAggregator = (IEventAggregator?)_eventAggregatorInfo?.GetValue(vm);
            Assert.IsNotNull(_eventAggregator);
        }
    }
}
