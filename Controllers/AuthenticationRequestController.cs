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
using static FBClassesObjectService.AuthenticationRequestObjectService;

namespace FBClassesODataService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationRequestController : ODataController
    {
        [HttpGet]
        [EnableQuery]
        public IEnumerable<AuthenticationRequest> Get()
        {

            return new Context().AuthenticationRequests;
        }
        
        [HttpDelete]
        [EnableQuery]
        public IActionResult Delete([FromODataUri] int key)
        {
            return Ok(new AuthenticationRequestObjectService().DeleteById(new DeleteByIdRequest() { Id = key }));
        }

        [HttpPatch]
        [EnableQuery]
        public IActionResult Patch([FromODataUri] int key, [FromBody] Delta<AuthenticationRequest> request)
        {
            UpdateByIdResponse response =
                new AuthenticationRequestObjectService().UpdateById(new UpdateByIdRequest()
                {
                    Id = key,
                    AuthenticationRequest = request.GetInstance()
                }); 

            return Ok(response);
        }
        
        [HttpPost]
        [EnableQuery]
        public InsertResponse Post([FromBody] AuthenticationRequest newObject)
        {
            return new AuthenticationRequestObjectService().Insert(new InsertRequest() { AuthenticationRequest = newObject });
        }    
    } // AuthenticationRequestODataController
} // AuthenticationRequestController.Controllers