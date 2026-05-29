using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using StudentDormitoryApp.Domain.DomainModels;
using StudentDormitoryApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Repository.Implementations
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {

        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            entities = _context.Set<T>();
        }

        public T Delete(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
            return entity;
        }

        public E Get<E>(Expression<Func<T, E>> selector, Expression<Func<T, bool>> predicate = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>>? orederBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = entities;
            if (include != null){
                query=include(query);
            }
            if (predicate != null){
                query=query.Where(predicate);
            }
            if (orederBy != null) {
                return orederBy(query).Select(selector).FirstOrDefault();
            }
            return query.Select(selector).FirstOrDefault();

        }


        public IEnumerable<E> GetAll<E>(Expression<Func<T, E>> selector,
             Expression<Func<T, bool>>? predicate = null,
             Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
             Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = entities;

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).Select(selector).AsEnumerable();
            }

            return query.Select(selector).AsEnumerable();
        }

 

        public T Insert(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public T Update(T entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }

       
    }
}
