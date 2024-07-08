using UI.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.ApiController
{
    [Route("api/categories")]
    [ApiController]
    public class FoodCategoriesAPIController : ControllerBase
    {
        private readonly IAddable<FoodCategory> _addsvc;
        private readonly IReadable<FoodCategory> _readsvc;
        private readonly ILookupSvc<string, FoodCategory> _lookupsvc;
        private readonly IEditable<FoodCategory> _editsvc;
        private readonly IDeletable<string, FoodCategory> _deletesvc;
        public FoodCategoriesAPIController(IAddable<FoodCategory> addsvc,
            IReadable<FoodCategory> readsvc,
            ILookupSvc<string, FoodCategory> lookupsvc,
            IEditable<FoodCategory> editsvc,
            IDeletable<string, FoodCategory> deletesvc)
        {
            _addsvc = addsvc;
            _readsvc = readsvc;
            _lookupsvc = lookupsvc;
            _editsvc = editsvc;
            _deletesvc = deletesvc;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<IEnumerable<FoodCategory>> Getfoodcategories()
        {
            return await _readsvc.ReadDatas();
        }

        // GET: api/categories/5
        [HttpGet("{code}")]
        public async Task<ActionResult<FoodCategory>> Getfoodcategory(string code)
        {
            var fcate = await _lookupsvc.GetDataByKey(code);

            if (fcate == null)
            {
                return NotFound();
            }

            return fcate;
        }

        // GET: api/categories/string/name
        [HttpGet("string/{name}")]
        public async Task<ActionResult<FoodCategory>> GetfoodcategoryByString(string name)
        {
            var fcate = await _lookupsvc.GetDataByString(name);
            return fcate;
        }

        // PUT: api/categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{code}")]
        public async Task<bool> PutFoodcategory(string code, FoodCategory fcate)
        {
            bool done = await _editsvc.EditData(fcate);
            return done;
        }

        // POST: api/categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<bool> PostFoodcategory(FoodCategory fcate)
        {
            bool done = await _addsvc.AddNewData(fcate);
            return done;
        }

        // DELETE: api/categories/5
        [HttpDelete("{code}")]
        public async Task<bool> DeleteFoodcategory(string code)
        {
            bool done = await _deletesvc.DeleteData(code);
            return done;
        }
    }
}
