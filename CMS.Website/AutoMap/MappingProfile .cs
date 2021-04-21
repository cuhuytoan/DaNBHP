using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Data.ModelEntity;
using CMS.Data.ModelDTO;
using CMS.Data.ModelFilter;

namespace CMS.Website.AutoMap
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Account
            CreateMap<AspNetUsers, AspNetUsersDTO>().ReverseMap();
            CreateMap<AspNetUserProfiles, AspNetUserProfilesDTO>().ReverseMap();
            CreateMap<AspNetUserRoles, AspNetUserRolesDTO>().ReverseMap();
            CreateMap<AspNetUserInfo, AspNetUserInfoDTO>().ReverseMap();
            //Article
            CreateMap<Article, ArticleDTO>().ReverseMap();
            CreateMap<ArticleCategory, ArticleCategoryDTO>().ReverseMap();

            CreateMap<ArticleSearchDTO, ArticleSearchResult>().ReverseMap();


            CreateMap<ArticleGetByBlockIdDTO, ArticleGetByBlockIdResult>().ReverseMap();
            CreateMap<ArticleGetTopByCategoryIdDTO, ArticleGetTopByCategoryIdResult>().ReverseMap();
            CreateMap<ArticleGetByBlockIdDTO, ArticleSearchResult>().ReverseMap();
            CreateMap<ArticleGetByBlockIdDTO, ArticleGetTopByCategoryIdResult>().ReverseMap();
            CreateMap<ArticleGetByBlockIdDTO, ArticleGetNewByCategoryIdResult>().ReverseMap();
            CreateMap<ArticleSearchDTO, ArticleGetNewByCategoryIdResult>().ReverseMap();
            CreateMap<Article, ArticleSearchResult>().ReverseMap();
            CreateMap<ArticleGetByBlockIdDTO, ArticleGetByCategoryIdResult>().ReverseMap();
            CreateMap<ArticleDTO, ArticleSearchResult>().ReverseMap();
            //ArticleComment 
            CreateMap<ArticleCommentDTO, ArticleComment>().ReverseMap();
            CreateMap<ArticleSearchDTO, ArticleCommentSearchResult>().ReverseMap();
            //Setting
            CreateMap<SettingDTO, Setting>().ReverseMap();

        }
    }
}
