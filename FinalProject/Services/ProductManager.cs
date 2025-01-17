﻿using FinalProject.Areas.Admin.ViewModels;
using FinalProject.Data;
using FinalProject.DTOs;
using FinalProject.Interfaces;
using FinalProject.Model;
using FinalProject.ServiceModels;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace FinalProject.Services
{
    public class ProductManager : IProductManager
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;

        private readonly HttpContext _httpContext;

        private readonly string _productImageBasePath;
        private readonly string _userId;
        public ProductManager(IHttpContextAccessor httpContextAccessor,
                             IConfiguration configuration,
                             ApplicationDbContext context)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _configuration = configuration;
            _context = context;
            _productImageBasePath = _configuration["Folders:Products"];
            _userId = GetUserId();

        }

        public async Task<bool> CreateProduct(ProductPostModel request)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                #region Create product entity

                var productVariant = new ProductVariant();

                productVariant.Name = request.Name;

                await _context.ProductVariants.AddAsync(productVariant);
                await _context.SaveChangesAsync();

                var product = new Product();

                product.Name = request.Name;
                product.Barcode = request.Barcode;
                product.CategoryId = request.CategoryId;
                product.GendrTypeId = request.GenderTypeId;
                product.Description = request.Description;
                product.SellAmount = request.SellAmount;
                product.BuyAmount = request.BuyAmount;
                product.BuyLimit = request.BuyLimit ?? 10; 
                product.Quantity = request.Quantity;
                product.InStock = request.InStock;
                product.ShowQuantity = request.ShowQuantity;
                product.HasShipping = request.HasShipping;
                product.Discount = request.Discount;
                product.ProductVariantId = productVariant.Id;
                product.Slug = request.Name;

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                #endregion

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "products");

                if (request.MainImage != null)
                {

                    #region Main Image
                    string mainImageFileName = Guid.NewGuid().ToString() + request.MainImage.FileName;

                    using (FileStream fs = new FileStream(Path.Combine(path, mainImageFileName), FileMode.Create))
                    {
                        await request.MainImage.CopyToAsync(fs);
                    }


                    var mainPhoto = new ProductPhoto();

                    mainPhoto.ProductId = product.Id;
                    mainPhoto.ImageUrl = mainImageFileName;
                    mainPhoto.IsMain = true;

                    await _context.ProductPhotos.AddAsync(mainPhoto);
                    await _context.SaveChangesAsync();
                }

                #endregion

                #region Other Images
                foreach (var otherImage in request.OtherImages)
                {
                    string otherImageFileName = Guid.NewGuid().ToString() + otherImage.FileName;

                    using (FileStream fs = new FileStream(Path.Combine(path, otherImageFileName), FileMode.Create))
                    {
                        await otherImage.CopyToAsync(fs);
                    }


                    var otherPhoto = new ProductPhoto();

                    otherPhoto.ProductId = product.Id;
                    otherPhoto.ImageUrl = otherImageFileName;
                    otherPhoto.IsMain = false;

                    await _context.ProductPhotos.AddAsync(otherPhoto);
                    await _context.SaveChangesAsync();
                }
                #endregion

                #region Speficiations 
                if (request.Specifications != null)
                   {
                        foreach (var specification in request.Specifications)
                        {
                           ProductOption po = new ProductOption();

                            po.ProductId = product.Id;
                            po.OptionId = specification.OptionId;
                            po.Value = specification.Value;

                            await _context.ProductOptions.AddAsync(po);
                            await _context.SaveChangesAsync();
                        }
                    }
                    #endregion

                    await _context.Database.CommitTransactionAsync();

                return true;

            }
            catch (Exception exp)
            {
                await _context.Database.RollbackTransactionAsync();

                return false;
            }
        }


        public Product GetById(Guid Id)
        {
            return _context.Products.Find(Id);
        }

        public void DeleteProduct(Guid Id)
        {
            var product = _context.Products.Find(Id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        
        public async Task<ProductListVm> GetFilteredProducts(ProductListQueryModel request)

        {
            var takeNumber = request.Take ?? Convert.ToInt32(_configuration["List:Products"]);


            var query = _context.Products.Where(c => (request.CategoryId == null || c.CategoryId == request.CategoryId));
            var count = await query.CountAsync();

            query = query.OrderByDescending(c => c.Created);

            query = query.Skip((request.Page - 1) * takeNumber).Take(takeNumber);

            var productCount = await query.CountAsync();

            query = query.Include(c => c.ProductPhotos)
                         .Include(c => c.UserWishlists);


            var products = await query.Select(c => new ProductDto
            {
                ProductId = c.Id,

                Slug = c.Slug,

                Description = c.Description,

                ImageURL = _productImageBasePath + c.ProductPhotos.Where(a => a.IsMain).Select(a => a.ImageUrl).FirstOrDefault(),

                Name = c.Name,

                Price = c.SellAmount,

                IsWishlist = _userId == null ? false : c.UserWishlists.Any(a => a.UserId.ToString() == _userId),

                Discount = c.Discount,

                DiscountedPrice = c.Discount != null ? c.SellAmount - (c.SellAmount * (double)c.Discount / 100) : null
            }).ToListAsync();

            var totalPage = (int)Math.Ceiling(count / (decimal)takeNumber);

            var vm = new ProductListVm
            {
                CurrentPage = request.Page,
                TotalPage = totalPage,
                Products = products,
                ProductCount = productCount
            };

            return vm;

        }

        public async Task<ProductDto> GetProductById(Guid productId)
        {
            ProductDto result = null;

            var productFindResult = await _context.Products.AnyAsync(c => c.Id == productId);
            if (!productFindResult)
            {
                return result;
            }

            result = await _context.Products
                                     .Include(c => c.ProductPhotos)
                                    .Include(c => c.Category)
                                    .Include(c => c.ProductOptions)
                                    .ThenInclude(c => c.Option)
                                    .ThenInclude(c => c.OptionGroup)
                                    .Where(c => c.Id == productId)
                                    .Select(c => new ProductDto
                                    {
                                        ProductId = c.Id,

                                        Slug = c.Slug,

                                        Barcode=c.Barcode,

                                        Description = c.Description,

                                        ImageURL = _productImageBasePath + c.ProductPhotos.Where(a => a.IsMain).Select(a => a.ImageUrl).FirstOrDefault(),

                                        OtherImages = c.ProductPhotos.Where(a => !a.IsMain).Select(a => _productImageBasePath + a.ImageUrl).ToList(),

                                        Name = c.Name,

                                        Price = c.SellAmount,

                                        TotalPrice  = c.SellAmount * c.Quantity,

                                        IsWishlist = _userId == null ? false : c.UserWishlists.Any(a => a.UserId.ToString() == _userId),

                                        Discount = c.Discount,

                                        DiscountedPrice = c.Discount != null ? c.SellAmount - (c.SellAmount * (double)c.Discount / 100) : null,

                                        CategoryName = c.Category.Name,

                                        CategoryId = c.CategoryId,
                                        Specifications = c.ProductOptions.Select(a => new ProductSpecificationValue
                                        {
                                            Value = a.Value,
                                            OptionId = a.Id,
                                            Name = a.Option.OptionGroup.Name
                                        }).ToList()

                                    }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<(bool, Dictionary<string, string>)> ValidateProduct(ProductPostModel request)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            bool IsValid = true;

            #region Name validation
            if (String.IsNullOrEmpty(request.Name))
            {
                IsValid = false;
                errors.Add("ProductPost.Name", "Məhsulun adı boş ola bilməz.");
            }

            if (request.Name?.Length > 300)
            {
                IsValid = false;
                errors.Add("ProductPost.Name", "Məhsulun adı bu qədər uzun ola bilməz. (MAX 300)");
            }
            #endregion

            #region Barcode validation
            if (String.IsNullOrEmpty(request.Barcode))
            {
                IsValid = false;
                errors.Add("ProductPost.Barcode", "Məhsulun barkodu boş ola bilməz.");
            }
            #endregion

            return (IsValid, errors);
        }

        private string GetUserId()
        {
            string userId = null;

            if (_httpContext.User.Identity.IsAuthenticated)
            {
                userId = _httpContext.User.Claims.Where(c => c.Type == "Id").FirstOrDefault().Value;
            }

            return userId;
        }

        
    }

    
}
