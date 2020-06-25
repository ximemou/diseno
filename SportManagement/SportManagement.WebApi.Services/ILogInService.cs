using SportManagement.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportManagement.WebApi.Services
{
    public interface ILogInService
    {
        User Authenticate(string userName, string passsword);
    }
}
