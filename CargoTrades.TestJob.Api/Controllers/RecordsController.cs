using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CargoTrades.TestJob.Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft;
using Newtonsoft.Json;

namespace CargoTrades.TestJob.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public RecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("list")]
        public ActionResult<List<Record>> GetRecords()
        {
            return Ok(_context.Records.ToList());
        }

        [HttpPost]
        [Route("save")]
        public ActionResult<Record> AddRecord(Record record)
        {
            if (record != null)
            {
                try
                {
                    _context.Records.Add(record);
                    _context.SaveChanges();
                    return Ok(record);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            else return BadRequest();
        }
    }
}