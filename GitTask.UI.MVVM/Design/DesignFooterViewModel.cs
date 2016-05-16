using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Project;

namespace GitTask.UI.MVVM.Design
{
    public class DesignFooterViewModel : ViewModelBase // based on GitTask.UI.MVVM.ViewModel.Footer.FooterViewModel
    {
        public string ProjectName => "Project 1";
        public ProjectMember CurrentUser { get; }

        public DesignFooterViewModel()
        {
            CurrentUser = new DesignProjectMember();
        }
    }
}