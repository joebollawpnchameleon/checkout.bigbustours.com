using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using bigbus.checkout.data.Repositories.Infrastructure;
using  bigbus.checkout.data;
using bigbus.checkout.data.Model;

namespace bigbus.checkout.data.Repositories.Implementation
{
    public class GenericDataRepository<T> : IGenericDataRepository<T> where T : class
    {

        private void Log(string message)
        {
            using (var context = new CheckoutDbContext())
            {
                context.Logs.Add(new Log
                {
                    Level = "1",
                    Logger = "GenericDataRepository",
                    Message = message
                });
            }
        }

        public string FleshExeptionOut(DbEntityValidationException exception)
        {
            var sbTemp = new StringBuilder();

            foreach (var eve in exception.EntityValidationErrors)
            {
                sbTemp.AppendLine(
                    string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));

                foreach (var ve in eve.ValidationErrors)
                {
                    sbTemp.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                        ve.PropertyName, ve.ErrorMessage));
                }
            }

            return sbTemp.ToString();
        }

        public virtual IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            try
            {
                List<T> list;
                using (var context = new CheckoutDbContext())
                {

                    IQueryable<T> dbQuery = context.Set<T>();

                    //Apply eager loading
                    foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                        dbQuery = dbQuery.Include<T, object>(navigationProperty);

                    list = dbQuery
                        .AsNoTracking()
                        .ToList<T>();
                }
                return list;
            }
            catch (DbEntityValidationException e)
            {               
                Log(FleshExeptionOut(e));
                throw;
            }

        }

        public virtual IList<T> GetList(Func<T, bool> where,
             params Expression<Func<T, object>>[] navigationProperties)
        {

            try
            {
                List<T> list;
                using (var context = new CheckoutDbContext())
                {
                    IQueryable<T> dbQuery = context.Set<T>();

                    //Apply eager loading
                    foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                        dbQuery = dbQuery.Include<T, object>(navigationProperty);

                    list = dbQuery
                        .AsNoTracking()
                        .Where(where)
                        .ToList<T>();

                }
                return list;
            }
            catch(DbEntityValidationException e)
            {
                Log(FleshExeptionOut(e));
                throw;
            }
        }

        public virtual T GetSingle(Func<T, bool> where,
             params Expression<Func<T, object>>[] navigationProperties)
        {
            try
            {
                T item = null;
                using (var context = new CheckoutDbContext())
                {
                    IQueryable<T> dbQuery = context.Set<T>();

                    //Apply eager loading
                    foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                        dbQuery = dbQuery.Include<T, object>(navigationProperty);

                    item = dbQuery
                        //.AsNoTracking() //Don't track any changes for the selected item
                        .FirstOrDefault(where); //Apply where clause
                }
                return item;
            }
            catch (DbEntityValidationException ex)
            {
                Log(FleshExeptionOut(ex));
                throw;
            }
        }

        public virtual void Add(params T[] items)
        {
            try { 
                using (var context = new CheckoutDbContext())
                {
                    foreach (T item in items)
                    {
                        context.Entry(item).State = EntityState.Added;
                    }
                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                Log(FleshExeptionOut(ex));
                throw;
            }
        }

        public virtual void Update(params T[] items)
        {
            try { 
                using (var context = new CheckoutDbContext())
                {
                    foreach (T item in items)
                    {
                        context.Entry(item).State = EntityState.Modified;
                    }
                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                Log(FleshExeptionOut(ex));
                throw;
            }
        }

        public virtual void Remove(params T[] items)
        {
            try { 
                using (var context = new CheckoutDbContext())
                {
                    foreach (T item in items)
                    {
                        context.Entry(item).State = EntityState.Deleted;
                    }
                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                Log(FleshExeptionOut(ex));
                throw;
            }
        }
    }
}
