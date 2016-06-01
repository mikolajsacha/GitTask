using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Project;

namespace GitTask.UI.MVVM.Design
{
    public class DesignProjectMembersViewModel : ViewModelBase // based on GitTask.UI.MVVM.ViewModel.Elements.ProjectMembersViewModel
    {
        public ObservableCollection<ProjectMember> ProjectMembers { get; }
        public bool IsLoading => false;

        public DesignProjectMembersViewModel()
        {
            ProjectMembers = new ObservableCollection<ProjectMember>
            {
                new ProjectMember("Mikołaj Sacha", "mikolajsacha@gmail.com"),
                new ProjectMember("Janusz Nowak", "janusz@o2.pl"),
                new ProjectMember("Anna Wiśniewska", "aniaw@hotmail.com"),
                new ProjectMember("Janina Grodzka", "jan007@onet.pl"),
                new ProjectMember("Maciej Łos", "losmaciej@gmail.com")
            };
        }
    }
}
