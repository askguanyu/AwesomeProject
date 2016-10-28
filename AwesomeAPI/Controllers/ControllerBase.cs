using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AwesomeAPI.Controllers.Attributes;
using AwesomeAPI.Filters;
using AwesomeAPI.Models;
using AwesomeAPI.Repositories.Interfaces;
using AwesomeAPI.Services.Interfaces;
using AwesomeAPI.ViewModels;
using AwesomeLib;
using AwesomeLib.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace AwesomeAPI.Controllers
{
    [ApiController]
    [TypeFilter(typeof(ProductionRequireHttpsAttribute))]
    public abstract class ControllerBase<TModel, TViewModel, TController> : Controller
        where TModel : ModelBase
        where TViewModel : ViewModelBase
        where TController : ControllerBase
    {
        abstract protected int LogEventId { get; }

        readonly IRepository<TModel> _repository;
        readonly ILogger<TController> _logger;
        readonly IConverter _converter;
        readonly IStringLocalizer<SharedResources> _localizer;
        readonly IResponseProvider _response;

        public ControllerBase(
            IRepository<TModel> repository,
            ILogger<TController> logger,
            IConverter converter,
            IStringLocalizer<SharedResources> localizer,
            IResponseProvider response
            )
        {
            _repository = repository;
            _logger = logger;
            _converter = converter;
            _localizer = localizer;
            _response = response;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _repository.GetAll();
            _logger.LogTrace(LogEventId, AwesomeMethods.ConvertDataForLog(items, _converter));

            return Ok(_response
                .AddData(Mapper.Map<IEnumerable<TViewModel>>(items))
                .Get());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var model = await _repository.GetById(id);
            if (model == null)
            {
                _logger.LogWarning(LogEventId, $"NOT FOUND : ID ({id})");
                return NotFound(_response
                    .AddWarning(_localizer["IdNotFound"])
                    .Get());
            }

            _logger.LogTrace(LogEventId, AwesomeMethods.ConvertDataForLog(model, _converter));

            return Ok(_response
                .AddData(Mapper.Map<TViewModel>(model))
                .Get());
        }

        [HttpPost]
        [ValidateViewModel]
        public async Task<IActionResult> Post([FromBody]TViewModel viewModel)
        {
            _logger.LogTrace(LogEventId, AwesomeMethods.ConvertDataForLog(viewModel, _converter));

            var model = Mapper.Map<TModel>(viewModel);

            _repository.Add(model);

            if (await _repository.Save())
            {
                _logger.LogInformation(LogEventId, $"SAVED : ID ({model.Id})");
                return CreatedAtAction("Get", new { id = model.Id },
                    _response
                        .AddInfo(_localizer["Saved"])
                        .AddData(Mapper.Map<TViewModel>(model))
                        .Get());
            }
            else
            {
                _logger.LogWarning(LogEventId, "NOT SAVED");
                return NoContent();
            }
        }

        [HttpPut("{id}")]
        [ValidateViewModel]
        public async Task<IActionResult> Put(int id, [FromBody] TViewModel viewModel)
        {
            _logger.LogTrace(LogEventId, AwesomeMethods.ConvertDataForLog(viewModel, _converter));

            if (viewModel.Id != id)
            {
                _logger.LogError(LogEventId, $"INVALID ID : ({id})");
                return BadRequest(_response
                    .AddError(_localizer["InvalidId"])
                    .Get());
            }

            if (!await _repository.Exists(id))
            {
                _logger.LogWarning(LogEventId, $"NOT FOUND : ID ({id})");
                return NotFound(_response
                    .AddWarning(_localizer["IdNotFound"])
                    .Get());
            }

            var model = Mapper.Map<TModel>(viewModel);

            _repository.Update(model);

            return await ApplyChanges(model);
        }

        [HttpPatch("{id}")]
        [ValidateViewModel]
        public async Task<IActionResult> Patch(int id, [FromBody] TViewModel viewModel)
        {
            _logger.LogTrace(LogEventId, AwesomeMethods.ConvertDataForLog(viewModel, _converter));

            if (viewModel.Id != id)
            {
                _logger.LogError(LogEventId, $"INVALID ID : ({id})");
                return BadRequest(_response
                    .AddError(_localizer["InvalidId"])
                    .Get());
            }

            var model = await _repository.GetById(viewModel.Id);
            if (model == null)
            {
                _logger.LogWarning(LogEventId, $"NOT FOUND : ID ({id})");
                return NotFound(_response
                    .AddWarning(_localizer["IdNotFound"])
                    .Get());
            }

            Mapper.Map<TViewModel, TModel>(viewModel, model);

            return await ApplyChanges(model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _repository.GetById(id);
            if (model == null)
            {
                _logger.LogWarning(LogEventId, $"NOT FOUND : ID ({id})");
                return NotFound(_response
                    .AddWarning(_localizer["IdNotFound"])
                    .Get());
            }

            _repository.Remove(model);

            return await ApplyChanges(model);
        }

        async Task<IActionResult> ApplyChanges(TModel model)
        {
            if (await _repository.Save())
            {
                _logger.LogInformation(LogEventId, $"SAVED : ID ({model.Id})");
                return Ok(_response
                    .AddInfo(_localizer["Saved"])
                    .AddData(Mapper.Map<TViewModel>(model))
                    .Get());
            }
            else
            {
                _logger.LogWarning(LogEventId, $"NOT SAVED : ID ({model.Id})");
                return NoContent();
            }
        }
    }
}