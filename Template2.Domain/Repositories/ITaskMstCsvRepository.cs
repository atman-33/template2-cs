using Template2.Domain.Entities;

namespace Template2.Domain.Repositories
{
    public interface ITaskMstCsvRepository
    {
        IReadOnlyList<TaskMstEntity> GetData(string filePath);
    }
}
