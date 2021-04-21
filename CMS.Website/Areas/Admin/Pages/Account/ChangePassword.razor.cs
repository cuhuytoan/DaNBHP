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

namespace CMS.Website.Areas.Admin.Pages.Account
{
    public partial class ChangePassword : IDisposable
    {
        #region Inject   
        [Inject]
        IMapper Mapper { get; set; }
        [Inject]
        ILoggerManager Logger { get; set; }
        [Inject]
        UserManager<IdentityUser> UserManager { get; set; }
        [Inject]
        SignInManager<IdentityUser> SignInManager { get; set; }
        #endregion



        #region Parameter

        public ChangePwdModel changePwdModel { get; set; } = new ChangePwdModel();

      
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        ClaimsPrincipal user;
        public string userId { get; set; }



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
            userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

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


        }
        #endregion

        #region Event

        async Task PostChangePassword()
        {
            var currentUser = await UserManager.FindByIdAsync(userId);
            if (currentUser != null)
            {
                try { 
                 
                    var result = await UserManager.ChangePasswordAsync(currentUser, changePwdModel.CurrentPassword, changePwdModel.Password);
                    await UserManager.UpdateAsync(currentUser);
                    await SignInManager.RefreshSignInAsync(currentUser);

                    if (result.Succeeded)
                    {
                        //ToastMessage
                        toastService.ShowToast(ToastLevel.Success, "Cập nhật mật khẩu thành công", "Thành công");
                    }
                    else
                    {
                        //ToastMessage
                        toastService.ShowToast(ToastLevel.Error, $"Có lỗi trong quá trình cập nhật", "Lỗi");
                    }
                    
                }
                catch (Exception ex)
                {
                    //ToastMessage
                    toastService.ShowToast(ToastLevel.Error, $"Có lỗi trong quá trình cập nhật", "Lỗi");
                }

            }
            else
            {
                //ToastMessage
                toastService.ShowToast(ToastLevel.Error, $"Không tồn tại tài khoản cập nhật", "Lỗi");
            }

        }

        #endregion
    }
}
