using _0_Framework.Infrastructure;
using BlogManagement.Application.Contract.ArticleCategory;
using BlogManagement.Infrastructure.Configuration.Permissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Administration.Pages.Blog.ArticleCategories {
    public class IndexModel: PageModel {
        private readonly IArticleCategoryApplication _articleCategoryApplication;
        public List<ArticleCategoryViewModel>? ArticleCategories;
        public ArticleCategorySearchModel? SearchModel;

        public IndexModel (IArticleCategoryApplication articleCategoryApplication) {
            _articleCategoryApplication = articleCategoryApplication;
        }

        [NeedsPermission(BlogPermissions.ListArticleCategories)]
        public void OnGet (ArticleCategorySearchModel searchModel) {
            ArticleCategories = _articleCategoryApplication.Search(searchModel);
        }

        public IActionResult OnGetCreate () {
            return Partial("./Create", new CreateArticleCategory());
        }

        [NeedsPermission(BlogPermissions.CreateArticleCategory)]
        public JsonResult OnPostCreate (CreateArticleCategory command) {
            var result = _articleCategoryApplication.Create(command);
            return new JsonResult(result);
        }
        
        public IActionResult OnGetEdit (long id) {
            var articleCategory = _articleCategoryApplication.GetDetails(id);
            return Partial("./Edit", articleCategory);
        }
        
        [NeedsPermission(BlogPermissions.EditArticleCategory)]
        public JsonResult OnPostEdit (EditArticleCategory command) {
            var result = _articleCategoryApplication.Edit(command);
            return new JsonResult(result);
        }
    }
}
