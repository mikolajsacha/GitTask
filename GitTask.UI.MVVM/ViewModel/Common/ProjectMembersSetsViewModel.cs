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
        private readonly IRepositoryService _repositoryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly Dictionary<ProjectMember, int> _projectMembersSetsDictionary;
        private readonly List<HashSet<ProjectMember>> _projectMembersSets;

        public ProjectMembersSetsViewModel(IRepositoryService repositoryService, IProjectQueryService projectQueryService)
        {
            _repositoryService = repositoryService;
            _projectQueryService = projectQueryService;
            _projectMembersSetsDictionary = new Dictionary<ProjectMember, int>();
            _projectMembersSets = new List<HashSet<ProjectMember>>();
            projectQueryService.UserAdded += ProjectQueryServiceOnUserAdded;
        }

        private void ProjectQueryServiceOnUserAdded(ProjectMember projectMember)
        {
            foreach (var membersSet in _projectMembersSets
                     .Where(membersSet => membersSet.Any(memberFromSet => memberFromSet.Name == projectMember.Name || memberFromSet.Email == projectMember.Email)))
            {
                membersSet.Add(projectMember);
            }
        }

        public async Task<HashSet<ProjectMember>> Resolve(ProjectMember projectMember)
        {
            return await Task.Run(async () =>
            {
                if (_projectMembersSetsDictionary.ContainsKey(projectMember))
                {
                    return _projectMembersSets[_projectMembersSetsDictionary[projectMember]];
                }
                foreach (var key in _projectMembersSetsDictionary.Keys.Where(
                            key => key.Name == projectMember.Name || key.Email == projectMember.Email))
                {
                    _projectMembersSetsDictionary.Add(projectMember, _projectMembersSetsDictionary[key]);
                    return _projectMembersSets[_projectMembersSetsDictionary[key]];
                }

                _projectMembersSets.Add(new HashSet<ProjectMember>());

                var namesHashSet = new HashSet<string>();
                var emailsHashSet = new HashSet<string>();

                foreach (var newProjectMember in
                            (from commiter in await _repositoryService.GetAllUniqueCommiters()
                             where commiter.Name == projectMember.Name || commiter.Email == projectMember.Email
                               || namesHashSet.Contains(commiter.Name) || emailsHashSet.Contains(commiter.Email)
                             select new ProjectMember(commiter.Name, commiter.Email)))
                {
                    if (!namesHashSet.Contains(newProjectMember.Name)) namesHashSet.Add(newProjectMember.Name);
                    if (!emailsHashSet.Contains(newProjectMember.Email)) emailsHashSet.Add(newProjectMember.Email);
                }

                foreach (var newProjectMember in
                            (from commiter in await _repositoryService.GetAllUniqueCommiters()
                             where commiter.Name == projectMember.Name || commiter.Email == projectMember.Email
                               || namesHashSet.Contains(commiter.Name) || emailsHashSet.Contains(commiter.Email)
                             select new ProjectMember(commiter.Name, commiter.Email)))
                {
                    if (!namesHashSet.Contains(newProjectMember.Name)) namesHashSet.Add(newProjectMember.Name);
                    if (!emailsHashSet.Contains(newProjectMember.Email)) emailsHashSet.Add(newProjectMember.Email);

                    _projectMembersSetsDictionary.Add(newProjectMember, _projectMembersSets.Count - 1);
                    _projectMembersSets[_projectMembersSets.Count - 1].Add(newProjectMember);
                }

                if (_projectQueryService.Project?.ProjectMembersNotInRepository != null)
                {
                    foreach (var newProjectMember in _projectQueryService.Project.ProjectMembersNotInRepository
                            .Where(member => member.Name == projectMember.Name || member.Email == projectMember.Email
                                         || namesHashSet.Contains(member.Name) || emailsHashSet.Contains(member.Email))
                            .Select(member => new ProjectMember(member.Name, member.Email))
                            .Distinct())
                    {
                        if (!namesHashSet.Contains(newProjectMember.Name)) namesHashSet.Add(newProjectMember.Name);
                        if (!emailsHashSet.Contains(newProjectMember.Email)) emailsHashSet.Add(newProjectMember.Email);
                    }

                    foreach (var newProjectMember in _projectQueryService.Project.ProjectMembersNotInRepository
                            .Where(member => member.Name == projectMember.Name || member.Email == projectMember.Email
                                         || namesHashSet.Contains(member.Name) || emailsHashSet.Contains(member.Email))
                            .Select(member => new ProjectMember(member.Name, member.Email))
                            .Distinct())
                    {
                        if (!namesHashSet.Contains(newProjectMember.Name)) namesHashSet.Add(newProjectMember.Name);
                        if (!emailsHashSet.Contains(newProjectMember.Email)) emailsHashSet.Add(newProjectMember.Email);

                        _projectMembersSetsDictionary.Add(newProjectMember, _projectMembersSets.Count - 1);
                        _projectMembersSets[_projectMembersSets.Count - 1].Add(newProjectMember);
                    }
                }
                return _projectMembersSets[_projectMembersSets.Count - 1];
            });
        }
    }
}