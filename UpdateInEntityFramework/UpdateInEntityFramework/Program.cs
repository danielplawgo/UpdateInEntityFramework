using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateInEntityFramework
{
    class Program
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            var productId = 0;

            #region Wykonujemy wcześniej zapytanie, aby w logu nie było sql wykonywanych przy starcie Entity framework np. sprawdzenie schematu bazy
            using (DataContext db = new DataContext())
            {
                var product = db.Products.FirstOrDefault();

                if (product != null)
                {
                    productId = product.Id;
                }
            }
            #endregion

            _logger.Info("Updating Product with getting it.");

            using (DataContext db = new DataContext())
            {
                db.Database.Log = m => _logger.Info(m);

                var product = db.Products.FirstOrDefault(p => p.Id == productId);

                if (product == null)
                {
                    return;
                }

                product.LockTime = DateTime.UtcNow;

                db.SaveChanges();
            }

            _logger.Info("Updating Product without getting it.");

            using (DataContext db = new DataContext())
            {
                db.Database.Log = m => _logger.Info(m);

                var product = new Product()
                {
                    Id = productId
                };

                db.Set<Product>().Attach(product);

                product.LockTime = DateTime.UtcNow;
                product.Count = 0;
                db.Entry(product).Property(p => p.Count).IsModified = true;

                db.SaveChanges();
            }

            _logger.Info("Deleting Product without getting it.");

            using (DataContext db = new DataContext())
            {
                db.Database.Log = m => _logger.Info(m);

                var product = new Product()
                {
                    Id = productId
                };

                db.Entry(product).State = EntityState.Deleted;

                db.SaveChanges();
            }
        }
    }
}
