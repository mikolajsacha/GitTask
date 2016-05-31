using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitTask.Domain.Model.Project;

namespace GitTask.Domain.Services
{
    public class ProjectMembersService
    {
        public async Task<IEnumerable<ProjectMember>> GetAllMostRecentUsers(IEnumerable<ProjectMember> allUsers)
        {
            return await Task.Run(() =>
            {
                var nameKeyedDictionary = new Dictionary<string, string>(); // key = name, value = email
                var emailKeyedDictionary = new Dictionary<string, string>(); // key = email, value = name

                // we want unique names, with each project member having the most up-to-date e-mail (from newest commit)
                foreach (var member in allUsers)
                {
                    nameKeyedDictionary[member.Name] = member.Email;
                    emailKeyedDictionary[member.Email] = member.Name;
                }

                var keysToRemove = (from nameKey in nameKeyedDictionary.Keys
                                    let emailValue = nameKeyedDictionary[nameKey]
                                    where emailKeyedDictionary[emailValue] != nameKey
                                    select nameKey);

                var keysTomoveHashSet = new HashSet<string>(keysToRemove);

                return nameKeyedDictionary.Where(commiterPair => !keysTomoveHashSet.Contains(commiterPair.Key)).
                    Select(commiterPair => new ProjectMember(commiterPair.Key, commiterPair.Value));
            });
        }
    }
}
