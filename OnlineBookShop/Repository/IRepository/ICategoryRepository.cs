using OnlineBookShop.Models;

namespace OnlineBookShop.Repository;

public interface ICategoryRepository<T> : IRepository<Category>
{
    void Update(Category obj);

}