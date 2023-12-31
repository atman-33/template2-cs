using Moq;
using Prism.Events;
using System.Reflection;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;
using Template2.WPF.Services;
using Template2.WPF.ViewModels;

namespace Template2Test.Tests.ViewModelTests
{
    [TestClass]
    public class Sample002ViewModelTest
    {
        [TestMethod]
        public void シナリオ()
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var messageServiceMock = new Mock<IMessageService>();
            var workerMstRepositoryMock = new Mock<IWorkerMstRepository>();
            var workerGroupMstRepositoryMock = new Mock<IWorkerGroupMstRepository>();

            eventAggregatorMock.Setup(
                x => x.GetEvent<MainWindowSetSubTitleEvent>().Publish(It.IsAny<string>())).Callback<string>(
                    subTitle =>
                    {
                        //Assert.AreEqual("サブタイトル名称", subTitle);
                    });

            messageServiceMock.Setup(
                x => x.Question(It.IsAny<string>())).Callback<string>(
                    message =>
                    {
                        //Assert.AreEqual("メッセージ", message);
                    });

            var workerMstEntities = new List<WorkerMstEntity>();
            workerMstRepositoryMock.Setup(x => x.GetData()).Returns(workerMstEntities);

            var workerGroupMstEntities = new List<WorkerGroupMstEntity>();
            workerGroupMstRepositoryMock.Setup(x => x.GetData()).Returns(workerGroupMstEntities);

            var vm = new Sample002ViewModel(
                eventAggregatorMock.Object,
                messageServiceMock.Object,
                workerMstRepositoryMock.Object,
                workerGroupMstRepositoryMock.Object);

            var _eventAggregatorInfo = typeof(Sample002ViewModel).GetField("_eventAggregator", BindingFlags.NonPublic | BindingFlags.Instance);
            var _eventAggregator = (IEventAggregator?)_eventAggregatorInfo?.GetValue(vm);
            Assert.IsNotNull(_eventAggregator);

            var _messageServiceInfo = typeof(Sample002ViewModel).GetField("_messageService", BindingFlags.NonPublic | BindingFlags.Instance);
            var _messageService = (IMessageService?)_messageServiceInfo?.GetValue(vm);
            Assert.IsNotNull(_messageService);

        }
    }
}
