using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class AppUserRoleRepository :IAppUserRoleRepository
    {
        private readonly AppDbContext _context;

        public AppUserRoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(AppRole entity)
        {
            _context.AppRoles.Add(entity);
            _context.SaveChanges();

        }

        public IQueryable<AppRole> FindAll(params Expression<Func<AppRole, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<AppRole> FindAll(Expression<Func<AppRole, bool>> predicate, params Expression<Func<AppRole, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public AppRole FindById(Guid id, params Expression<Func<AppRole, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public AppRole FindSingle(Expression<Func<AppRole, bool>> predicate, params Expression<Func<AppRole, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public void Remove(AppRole entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public void RemoveMultiple(List<AppRole> entities)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(AppRole entity)
        {
            throw new NotImplementedException();
        }
    }
}
