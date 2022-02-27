﻿
using System.Security.Cryptography.X509Certificates;
using _0_Framework.Application;
using ShopManagement.Application.Contract.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Application {
    public class ProductCategoryApplication: IProductCategoryApplication {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryApplication (IProductCategoryRepository productCategoryRepository) {
            _productCategoryRepository = productCategoryRepository;
        }

        public OperationResult Create (CreateProductCategory command) {
            var operation = new OperationResult();
            if(_productCategoryRepository.Exists(x => x.Name == command.Name)) {
                return operation.Failed("امکان ثبت رکورد تکراری وجود ندارد");
            }

            var Slug = command.Slug.Slugify();
            var productCategory = new ProductCategory(command.Name, command.Description, command.Picture,
                command.PictureAlt, command.PictureTitle, command.Keywords, command.MetaDescription, Slug);

            _productCategoryRepository.Create(productCategory);
            _productCategoryRepository.SaveChanges();
            return operation.Succeeded();

        }

        public OperationResult Edit (EditProductCategory command) {
            var operation = new OperationResult();
            var productCategory = _productCategoryRepository.GetById(command.Id);
            if(productCategory == null) {
                operation.Failed("رکورد با اطلاعات درخواست شده یافت نشد");
            }

            if(_productCategoryRepository.Exists(x => x.Name == command.Name && x.Id != command.Id)) {
                operation.Failed("امکان ثبت رکورد تکراری وجود ندارد");
            }

            var slug = command.Slug.Slugify();
            productCategory.Edit(command.Name, command.Description, command.Picture, command.PictureAlt,
                command.PictureTitle, command.Keywords, command.MetaDescription, slug);
            _productCategoryRepository.SaveChanges();
            return operation.Succeeded();
        }

        public EditProductCategory GetDetails (long id) {
            return _productCategoryRepository.GetDetails(id);
        }

        public List<ProductCategoryViewModel> Search (ProductCategorySearchModel command) {
            return _productCategoryRepository.Search(command);
        }
    }
}