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

    public class Sample001ViewModelTest
    {
        [TestMethod]
        public void シナリオ()
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var messageServiceMock = new Mock<IMessageService>();
            var workGroupMstRepositoryMock = new Mock<IWorkerGroupMstRepository>();

            eventAggregatorMock.Setup(
                x => x.GetEvent<MainWindowSetSubTitleEvent>().Publish(It.IsAny<string>())).Callback<string>(
                    subTitle =>
                    {
                        Assert.AreEqual("> サンプル001（読み取り専用DataGrid）", subTitle);
                    });

            messageServiceMock.Setup(
                x => x.Question(It.IsAny<string>())).Callback<string>(
                    message =>
                    {
                        Assert.AreEqual("保存しますか？", message);
                    });
            
            var entities = new List<WorkerGroupMstEntity>()
            {
                new WorkerGroupMstEntity("A", "花"),
                new WorkerGroupMstEntity("B", "月"),
                new WorkerGroupMstEntity("C", "雪"),
            };

            workGroupMstRepositoryMock.Setup(x => x.GetData()).Returns(entities);

            var vm = new Sample001ViewModel(
                eventAggregatorMock.Object, 
                messageServiceMock.Object,
                workGroupMstRepositoryMock.Object);

            var _eventAggregatorInfo = typeof(Sample001ViewModel).GetField("_eventAggregator", BindingFlags.NonPublic | BindingFlags.Instance);
            var _eventAggregator = (IEventAggregator?)_eventAggregatorInfo?.GetValue(vm);
            Assert.IsNotNull(_eventAggregator);

            var _messageServiceInfo = typeof(Sample001ViewModel).GetField("_messageService", BindingFlags.NonPublic | BindingFlags.Instance);
            var _messageService = (IMessageService?)_messageServiceInfo?.GetValue(vm);
            Assert.IsNotNull(_messageService);

            vm.WorkerGroupCodeText = "D";
            vm.WorkerGroupNameText = "松";

            vm.SaveButton.Execute();
        }
    }
}
