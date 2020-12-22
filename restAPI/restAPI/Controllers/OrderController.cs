using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderApi;
using restAPI.Model;

namespace restAPI.Controllers
{
    [Route("api/")]
    public class OrderController : ControllerBase
    {
        public OrderController(AppDb db)
        {
            Db = db;
        }

        [HttpGet("/order")]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new OrderItemQuery(Db);
            var result = await query.LatestOrdersAsync();
            return new OkObjectResult(result);
        }

        //[HttpGet("/order/{id}")]
        //public async Task<IActionResult> GetOne(int id)
        //{
        //    await Db.Connection.OpenAsync();
        //    var query = new OrderItemQuery(Db);
        //    var result = await query.FindOneAsync(id);
        //    if (result is null)
        //        return new NotFoundResult();
        //    return new OkObjectResult(result);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody]OrderItem body)
        //{
        //    await Db.Connection.OpenAsync();
        //    body.Db = Db;
        //    await body.InsertAsync();
        //    return new OkObjectResult(body);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutOne(int id, [FromBody]OrderItem body)
        //{
        //    await Db.Connection.OpenAsync();
        //    var query = new OrderItemQuery(Db);
        //    var result = await query.FindOneAsync(id);
        //    if (result is null)
        //        return new NotFoundResult();
        //    result.url = body.url;
        //    await result.UpdateAsync();
        //    return new OkObjectResult(result);
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOne(int id)
        //{
        //    await Db.Connection.OpenAsync();
        //    var query = new OrderItemQuery(Db);
        //    var result = await query.FindOneAsync(id);
        //    if (result is null)
        //        return new NotFoundResult();
        //    await result.DeleteAsync();
        //    return new OkResult();
        //}

        //[HttpDelete]
        //public async Task<IActionResult> DeleteAll()
        //{
        //    await Db.Connection.OpenAsync();
        //    var query = new OrderItemQuery(Db);
        //    await query.DeleteAllAsync();
        //    return new OkResult();
        //}

        public AppDb Db { get; }
    }
}