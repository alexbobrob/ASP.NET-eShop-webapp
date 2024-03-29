﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    //Principal entity
    public class Basket
    {
        //Principal key
        public int BasketId { get; set; }
        //Foreign key
        public string OwnerID { get; set; }
        //Collection navigation property:
        public List<BasketItem> BasketItems { get; set; }

        public override string ToString()
        {
            return $"{{ BasketId: {BasketId}, OwnerId: {OwnerID}, BasketItems: {PostsToString()} }}";
        }

        private string PostsToString()
        {
            string str = "";
            if (BasketItems != null)
            {
                foreach (var post in BasketItems)
                {
                    str += post.ToString();
                }
            }
            return str;
        }

        public int CalculateTotalCost()
        {
            int sum = 0;
            foreach(var basketItem in BasketItems)
            {
                sum += basketItem.ProductQuantity * basketItem.Product.Price;
            }
            return sum;
        }

        public int CalculateTotalNumberOfProducts()
        {
            int sum = 0;
            foreach(var basketItem in BasketItems)
            {
                sum += basketItem.ProductQuantity;
            }
            return sum;
        }
    }
}
