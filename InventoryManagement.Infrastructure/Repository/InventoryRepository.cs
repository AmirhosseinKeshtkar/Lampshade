﻿using _0_Framework.Infrastructure;
using InventoryManagement.Application.Contract.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using SM.Infrastructure.EfCore;

namespace InventoryManagement.Infrastructure.EFCore.Repository {
    public class InventoryRepository : RepositoryBase<long, Inventory>, IInventoryRepository {
        private readonly InventoryContext _context;
        private readonly ShopContext _shopContext;
        public InventoryRepository(InventoryContext context, ShopContext shopContext) : base(context) {
            _context = context;
            _shopContext = shopContext;
        }

        public EditInventory? GetDetails(long id) {
            return _context.Inventory.Select(x => new EditInventory {
                Id = x.Id,
                ProductId = x.ProductId,
                UnitPrice = x.UnitPrice,
            }).FirstOrDefault(x=>x.Id==id);
        }

        public List<InventoryViewModel> Search(InventorySearchModel searchModel) {
            var products = _shopContext.Products.Select(x => new { x.Id, x.Name});
            var query = _context.Inventory.Select(x => new InventoryViewModel {
                Id = x.Id,
                InStock = x.InStock,
                ProductId = x.ProductId,
                UnitPrice = x.UnitPrice,
                CurrentCount = x.CalculateInventoryStock()
            });
            if (!searchModel.InStock) {
                query = query.Where(x => !x.InStock);
            }

            if (searchModel.ProductId>0) {
                query = query.Where(x => x.ProductId == searchModel.ProductId);
            }

            var inventory = query.OrderByDescending(x=>x.Id).ToList();
            inventory.ForEach(item=> {
                item.Product = products.FirstOrDefault(x => x.Id == item.ProductId)?.Name;
            });
            return inventory;
        }

        public Inventory GetByProductId(long productId) {
            return _context.Inventory.FirstOrDefault(x => x.ProductId==productId);
        }
    }
}
