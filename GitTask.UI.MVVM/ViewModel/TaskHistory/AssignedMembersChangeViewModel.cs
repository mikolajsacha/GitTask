using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GitTask.Domain.Model.Project;

namespace GitTask.UI.MVVM.ViewModel.TaskHistory
{
    public class AssignedMembersChangeViewModel : BaseChangeViewModel<IEnumerable<ProjectMember>>
    {
        public ObservableCollection<ProjectMember> AddedMembers { get; }
        public ObservableCollection<ProjectMember> RemovedMembers { get; }

        public bool AnyMembersAdded => AddedMembers.Any();
        public bool AnyMembersRemoved => RemovedMembers.Any();

        public AssignedMembersChangeViewModel(IEnumerable<ProjectMember> oldAssignedMembers,
            IEnumerable<ProjectMember> newAssignedMembers)
            : base(oldAssignedMembers, newAssignedMembers)
        {
            AddedMembers = new ObservableCollection<ProjectMember>();
            RemovedMembers = new ObservableCollection<ProjectMember>();
            ResolveRemovedMembers();
            ResolveAddedMembers();
        }

        private void ResolveRemovedMembers()
        {
            RemovedMembers.Clear();
            foreach (var removedMember in OldValue.Where(pm => !NewValue.Contains(pm)))
            {
                RemovedMembers.Add(removedMember);
            }
            RaisePropertyChanged("AnyMembersRemoved");
        }

        private void ResolveAddedMembers()
        {
            AddedMembers.Clear();
            foreach(var addedMember in NewValue.Where(pm => !OldValue.Contains(pm)))
            {
                AddedMembers.Add(addedMember);
            }
            RaisePropertyChanged("AnyMembersAdded");
        }
    }
}