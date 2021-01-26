using Practice.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Practice.Repositories
{
    public class UserRepository
    {
        protected PracticeDBEntities context = new PracticeDBEntities();
        public int Validate(user u)
        {
            user vu =  this.context.Set<user>().Where<user>(x => x.userName == u.userName && x.password == u.password).FirstOrDefault();
            if(vu != null)
            {
                return vu.userId;
            }
            else
            {
                return -1;
            }
        }

        public List<user> GetAll()
        {
            return this.context.Set<user>().ToList();
        }

        public user Insert(user entity)
        {
            this.context.Set<user>().Add(entity);
            this.context.SaveChanges();
            return entity;
        }
        public user Get(int id)
        {
            return this.context.Set<user>().Find(id);
        }

        public user Update(user entity)
        {
            this.context.Entry(entity).State = EntityState.Modified;
            this.context.SaveChanges();

            return entity;
        }

        public void Delete(int id)
        {
            this.context.Set<user>().Remove(Get(id));
            this.context.SaveChanges();
        }
    }
}