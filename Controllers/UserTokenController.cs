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
using static FBClassesObjectService.UserTokenObjectService;

namespace FBClassesODataService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserTokenController : ODataController
    {
        [HttpGet]
        [EnableQuery]
        public IEnumerable<UserToken> Get()
        {

            return new Context().UserTokens;
        }
        
        [HttpDelete]
        [EnableQuery]
        public IActionResult Delete([FromODataUri] int key)
        {
            return Ok(new UserTokenObjectService().DeleteById(new DeleteByIdRequest() { Id = key }));
        }

        [HttpPatch]
        [EnableQuery]
        public IActionResult Patch([FromODataUri] int key, [FromBody] Delta<UserToken> request)
        {
            UpdateByIdResponse response =
                new UserTokenObjectService().UpdateById(new UpdateByIdRequest()
                {
                    Id = key,
                    UserToken = request.GetInstance()
                }); 

            return Ok(response);
        }
        
        [HttpPost]
        [EnableQuery]
        public InsertResponse Post([FromBody] UserToken newObject)
        {
            return new UserTokenObjectService().Insert(new InsertRequest() { UserToken = newObject });
        }    
    } // UserTokenODataController
} // UserTokenController.Controllers