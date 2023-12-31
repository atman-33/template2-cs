using Moq;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Reflection;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;
using Template2.WPF.Services;
using Template2.WPF.ViewModels;

namespace Template2Test.Tests.ViewModelTests
{
    [TestClass]
    public class Sample004PageEditingViewModelTest
    {
        [TestMethod]
        public void シナリオ()
        {
            var regionManagerMock = new Mock<IRegionManager>();
            var dialogServiceMock = new Mock<IDialogService>();

            var eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock.Setup(
                x => x.GetEvent<MainWindowSetSubTitleEvent>().Publish(It.IsAny<string>())).Callback<string>(
                    subTitle =>
                    {
                        //Assert.AreEqual("サブタイトル名称", subTitle);
                    });

            var messageServiceMock = new Mock<IMessageService>();
            messageServiceMock.Setup(
                x => x.Question(It.IsAny<string>())).Callback<string>(
                    message =>
                    {
                        //Assert.AreEqual("メッセージ", message);
                    });

            var pageMstRepositoryMock = new Mock<IPageMstRepository>();
            pageMstRepositoryMock.Setup(x => x.GetData()).Returns(new List<PageMstEntity>());

            var vm = new Sample004PageEditingViewModel(
                regionManagerMock.Object,
                messageServiceMock.Object,
                pageMstRepositoryMock.Object);

            var _messageServiceInfo = typeof(Sample004PageEditingViewModel).GetField("_messageService", BindingFlags.NonPublic | BindingFlags.Instance);
            var _messageService = (IMessageService?)_messageServiceInfo?.GetValue(vm);
            Assert.IsNotNull(_messageService);
        }
    }
}
