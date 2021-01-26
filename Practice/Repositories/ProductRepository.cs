using Practice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practice.Repositories
{
    public class ProductRepository:Repository<product>
    {
        public product UpdateProduct(product p)
        {
            product up = this.Get(p.productId);
            up.productName = p.productName;
            up.quantity = p.quantity;
            up.price = p.price;
            up.userId = p.userId;
            up.updatedDate = DateTime.Now;
            this.context.SaveChanges();
            UserRepository ur = new UserRepository();
            up.user = ur.Get(up.userId);
            return up;
        }

        public product InsertProduct(product p)
        {
            UserRepository ur = new UserRepository();
            this.Insert(p);
            product sp = Get(p.productId);
            sp.user = ur.Get(sp.userId);
            return sp;
        }
    }
}