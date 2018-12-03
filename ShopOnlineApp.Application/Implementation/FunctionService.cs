using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Function;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Application.Implementation
{
    public class FunctionService:IFunctionService
    {
        public readonly IFunctionRepository _functionRepository;

        public FunctionService(IFunctionRepository functionRepository)
        {
            _functionRepository = functionRepository;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<List<FunctionViewModel>> GetAll()
        {
            var response =await _functionRepository.FindAll().ToListAsync();

            var functionReturn= new FunctionViewModel().Map(response);

           // Task.CompletedTask;
            return   functionReturn.ToList();

        }

        public Task<List<FunctionViewModel>>GetAllByPermission(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
