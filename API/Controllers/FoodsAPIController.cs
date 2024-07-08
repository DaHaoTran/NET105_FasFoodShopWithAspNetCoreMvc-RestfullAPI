using UI.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.ApiController
{
    [Route("api/foods")]
    [ApiController]
    public class FoodsAPIController : ControllerBase
    {
        private readonly IAddable<Food> _addsvc;
        private readonly IReadable<Food> _readsvc;
        private readonly ILookupSvc<string, Food> _lookupsvc;
        private readonly IEditable<Food> _editsvc;
        private readonly IDeletable<string, Food> _deletesvc;
        private readonly IReadable<FoodCategory> _foodcategorysvc;
        private readonly IReadable<FoodType> _foodtypesvc;
        public FoodsAPIController(IAddable<Food> addsvc,
            IReadable<Food> readsvc,
            ILookupSvc<string, Food> lookupsvc,
            IEditable<Food> editsvc,
            IDeletable<string, Food> deletesvc,
            IReadable<FoodCategory> foodcategorysvc,
            IReadable<FoodType> foodtypesvc)
        {
            _addsvc = addsvc;
            _readsvc = readsvc;
            _lookupsvc = lookupsvc;
            _editsvc = editsvc;
            _deletesvc = deletesvc;
            _foodcategorysvc = foodcategorysvc;
            _foodtypesvc = foodtypesvc;
        }

        // GET: api/foods
        [HttpGet]
        public async Task<IEnumerable<Food>> Getfoods()
        {
            return await _readsvc.ReadDatas();
        }

        // GET: api/foods
        [HttpGet("foodtype/name")]
        public async Task<IEnumerable<string>> GetTypes()
        {
            var types = await _foodtypesvc.ReadDatas();
            return types.Select(x => x.TypeName).ToList();
        }

        // GET: api/foods
        [HttpGet("foodcategory/name")]
        public async Task<IEnumerable<string>> GetCategories()
        {
            var fcates = await _foodcategorysvc.ReadDatas();
            return fcates.Select(x => x.CategoryName).ToList();
        }

        // GET: api/foods/5
        [HttpGet("{code}")]
        public async Task<ActionResult<Food>> Getfood(string code)
        {
            var food = await _lookupsvc.GetDataByKey(code);

            if (food == null)
            {
                return NotFound();
            }

            return food;
        }

        // GET: api/foods/string/email
        [HttpGet("string/{name}")]
        public async Task<ActionResult<Food>> GetFoodByString(string name)
        {
            var food = await _lookupsvc.GetDataByString(name);
            return food;
        }

        // PUT: api/foods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{code}")]
        public async Task<bool> PutFood(string code, Food food)
        {
            bool done = false;
            if (code != food.FoodCode)
            {
                return false;
            }
            else
            {
                done = await _editsvc.EditData(food);
            }
            return done;
        }

        // POST: api/foods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<bool> PostFood([FromBody]Food food)
        {
            bool done = await _addsvc.AddNewData(food);
            return done;
        }

        // DELETE: api/foods/5
        [HttpDelete("{code}")]
        public async Task<bool> DeleteFood(string code)
        {
            bool done = await _deletesvc.DeleteData(code);
            return done;
        }
    }
}
