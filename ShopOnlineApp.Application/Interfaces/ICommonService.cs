using System.Collections.Generic;
using ShopOnlineApp.Application.ViewModels.Footer;
using ShopOnlineApp.Application.ViewModels.Slide;
using ShopOnlineApp.Application.ViewModels.SystemConfig;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface ICommonService
    {
        FooterViewModel GetFooter();
        List<SlideViewModel> GetSlides(string groupAlias);
        SystemConfigViewModel GetSystemConfig(string code);
    }
}
