using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Services.RepositoriesBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Data;
using CMS.Data.ModelEntity;
using Microsoft.AspNetCore.Hosting;
using CMS.Common;
using CMS.Data.ModelDTO;
using CMS.Data.ModelFilter;

namespace CMS.Services.Repositories
{

    public interface IArticleCommentRepository : IRepositoryBase<ArticleComment>
    {
        Task<VirtualizeResponse<ArticleCommentSearchResult>> ArticleCommentSearch(ArticleCommentSearchFilter model);
        Task<int> ArticleCommentPostComment(ArticleComment model);
        Task<bool> ArticleCommentDelete(int articleCommentId);
    }
    public class ArticleCommentRepository : RepositoryBase<ArticleComment>, IArticleCommentRepository
    {     
        public ArticleCommentRepository(CmsContext CmsDBContext) : base(CmsDBContext)
        {
           
        }

        public async Task<bool> ArticleCommentDelete(int articleCommentId)
        {
            var item = await CmsContext.ArticleComment.FindAsync(articleCommentId);
            if(item !=null)
            {
                CmsContext.ArticleComment.Remove(item);
                await CmsContext.SaveChangesAsync();
            }
            else
            {
                return false;
            }
            return true;
        }

        public async Task<VirtualizeResponse<ArticleCommentSearchResult>> ArticleCommentSearch(ArticleCommentSearchFilter model)
        {
            var output = new VirtualizeResponse<ArticleCommentSearchResult>();
            var itemCounts = new OutputParameter<int?>();
            var returnValues = new OutputParameter<int>();

            var result = await CmsContext.GetProcedures().ArticleCommentSearchAsync(
                model.Keyword,
                model.ArticleId,
                model.Active,
                model.CreateBy,
                model.PageSize,
                model.CurrentPage,
                itemCounts,
                returnValues
                );
            output.Items = result.ToList();
            output.TotalSize = (int)itemCounts.Value;
            return output;
        }

        public async Task<int> ArticleCommentPostComment(ArticleComment model)
        {
            CmsContext.Entry(model).State = EntityState.Added;
            await CmsContext.SaveChangesAsync();

            return model.Id;
        }
    }
}
