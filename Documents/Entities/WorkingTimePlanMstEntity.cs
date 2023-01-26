using Template2.Domain.ValueObjects;

namespace Template2.Domain.Entities
{
    public sealed class WorkingTimePlanMstEntity
    {

        public WorkingTimePlanMstEntity(
            string workerCode,
			int weekday,
			float? workingTime)
        {
            WorkerCode = new WorkerCode(workerCode);
			Weekday = new Weekday(weekday);
			WorkingTime = new WorkingTime(workingTime);
        }

        public WorkerCode WorkerCode { get; }
public Weekday Weekday { get; }
public WorkingTime WorkingTime { get; }

    }
}
