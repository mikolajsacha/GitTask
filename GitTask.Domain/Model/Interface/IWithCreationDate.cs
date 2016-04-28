using System;

namespace GitTask.Domain.Model.Interface
{
    public interface IWithCreationDate
    {
        DateTime DateCreated { get; set; }
    }
}
