using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication2
{
    public interface IUserStorage
    {

        void AddUser(User user);
        IEnumerable<User> GetAll();
        User Find(int id);
        bool RemoveUser(int id);
        void Update(User user);

    }

    class UserStorage : IUserStorage
    {
        
        private static ConcurrentDictionary<int, User> instance = new ConcurrentDictionary<int, User>();

     
        public void AddUser(User user)
        {
             instance.TryAdd(user.Id, user);
        }

        public IEnumerable<User> GetAll()
        {
            return instance.Values.ToList();
        }

        public User Find(int id)
        {
            User user;
            instance.TryGetValue(id, out user);
            return user;
        }

        public bool RemoveUser(int id)
        {
           
            User user;
            int curentSize = instance.Count();
            instance.TryRemove(id,out user);
            int newSize= instance.Count();
            if (curentSize > newSize)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public void Update(User user)
        {
            instance[user.Id] = user;
        }

    }

    
    class UserStorageB : IUserStorage
    {

        private static ConcurrentBag<KeyValuePair<int, User>> instance = new ConcurrentBag<KeyValuePair<int, User>>();

        public void AddUser(User user)
        {
            instance.Add(new KeyValuePair<int, User>(user.Id, user));

        }

        public IEnumerable<User> GetAll()
        {
            var users = new List<User>();
            var temp = instance.ToList();
            temp.ForEach(x => users.Add(x.Value));
            return users;
        }

        public User Find(int id)
        {
            
            var SelectedUser = instance.SingleOrDefault(u=>u.Key==id);
            return SelectedUser.Value;
        }

        public bool RemoveUser(int id)
        {
            bool result;
            ConcurrentBag<KeyValuePair<int, User>> newBag = new ConcurrentBag<KeyValuePair<int, User>>();
            var newUserList = new List<KeyValuePair<int, User>>();
            var currentBag = instance.ToList();

            newUserList = currentBag.Where(x => x.Key != id).ToList();

            newUserList.ForEach(x => newBag.Add(x));

            if (instance.Count() > newBag.Count())
            {
                result = true;
                instance = newBag;
            }
            else
            {
                result = false;
            }

            return result;
        }


        public void Update(User user)
        {

            ConcurrentBag<KeyValuePair<int, User>> newBag = new ConcurrentBag<KeyValuePair<int, User>>();
            var newUserList = new List<KeyValuePair<int, User>>();
            var currentBag = instance.ToList();

            newUserList = currentBag.Where(x => x.Key != user.Id).ToList();

            newUserList.ForEach(x => newBag.Add(x));

            
            instance = newBag;
            instance.Add(new KeyValuePair<int, User>(user.Id, user));
            
        }
    }

    
}
