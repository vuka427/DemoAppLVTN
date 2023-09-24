using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model.Branch;
using WebApi.Model.JQDataTable;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class BranchController : Controller
    {

        [HttpPost]
        [Route("branchesfordatatable")]
        public IActionResult GetBrachesForDataTable( [FromBody] DatatableParam param)
        {
            int filteredResultsCount =1;
            int totalResultsCount=1;
            

            
           /* totalResultsCount = allEntities.Count();
            filteredResultsCount = allEntities.Count();

            var result = new List<YourCustomSearchClass>();

            foreach (var en in allEntities)
            {
                result.Add(new YourCustomSearchClass
                {
                    Name = en.Name
                });
            }*/

            return Json(new
            {
                // this is what datatables wants sending back
                draw = param.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = new List<BranchModel>()
            });
        }



        [HttpPost]
        [Route("add")]
        public IActionResult CreateBrach(BranchModel branch)
        {



            return Ok(); 
        }



    }
}
