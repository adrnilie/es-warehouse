using System.Collections.Generic;
using System.Linq;

namespace EsWarehouse.Infrastructure.Repositories
{
    public class Product
    {
        public int Sku { get; set; }
        public string Name { get; set; } = default!;
        public int Quantity { get; set; }
    }

    public class ProductsRepository
    {
        private readonly IList<Product> _products = new List<Product>();

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public void AddProducts(IEnumerable<Product> products)
        {
            foreach (var product in _products)
            {
                _products.Add(product);
            }
        }

        public Product? GetProduct(int sku)
        {
            return _products.SingleOrDefault(p => p.Sku == sku);
        }
    }
}