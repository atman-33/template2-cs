using Moq;
using Prism.Events;
using Prism.Regions;
using System.Reflection;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;
using Template2.WPF.Events;
using Template2.WPF.ViewModels;

namespace Template2Test.Tests.ViewModelTests
{
    [TestClass]
    public class Sample005ViewModelTest
    {
        [TestMethod]
        public void シナリオ()
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock.Setup(
                x => x.GetEvent<MainWindowSetSubTitleEvent>().Publish(It.IsAny<string>())).Callback<string>(
                    subTitle =>
                        {
                            //Assert.AreEqual("サブタイトル名称", subTitle);
                        });

            var workerGroupMstRepositoryMock = new Mock<IWorkerGroupMstRepository>();
            workerGroupMstRepositoryMock.Setup(x => x.GetData()).Returns(new List<WorkerGroupMstEntity>());

            var workerMstRepositoryMock = new Mock<IWorkerMstRepository>();
            workerMstRepositoryMock.Setup(x => x.GetData()).Returns(new List<WorkerMstEntity>());

            var vm = new Sample005ViewModel(
                eventAggregatorMock.Object,
                workerGroupMstRepositoryMock.Object,
                workerMstRepositoryMock.Object);

            var _eventAggregatorInfo = typeof(Sample005ViewModel).GetField("_eventAggregator", BindingFlags.NonPublic | BindingFlags.Instance);
            var _eventAggregator = (IEventAggregator?)_eventAggregatorInfo?.GetValue(vm);
            Assert.IsNotNull(_eventAggregator);
        }
    }
}
