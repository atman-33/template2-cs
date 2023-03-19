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
using Template2.WPF.ViewModels;

namespace Template2Test.Tests.ViewModelTests
{
    [TestClass]
    public class Sample006ViewModelTest
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

            var vm = new Sample006ViewModel(eventAggregatorMock.Object);
            
            var _eventAggregatorInfo = typeof(Sample006ViewModel).GetField("_eventAggregator", BindingFlags.NonPublic | BindingFlags.Instance);
            var _eventAggregator = (IEventAggregator?)_eventAggregatorInfo?.GetValue(vm);
            Assert.IsNotNull(_eventAggregator);
        }
    }
}
