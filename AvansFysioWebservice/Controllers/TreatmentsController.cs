using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvansFysioWebservice.Authorization;
using AvansFysioWebservice.Model;
using AvansFysioWebservice.Model.Errors;
using Core.Domain.Webservice;
using Core.DomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AvansFysioWebservice.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class TreatmentsController : ControllerBase
    {
        private readonly ITreatmentInfoService _treatmentInfoService;

        public TreatmentsController(ITreatmentInfoService treatmentInfoService)
        {
            _treatmentInfoService = treatmentInfoService;
        }

        // GET: api/<TreatmentsController>
        [HttpGet]
        public List<TreatmentInfo> Get()
        {
            var treatmentInfo = _treatmentInfoService.GetAllTreatmentInfo();
            return treatmentInfo;
        }

        // GET api/<TreatmentsController>/5
        [HttpGet("{id}")]
        public ActionResult<TreatmentInfo> Get(int id)
        {
            var treatmentInfo = _treatmentInfoService.GetTreatmentInfo(id);

            if (treatmentInfo == null)
            {
                return NotFound();
            }

            return treatmentInfo;
        }

        // POST api/<TreatmentsController>
        [HttpPost]
        public ActionResult Post([FromBody] string value)
        {
            Response.Headers.Add("Allow", "GET, HEAD");
            return StatusCode(StatusCodes.Status405MethodNotAllowed, new ErrorMessage405());
        }

        // PUT api/<TreatmentsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            Response.Headers.Add("Allow", "GET, HEAD");
            return StatusCode(StatusCodes.Status405MethodNotAllowed, new ErrorMessage405());
        }

        // DELETE api/<TreatmentsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Response.Headers.Add("Allow", "GET, HEAD");
            return StatusCode(StatusCodes.Status405MethodNotAllowed, new ErrorMessage405());
        }
    }
}
