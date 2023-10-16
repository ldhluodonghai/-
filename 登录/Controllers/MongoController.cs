using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model.Result;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using 登录.Common;

namespace 登录.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MongoController : ControllerBase
    {
        
        private readonly MongoDBOption _mongoDBOption;
        private readonly MongoDBBase _context = null;

        public MongoController(IOptions<MongoDBOption> mongoDBOption)
        {
            _mongoDBOption = mongoDBOption.Value;
            _context = new MongoDBBase(mongoDBOption.Value.Conn, mongoDBOption.Value.Database);
        }
        [HttpGet]
        
        public ResultApi GetMyOption()
        {
            return  ResultHelper.Success(_mongoDBOption.Conn);
        }
        [HttpGet]
        public IActionResult AddList()
        {
            List<MongoDBPostTest> list = new List<MongoDBPostTest>()
            {
                new MongoDBPostTest()
                {
                    Id = "2",
                    Body = "Test note 3",
                    UpdatedOn = DateTime.Now,
                    UserId = 1,
                    HeaderImage = new NoteImage
                    {
                     ImageSize = 10,
                     Url = "http://localhost/image1.png",
                     ThumbnailUrl = "http://localhost/image1_small.png"
                    }
                 },
                new MongoDBPostTest()
                {
                    Id = "3",
                     Body = "Test note 4",
                    UpdatedOn = DateTime.Now,
                    UserId = 1,
                    HeaderImage = new NoteImage
                   {
                    ImageSize = 14,
                     Url = "http://localhost/image3.png",
                     ThumbnailUrl = "http://localhost/image3_small.png"
                    }
                }
            };

            try
            {
                _context.InsertMany(list);
            }
            catch (Exception ex)
            {

                throw;
            }

            return Ok("成功");
        }
        [HttpGet]
        public ResultApi SelectSingle()
        {
            //无条件
            var list = _context.GetList<MongoDBPostTest>();

            //有条件
            //var list = _context.GetList<MongoDBPostTest>(a => a.Id == "1");

            //得到单条数据,无条件
            //var list = _context.GetSingle<MongoDBPostTest>();

            //得到单条数据,有条件
            //var list = _context.GetSingle<MongoDBPostTest>(a => a.Id == "3");

           /* ObjectId internalId = _context.GetInternalId("5bbf41651d3b66668cbb5bfc");

            var a = _context.GetSingle<MongoDBPostTest>(note => note.Id == "5bbf41651d3b66668cbb5bfc" || note.InternalId == internalId);
*/
            return ResultHelper.Success(list);
            
        }
    }

}

