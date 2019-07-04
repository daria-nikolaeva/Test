using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;

namespace WebApplication2
{
    public interface IUserStorage
    {
        IDictionary<int, User> Instance { get; }
        void addUser(User user);
     
    }

    class UserStorage : IUserStorage
    {
       
        private static IDictionary<int, User> instance = new ConcurrentDictionary<int, User>();

        public IDictionary<int, User> Instance  => instance;


        public void addUser(User user)
        {
            Instance.Add(user.Id, user);
        }
    }
}
