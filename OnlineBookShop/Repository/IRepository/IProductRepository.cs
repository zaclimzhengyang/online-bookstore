using OnlineBookShop.Models;

namespace OnlineBookShop.Repository;

public interface IProductRepository<T> : IRepository<Product>
{
    void Update(Product obj);

}