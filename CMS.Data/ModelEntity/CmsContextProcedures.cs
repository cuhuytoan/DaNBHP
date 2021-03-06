// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using CMS.Data.ModelEntity;

namespace CMS.Data.ModelEntity
{
    public partial class CmsContext
    {
        private CmsContextProcedures _procedures;

        public CmsContextProcedures Procedures
        {
            get
            {
                if (_procedures is null) _procedures = new CmsContextProcedures(this);
                return _procedures;
            }
            set
            {
                _procedures = value;
            }
        }

        public CmsContextProcedures GetProcedures()
        {
            return Procedures;
        }
    }

    public partial class CmsContextProcedures
    {
        private readonly CmsContext _context;

        public CmsContextProcedures(CmsContext context)
        {
            _context = context;
        }

        public virtual async Task<AccountSearchResult[]> AccountSearchAsync(string Keyword, Guid? RoleId, bool? Active, int? PageSize, int? CurrentPage, OutputParameter<int?> ItemCount, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterItemCount = new SqlParameter
            {
                ParameterName = "ItemCount",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "Keyword",
                    Size = 4000,
                    Value = Keyword ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "RoleId",
                    Value = RoleId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "Active",
                    Value = Active ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Bit,
                },
                new SqlParameter
                {
                    ParameterName = "PageSize",
                    Value = PageSize ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "CurrentPage",
                    Value = CurrentPage ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterItemCount,
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<AccountSearchResult>("EXEC @returnValue = [dbo].[AccountSearch] @Keyword, @RoleId, @Active, @PageSize, @CurrentPage, @ItemCount OUTPUT", sqlParameters, cancellationToken);

            ItemCount.SetValue(parameterItemCount.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<ArticleCommentSearchResult[]> ArticleCommentSearchAsync(string Keyword, int? ArticleId, bool? Active, string CreateBy, int? PageSize, int? CurrentPage, OutputParameter<int?> ItemCount, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterItemCount = new SqlParameter
            {
                ParameterName = "ItemCount",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "Keyword",
                    Size = 4000,
                    Value = Keyword ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "ArticleId",
                    Value = ArticleId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "Active",
                    Value = Active ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Bit,
                },
                new SqlParameter
                {
                    ParameterName = "CreateBy",
                    Size = 200,
                    Value = CreateBy ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "PageSize",
                    Value = PageSize ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "CurrentPage",
                    Value = CurrentPage ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterItemCount,
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<ArticleCommentSearchResult>("EXEC @returnValue = [dbo].[ArticleCommentSearch] @Keyword, @ArticleId, @Active, @CreateBy, @PageSize, @CurrentPage, @ItemCount OUTPUT", sqlParameters, cancellationToken);

            ItemCount.SetValue(parameterItemCount.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<ArticleGetByBlockIdResult[]> ArticleGetByBlockIdAsync(int? ArticleBlockId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "ArticleBlockId",
                    Value = ArticleBlockId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<ArticleGetByBlockIdResult>("EXEC @returnValue = [dbo].[ArticleGetByBlockId] @ArticleBlockId", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<ArticleGetByCategoryIdResult[]> ArticleGetByCategoryIdAsync(int? ArticleCategoryId, int? PageSize, int? CurrentPage, OutputParameter<int?> ItemCount, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterItemCount = new SqlParameter
            {
                ParameterName = "ItemCount",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "ArticleCategoryId",
                    Value = ArticleCategoryId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "PageSize",
                    Value = PageSize ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "CurrentPage",
                    Value = CurrentPage ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterItemCount,
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<ArticleGetByCategoryIdResult>("EXEC @returnValue = [dbo].[ArticleGetByCategoryId] @ArticleCategoryId, @PageSize, @CurrentPage, @ItemCount OUTPUT", sqlParameters, cancellationToken);

            ItemCount.SetValue(parameterItemCount.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<ArticleGetNewByCategoryIdResult[]> ArticleGetNewByCategoryIdAsync(int? ArticleCategoryId, int? Number, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "ArticleCategoryId",
                    Value = ArticleCategoryId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "Number",
                    Value = Number ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<ArticleGetNewByCategoryIdResult>("EXEC @returnValue = [dbo].[ArticleGetNewByCategoryId] @ArticleCategoryId, @Number", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<ArticleGetTopByCategoryIdResult[]> ArticleGetTopByCategoryIdAsync(int? ArticleCategoryId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "ArticleCategoryId",
                    Value = ArticleCategoryId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<ArticleGetTopByCategoryIdResult>("EXEC @returnValue = [dbo].[ArticleGetTopByCategoryId] @ArticleCategoryId", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<ArticleSearchResult[]> ArticleSearchAsync(string Keyword, string Tags, int? ArticleCategoryId, int? ArticleStatusId, int? ProductBrandId, int? ArticleTypeId, int? ExceptionId, bool? ExceptionArticleTop, DateTime? FromDate, DateTime? ToDate, bool? Efficiency, bool? Active, Guid? CreateBy, int? PageSize, int? CurrentPage, OutputParameter<int?> ItemCount, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterItemCount = new SqlParameter
            {
                ParameterName = "ItemCount",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "Keyword",
                    Size = 4000,
                    Value = Keyword ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Tags",
                    Size = 400,
                    Value = Tags ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "ArticleCategoryId",
                    Value = ArticleCategoryId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "ArticleStatusId",
                    Value = ArticleStatusId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "ProductBrandId",
                    Value = ProductBrandId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "ArticleTypeId",
                    Value = ArticleTypeId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "ExceptionId",
                    Value = ExceptionId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "ExceptionArticleTop",
                    Value = ExceptionArticleTop ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Bit,
                },
                new SqlParameter
                {
                    ParameterName = "FromDate",
                    Value = FromDate ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.DateTime,
                },
                new SqlParameter
                {
                    ParameterName = "ToDate",
                    Value = ToDate ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.DateTime,
                },
                new SqlParameter
                {
                    ParameterName = "Efficiency",
                    Value = Efficiency ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Bit,
                },
                new SqlParameter
                {
                    ParameterName = "Active",
                    Value = Active ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Bit,
                },
                new SqlParameter
                {
                    ParameterName = "CreateBy",
                    Value = CreateBy ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "PageSize",
                    Value = PageSize ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "CurrentPage",
                    Value = CurrentPage ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterItemCount,
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<ArticleSearchResult>("EXEC @returnValue = [dbo].[ArticleSearch] @Keyword, @Tags, @ArticleCategoryId, @ArticleStatusId, @ProductBrandId, @ArticleTypeId, @ExceptionId, @ExceptionArticleTop, @FromDate, @ToDate, @Efficiency, @Active, @CreateBy, @PageSize, @CurrentPage, @ItemCount OUTPUT", sqlParameters, cancellationToken);

            ItemCount.SetValue(parameterItemCount.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<SearchBreadCrumbByCateResult[]> SearchBreadCrumbByCateAsync(int? ArticleCategoryId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "ArticleCategoryId",
                    Value = ArticleCategoryId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SearchBreadCrumbByCateResult>("EXEC @returnValue = [dbo].[SearchBreadCrumbByCate] @ArticleCategoryId", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<UserNotifySearchResult[]> UserNotifySearchAsync(int? UserNotifyTypeId, Guid? AspNetUsersId, bool? Readed, int? PageSize, int? CurrentPage, OutputParameter<int?> ItemCount, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterItemCount = new SqlParameter
            {
                ParameterName = "ItemCount",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "UserNotifyTypeId",
                    Value = UserNotifyTypeId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "AspNetUsersId",
                    Value = AspNetUsersId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "Readed",
                    Value = Readed ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Bit,
                },
                new SqlParameter
                {
                    ParameterName = "PageSize",
                    Value = PageSize ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "CurrentPage",
                    Value = CurrentPage ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterItemCount,
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<UserNotifySearchResult>("EXEC @returnValue = [dbo].[UserNotifySearch] @UserNotifyTypeId, @AspNetUsersId, @Readed, @PageSize, @CurrentPage, @ItemCount OUTPUT", sqlParameters, cancellationToken);

            ItemCount.SetValue(parameterItemCount.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
    }
}
