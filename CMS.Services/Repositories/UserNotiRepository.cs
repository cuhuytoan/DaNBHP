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

namespace CMS.Services.Repositories
{
    public interface IUserNotiRepository  :IRepositoryBase<UserNotify>
    {
        Task<VirtualizeResponse<UserNotifySearchResult>> GetAllNoti(int? UserNotifyTypeId, string AspNetUsersId, bool? Readed, int PageSize, int CurrentPage);
        Task<int> UserNotiCreateNew(UserNotify model);
    }
    public class UserNotiRepository : RepositoryBase<UserNotify>, IUserNotiRepository
    {
        public UserNotiRepository(CmsContext CmsDBContext) : base(CmsDBContext)
        {

        }

        public async Task<VirtualizeResponse<UserNotifySearchResult>> GetAllNoti(int? UserNotifyTypeId, string AspNetUsersId, bool? Readed, int PageSize, int CurrentPage)
        {
            var output = new VirtualizeResponse<UserNotifySearchResult>();
            var itemCounts = new OutputParameter<int?>();
            var returnValues = new OutputParameter<int>();
            var userId = new Guid();
            if(Guid.TryParse(AspNetUsersId,out Guid _userId))
            {
                userId = _userId;
            }
            var result = await CmsContext.GetProcedures().UserNotifySearchAsync(
                null,
                userId,
                Readed,
                PageSize,
                CurrentPage,
                itemCounts, 
                returnValues
           );
            output.Items = result.ToList();
            output.TotalSize = (int)itemCounts.Value;
            return output;
        }

        public async Task<int> UserNotiCreateNew(UserNotify model)
        {
            model.UserNotifyTypeId = 1;
            model.CreateDate = DateTime.Now;
            model.Readed = false;
            model.ReadCount = 0;
            model.ImageUrl = model.ImageUrl ?? "/assets/images/notidefault.jpg";
            CmsContext.Entry(model).State = EntityState.Added;
            await CmsContext.SaveChangesAsync();
            return model.Id;
        }
    }
}
