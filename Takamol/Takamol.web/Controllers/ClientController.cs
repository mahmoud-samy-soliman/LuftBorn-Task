using EnvDTE;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using Takamol.Application.Clients.Commands.Delete;
using Takamol.Application.Clients.CreateClientCommand;
using Takamol.Application.Clients.Query;
using Takamol.Application.Clients.UpdateClientCommand;
using Takamol.web.Models;

namespace Takamol.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ClientController> _logger;

        public ClientController(IMediator mediator, ILogger<ClientController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientCommand createClientCommand)
        {
            try
            {
                CreateClientCommandValidator validator = new CreateClientCommandValidator();
                var validationResult = validator.Validate(createClientCommand);

                if (validationResult.IsValid)
                {
                    //createClientCommand.CreatedById = SiteSettings.CurrentUserId;
                    var clientId = await _mediator.Send(createClientCommand);

                    if (clientId != null)
                    {
                        return Ok(new { ClientId = clientId });
                    }
                    else
                    {
                        return BadRequest(new { Message = "Failed to create client" });
                    }
                }
                else
                {
                    foreach (var failure in validationResult.Errors)
                    {
                        ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                    }

                    return BadRequest(new { Message = "Validation failed", Errors = validationResult.Errors });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);

                return BadRequest(new { Message = "Exception occurred", Errors = new List<string> { ex.Message } });
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetClients()
        {
            try
            {
                var clients = await _mediator.Send(new GetAllClientsQuery());

                return Ok(clients);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);

                return BadRequest(new { Message = "Exception occurred", Errors = new List<string> { ex.Message } });
            }
        }

        [HttpDelete("delete/{clientId}")]
        public async Task<IActionResult> DeleteClient(string clientId)
        {
            try
            {
                var deleted = await _mediator.Send(new DeleteClientCommand { ClientId = clientId });

                if (deleted!=null)
                {
                    return Ok(new { Message = "Client deleted successfully" });
                }
                else
                {
                    return BadRequest(new { Message = "Failed to delete client" });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);

                return BadRequest(new { Message = "Exception occurred", Errors = new List<string> { ex.Message } });
            }
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateClient(string id , [FromBody] UpdateClientCommand updateClientCommand)
        {
            try
            {
                UpdateClientCommandValidator validator = new UpdateClientCommandValidator();
                var validationResult = validator.Validate(updateClientCommand);
                updateClientCommand.Id = id;
                if (validationResult.IsValid)
                {
                    //updateClientCommand.ModifiedById = SiteSettings.CurrentUserId;


                    var updatedClientId = await _mediator.Send(updateClientCommand);

                    if (updatedClientId != null)
                    {
                        _logger.LogInformation($"Client updated successfully. Updated ID: {updatedClientId}");
                        return Ok(new { Message = "Client updated successfully" });
                    }
                    else
                    {
                        _logger.LogError("Failed to update client. The mediator returned a null result.");
                        return BadRequest(new { Message = "Failed to update client" });
                    }
                }
                else
                {
                    foreach (var failure in validationResult.Errors)
                    {
                        ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                    }

                    _logger.LogError("Validation failed during client update.");
                    return BadRequest(new { Message = "Validation failed", Errors = validationResult.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred during client update: {ex.Message}");
                ModelState.AddModelError("Exception", ex.Message);

                return BadRequest(new { Message = "Exception occurred", Errors = new List<string> { ex.Message } });
            }
        }


        public SiteSettings SiteSettings
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    var claims = HttpContext.User.Claims.ToList();
                    var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                    SiteSettings siteSettings = new SiteSettings
                    {
                        CurrentUserId = userIdClaim?.Value
                    };
                    return siteSettings;

                }
                return null;
            }
        }
    }
}
