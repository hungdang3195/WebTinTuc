using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Footer;
using ShopOnlineApp.Application.ViewModels.Slide;
using ShopOnlineApp.Application.ViewModels.SystemConfig;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Constants;

namespace ShopOnlineApp.Application.Implementation
{
    public class CommonService : ICommonService
    {
        private readonly IFooterRepository _footerRepository;
        private readonly ISystemConfigRepository _systemConfigRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISlideRepository _slideRepository;
        public CommonService(IFooterRepository footerRepository,
            ISystemConfigRepository systemConfigRepository,
            IUnitOfWork unitOfWork,
            ISlideRepository slideRepository)
        {
            _footerRepository = footerRepository;
            _unitOfWork = unitOfWork;
            _systemConfigRepository = systemConfigRepository;
            _slideRepository = slideRepository;
        }

        public FooterViewModel GetFooter()
        {
            return new FooterViewModel().Map(_footerRepository.FindSingle(x => x.Id == CommonConstants.DefaultFooterId));
        }

        public List<SlideViewModel> GetSlides(string groupAlias)
        {
            return new SlideViewModel().Map(_slideRepository.FindAll(x => x.Status && x.GroupAlias == groupAlias)).ToList();
        }

        public SystemConfigViewModel GetSystemConfig(string code)
        {
            return new SystemConfigViewModel().Map(_systemConfigRepository.FindSingle(x => x.Id == code));
        }
    }
}
