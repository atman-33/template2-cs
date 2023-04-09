using System.Text.Json.Serialization;
using Template2.Domain.Modules.Objects;
using Template2.Domain.ValueObjects;

namespace Template2.Domain.Entities
{
    public sealed class WorkerGroupMstEntity
    {

        public WorkerGroupMstEntity(
            string workerGroupCode,
            string workerGroupName)
        {
            WorkerGroupCode = new WorkerGroupCode(workerGroupCode);
            WorkerGroupName = new WorkerGroupName(workerGroupName);
        }

        [JsonPropertyName("workerGroupCode")]
        [JsonConverter(typeof(ValueObjectConverter<WorkerGroupCode>))]
        public WorkerGroupCode WorkerGroupCode { get; set; }

        [JsonPropertyName("workerGroupName")]
        [JsonConverter(typeof(ValueObjectConverter<WorkerGroupName>))]
        public WorkerGroupName WorkerGroupName { get; set; }

    }
}
