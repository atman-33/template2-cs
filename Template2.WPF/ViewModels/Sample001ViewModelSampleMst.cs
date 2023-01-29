using Template2.Domain.Entities;

namespace Template2.WPF.ViewModels
{
    public class Sample001ViewModelSampleMst
    {
        public Sample001ViewModelSampleMst(SampleMstEntity entity)
        {
            Entity = entity;
        }
        public SampleMstEntity Entity { get; private set; }

        public string SampleCode => Entity.SampleCode.Value;
        public string SampleName => Entity.SampleName.Value;
    }
}
