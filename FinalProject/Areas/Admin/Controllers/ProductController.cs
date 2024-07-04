using FinalProject.Helper;
using FinalProject.Areas.Admin.DTOs;
using FinalProject.Areas.Admin.Models;
using FinalProject.Areas.Admin.ViewModels;
using FinalProject.Data;
using FinalProject.DTOs;
using FinalProject.Enums;
using FinalProject.Helper;
using FinalProject.Interfaces;
using FinalProject.Model;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using FinalProject.Models;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminCookies")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IMemoryCache _memoryCache; 
        private ICompositeViewEngine _viewEngine;

        private readonly IProductManager _productManager;
        private readonly IConfiguration _configuration;

        public ProductController(ApplicationDbContext context,
                                IMemoryCache memoryCache,
                                IProductManager productManager,
                                IConfiguration configuration,
                                ICompositeViewEngine viewEngine)
        {
            _context = context;
            _productManager = productManager;
            _memoryCache = memoryCache;
            _configuration = configuration;
            _viewEngine = viewEngine;
        }

        [HttpGet]
        public async Task<IActionResult> List(int page = 1)
        {
            var query = _context.Products.Where(u => true);

            var result = await SelectProduct(query, page);

            var vm = new ProductsListVm
            {
                CurrentPage = page,
                TotalPage = result.Item2,
                Products = result.Item1
            };

            return View(vm);
        }
        public async Task<JsonResult> Filter([FromQuery] ProductFilterModel request)
        {
            

            var query = _context.Products.Where(u =>
                                            (String.IsNullOrEmpty(request.Name) || u.Name.ToLower().StartsWith(request.Name.ToLower())));
                                            

            var result = await SelectProduct(query, request.Page);
            

            var vm = new ProductListVm
            {
                CurrentPage = request.Page,
                TotalPage = result.Item2,
                Products = result.Item1
            };

            var html = await RenderPartialView.InvokeAsync("_ProductListPartialView",
                                                            ControllerContext,
                                                            _viewEngine,
                                                            ViewData,
                                                            TempData,
                                                            vm);

            return Json(new
            {
                status = 200,
                data = html
            });
        }

        private async Task<(List<ProductDto>, int)> SelectProduct(IQueryable<Product> query, int page)
        {
            var takeNumber = Convert.ToInt32(_configuration["List:AdminProduct"]);

            query = query.OrderByDescending(u => u.Created);

            var count = await query.CountAsync();

            var totalPage = (int)Math.Ceiling(count / (decimal)takeNumber);

            var products = await query
                                 .Select(u => new ProductDto
                                 {
                                     ProductId = u.Id,
                                     Name = u.Name,
                                     Barcode = u.Barcode,
                                     Description = u.Description,
                                     Price=u.SellAmount
                                     

                                 })
                                 .Skip((page - 1) * takeNumber)
                                 .Take(takeNumber)
                                 .ToListAsync();


            return (products, totalPage);
        }

        private async Task<(List<ProductDto>, int)> SelectOrderProduct(IQueryable<Cart> query, int page)
        {
            var takeNumber = Convert.ToInt32(_configuration["List:AdminProduct"]);

            query = query.OrderByDescending(u => u.Created);

            var count = await query.CountAsync();

            var totalPage = (int)Math.Ceiling(count / (decimal)takeNumber);

            var products = await query
                                 .Select(u => new ProductDto
                                 {
                                     ProductId = u.Id,
                                     Name = u.Name,
                                     Barcode = u.Barcode,
                                     TotalPrice = u.TotalPrice,
                                     Price = u.SellAmount,
                                    Quantity = u.Quantity


                                 })
                                 .Skip((page - 1) * takeNumber)
                                 .Take(takeNumber)
                                 .ToListAsync();


            return (products, totalPage);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var vm = new ProductAddVm
            {
                ProductGet = await CreateProductGet()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<JsonResult> Add([FromForm] ProductAddVm request)
        {
            var validationResult = await _productManager.ValidateProduct(request.ProductPost);

            if (!validationResult.Item1)
            {
                await FillModelState(validationResult.Item2);

                request.ProductGet = await CreateProductGet();

                return Json(new
                {
                    status = 400
                });
            }

            var productCreateResult = await _productManager.CreateProduct(request.ProductPost);

            if (!productCreateResult)
            {
                request.ProductGet = await CreateProductGet();

                return Json(new
                {
                    status = 400
                });
            }

            return Json(new { status = 200 });
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> OrderList(int page = 1)
        {
            var query = _context.Carts.Where(u => true);

            var result = await SelectOrderProduct(query, page);

            var vm = new OrderProductListVm
            {
                CurrentPage = page,
                TotalPage = result.Item2,
                Carts = result.Item1
            };

            return View(vm);

        }

            private async Task<ProductGetModel> CreateProductGet()
        {
            List<CategoryDto> res;

            if (!_memoryCache.TryGetValue("AdminProductAddCategories", out res))
            {
                res = await GeneralHelper.GetAllCategories(_context);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                                              .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _memoryCache.Set("AdminProductAddCategories", res, cacheEntryOptions);

            }

            var genderTypes = await _context.GendrTypes.Select(c => new GenderTypeDto
            {
                GenderName = c.Name,
                GenderTypeId = c.Id
            }).ToListAsync();

            var getModel = new ProductGetModel();

            getModel.Categories = res;

            getModel.Genders = genderTypes;

            return getModel;
        }


        [HttpGet]
        public IActionResult Delete(Guid productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("List");
        }

        private async Task FillModelState(Dictionary<string, string> errors)
        {
            ModelState.Clear();

            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }

        }

    }
}
