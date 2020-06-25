using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SportManagement.Data;
using SportManagement.Data.Repository;

namespace SportManagement.WebApi.Services
{
    public class LogInService : ILogInService
    {
        private readonly IUnitOfWork unitOfWork;

        public LogInService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public User Authenticate(string userName, string passsword)
        {
            User user = unitOfWork.UserRepository.Get(u => u.Password == passsword && u.UserName == userName).FirstOrDefault();
            return user;
        }
    }
}
