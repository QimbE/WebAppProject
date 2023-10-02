using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models;
using TestShop.Models.ViewModels;
using TestShop.Utility;

namespace TestShopProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

		}
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> objProductList = await _unitOfWork.Product.GetAll(includeProperties:"Category");
            
            return View(objProductList);
        }
        //GET
        // Update/Insert
        public async Task<IActionResult> Upsert(int? id)
        {
            //ViewBag.CategoryList = categoryList;
            //ViewData["CategoryList"] = categoryList;


            ProductVM productVm = new ProductVM()
            {
	            CategoryList = (await _unitOfWork.Category
		            .GetAll())
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
                productVm.Product = await _unitOfWork.Product.Get(x => x.Id==id);
				return View(productVm);
			}
			
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductVM productVm, IFormFile? file)
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
	                    await file.CopyToAsync(fileStream);
                        
                    }

                    productVm.Product.ImageUrl = @"\images\product\" + fileName;
	            }
	            else
	            {
		            productVm.Product.ImageUrl = "";

	            }

	            if (productVm.Product.Id == 0)
	            {
		            await _unitOfWork.Product.Add(productVm.Product);
				}
	            else
	            {
		            await _unitOfWork.Product.Update(productVm.Product);
				}
                await _unitOfWork.Save();
                TempData["success"] = (productVm.Product.Id!=0)? "Product updated successfully": "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {
	            productVm.CategoryList = (await _unitOfWork.Category
		            .GetAll())
		            .Select(x => new SelectListItem
		            {
			            Text = x.Name,
			            Value = x.Id.ToString()
		            });
	            return View(productVm);
			}
        }
        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
	        IEnumerable<Product> objProductList = await _unitOfWork.Product.GetAll(includeProperties: "Category");
	        return Json(objProductList);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
	        Product productToBeDeleted = await _unitOfWork.Product.Get(x => x.Id == id);
	        if (productToBeDeleted == null)
	        {
		        return Json(new { success = false, message = "Error while deleting" });
	        }

            //delete image
			var oldImagePath =
								Path.Combine(_webHostEnvironment.WebRootPath,
								productToBeDeleted.ImageUrl.TrimStart('\\'));
			if (System.IO.File.Exists(oldImagePath))
			{
				System.IO.File.Delete(oldImagePath);
			}

            await _unitOfWork.Product.Remove(productToBeDeleted);
            await _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
		}

		#endregion
	}
}
