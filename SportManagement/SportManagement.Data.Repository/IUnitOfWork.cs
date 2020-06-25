using System;
using System.Collections.Generic;
using System.Text;

namespace SportManagement.Data.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }
        IRepository<Team> TeamRepository { get; }
        IRepository<Sport> SportRepository { get; }
        IRepository<Game > GameRepository { get; }
        IRepository<Comment> CommentRepository { get; }
        IRepository<UserTeam> UserTeamRepository { get; }
        void Save();
    }
}
