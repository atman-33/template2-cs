using Template2.Domain.Entities;

namespace Template2.WPF.ViewModels
{
    public class Sample001ViewModelSampleMst
    {
        private SampleMstEntity _entity;

        public Sample001ViewModelSampleMst(SampleMstEntity entity)
        {
            _entity = entity;
        }

        public string SampleCode => _entity.SampleCode.Value;
        public string SampleName => _entity.SampleName.Value;

        public SampleMstEntity Entity { get { return _entity; } }
    }
}
