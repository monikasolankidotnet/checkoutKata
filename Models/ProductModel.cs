using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckoutKataMVC.Models
{
    public class ProductModel
    {
        private List<Product> products;

        public ProductModel()
        {
            this.products = new List<Product>() {
                new Product {
                    Id = "p01",
                    Name = "A",
                    Price = 10
                },
                new Product {
                    Id = "p02",
                    Name = "B",
                    Price = 15
                },
                new Product {
                    Id = "p03",
                    Name = "C",
                    Price = 40
                },
                new Product {
                    Id = "p04",
                    Name = "D",
                    Price = 55
                }
            };
        }

        public List<Product> findAll()
        {
            return this.products;
        }

        public Product find(string id)
        {
            return this.products.Single(p => p.Id.Equals(id));
        }

    }
}