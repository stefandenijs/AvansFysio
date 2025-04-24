using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvansFysioWebservice.Authorization;
using AvansFysioWebservice.Model;
using AvansFysioWebservice.Model.Errors;
using AvansFysioWebservice.Model.Responses;
using Core.DomainServices;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AvansFysioWebservice.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class BodyLocalizationsController : ControllerBase
    {
        private readonly IDiagnosisService _diagnosisService;

        public BodyLocalizationsController(IDiagnosisService diagnosisService)
        {
            _diagnosisService = diagnosisService;
        }

        // GET: api/<BodyLocalizationsController>
        [HttpGet]
        public List<string> Get()
        {
            var bodyLocalizations = _diagnosisService.GetBodyLocalizations();
            return bodyLocalizations;
        }

        // GET api/<BodyLocalizationsController>/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var bodyLocalization = _diagnosisService.GetBodyLocalization(id);

            if (string.IsNullOrEmpty(bodyLocalization))
            {
                return NotFound();
            }

            return bodyLocalization;
        }

        // POST api/BodyLocalizations
        [HttpPost]
        public ActionResult Post([FromBody] string value)
        {
            Response.Headers.Add("Allow", "GET, HEAD");
            return StatusCode(StatusCodes.Status405MethodNotAllowed, new ErrorMessage405());
        }

        // PUT api/BodyLocalizations/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            Response.Headers.Add("Allow", "GET, HEAD");
            return StatusCode(StatusCodes.Status405MethodNotAllowed, new ErrorMessage405());
        }

        // DELETE api/BodyLocalizations/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Response.Headers.Add("Allow", "GET, HEAD");
            return StatusCode(StatusCodes.Status405MethodNotAllowed, new ErrorMessage405());
        }
    }
}
