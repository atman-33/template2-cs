using Template2.Domain.ValueObjects;

namespace Template2.Domain.Entities
{
    public sealed class WorkingTimePlanMstEntity
    {

        public WorkingTimePlanMstEntity(
            string workerCode,
            int weekday,
            float? workingTime)
            : this(workerCode, weekday, workingTime, string.Empty)
        {
        }

        public WorkingTimePlanMstEntity(
            string workerCode,
            int weekday,
            float? workingTime,
            string workerName)
        {
            WorkerCode = new WorkerCode(workerCode);
            Weekday = new Weekday(weekday);
            WorkingTime = new WorkingTime(workingTime);

            WorkerName = new WorkerName(workerName);
        }


        public WorkerCode WorkerCode { get; }
        public Weekday Weekday { get; }
        public WorkingTime WorkingTime { get; }

        public WorkerName WorkerName { get; }

    }
}
