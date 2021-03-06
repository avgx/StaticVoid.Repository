﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace StaticVoid.Repository
{
    public interface IRepository<T> : IDisposable where T : class
    {
        void Create(T entity);

        void Delete(T entity);

        void Update(T entity);

        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);

        T GetBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    }
}
