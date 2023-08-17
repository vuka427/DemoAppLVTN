using Application.Interface.IDomainServices;
using Application.ViewModel.Product;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("product/[action]")]
    public class ProductController : Controller
    {
        private IProductService _productService;
        private IRoleService _roleService;

        public ProductController(IProductService productService, IRoleService roleService)
        {
            _productService = productService;
            _roleService = roleService;
        }

        [HttpGet(Name = "getproductbyid")]
        public IActionResult GetById(int id)
        {
            ProductViewModel a = _productService.GetById(id);
           return new OkObjectResult(a );
            
        }

        
        [HttpPost(Name = "createrole")]
        
        public async Task<IActionResult> CreateRole(string roleName, string description)
        {
           
            await _roleService.AddRoleAsync(roleName,description);
           
            return  Ok();

        }
    }
}
