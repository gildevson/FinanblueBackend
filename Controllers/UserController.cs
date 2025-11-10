using System;
using Dapper;
using FinanblueBackend.Data;    
using FinanblueBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinanblueBackend.Controllers {


        [ApiController]
        [Route("api/controller")]
        public class UserController : ControllerBase 
        {
               private readonly DbContextDapper _dapper;

              public UserController(DbContextDapper Dapper) {

              _dapper = Dapper;
        }



    }
}
