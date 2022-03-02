﻿using System.Globalization;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contract.Product;
using ShopManagement.Domain.ProductAgg;

namespace SM.Infrastructure.EfCore.Repository {
    public class ProductRepository: RepositoryBase<long, Product>, IProductRepository {
        private readonly ShopContext _context;

        public ProductRepository (ShopContext context) : base(context) {
            _context = context;
        }

        public EditProduct GetDetails (long id) {
            return _context.Products.Select(x => new EditProduct {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                Picture = x.Picture,
                PictureTitle = x.PictureTitle,
                PictureAlt = x.PictureAlt,
                UnitPrice = x.UnitPrice,
                Slug = x.Slug,
                MetaDescription = x.MetaDescription,
                Keywords = x.Keywords,
                CategoryId = x.CategoryId
            }).FirstOrDefault(x => x.Id == id) ?? throw new InvalidOperationException();
        }

        public List<ProductViewModel> Search (ProductSearchModel searchModel) {
            var query = _context.Products.Include(x => x.Category)
                .Select(x => new ProductViewModel {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    UnitPrice = x.UnitPrice,
                    Picture = x.Picture,
                    Category = x.Category.Name,
                    CategoryId= x.CategoryId,
                    IsInStock = x.IsInStock,
                    CreationDate = x.CreationDate.ToString(CultureInfo.InvariantCulture),
                });
            if(!string.IsNullOrWhiteSpace(searchModel.Name)) {
                query = query.Where(x => x.Name.Contains(searchModel.Name));
            }

            if(!string.IsNullOrWhiteSpace(searchModel.Code)) {
                query = query.Where(x => x.Code.Contains(searchModel.Code));
            }

            if (searchModel.CategoryId != 0) {
                query=query.Where(x=>x.CategoryId==searchModel.CategoryId);
            }
            return query.OrderByDescending(x => x.Id).ToList();
        }

    }
}