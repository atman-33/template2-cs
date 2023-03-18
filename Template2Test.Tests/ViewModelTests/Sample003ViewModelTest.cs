using Moq;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;
using Template2.WPF.Events;
using Template2.WPF.Services;
using Template2.WPF.ViewModels;

namespace Template2Test.Tests.ViewModelTests
{
    [TestClass]
    public class Sample003ViewModelTest
    {
        [TestMethod]
        public void シナリオ()
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var messageServiceMock = new Mock<IMessageService>();

            var workingTimePlanMstRepositoryMock = new Mock<IWorkingTimePlanMstRepository>();
            var workerMstRepositoryMock = new Mock<IWorkerMstRepository>();

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

            var workingTimePlanMstEntities = new List<WorkingTimePlanMstEntity>();
            workingTimePlanMstRepositoryMock.Setup(x => x.GetDataWithWorkerName()).Returns(workingTimePlanMstEntities);

            var workerMstEntities = new List<WorkerMstEntity>();
            workerMstRepositoryMock.Setup(x => x.GetData()).Returns(workerMstEntities);

            var vm = new Sample003ViewModel(
                eventAggregatorMock.Object,
                messageServiceMock.Object,
                workingTimePlanMstRepositoryMock.Object,
                workerMstRepositoryMock.Object);

            var _eventAggregatorInfo = typeof(Sample003ViewModel).GetField("_eventAggregator", BindingFlags.NonPublic | BindingFlags.Instance);
            var _eventAggregator = (IEventAggregator?)_eventAggregatorInfo?.GetValue(vm);
            Assert.IsNotNull(_eventAggregator);

            var _messageServiceInfo = typeof(Sample003ViewModel).GetField("_messageService", BindingFlags.NonPublic | BindingFlags.Instance);
            var _messageService = (IMessageService?)_messageServiceInfo?.GetValue(vm);
            Assert.IsNotNull(_messageService);
        }
    }
}
