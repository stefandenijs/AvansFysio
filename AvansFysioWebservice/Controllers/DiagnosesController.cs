using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvansFysioWebservice.Authorization;
using AvansFysioWebservice.Model;
using AvansFysioWebservice.Model.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Domain.Webservice;
using Core.DomainServices;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;

namespace AvansFysioWebservice.Controllers
{
    [TypeFilter(typeof(AuthorizationFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosesController : ControllerBase
    {
        private readonly IDiagnosisService _diagnoseService;

        public DiagnosesController(IDiagnosisService diagnoseService)
        {
            _diagnoseService = diagnoseService;
        }

        // GET: api/Diagnoses
        [HttpGet]
        public ActionResult<List<Diagnosis>> GetDiagnoses()
        {
            var diagnoses = _diagnoseService.GetDiagnoses();
            return diagnoses.ToList();
        }

        // GET: api/Diagnoses/5
        [HttpGet("{id}")]
        public ActionResult<Diagnosis> GetDiagnosis(int id)
        {
            var diagnose = _diagnoseService.GetDiagnosis(id);

            if (diagnose == null)
            {
                return NotFound();
            }

            return diagnose;
        }


        // GET: api/Diagnoses/Category/{string}
        [HttpGet("Category/{*name}")]
        public ActionResult<List<Diagnosis>> GetDiagnoses(string name)
        {
            var diagnoses = _diagnoseService.GetDiagnosesByBodyLocalization(name);

            if (diagnoses.Count == 0)
            {
                return NotFound();
            }

            return diagnoses;
        }

        // POST api/Diagnoses
        [HttpPost]
        public ActionResult Post([FromBody] string value)
        {
            Response.Headers.Add("Allow", "GET, HEAD");
            return StatusCode(StatusCodes.Status405MethodNotAllowed, new ErrorMessage405());
        }

        // PUT api/Diagnoses/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            Response.Headers.Add("Allow", "GET, HEAD");
            return StatusCode(StatusCodes.Status405MethodNotAllowed, new ErrorMessage405());
        }

        // DELETE api/Diagnoses/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Response.Headers.Add("Allow", "GET, HEAD");
            return StatusCode(StatusCodes.Status405MethodNotAllowed, new ErrorMessage405());
        }
    }
}
