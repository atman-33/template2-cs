using System.Text.Json;
using System.Text;
using Template2.Domain.Entities;
using Template2.Domain.Modules.Helpers;
using Template2.Domain.Repositories;
using Template2.Domain.Modules.Objects;
using Template2.Domain.ValueObjects;

namespace Template2.Infrastructure.RestApi
{
    public class WorkerGroupMstRestApi : IWorkerGroupMstRepositoryAsync
    {
        private const string RequestUri = "http://localhost:3000/api/worker-group-mst";

        public async Task<IReadOnlyList<WorkerGroupMstEntity>> GetDataAsync()
        {
            return await RestApiHelper.Get(RequestUri,
                jsonElement =>
                {
                    return new WorkerGroupMstEntity(
                        jsonElement.GetProperty(
                            JsonHelper.GetJsonPropertyName(
                                typeof(WorkerGroupMstEntity), nameof(WorkerGroupMstEntity.WorkerGroupCode))).GetString() ?? string.Empty,
                        jsonElement.GetProperty(
                            JsonHelper.GetJsonPropertyName(
                                typeof(WorkerGroupMstEntity), nameof(WorkerGroupMstEntity.WorkerGroupName))).GetString() ?? string.Empty
                        );
                });
        }

        public async void SaveAsync(WorkerGroupMstEntity entity)
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new ValueObjectConverter<WorkerGroupCode>(), new ValueObjectConverter<WorkerGroupName>() },
            };

            var json = JsonSerializer.Serialize(entity, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await RestApiHelper.PostAsync(RequestUri, content);
        }

        public async void DeleteAsync(WorkerGroupMstEntity entity)
        {
            var query = JsonHelper.GetJsonPropertyName(typeof(WorkerGroupMstEntity), nameof(WorkerGroupMstEntity.WorkerGroupCode))
                        + "=" + entity.WorkerGroupCode.Value;

            await RestApiHelper.DeleteAsync(RequestUri, query);
        }
    }
}
