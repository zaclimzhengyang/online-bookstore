using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBookShop.Data;
using OnlineBookShop.Models;
using OnlineBookShop.Repository;
using OnlineBookShop.Utility;

namespace OnlineBookShop.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class CategoryController : Controller
{
    // private readonly ICategoryRepository<Category> _categoryRepository;
    
    // public CategoryController(ICategoryRepository<Category> categoryRepository)
    // {
    //     _categoryRepository = categoryRepository;
    // }
    
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public IActionResult Index()
    {
        List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
        foreach (var item in objCategoryList)
        {
            Console.Write($"id: {item.Id}. name: {item.Name}. displayOrder: {item.DisplayOrder}");
            Console.WriteLine();
        }
        return View(objCategoryList);
    }

    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(Category obj)
    {
        if (obj.Name.ToLower() == "test")
        {
            ModelState.AddModelError("Name","Name cannot be test");    
        }
        
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name");
        }
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();
            // to be more explicit, we can write this
            // return RedirectToAction("Index", "Category");
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }
        return View();
    }

    public IActionResult Edit(int id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        // different ways to retrieve the Category object
        // Category? categoryFromDb2 = _db.Categories.FirstOrDefault(u=>u.Id==id);
        // Category? categoryFromDb3 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
        Category? categoryFromDb = _unitOfWork.Category.Get(u=>u.Id==id);
        
        if (categoryFromDb == null)
        {
            return NotFound();
        }

        return View(categoryFromDb);
    }

    [HttpPost]
    public IActionResult Edit(Category obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }

        return View();
    }

    // public IActionResult Delete(int? id)
    // {
    //     if (id == null || id == 0)
    //     {
    //         return NotFound();
    //     }
    //
    //     Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
    //
    //     if (categoryFromDb == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     return View(categoryFromDb);
    // }
    
    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        Category obj = _unitOfWork.Category.Get(u=>u.Id==id);
        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.Category.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Category deleted successfully";
        return RedirectToAction("Index");
    }
}