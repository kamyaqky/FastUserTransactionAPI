using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastUserTransactionAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastUserTransactionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly DataService _service;

        public TransactionsController(DataService service)
        {
            _service = service;
        }

        [HttpGet("user/{username}")]
        public async Task<IActionResult> GetUserTransactions(string username)
        {
            var result = await _service.GetUserTransactions(username);
            return Ok(result);
        }

        [HttpGet("group/{username}")]
        public async Task<IActionResult> GetGroupTransactions(string username)
        {
            var result = await _service.GetGroupTransactionsIfAdmin(username);
            return Ok(result);
        }
    }
}