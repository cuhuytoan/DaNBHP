using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CMS.Data.ModelEntity;
using CMS.Services.RepositoriesBase;
using CMS.Website.Logging;
using CMS.Data.ModelFilter;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using CMS.Services.Repositories;
using CMS.Data.ModelDTO;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Editor;
using Telerik.Blazor.Components.DropDownList;
using Telerik.Blazor;
using Microsoft.AspNetCore.Components.Forms;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using CMS.Common;
using Microsoft.AspNetCore.WebUtilities;
namespace CMS.Website.Areas.Admin.Pages.Settings
{
    public partial class Edit:IDisposable
    {
        #region Inject   
        [Inject]
        IMapper Mapper { get; set; }
        [Inject]
        ILoggerManager Logger { get; set; }
        [Inject]
        UserManager<IdentityUser> UserManager { get; set; }

        #endregion

        #region Parameter
        public SettingDTO settings { get; set; } = new SettingDTO();
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        ClaimsPrincipal user;

        #endregion

        #region LifeCycle
        protected override async Task OnParametersSetAsync()
        {

        }

        protected override async Task OnInitializedAsync()
        {
            //get claim principal
            var authState = await authenticationStateTask;
            user = authState.User;
            //
            await InitControl();
            await InitData();


        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Init
        protected async Task InitControl()
        {
        
        }
        protected async Task InitData()
        {
            var result = await Repository.Setting.GetSetting();
            if(result !=null)
            {
                settings = Mapper.Map<SettingDTO>(result);
            }    
        }
        #endregion

        #region Event
        async Task PostSetings()
        {
            settings.Id = await Repository.Setting.PostSetting(Mapper.Map<Setting>(settings));
            if(settings.Id > 0)
            {
                //ToastMessage
                toastService.ShowToast(ToastLevel.Success, "Cài đặt thành công", "Thành công");
            }
            else
            {
                //ToastMessage
                toastService.ShowToast(ToastLevel.Error, $"Có lỗi trong quá trình cập nhật ", "Lỗi");
            }
        }
        #endregion

    }
}
