using Barcode.Generator;
using FinalProject.Data;
using FinalProject.DTOs;
using FinalProject.Helper;
using FinalProject.Interfaces;
using FinalProject.Model;
using FinalProject.Models;
using FinalProject.ServiceModels;
using FinalProject.ViewModels;
using Gma.QrCodeNet.Encoding;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.Encodings.Web;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ErrorCorrectionLevel = ZXing.QrCode.Internal.ErrorCorrectionLevel;

namespace FinalProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IProductManager _productManager;
        private readonly ApplicationDbContext _context;
        //private readonly QrCodeService _qrCodeService;
        public ProductController(IProductManager productManager,
                                ICompositeViewEngine viewEngine, ApplicationDbContext context/*QrCodeService qrCodeService*/)
        {
            _productManager = productManager;
            _viewEngine = viewEngine;
            _context = context;
            //_qrCodeService = qrCodeService;
        }
        public IActionResult List([FromQuery] ProductListQueryModel request)
        {
            if (request == null) request = new ProductListQueryModel();

            return View(request);
        }
        //[HttpGet]
        //public async Task<IActionResult> GetProductQrCode(Guid productId)
        //{
        //    var product = await _productManager.GetProductById(productId);

        //    if (product == null)
        //    {
        //        return NotFound(); // Handle non-existent product
        //    }

        //    // Generate QR code content (product barcode)
        //    var qrCodeContent = product.Barcode;

        //    // Check if barcode exists (optional)
        //    if (string.IsNullOrEmpty(qrCodeContent))
        //    {
        //        return BadRequest("Product does not have a barcode"); // Handle missing barcode
        //    }

        //    // Create QR code writer and options
        //    var writer = new ZXing.BarcodeWriter { Format = ZXing.BarcodeFormat.EAN_13 }; // Change format for other barcode types
        //    var options = new QrCodeEncodingOptions
        //    {
        //        Height = 250,  // Adjust height and width as needed
        //        Width = 250,
        //        Margin = 1,
        //        ErrorCorrection = ErrorCorrectionLevel.M  // Adjust error correction level if desired
        //    };

        //    // Generate QR code bitmap
        //    var bitmap = writer.Write(qrCodeContent, options);

        //    // Convert bitmap to memory stream for sending as image
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        bitmap.Save(memoryStream, ImageFormat.Jpeg);
        //        return File(memoryStream.ToArray(), "image/jpeg");
        //    }
        //}


        [HttpGet]
        public async Task<JsonResult> Filter([FromQuery] ProductListQueryModel request)
        {
            try
            {
                var vm = await _productManager.GetFilteredProducts(request);

                var html = await RenderPartialView.InvokeAsync("_ProductListPartial",
                ControllerContext,
                                                                _viewEngine,
                                                                ViewData,
                                                                TempData,
                                                                vm);
                return Json(new
                {
                    data = html,
                    status = 200,
                    productCount = vm.ProductCount,
                    totalPage = vm.TotalPage,
                    currentPage = request.Page
                });
            }
            catch (Exception exp)
            {
                return Json(new
                {
                    error = "xeta bas verdi",
                    status = 500
                });
            }

        }
        [HttpGet]
        public async Task<IActionResult> Detail(Guid productid)
        {
            var productGetResult = await _productManager.GetProductById(productid);

            if (productGetResult is null)
            {
                return Content("bele bir mehsul yoxdur");
            }

            var vm = new ProductDetailVm();

            vm.Product = productGetResult;

            return View(vm);
        }
        
        [HttpGet]
        public async Task<IActionResult> AddToCart(Guid productid)
        {

            ProductDto productGetResult = await _productManager.GetProductById(productid);
            TempData["productId"] = productid;
            if (productGetResult is null)
            {
                return Content("bele bir mehsul yoxdur");
            }

            var vm = new ProductDetailVm();

            vm.Product = productGetResult;

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart()
        {
            ProductDto productGetResult = await _productManager.GetProductById((Guid)TempData["productId"]);

            Cart productCart = new Cart();


            productCart.Name = productGetResult.Name;
            productCart.Barcode = productGetResult.Barcode;
            productCart.SellAmount = productGetResult.Price;
       
            productCart.TotalPrice = productGetResult.TotalPrice;
            productCart.Quantity = productGetResult.Quantity;

            await _context.Carts.AddAsync(productCart);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> ReviewDetail(Guid productid)
        {
            var productGetResult = await _productManager.GetProductById(productid);
            TempData["productId1"] = productid;

            if (productGetResult is null)
            {
                return Content("bele bir mehsul yoxdur");
            }

            var vm = new ProductDetailVm();

            vm.Product = productGetResult;

            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> ReviewDetail(ProductDetailVm request)
        {
            if (!ModelState.IsValid)
            {
               return View(request);
           }

            var productReview = new ProductReview();
            
           productReview.Text = request.Message;
            
            productReview.ProductId = (Guid)TempData["productId"];
           await _context.ProductReviews.AddAsync(productReview);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");


        }


    }
}
