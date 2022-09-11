using System;
using EsWarehouse.Domain.Events;
using EsWarehouse.Domain.Primitives;
using EsWarehouse.Infrastructure.Repositories;

namespace EsWarehouse.Infrastructure.Projections
{
    public class ProductProjection : IProjection,
        IApply<ProductCreated>,
        IApply<QuantityAdjusted>
    {
        private readonly ProductsRepository _productRepository;

        public ProductProjection(ProductsRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void ReceiveEvent(IEvent evnt)
        {
            switch (evnt)
            {
                case ProductCreated productCreated:
                    Apply(productCreated);
                    break;
                case QuantityAdjusted quantityAdjusted:
                    Apply(quantityAdjusted);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported event");
            }
        }

        public Product GetProduct(int sku)
        {
            var product = _productRepository.GetProduct(sku);
            if (product == null)
            {
                product = new Product
                {
                    Sku = sku
                };

                _productRepository.AddProduct(product);
            }

            return product;
        }

        public void Apply(ProductCreated evnt)
        {
            var product = GetProduct(evnt.Sku);
            product.Quantity += evnt.Quantity;
            product.Name = evnt.Name;
        }

        public void Apply(QuantityAdjusted evnt)
        {
            var product = GetProduct(evnt.Sku);
            product.Quantity += evnt.Quantity;
        }
    }
}