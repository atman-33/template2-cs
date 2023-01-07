using Template2.Domain.ValueObjects;

namespace Template2.Domain.Entities
{
    public sealed class SampleMstEntity
    {

        public SampleMstEntity(
            string sampleCode,
            string sampleName)
        {
            SampleCode = new SampleCode(sampleCode);
            SampleName = new SampleName(sampleName);
        }

        public SampleCode SampleCode { get; }
        public SampleName SampleName { get; }

    }
}
