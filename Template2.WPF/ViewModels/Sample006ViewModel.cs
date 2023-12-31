using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;
using Template2.Infrastructure.Aws;
using Template2.WPF.Services;

namespace Template2.WPF.ViewModels
{
    public class Sample006ViewModel : ViewModelBase
    {
        const int MinInferenceUnits = 1;
        const string InitialFilePath = @"C:\Repos\template2-cs\Fake\l4v\download.png";

        private LookoutforVisionController _lookoutforVisionController;

        public Sample006ViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MainWindowSetSubTitleEvent>().Publish("> サンプル006（AWS 操作）");

            StartModelButton = new DelegateCommand(StartModelButtonExecute);
            StopModelButton = new DelegateCommand(StopModelButtonExecute);
            DescribeModelButton = new DelegateCommand(DescribeModelButtonExecute);
            DetectAnomaliesButton = new DelegateCommand(DetectAnomaliesButtonExecute);
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private string _projectNameText = "l4v_test_project";
        public string ProjectNameText
        {
            get { return _projectNameText; }
            set { SetProperty(ref _projectNameText, value); }
        }

        private string _modelVersionText = "1";
        public string ModelVersionText
        {
            get { return _modelVersionText; }
            set { SetProperty(ref _modelVersionText, value); }
        }

        private string _lookoutforVisionStatusLabel = "Unknown";
        public string LookoutforVisionStatusLabel
        {
            get { return _lookoutforVisionStatusLabel; }
            set { SetProperty(ref _lookoutforVisionStatusLabel, value); }
        }

        private string _imagePathText = InitialFilePath;
        public string ImagePathText
        {
            get { return _imagePathText; }
            set { SetProperty(ref _imagePathText, value); }
        }

        private string _detectAnomaliesLabel;
        public string DetectAnomaliesLabel
        {
            get { return _detectAnomaliesLabel; }
            set { SetProperty(ref _detectAnomaliesLabel, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand StartModelButton { get; }

        private async void StartModelButtonExecute()
        {
            if (LookoutforVisionStatusLabel != "TRAINED")
            {
                return;
            }

            await Task.Run(() => DescribeModelButtonExecute());
            await Task.Run(() => _lookoutforVisionController.StartModelAsync());
        }

        public DelegateCommand StopModelButton { get; }

        private async void StopModelButtonExecute()
        {
            if (LookoutforVisionStatusLabel != "HOSTED")
            {
                return;
            }

            await Task.Run(() => DescribeModelButtonExecute());
            await Task.Run(() => _lookoutforVisionController.StopModelAsync());
        }
        public DelegateCommand DescribeModelButton { get; }
        private async void DescribeModelButtonExecute()
        {
            _lookoutforVisionController = new LookoutforVisionController(ProjectNameText, ModelVersionText, MinInferenceUnits);
            LookoutforVisionStatusLabel = await _lookoutforVisionController.DescribeModelStatusAsync();
        }
        public DelegateCommand DetectAnomaliesButton { get; }

        private async void DetectAnomaliesButtonExecute()
        {
            if (LookoutforVisionStatusLabel != "HOSTED")
            {
                return;
            }

            await Task.Run(() => DescribeModelButtonExecute());
            var result = await Task.Run(() => _lookoutforVisionController.DetectAnomaliesIsAnomalousAsync(ImagePathText));
            var result2 = await Task.Run(() => _lookoutforVisionController.DetectAnomaliesConfidenceAsync(ImagePathText));

            DetectAnomaliesLabel = result.ToString() + " / " + result2.ToString();
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion
    }
}
