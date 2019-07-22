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
        User FindUser(int id);
        void RemoveUser(int id);
        void UpdateUser(User user);
        bool ContainsUser(int id);

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

        public User FindUser(int id)
        {
            User user;
            instance.TryGetValue(id, out user);
            return user;
        }

        public void RemoveUser(int id)
        {
            User user;
            instance.Remove(id, out user);

        }

        public void UpdateUser(User user)
        {
            instance[user.Id] = user;
        }

        public bool ContainsUser(int id)
        {
            if (instance.ContainsKey(id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    
    class UserStorageB : IUserStorage
    {

        private static readonly ConcurrentBag<KeyValuePair<int, User>> instance = new ConcurrentBag<KeyValuePair<int, User>>();

        public void AddUser(User user)
        {
            if(instance.All(u=>u.Key!=user.Id))
            instance.Add(new KeyValuePair<int, User>(user.Id, user));

        }

        public IEnumerable<User> GetAll()
        {
           
            foreach (var variable in instance.Where(u => u.Value != null).ToList())
            {
                yield return variable.Value;
            }
            
              
        }

        public User FindUser(int id)
        {
            
            var SelectedUser =  instance.SingleOrDefault(u => u.Key == id);
            return SelectedUser.Value;
        }

        public void RemoveUser(int id)
        {
            KeyValuePair<int, User> kvp;
            var tempList = new List<KeyValuePair<int, User>>();
            while (!instance.IsEmpty)
            {
                instance.TryTake(out kvp);
                if (kvp.Key == id)
                {
                    tempList.ForEach(x => instance.Add(x));
                    break;
                }
                else
                {
                    tempList.Add(kvp);
                }
            }


            
        }


        public void UpdateUser(User user)
        {
            IEnumerator<KeyValuePair<int, User>> it = instance.GetEnumerator();
            foreach (var variable in instance)
            {
                if (variable.Key == user.Id)
                {
                    variable.Value.Name=user.Name;
                }
                else
                {
                    it.MoveNext();
                }

            }
            


        }

        public bool ContainsUser(int id)
        {
           
            if (instance.Any(u => u.Key == id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    
}
