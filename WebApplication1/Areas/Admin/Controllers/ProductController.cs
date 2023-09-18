using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models;
using TestShop.Models.ViewModels;

namespace TestShopProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

		}
        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category");
            
            return View(objProductList);
        }
        //GET
        // Update/Insert
        public IActionResult Upsert(int? id)
        {
            //ViewBag.CategoryList = categoryList;
            //ViewData["CategoryList"] = categoryList;


            ProductVM productVm = new ProductVM()
            {
	            CategoryList = _unitOfWork.Category
		            .GetAll()
		            .Select(x => new SelectListItem
		            {
			            Text = x.Name,
			            Value = x.Id.ToString()
		            }),
	            Product = new Product()
            };
            if (id is null || id == 0)
            {
                //Create case
	            return View(productVm);
			}
            else
            {
                //Update case
                productVm.Product = _unitOfWork.Product.Get(x => x.Id==id);
				return View(productVm);
			}
			
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
	            string wwwRootPath = _webHostEnvironment.WebRootPath;
	            if (file != null)
	            {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVm.Product.ImageUrl))
                    {
						//delete old image
						var oldImagePath = 
							Path.Combine(wwwRootPath, productVm.Product.ImageUrl.TrimStart('\\'));
						if (System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}

                    }
                    using (FileStream fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
	                    file.CopyTo(fileStream);
                        
                    }

                    productVm.Product.ImageUrl = @"\images\product\" + fileName;
	            }

	            if (productVm.Product.Id == 0)
	            {
		            _unitOfWork.Product.Add(productVm.Product);
				}
	            else
	            {
		            _unitOfWork.Product.Update(productVm.Product);
				}
                _unitOfWork.Save();
                TempData["success"] = (productVm.Product.Id!=0)? "Product updated successfully": "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {
	            productVm.CategoryList = _unitOfWork.Category
		            .GetAll()
		            .Select(x => new SelectListItem
		            {
			            Text = x.Name,
			            Value = x.Id.ToString()
		            });
	            return View(productVm);
			}
        }
        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product productFromDb = _unitOfWork.Product.Get(x => x.Id == id);

            if (productFromDb == null)
            {
                return NotFound();
            }

            return View(productFromDb);
        }
        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            Product obj = _unitOfWork.Product.Get(x => x.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
	        IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category");
	        return Json(objProductList);
        }
        

        #endregion
    }
}
