﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Reflection;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;

namespace StaticVoid.Core.Repository
{
    public class DbContextRepositoryDataSource<T> : IRepositoryDataSource<T> where T : class
    {
        private DbContext m_Context;
        private DbSet<T> m_Set;
        public DbContextRepositoryDataSource(DbContext context)
        {
            m_Context = context;
            var propertyInfos = context.GetType().GetProperties().Where(p => p.PropertyType == typeof(DbSet<T>));
            if (!propertyInfos.Any())
            {
                throw new ArgumentException("DB doesnt contain type");
            }

            m_Set = (DbSet<T>)propertyInfos.First().GetValue(context,null);
        }

        public IQueryable<T> GetAll()
        {
            return m_Set.AsQueryable<T>();
        }

        public void AddOnSave(T entity)
        {
            m_Set.Add(entity);
        }

        public void RemoveOnSave(T entity)
        {
            m_Set.Remove(entity);
        }

        public void SaveChanges()
        {
            m_Context.SaveChanges();
        }

        public void Dispose()
        {
            m_Context.Dispose();
        }


        public T GetBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var result = GetAll();
            if (includes.Any())
            {
                foreach (var include in includes)
                {
                    result = result.Include(include);
                }
            }
            return result.FirstOrDefault(predicate);
        }
    }
}
