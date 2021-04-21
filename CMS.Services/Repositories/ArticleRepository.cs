﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Services.RepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Data.SqlClient;
using System.Data;
using CMS.Data.ModelEntity;
using Microsoft.AspNetCore.Http;
using CMS.Data.ModelFilter;
using Microsoft.AspNetCore.Hosting;
using CMS.Common;
using CMS.Data.ModelDTO;

namespace CMS.Services.Repositories
{

    public interface IArticleRepository  : IRepositoryBase<Article>
    {
        Task<int?> ArticleInsert(Article model,string UserId, int ArticleStatusId);
        Task ArticleUpdate(Article model,string UserId,  int ArticleStatusId);
        Task ArticleDelete(int ArticleId);
        Task<Article> ArticleGetById(int ArticleId);
        Task<List<Article>> ArticleSearch(string keyword);
        List<Article> ArticleBlockGetAll(int BlockId);
        Task<List<ArticleGetByBlockIdResult>> ArticleGetByBlockId(int ArticleBlockId);
        Task<List<ArticleGetTopByCategoryIdResult>> ArticleGetTopByCategoryId(int ArticleCategoryId);
        Task<List<ArticleSearchResult>> ArticleSearch(ArticleSearchFilter model);
        Task<VirtualizeResponse<ArticleSearchResult>> ArticleSearchWithPaging(ArticleSearchFilter model);
        void ArticleBlockArticleSave(int IsAdd, int ArticleBlockID, int ArticleID);
        void ArticleTopCategorySave(int ArticleId);
        void ArticleTopParentCategorySave(int ArticleId);
        string ArticleGetStatusString(int? ArticleStatusId);
        Task<List<SearchBreadCrumbByCateResult>> ArticleBreadCrumbGetByCategoryId(int ArticleCategoryId);
        Task<List<ArticleGetNewByCategoryIdResult>> ArticleGetNewByCategoryId(int ArticleCategoryId, int Number);
        Task ArticleUpdateStatusType(int ArticleId, int StatusTypeId);
        Task ArticleAddCounter(int ArticleId);
        Task<Tuple<List<ArticleGetByCategoryIdResult>,int>> ArticleGetByCategoryId(int ArticleCategoryId, int? PageSize, int? CurrentPage);
        List<Article> ArticleGetRelationArticle(int ArticleId);
        void ArticleSaveRelatedArticle(int IsAdd, int ArticleRelateId, int ArticleID);
        Task<string> CreateArticleURL(int ArticleId);
        Task<List<ArticleStatus>> GetLstArticleStatus();
        Task<List<ArticleAttachFile>> ArticleAttachGetLstByArticleId(int articleId);
        Task<bool> ArticleAttachDelete(int articleAttachFileId);
        Task<bool> ArticleAttachInsert(List<ArticleAttachFile> model);

    }
    public class ArticleRepository : RepositoryBase<Article>,  IArticleRepository
    {
        

        public ArticleRepository(CmsContext CmsDBContext) : base(CmsDBContext)
        {            
        }

        public async Task<int?> ArticleInsert(Article model, string UserId, int ArticleStatusId)
        {
            
            // Add một lần
            model.ArticleTypeId = 1;
            model.ProductBrandId = 0;
            model.ArticleStatusId = ArticleStatusId;
            model.CreateBy = UserId;
            model.CreateDate = DateTime.Now;
            model.LastEditDate = DateTime.Now;
            model.LastEditBy = UserId;
            model.CanCopy = true;
            model.CanComment = true;
            model.CanDelete = true;
            model.Active = true;
            model.Counter = 0;
            CmsContext.Article.Add(model);
            
            await CmsContext.SaveChangesAsync();
            //Insert articleCategoryArticle
            await ArticleSetArticleCategory(model.Id, Int32.Parse(model.ArticleCategoryIds));
            return model.Id;
        }

