using System.Collections.Generic;
using System.Data;
using System.Linq;
using Template2.Domain.Entities;
using Template2.Domain.Modules.Helpers;

namespace Template2.WPF.ViewModels
{
    public class Sample002ViewModelWorkingTimePlanMst
    {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Sample002ViewModelWorkingTimePlanMst(List<WorkingTimePlanMstEntity> workingTimePlanMstEntities)
        {
            DataView = DataViewHelper.CreatePivotTable<WorkingTimePlanMstEntity, float?>(
                "作業者",
                workingTimePlanMstEntities.ToLookup(o => o.WorkerCode.Value),
                getColumn => { return getColumn.Weekday.Value.ToString(); },
                getValue => { return getValue.WorkingTime.Value; });
        }

        /// <summary>
        /// ViewにバインドするDataView
        /// </summary>
        public DataView DataView { get; set; }
    }
}
