namespace GitTask.Domain.Model.Project
{
    public class ProjectMember
    {
        public string Name { get; }
        public string Email { get; }

        public ProjectMember(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public override bool Equals(object obj)
        {
            var pm = obj as ProjectMember;
            if (pm == null)
            {
                return false;
            }

            return Name == pm.Name && Email == pm.Email;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + Email.GetHashCode();
        }
    }
}