        public async Task ArticleUpdate(Article model, string UserId,  int ArticleStatusId)
        {
            
            try
            {
                var items = CmsContext.Article.FirstOrDefault(p => p.Id == model.Id);
                if (items != null)
                {
                    items.ArticleCategoryIds = model.ArticleCategoryIds;
                    items.ArticleStatusId = ArticleStatusId;
                    items.Name = model.Name;
                    items.SubTitle = model.SubTitle;
                    items.ImageDescription = model.ImageDescription;
                    items.BannerImage = model.BannerImage;
                    items.Description = model.Description;
                    items.Content = model.Content;
                    items.Author = model.Author;
                    items.StartDate = model.StartDate;
                    items.EndDate = model.EndDate;
                    items.Url = model.Url;
                    items.Tags = model.Tags;
                    items.MetaTitle = model.MetaTitle;
                    items.MetaDescription = model.MetaDescription;
                    items.MetaKeywords = model.MetaKeywords;
                    items.Image = model.Image;

                    if (string.IsNullOrEmpty(items.Url))
                    {
                        items.Url = await CreateArticleURL(items.Id);
                    }    

                    await CmsContext.SaveChangesAsync();
                    //Insert articleCategoryArticle
                    await ArticleSetArticleCategory(model.Id, Int32.Parse(model.ArticleCategoryIds));
                  
                }
            }
            catch 
            {

            }
        }
        public async Task ArticleDelete(int ArticleId)
        {
            
            try
            {
                var items = await CmsContext.Article.FirstOrDefaultAsync(p => p.Id == ArticleId);
                if (items != null)
                {
                    CmsContext.Article.Remove(items);
                    await CmsContext.SaveChangesAsync();
                }
            }
            catch
            {

            }
        }
        public async Task<Article> ArticleGetById(int ArticleId)
        {
            
            return await CmsContext.Article.FirstOrDefaultAsync(p => p.Id == ArticleId);
        }

        public async Task<List<Article>> ArticleSearch(string keyword)
        {
            
            if (!String.IsNullOrEmpty(keyword))
            {
                return await CmsContext.Article.Where(x => x.Name.Contains(keyword)).OrderByDescending(p => p.LastEditDate).ToListAsync();
            }
            return await CmsContext.Article.OrderByDescending(p => p.LastEditDate).ToListAsync();
        }

