using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GitTask.Domain.Model.Project;
using GitTask.Domain.Services.Interface;

namespace GitTask.UI.MVVM.ViewModel.Common
{
    public class ProjectMembersSetsViewModel : ViewModelBase
    {
        private readonly ProjectMembersViewModel _projectMembersViewModel;
        private readonly Dictionary<string, int> _projectMembersNamesSetsDictionary;
        private readonly List<HashSet<ProjectMember>> _projectMembersSets;

        public ProjectMembersSetsViewModel(IProjectQueryService projectQueryService,
                                           IProjectPathsReadonlyService projectPathsService,
                                           ProjectMembersViewModel projectMembersViewModel)
        {
            _projectMembersViewModel = projectMembersViewModel;
            _projectMembersNamesSetsDictionary = new Dictionary<string, int>();
            _projectMembersSets = new List<HashSet<ProjectMember>>();
            projectQueryService.UserAdded += ProjectQueryServiceOnUserAdded;
            projectPathsService.ProjectPathChanged += ProjectPathsServiceOnProjectPathChanged;
        }

        private void ProjectPathsServiceOnProjectPathChanged()
        {
            _projectMembersNamesSetsDictionary.Clear();
            _projectMembersSets.Clear();
        }

        private void ProjectQueryServiceOnUserAdded(ProjectMember projectMember)
        {
            foreach (var membersSet in _projectMembersSets
                .Where(
                    membersSet =>
                        membersSet.Any(
                            memberFromSet =>
                                memberFromSet.Name == projectMember.Name || memberFromSet.Email == projectMember.Email))
                )
            {
                membersSet.Add(projectMember);
            }
        }

        public async Task<HashSet<ProjectMember>> Resolve(ProjectMember projectMember)
        {
            return await Task.Run(() =>
            {
                if (_projectMembersNamesSetsDictionary.ContainsKey(projectMember.Name))
                {
                    return _projectMembersSets[_projectMembersNamesSetsDictionary[projectMember.Name]];
                }

                var alreadyResolved = new HashSet<ProjectMember>();
                var toBeResolved = new HashSet<ProjectMember>();
                var other = new HashSet<ProjectMember>(_projectMembersViewModel.ProjectMembers);
                Move(projectMember, other, toBeResolved);

                while (toBeResolved.Any())
                {
                    var current = toBeResolved.First();
                    if (alreadyResolved.Any(x => x.Name == current.Name) &&
                        alreadyResolved.Any(x => x.Email == current.Email))
                    {
                        Move(current, toBeResolved, alreadyResolved);
                        continue;
                    }

                    _projectMembersNamesSetsDictionary[current.Name] = _projectMembersSets.Count;
                    Move(current, toBeResolved, alreadyResolved);

                    foreach (var duplicate in
                        other.Where(x => x.Name == current.Name || x.Email == current.Email).ToList())
                    {
                        Move(duplicate, other, toBeResolved);
                    }
                }

                _projectMembersSets.Add(alreadyResolved);
                return _projectMembersSets[_projectMembersSets.Count - 1];
            });
        }

        private static void Move<T>(T element, ICollection<T> from, ISet<T> to)
        {
            from.Remove(element);
            to.Add(element);
        }
    }
}