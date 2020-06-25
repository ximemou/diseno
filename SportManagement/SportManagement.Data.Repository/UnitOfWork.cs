using SportManagement.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportManagement.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private DomainContext context;

        private GenericRepository<User> userRepository;


        private GenericRepository<Team> teamRepository;

        private GenericRepository<Sport> sportRepository;

        private GenericRepository<Game> gameRepository;

        private GenericRepository<Comment> commentRepository;
        private GenericRepository<UserTeam> userTeamRepository;
       

        private bool disposed = false;


        public UnitOfWork(DomainContext aContext)
        {
            this.context = aContext;
        }

        public IRepository<User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<User>(context);
                }
                return userRepository;
            }
        }

      
        public IRepository<UserTeam> UserTeamRepository
        {
            get
            {
                if (this.userTeamRepository == null)
                {
                    this.userTeamRepository = new GenericRepository<UserTeam>(context);
                }
                return userTeamRepository;
            }
        }

        public IRepository<Team> TeamRepository
        {
            get
            {
                if (this.teamRepository == null)
                {
                    this.teamRepository = new GenericRepository<Team>(context);
                }
                return teamRepository;
            }
        }

        public IRepository<Sport> SportRepository
        {
            get
            {
                if (this.sportRepository == null)
                {
                    this.sportRepository = new GenericRepository<Sport>(context);
                }
                return sportRepository;
            }
        }

        public IRepository<Comment> CommentRepository
        {
            get
            {
                if (this.commentRepository == null)
                {
                    this.commentRepository = new GenericRepository<Comment>(context);
                }
                return commentRepository;
            }
        }

        public IRepository<Game> GameRepository
        {
            get
            {
                if (this.gameRepository == null)
                {
                    this.gameRepository = new GenericRepository<Game>(context);
                }
                return gameRepository;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
