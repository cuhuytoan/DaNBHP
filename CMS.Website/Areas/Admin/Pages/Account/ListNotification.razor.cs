﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using AutoMapper;
using CMS.Data.ModelEntity;
using CMS.Website.Logging;
using CMS.Data.ModelFilter;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using CMS.Website.Areas.Admin.Pages.Shared;
using Blazored.Toast.Services;
using CMS.Data.ModelDTO;
using Microsoft.AspNetCore.WebUtilities;

namespace CMS.Website.Areas.Admin.Pages.Account
{
    public partial class ListNotification: IDisposable
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
        [Parameter]
        public int? p { get; set; }
        #endregion

        #region Model
        List<UserNotifySearchResult> lstUserNoti { get; set; } = new List<UserNotifySearchResult>();
        public int? totalUnread { get; set; }
        public int currentPage { get; set; }
        public int totalCount { get; set; }
        public int pageSize { get; set; } = 30;
        public int totalPages => (int)Math.Ceiling(decimal.Divide(totalCount, pageSize));
   

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        ClaimsPrincipal user;
        protected ConfirmBase DeleteConfirmation { get; set; }
        string userId { get; set; }
        #endregion

        #region LifeCycle
        protected override async Task OnParametersSetAsync()
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var queryStrings = QueryHelpers.ParseQuery(uri.Query);
           
            if (queryStrings.TryGetValue("p", out var _p))
            {
                this.currentPage = Convert.ToInt32(_p);
                this.p = Convert.ToInt32(_p);
            }

            await InitData();
        }

        protected override async Task OnInitializedAsync()
        {
            //get claim principal
            var authState = await authenticationStateTask;
            user = authState.User;
            userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            await InitData();


        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Init
        private async Task InitData()
        {
            var result = await Repository.UserNoti.GetAllNoti(null, userId, null, 10, 1);
            lstUserNoti = result.Items;
            totalUnread = result.TotalSize;
        }
        #endregion
    }
}
