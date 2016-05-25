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
        private readonly Dictionary<ProjectMember, int> _projectMembersSetsDictionary;
        private readonly List<HashSet<ProjectMember>> _projectMembersSets;

        public ProjectMembersSetsViewModel(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
            _projectMembersSetsDictionary = new Dictionary<ProjectMember, int>();
            _projectMembersSets = new List<HashSet<ProjectMember>>();
        }

        public async Task<HashSet<ProjectMember>> Resolve(ProjectMember projectMember)
        {
            if (_projectMembersSetsDictionary.ContainsKey(projectMember))
            {
                return _projectMembersSets[_projectMembersSetsDictionary[projectMember]];
            }
            _projectMembersSets.Add(new HashSet<ProjectMember>());

            foreach (var newProjectMember in from commiter in await _repositoryService.GetAllCommiters()
                                             where commiter.Name == projectMember.Name || commiter.Email == projectMember.Email
                                             select new ProjectMember(commiter.Name, projectMember.Email))
            {
                _projectMembersSetsDictionary.Add(newProjectMember, _projectMembersSets.Count - 1);
                _projectMembersSets[_projectMembersSets.Count - 1].Add(newProjectMember);
            }
            return _projectMembersSets[_projectMembersSets.Count - 1];
        }
    }
}