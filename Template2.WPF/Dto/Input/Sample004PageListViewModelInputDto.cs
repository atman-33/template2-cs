using Template2.Domain.Entities;

namespace Template2.WPF.Dto.Input
{
    internal class Sample004PageListViewModelInputDto
    {
        public Sample004PageListViewModelInputDto(
            PageMstEntity? pageMstEntityToSave,
            PageMstEntity? pageMstEntityToDelete)
        {
            PageMstEntityToSave = pageMstEntityToSave;
            PageMstEntityToDelete = pageMstEntityToDelete;
        }


        public PageMstEntity? PageMstEntityToSave { get; private set; }
        public PageMstEntity? PageMstEntityToDelete { get; private set; }
    }
}
