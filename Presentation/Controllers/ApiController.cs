﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[Action]")]
    [ProducesResponseType(typeof(ErrorDetails) ,StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDetails) ,StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ValidationErrorResponse) ,StatusCodes.Status400BadRequest)]

    public class ApiController :ControllerBase
    {

    }
}
