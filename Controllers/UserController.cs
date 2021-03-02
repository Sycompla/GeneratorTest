using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CSUsers;
using FBClassesCap;
using FBClassesObjectService;
using static FBClassesObjectService.UserObjectService;

namespace FBClassesODataService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ODataController
    {
        [HttpGet]
        [EnableQuery]
        public IEnumerable<User> Get()
        {

            return new Context().Users;
        }
        
        [HttpDelete]
        [EnableQuery]
        public IActionResult Delete([FromODataUri] int key)
        {
            return Ok(new UserObjectService().DeleteById(new DeleteByIdRequest() { Id = key }));
        }

        [HttpPatch]
        [EnableQuery]
        public IActionResult Patch([FromODataUri] int key, [FromBody] Delta<User> request)
        {
            UpdateByIdResponse response =
                new UserObjectService().UpdateById(new UpdateByIdRequest()
                {
                    Id = key,
                    User = request.GetInstance()
                }); 

            return Ok(response);
        }
        
        [HttpPost]
        [EnableQuery]
        public InsertResponse Post([FromBody] User newObject)
        {
            return new UserObjectService().Insert(new InsertRequest() { User = newObject });
        }    
    } // UserODataController
} // UserController.Controllers