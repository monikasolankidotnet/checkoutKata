using CheckoutKataMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheckoutKataMVC.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        private readonly List<PricingRule> pricingRules = new List<PricingRule>();
        private readonly PricingRule ItemARule = new PricingRule
        {
            ItemName = "A",
            ItemCount = 1,
            Total = 10
        };

        private readonly PricingRule ItemBRule = new PricingRule
        {
            ItemName = "B",
            ItemCount = 3,
            Total = 40
        };

        private readonly PricingRule ItemCRule = new PricingRule
        {
            ItemName = "C",
            ItemCount = 1,
            Total = 40
        };

        private readonly PricingRule ItemDRule = new PricingRule
        {
            ItemName = "D",
            ItemCount = 1,
            Total = 55
        };

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Buy(string id)
        {
            ProductModel productModel = new ProductModel();
            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item { Product = productModel.find(id), Quantity = 1 });
                Session["cart"] = cart;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new Item { Product = productModel.find(id), Quantity = 1 });
                }
                Session["cart"] = cart;
            }

            AddPricingRule(id);
            CalculateTotal();
            return RedirectToAction("Index");
        }

        public ActionResult Remove(string id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            int index = isExist(id);
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return RedirectToAction("Index");
        }

        private int isExist(string id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.Id.Equals(id))
                    return i;
            return -1;
        }

        public void CalculateTotal()
        {
            double total = 0;
            List<Item> items = (List<Item>)Session["cart"];
            var itemGroups = items.GroupBy(g => g.Product.Name);

            foreach (var itemGroup in itemGroups)
            {
                total += PriceTotalForEachGroup(itemGroup);
            }
            Session["SumProducts"] = total;
        }

        private double PriceTotalForEachGroup(IGrouping<string, Item> itemGroup)
        {
            double total = 0;
            var ruleForGroup = pricingRules.FirstOrDefault(r => r.ItemName == itemGroup.Key);
            if (ruleForGroup != null)
            {
                var extra = itemGroup.FirstOrDefault().Quantity - ruleForGroup.ItemCount;
                if (extra < 0)
                {
                    total += itemGroup.Sum(g => g.Product.Price);
                }
                else
                {
                    total += ruleForGroup.Total;
                    total += extra * itemGroup.First()
                                            .Product.Price;
                }
            }
            else
            {
                total += itemGroup.Sum(x => x.Product.Price);
            }

            return total;
        }

        public void AddPricingRule(PricingRule rule)
        {
            if (rule == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrWhiteSpace(rule.ItemName))
            {
                throw new ArgumentException();
            }

            pricingRules.Add(rule);
        }

        public void AddPricingRule(string id)
        {
            PricingRule rule = new PricingRule();
            List<Item> cart = (List<Item>)Session["cart"];
            var name = cart.FirstOrDefault(x=> x.Product.Id == id).Product.Name;
            switch (name)
            {
                case "A":
                    rule = ItemARule;
                    break;
                case "B":
                    rule = ItemBRule;
                    break;
                case "C":
                    rule = ItemCRule;
                    break;
                case "D":
                    rule = ItemDRule;
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            if (rule == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrWhiteSpace(rule.ItemName))
            {
                throw new ArgumentException();
            }

            pricingRules.Add(rule);
        }
    }
}