using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Function;
using ShopOnlineApp.Application.ViewModels.Role;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.Enums;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;

namespace ShopOnlineApp.Application.Implementation
{
    public class FunctionService : IFunctionService
    {
        private readonly IFunctionRepository _functionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        private readonly RoleManager<AppRole> _roleManager;


        public FunctionService(IMapper mapper,
            IFunctionRepository functionRepository,
            IUnitOfWork unitOfWork, IPermissionRepository permissionRepository, RoleManager<AppRole> roleManager)
        {
            _functionRepository = functionRepository;
            _unitOfWork = unitOfWork;
            _permissionRepository = permissionRepository;
            _roleManager = roleManager;
            _mapper = mapper;
        }


        public bool CheckExistedId(string id)
        {
            return _functionRepository.FindById(id) != null;
        }

        public void Add(FunctionViewModel functionVm)
        {
            if (!CheckExistedId(functionVm.Id))
            {
                var function = new FunctionViewModel().Map(functionVm);
                _functionRepository.Add(function);
            }
        }

        public void Delete(string id)
        {
            _functionRepository.Remove(id);
        }

        public FunctionViewModel GetById(string id)
        {
            var function = _functionRepository.FindSingle(x => x.Id == id);
            return new FunctionViewModel().Map(function);
        }

        public async Task<List<FunctionViewModel>> GetAll(string filter)
        {
            var query = _functionRepository.FindAll(x => x.Status == Status.Active);

            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Name.Contains(filter));


            var results = await query.OrderBy(x => x.ParentId).ToListAsync();

            return new FunctionViewModel().Map(results).ToList();
        }

        public async Task<List<FunctionViewModel>> GetFunctionByRoles(FunctionRequest request)
        {
            string[] roles = request.Roles.ToArray();
            var functionIds = _permissionRepository.FindAll(await BuildingQuery(roles)).Where(x => x.CanRead).Select(x=>x.FunctionId);
            var ids= new List<string>();
            foreach (var id in functionIds)
            {
                var functionDetail = _functionRepository.FindById(id);
                if (functionDetail.ParentId != null)
                {
                    ids.AddRange(new List<string>
                    {
                        id,
                        functionDetail.ParentId
                    });
                }
                else
                {
                    ids.Add(id);
                }
            }

            var function = from fun in _functionRepository.FindAll()
                           join id in ids.Distinct()
                           on fun.Id equals id
                           select fun;

            return new FunctionViewModel().Map(function).ToList();
        }


        public IEnumerable<FunctionViewModel> GetAllWithParentId(string parentId)
        {
            return new FunctionViewModel().Map(_functionRepository.FindAll(x => x.ParentId == parentId).ToList());
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(FunctionViewModel functionVm)
        {
            var functionDb = _functionRepository.FindById(functionVm.Id);
            if (functionDb != null)
            {
                functionDb.Name = functionVm.Name;
                functionDb.IconCss = functionVm.IconCss;
                functionDb.ParentId = functionVm.ParentId;
                functionDb.SortOrder = functionVm.SortOrder;
                functionDb.URL = functionVm.URL;
                functionDb.Status = functionVm.Status;
                _functionRepository.Update(functionDb);
                _unitOfWork.Commit();
            }
        }

        public void ReOrder(string sourceId, string targetId)
        {

            var source = _functionRepository.FindById(sourceId);
            var target = _functionRepository.FindById(targetId);
            int tempOrder = source.SortOrder;

            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _functionRepository.Update(source);
            _functionRepository.Update(target);
        }

        public void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            //Update parent id for source
            var category = _functionRepository.FindById(sourceId);
            category.ParentId = targetId;
            _functionRepository.Update(category);

            //Get all sibling
            var sibling = _functionRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _functionRepository.Update(child);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private async Task<Expression<Func<Permission, bool>>> BuildingQuery(params string[] keywords)
        {
            var predicate = PredicateBuilder.True<Permission>();

            foreach (string keyword in keywords)
            {
                var test = await _roleManager.FindByNameAsync(keyword);

                predicate = predicate.Or(p => p.RoleId == test.Id);
            }
            return predicate;
        }
    }
}
