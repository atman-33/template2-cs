using Template2.Domain.Entities;

namespace Template2.WPF.Dto.Input
{
    internal class Sample004PageEditingViewModelInputDto
    {
        public Sample004PageEditingViewModelInputDto(
            bool isNewPage,
            PageMstEntity? entityToEdit)
        {
            IsNewPage = isNewPage;
            PageMstEntityToEdit = entityToEdit;
        }

        public bool IsNewPage { get; private set; }
        public PageMstEntity? PageMstEntityToEdit { get; private set; }
    }
}
