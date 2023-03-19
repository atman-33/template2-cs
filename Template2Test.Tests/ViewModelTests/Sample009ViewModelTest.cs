using Moq;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Template2.Domain.Repositories;
using Template2.WPF.Events;
using Template2.WPF.ViewModels;

namespace Template2Test.Tests.ViewModelTests
{
    [TestClass]
    public class Sample009ViewModelTest
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

            var taskMstExcelRepositoryMock = new Mock<ITaskMstExcelRepository>();

            var vm = new Sample009ViewModel(eventAggregatorMock.Object, taskMstExcelRepositoryMock.Object);

            var _eventAggregatorInfo = typeof(Sample009ViewModel).GetField("_eventAggregator", BindingFlags.NonPublic | BindingFlags.Instance);
            var _eventAggregator = (IEventAggregator?)_eventAggregatorInfo?.GetValue(vm);
            Assert.IsNotNull(_eventAggregator);
        }
    }
}