        public async Task<List<ArticleSearchResult>> ArticleSearch(ArticleSearchFilter model)
        {
            
            var output = new List<ArticleSearchResult>();
            //Parameter
            var pKeyword = new SqlParameter("@Keyword", model.Keyword ?? (object)DBNull.Value);
            var pTags = new SqlParameter("@Tags", model.Tags ?? (object)DBNull.Value);
            var pArticleCategoryId = new SqlParameter("@ArticleCategoryId", model.ArticleCategoryId ?? (object)DBNull.Value);
            var pProductBrandId = new SqlParameter("@ProductBrandId", model.ProductBrandId ?? (object)DBNull.Value);
            var pArticleTypeId = new SqlParameter("@ArticleTypeId", model.ArticleTypeId ?? (object)DBNull.Value);
            var pExceptionId = new SqlParameter("@ExceptionId", model.ExceptionId ?? (object)DBNull.Value);
            var pExceptionArticleTop = new SqlParameter("@ExceptionArticleTop", model.ExceptionArticleTop ?? (object)DBNull.Value);
            var pFromDate = new SqlParameter("@FromDate", model.FromDate);
            var pToDate = new SqlParameter("@ToDate", model.ToDate);
            var pEfficiency = new SqlParameter("@Efficiency", model.Efficiency ?? (object)DBNull.Value);
            var pActive = new SqlParameter("@Active", model.Active ?? (object)DBNull.Value);
            var pCreateBy = new SqlParameter("@CreateBy", model.CreateBy ?? (object)DBNull.Value);
            var pPageSize = new SqlParameter("@PageSize", model.PageSize ?? 10);
            var pCurrentPage = new SqlParameter("@CurrentPage", model.CurrentPage ?? 1);
            var pItemCount = new SqlParameter("@ItemCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            try
            {
                output = await CmsContext.Set<ArticleSearchResult>()
                    .FromSqlRaw($"EXECUTE dbo.ArticleSearch "
                    + $"@Keyword = @Keyword, "
                    + $"@Tags = @Tags, "
                    + $"@ArticleCategoryId = @ArticleCategoryId, "
                    + $"@ProductBrandId = @ProductBrandId, "
                    + $"@ArticleTypeId = @ArticleTypeId, "
                    + $"@ExceptionId = @ExceptionId, "
                    + $"@ExceptionArticleTop = @ExceptionArticleTop, "
                    + $"@FromDate = @FromDate, "
                    + $"@ToDate = @ToDate, "
                    + $"@Efficiency = @Efficiency, "
                    + $"@Active = @Active, "
                    + $"@CreateBy = @CreateBy, "
                    + $"@PageSize = @PageSize, "
                    + $"@CurrentPage = @CurrentPage, "
                    + $"@ItemCount = @ItemCount out"
                    , pKeyword, pTags, pArticleCategoryId, pProductBrandId, pArticleTypeId,
                    pExceptionId, pExceptionArticleTop, pFromDate, pToDate, pEfficiency, pActive, pCreateBy, pPageSize, pCurrentPage, pItemCount
                    )
                    .AsTracking()
                    .ToListAsync();
            }
            catch(Exception ex)
            {

            }

            return output;
        }
        
        public async  Task<VirtualizeResponse<ArticleSearchResult>> ArticleSearchWithPaging(ArticleSearchFilter model)
        {
            var output = new VirtualizeResponse<ArticleSearchResult>();
            var itemCounts = new OutputParameter<int?>();
            var returnValues = new OutputParameter<int>();           

            var result = await CmsContext.GetProcedures().ArticleSearchAsync(
            model.Keyword,
            model.Tags,
            model.ArticleCategoryId,
            model.ArticleStatusId,
            model.ProductBrandId,
            model.ArticleTypeId,
            model.ExceptionId,
            model.ExceptionArticleTop,
            model.FromDate,
            model.ToDate,
            model.Efficiency,
            model.Active,
            model.CreateBy,
            model.PageSize,
            model.CurrentPage, itemCounts, returnValues
            );
            output.Items = result.ToList();
            output.TotalSize = (int)itemCounts.Value;
            return output;     
        }

        public async Task ArticleSetArticleCategory( int ArticleId, int ArticleCategoryId)
        {
            
            var item = await CmsContext.ArticleCategoryArticle.FirstOrDefaultAsync(p => p.ArticleId == ArticleId && p.ArticleCategoryId == ArticleCategoryId);
            if(item != null) // Update
            {
                item.ArticleCategoryId = ArticleCategoryId;
                CmsContext.SaveChanges();
            }
            else
            {
                var addItem = new ArticleCategoryArticle();
                addItem.ArticleId = ArticleId;
                addItem.ArticleCategoryId = ArticleCategoryId;
                CmsContext.ArticleCategoryArticle.Add(addItem);
                await CmsContext.SaveChangesAsync();
            }
        }
        
        private async Task SaveImage(IEnumerable<IFormFile> files, int ArticleId, IHostingEnvironment _env)
        {
            
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file == null) return;
                    var urlArticle = CreateArticleURL(ArticleId);
                    var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                    var fileName = String.Format("{0}-{1}.{2}", urlArticle, timestamp, file.ContentType.Replace("image/", ""));
                    var physicalPath = Path.Combine(_env.WebRootPath,"data/article/mainimages/original", fileName);
                    using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    try
                    {
                        Utils.EditSize(Path.Combine(_env.WebRootPath, "data/article/mainimages/original", fileName), Path.Combine(_env.WebRootPath, "data/article/mainimages/small", fileName), 500, 500);
                        Utils.EditSize(Path.Combine(_env.WebRootPath, "data/article/mainimages/original", fileName), Path.Combine(_env.WebRootPath, "data/article/mainimages/thumb", fileName), 120, 120);
                    }
                    catch 
                    {
                    }
                    
                    var article = CmsContext.Article.FirstOrDefault(x => x.Id == ArticleId);
                    article.Image = fileName;
                    CmsContext.SaveChanges();                    
                }
            }
        }
        public async Task<string> CreateArticleURL(int ArticleId)
        {

            try
            {
                var currentArticle = CmsContext.Article.FirstOrDefault(p => p.Id == ArticleId);
                return FormatURL(currentArticle?.Name) + "-" + ArticleId.ToString();
            }
            catch
            {

            }
            return "nourl";
        }

        public async Task<List<ArticleGetByBlockIdResult>> ArticleGetByBlockId(int ArticleBlockId)
        {
            
            var output = new List<ArticleGetByBlockIdResult>();
            ////Parameter
            //var pArticleBlockId = new SqlParameter("@ArticleBlockId", ArticleBlockId);
            //try
            //{
            //    output = await CmsContext.Set<ArticleGetByBlockIdResult>()
            //        .FromSqlRaw($"EXECUTE dbo.ArticleGetByBlockId "
            //        + $"@ArticleBlockId = @ArticleBlockId", pArticleBlockId)
            //        .AsNoTracking()
            //        .ToListAsync();
            //}
            //catch
            //{

            //}
            //output = CmsContextProcedures.
            var result = await CmsContext.GetProcedures().ArticleGetByBlockIdAsync(ArticleBlockId);
            output = result.ToList();

            return output;
        }

        public async Task<List<ArticleGetTopByCategoryIdResult>> ArticleGetTopByCategoryId(int ArticleCategoryId)
        {
            
            var output = new List<ArticleGetTopByCategoryIdResult>();
            //Parameter
            var pArticleCategoryId = new SqlParameter("@ArticleCategoryId", ArticleCategoryId);
            try
            {
                output = await CmsContext.Set<ArticleGetTopByCategoryIdResult>()
                    .FromSqlRaw($"EXECUTE dbo.ArticleGetTopByCategoryId "
                    + $"@ArticleCategoryId = @ArticleCategoryId ", pArticleCategoryId)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch 
            {

            }
            return output;
        }
        
        public string ArticleGetStatusString(int? ArticleStatusId)
        {
            
            string Result = "";
            var currentArticleStatus = CmsContext.ArticleStatus.FirstOrDefault(p => p.Id == ArticleStatusId);

            if (currentArticleStatus.Id == 0)
            {
                Result = "<label class='badge badge-info'>Đã lưu</label>";
            }
            if (currentArticleStatus.Id == 1)
            {
                Result = "<label class='badge badge-warning'>Chờ duyệt</label>";
            }
            if (currentArticleStatus.Id == 2)
            {
                Result = "<label class='badge badge-success'>Đã đăng</label>";
            }

            return Result;
        }

        public List<Article> ArticleBlockGetAll(int ArticleBlockId)
        {
            
            var res = (from a in CmsContext.ArticleBlockArticle
                       join art in CmsContext.Article on a.ArticleId equals art.Id
                       where a.ArticleBlockId == ArticleBlockId
                       orderby art.LastEditDate descending
                       select art
                    ).ToList();
            return res;
        }

        public void ArticleBlockArticleSave(int IsAdd, int ArticleBlockID, int ArticleID)
        {
            
            var items = CmsContext.ArticleBlockArticle.FirstOrDefault(p => p.ArticleId == ArticleID && p.ArticleBlockId == ArticleBlockID);
            if (IsAdd == 1)
            {
                if (items == null)
                {
                    ArticleBlockArticle art = new ArticleBlockArticle();
                    art.ArticleBlockId = ArticleBlockID;
                    art.ArticleId = ArticleID;
                    CmsContext.ArticleBlockArticle.Add(art);
                    CmsContext.SaveChanges();
                }
            }
            else
            {
                if (items != null)
                {
                    CmsContext.ArticleBlockArticle.Remove(items);
                    CmsContext.SaveChanges();
                }
            }

        }

        public void ArticleTopCategorySave(int ArticleId)
        {
            
            var ArticleCategoryArticle_Item = CmsContext.ArticleCategoryArticle.FirstOrDefault(p => p.ArticleId == ArticleId);
            int ArticleCategoryId = ArticleCategoryArticle_Item.ArticleCategoryId;

            //var ArticleTop_Items = CmsContext.ArticleTop.Where(p => p.ArticleCategoryId == ArticleCategoryId);
            //CmsContext.ArticleTop.RemoveRange(ArticleTop_Items);

            ArticleTop ArticleTop_Item = new ArticleTop();
            ArticleTop_Item.ArticleCategoryId = ArticleCategoryId;
            ArticleTop_Item.ArticleId = ArticleId;
            CmsContext.ArticleTop.Add(ArticleTop_Item);
            
            CmsContext.SaveChanges();
        }

        public void ArticleTopParentCategorySave(int ArticleId)
        {
            
            var ArticleCategoryArticle_Item = CmsContext.ArticleCategoryArticle.FirstOrDefault(p => p.ArticleId == ArticleId);
            int ArticleCategoryId = ArticleCategoryArticle_Item.ArticleCategoryId;

            var ArticleCategory_Item = CmsContext.ArticleCategory.FirstOrDefault(p => p.Id == ArticleCategoryId);

            if (ArticleCategory_Item.ParentId != null)
            {
                int ArticleCategoryParentId = ArticleCategory_Item.ParentId.Value;
                //var ArticleTop_Items = CmsContext.ArticleTop.Where(p => p.ArticleCategoryId == ArticleCategoryParentId);
                //CmsContext.ArticleTop.RemoveRange(ArticleTop_Items);

                ArticleTop ArticleTop_Item = new ArticleTop();
                ArticleTop_Item.ArticleCategoryId = ArticleCategoryParentId;
                ArticleTop_Item.ArticleId = ArticleId;
                CmsContext.ArticleTop.Add(ArticleTop_Item);

                CmsContext.SaveChanges();
            }
        }

        public async Task<List<SearchBreadCrumbByCateResult>> ArticleBreadCrumbGetByCategoryId(int ArticleCategoryId)
        {
            
            var output = new List<SearchBreadCrumbByCateResult>();
            //Parameter
            var pArticleCategoryId = new SqlParameter("@ArticleCategoryId", ArticleCategoryId);
            try
            {
                output = await CmsContext.Set<SearchBreadCrumbByCateResult>()
                    .FromSqlRaw($"EXECUTE dbo.SearchBreadCrumbByCateResult "
                    + $"@ArticleCategoryId = @ArticleCategoryId ", pArticleCategoryId)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch 
            {

            }
            return output;
        }

        public async Task<List<ArticleGetNewByCategoryIdResult>> ArticleGetNewByCategoryId(int ArticleCategoryId, int Number)
        {
            
            var output = new List<ArticleGetNewByCategoryIdResult>();
            //Parameter
            var pArticleCategoryId = new SqlParameter("@ArticleCategoryId", ArticleCategoryId);
            var pNumber = new SqlParameter("@Number", Number);
            try
            {
                output = await CmsContext.Set<ArticleGetNewByCategoryIdResult>()
                    .FromSqlRaw($"EXECUTE dbo.ArticleGetNewByCategoryId "
                    + $"@ArticleCategoryId = @ArticleCategoryId, "
                    + $"@Number = @Number "
                    , pArticleCategoryId,pNumber)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch 
            {

            }
            return output;
        }


        public async Task ArticleUpdateStatusType(int ArticleId, int StatusTypeId)
        {
            
            var article = await CmsContext.Article.FirstOrDefaultAsync(p => p.Id == ArticleId);
            if(article != null)
            {
                article.ArticleStatusId = StatusTypeId;
                await CmsContext.SaveChangesAsync();
            }    
        }

        public async Task ArticleAddCounter(int ArticleId)
        {
            
            var article = await CmsContext.Article.FirstOrDefaultAsync(p => p.Id == ArticleId);
            if (article != null)
            {
                article.Counter = article.Counter + 1;
                CmsContext.SaveChanges();
            }
        }

        public async Task<Tuple<List<ArticleGetByCategoryIdResult>, int>> ArticleGetByCategoryId(int ArticleCategoryId, int? PageSize, int? CurrentPage)
        {
            
            var output = new List<ArticleGetByCategoryIdResult>();
            //Parameter
            var pArticleCategoryId = new SqlParameter("@ArticleCategoryId", ArticleCategoryId);
            var pPageSize = new SqlParameter("@PageSize", PageSize ?? 10);
            var pCurrentPage = new SqlParameter("@CurrentPage", CurrentPage ?? 1);
            var pItemCount = new SqlParameter("@ItemCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            try
            {
                output = await CmsContext.Set<ArticleGetByCategoryIdResult>()
                    .FromSqlRaw($"EXECUTE dbo.ArticleGetByCategoryId "
                    + $"@ArticleCategoryId = @ArticleCategoryId, "
                    + $"@PageSize = @PageSize, "
                    + $"@CurrentPage = @CurrentPage, "
                    + $"@ItemCount = @ItemCount out"
                    ,pArticleCategoryId, pPageSize, pCurrentPage, pItemCount)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch 
            {

            }
            var returnOutput = Tuple.Create(output, (Int32)pItemCount.Value);
            return returnOutput;
        }

        public List<Article> ArticleGetRelationArticle(int ArticleId)
        {
            
            var output = new List<Article>();
            try
            {
                output = (from A1 in CmsContext.ArticleRelationArticle
                          join A2 in CmsContext.Article on A1.ArticleRelationId equals A2.Id
                          where A1.ArticleId == ArticleId
                          select A2).ToList();
            }
            catch
            {

            }
            return output;
        }

        public void ArticleSaveRelatedArticle(int IsAdd, int ArticleRelateId, int ArticleID)
        {
            
            var items = CmsContext.ArticleRelationArticle.FirstOrDefault(p => p.ArticleId == ArticleID && p.ArticleRelationId == ArticleRelateId);
            if (IsAdd == 1)
            {
                if (items == null)
                {
                    ArticleRelationArticle art = new ArticleRelationArticle();
                    art.ArticleRelationId = ArticleRelateId;
                    art.ArticleId = ArticleID;
                    CmsContext.ArticleRelationArticle.Add(art);
                    CmsContext.SaveChanges();
                }
            }
            else
            {
                if (items != null)
                {
                    CmsContext.ArticleRelationArticle.Remove(items);
                    CmsContext.SaveChanges();
                }
            }
            
        }

        public async Task<List<ArticleStatus>> GetLstArticleStatus()
        {
            return await CmsContext.ArticleStatus.ToListAsync();
        }

        public async Task<List<ArticleAttachFile>> ArticleAttachGetLstByArticleId(int articleId)
        {
            return await CmsContext.ArticleAttachFile.Where(p => p.ArticleId == articleId).ToListAsync();
        }

        public async Task<bool> ArticleAttachDelete(int articleAttachFileId)
        {
            var item = await CmsContext.ArticleAttachFile.FindAsync(articleAttachFileId);
            if(item !=null)
            {
                CmsContext.ArticleAttachFile.Remove(item);
                await CmsContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> ArticleAttachInsert(List<ArticleAttachFile> model)
        {
            try
            {
                foreach (var p in model)
                {
                    CmsContext.Entry(p).State = EntityState.Added;
                    await CmsContext.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                return false;
            }
           
            return true;

        }
    }
}
