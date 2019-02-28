using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels;
using ShopOnlineApp.Application.ViewModels.Blogs;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Application.ViewModels.Tag;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.Enums;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Constants;
using ShopOnlineApp.Utilities.Enum;
using ShopOnlineApp.Utilities.Helpers;

namespace ShopOnlineApp.Application.Implementation
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlogCategoryRepository _categoryRepository;
        public BlogService(IBlogRepository blogRepository,
            IBlogTagRepository blogTagRepository,
            ITagRepository tagRepository,
            IUnitOfWork unitOfWork, IBlogCategoryRepository categoryRepository)
        {
            _blogRepository = blogRepository;
            _blogTagRepository = blogTagRepository;
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
        }

        public BlogViewModel Add(BlogViewModel blogVm)
        {
            var blog = new BlogViewModel().Map(blogVm);

            if (!string.IsNullOrEmpty(blog.Tags))
            {
               

                var tags = blog.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.BlogTag
                        };
                        _tagRepository.Add(tag);
                    }

                    var blogTag = new BlogTag { TagId = tagId };
                    blog.BlogTags.Add(blogTag);
                }
            }
            _blogRepository.Add(blog);
            return blogVm;
        }

        public void Delete(int id)
        {
            _blogRepository.Remove(id);
        }

        public List<BlogViewModel> GetAll()
        {
            return new BlogViewModel().Map(_blogRepository.FindAll(c => c.BlogTags)).ToList();
        }

        public async Task<BaseReponse<ModelListResult<BlogViewModel>>> GetAllPaging(BlogRequest request)
        {
            var response = from c in _categoryRepository.FindAll()
                           join p in _blogRepository.FindAll().AsNoTracking() on c.Id equals p.BlogCategoryId 
                           select new BlogViewModel
                           {
                               Name = p.Name,
                               Id = p.Id,
                               BlogCategoryId = p.BlogCategoryId,
                               BlogCategory = new BlogCategoryViewModel
                               {
                                   Name = c.Name
                               },
                               Description = p.Description,
                               Content = p.Content,
                               DateCreated = p.DateCreated,
                               DateModified = p.DateModified,
                               HomeFlag = p.HomeFlag,
                               HotFlag = p.HotFlag,
                               SeoAlias = p.SeoAlias,
                               SeoDescription = p.SeoDescription,
                               SeoKeywords = p.SeoKeywords,
                               SeoPageTitle = p.SeoPageTitle,
                               ViewCount = p.ViewCount,
                               Status = p.Status,
                               Image = p.Image
                           };

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                response = response.Where(x => x.Name.Contains(request.SearchText));
            }
            else if (!string.IsNullOrEmpty(request.Name))
            {
                response = response.Where(x => x.Name.Contains(request.Name));
            }
            else if (request?.CategoryId > 0)
            {
                response = response.Where(x => x.BlogCategoryId == request.CategoryId);
            }

            var totalCount = await response.CountAsync();

            if (request.IsPaging)
            {
                response = response.OrderByDescending(x => x.DateModified)
                    .Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);
            }

            //var items = new ProductViewModel().Map(response).ToList();

            return new BaseReponse<ModelListResult<BlogViewModel>>()
            {
                Data = new ModelListResult<BlogViewModel>()
                {
                    Items = response.ToList(),
                    Message = Message.Success,
                    RowCount = totalCount,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex
                },
                Message = Message.Success,
                Status = (int)QueryStatus.Success
            };
        }

        public BlogViewModel GetById(int id)
        {
            return new BlogViewModel().Map(_blogRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(BlogViewModel blog)
        {
            _blogRepository.Update(new BlogViewModel().Map(blog));
            if (!string.IsNullOrEmpty(blog.Tags))
            {
                var blogTags = _blogTagRepository.FindAll(x => x.BlogId == blog.Id);
                _blogTagRepository.RemoveMultiple(blogTags.ToList());

                string[] tags = blog.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        _tagRepository.Add(tag);
                    }
                    _blogTagRepository.RemoveMultiple(_blogTagRepository.FindAll(x => x.Id == blog.Id).ToList());
                    BlogTag blogTag = new BlogTag
                    {
                        BlogId = blog.Id,
                        TagId = tagId
                    };
                    _blogTagRepository.Add(blogTag);
                }
            }
        }

        public IEnumerable<BlogViewModel> GetLastest(int top)
        {
            return new BlogViewModel().Map(_blogRepository.FindAll(x => x.Status == Status.Active).OrderByDescending(x => x.DateCreated)
                .Take(top)).ToList();
        }

        public List<BlogViewModel> GetHotProduct(int top)
        {
            return new BlogViewModel().Map(_blogRepository.FindAll(x => x.Status == Status.Active && x.HotFlag == true)
                    .OrderByDescending(x => x.DateCreated)
                    .Take(top)).ToList();
        }

        public List<BlogViewModel> GetListPaging(int page, int pageSize, string sort, out int totalRow)
        {
            var query = _blogRepository.FindAll(x => x.Status == Status.Active);

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
            }

            totalRow = query.Count();

            return new BlogViewModel().Map(query.Skip((page - 1) * pageSize)
                    .Take(pageSize)).ToList();
        }

        public List<string> GetListByName(string name)
        {
            return _blogRepository.FindAll(x => x.Status == Status.Active
            && x.Name.Contains(name)).Select(y => y.Name).ToList();
        }

        public List<BlogViewModel> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _blogRepository.FindAll(x => x.Status == Status.Active
            && x.Name.Contains(keyword));

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
            }

            totalRow = query.Count();

            return new BlogViewModel().Map(query.Skip((page - 1) * pageSize)
                    .Take(pageSize)).ToList();
        }

        public List<BlogViewModel> GetReatedBlogs(int id, int top)
        {
            return new BlogViewModel().Map(_blogRepository
                    .FindAll(x => x.Status == Status.Active && x.Id != id)
                    .OrderByDescending(x => x.DateCreated)
                    .Take(top))
            .ToList();
        }

        public List<TagViewModel> GetListTagById(int id)
        {
            return _blogTagRepository.FindAll(x => x.BlogId == id, c => c.Tag)
                .Select(y => y.Tag)
                .ProjectTo<TagViewModel>()
                .ToList();
        }

        public void IncreaseView(int id)
        {
            var product = _blogRepository.FindById(id);
            if (product.ViewCount.HasValue)
                product.ViewCount += 1;
            else
                product.ViewCount = 1;
        }

        public List<BlogViewModel> GetListByTag(string tagId, int page, int pageSize, out int totalRow)
        {
            var query = from p in _blogRepository.FindAll()
                        join pt in _blogTagRepository.FindAll()
                        on p.Id equals pt.BlogId
                        where pt.TagId == tagId && p.Status == Status.Active
                        orderby p.DateCreated descending
                        select p;

            totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return new BlogViewModel().Map(query).ToList();

        }

        public TagViewModel GetTag(string tagId)
        {
            return new TagViewModel().Map(_tagRepository.FindSingle(x => x.Id == tagId));
        }

        public List<BlogViewModel> GetList(string keyword)
        {
            var query = !string.IsNullOrEmpty(keyword) ?
               new BlogViewModel().Map(_blogRepository.FindAll(x => x.Name.Contains(keyword))).ToList()
                : new BlogViewModel().Map(_blogRepository.FindAll());
            return query.ToList();
        }

        public List<TagViewModel> GetListTag(string searchText)
        {
            return new TagViewModel().Map(_tagRepository.FindAll(x => x.Type == CommonConstants.ProductTag
                                                                      && searchText.Contains(x.Name))).ToList();
        }
    }
}
