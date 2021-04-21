using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using AutoMapper;
using CMS.Data.ModelEntity;
using CMS.Website.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using CMS.Data.ModelDTO;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Editor;
using Microsoft.AspNetCore.Components.Forms;
using Blazored.Toast.Services;
using System.IO;
using CMS.Common;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.SignalR.Client;
using CMS.Data.ModelFilter;
using System.ComponentModel.DataAnnotations;

namespace CMS.Website.Areas.Admin.Pages.Article
{
    public partial class Preview : IDisposable
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

        public int? articleId { get; set; }
        public ArticleDTO article { get; set; } = new ArticleDTO();
        public int ArticleStatusId { get; set; } = 0;

        [Required(ErrorMessage = "Nhập bình luận")]
        [MinLength(50,ErrorMessage ="Bình luận tối thiểu 50 kí tự")]
        public string comment { get; set; }
        //List ArticleComment 
        List<ArticleCommentSearchResult> lstArticleComment { get; set; }
        ////List FileAttach binding
        List<ArticleAttachFile> lstAttachFileBinding { get; set; } = new List<ArticleAttachFile>();
        //Noti Hub
        private HubConnection hubConnection;
        //For reload
        bool _forceRerender;
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        ClaimsPrincipal user;
        public List<string> RemoveAttributes { get; set; } = new List<string>() { "data-id" };
        public List<string> StripTags { get; set; } = new List<string>() { "font" };
        #endregion

        #region LifeCycle
        protected override async Task OnParametersSetAsync()
        {


        }
        protected override void OnInitialized()
        {

        }
        protected override async Task OnInitializedAsync()
        {
            //get claim principal
            var authState = await authenticationStateTask;
            user = authState.User;
            //Init Hub
            hubConnection = new HubConnectionBuilder()
              .WithUrl(NavigationManager.ToAbsoluteUri("/notificationHubs"))
              .Build();

            await hubConnection.StartAsync();
            //
            await InitControl();
            await InitData();
            StateHasChanged();

        }

        public void Dispose()
        {
            //GC.SuppressFinalize(this);
            _ = hubConnection.DisposeAsync();
        }
        #endregion

        #region Init
        protected async Task InitControl()
        {
         

        }
        protected async Task InitData()
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var queryStrings = QueryHelpers.ParseQuery(uri.Query);
            if (queryStrings.TryGetValue("articleId", out var _articleId))
            {
                this.articleId = Convert.ToInt32(_articleId);
            }

            if (articleId != null)
            {
                var result = await Repository.Article.ArticleGetById((int)articleId);
                if (result != null)
                {
                    article = Mapper.Map<ArticleDTO>(result);                    
                }
                lstAttachFileBinding = await Repository.Article.ArticleAttachGetLstByArticleId((int)articleId);
                ArticleCommentSearchFilter model = new ArticleCommentSearchFilter();
                model.Keyword = "";
                model.ArticleId = (int)articleId;
                model.Active = true;
                model.CreateBy = null;
                model.PageSize = 100;
                model.CurrentPage = 1;

                var lstResult = await Repository.ArticleComment.ArticleCommentSearch(model);
                if(lstResult!=null)
                {
                    lstArticleComment = lstResult.Items;
                }    
            }
        }
        #endregion

        #region Event
        async Task OnPostComment()
        {
            ArticleComment item = new ArticleComment();
            item.ArticleId = articleId;
            item.Content = comment;
            item.Name = user.Identity.Name;
            item.CreateBy = UserManager.GetUserId(user);
            item.CreateDate = DateTime.Now;
            item.Email = UserManager.GetUserName(user);
            item.Active = true;


            try
            {
                await Repository.ArticleComment.ArticleCommentPostComment(item);
                await InitData();
                comment = "";
                StateHasChanged();

                toastService.ShowToast(ToastLevel.Success, "Bạn dã gửi bình luận thành công", "Thành công");
            }
            catch(Exception ex)
            {
                toastService.ShowToast(ToastLevel.Error, "Có lỗi trong quá trình gửi bình luận", "Thất bại");
            }
            
        }
        #endregion
    }
}
