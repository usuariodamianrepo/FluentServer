using Clone.DataAccess.Data;
using Clone.DataAccess.Repositories.IRepository;
using Clone.Models;
using System.Collections;

namespace Clone.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private Hashtable _repositories;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public IRepositoryAsync<T> Repository<T>() where T : AuditableEntity
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(RepositoryAsync<>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _db);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepositoryAsync<T>)_repositories[type];
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        #region Users
        //public async Task<List<UserBySearchDto>> UserBySearchAsync(string name, string phoneNumber)
        //{
        //    var users = await (
        //        from user in _db.Users
        //        join userRole in _db.UserRoles on user.Id equals userRole.UserId into userRoles
        //        select new UserBySearchDto
        //        {
        //            Id = user.Id,
        //            Name = user.Name,
        //            StreetAddress = user.StreetAddress,
        //            City = user.City,
        //            State = user.State,
        //            PostalCode = user.PostalCode,
        //            CompanyId = user.CompanyId,
        //            CompanyName = user.Company != null ? user.Company.Name : null,
        //            UserName = user.UserName,
        //            Email = user.Email,
        //            PhoneNumber = user.PhoneNumber,
        //            LockoutEnd = user.LockoutEnd,
        //            Roles = string.Join(", ",
        //                from ur in userRoles
        //                join role in _db.Roles on ur.RoleId equals role.Id
        //                select role.Name)
        //        }
        //        ).Take(MaxRows.MaxFifty).ToListAsync();

        //    return users;
        //}

        #endregion
    }
}
