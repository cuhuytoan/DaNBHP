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

namespace CMS.Services.Repositories
{

    public interface ISettingRepository :IRepositoryBase<Setting>
    {
        Task<Setting> GetSetting();
        Task<int> PostSetting(Setting model);       
    }
    public class SettingRepository : RepositoryBase<Setting>,ISettingRepository
    {

        public SettingRepository(CmsContext CmsDBContext) : base(CmsDBContext)
        {

        }

        public async Task<Setting> GetSetting()
        {
            return await CmsContext.Setting.AsNoTracking().FirstOrDefaultAsync(p => p.Id == 1);
        }

        public async Task<int> PostSetting(Setting model)
        {
            CmsContext.Entry(model).State =  EntityState.Modified;
            await CmsContext.SaveChangesAsync();

            return model.Id;
        }
    }
}
