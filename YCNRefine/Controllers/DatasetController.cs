using Microsoft.AspNetCore.Mvc;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;
using YCNRefine.Models;

namespace YCNRefine.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatasetController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDatasetService _datasetService;
        private readonly IIdentityService _identityService;

        public DatasetController(IConfiguration configuration, IDatasetService datasetService, IIdentityService identityService) 
        { 
            _configuration = configuration;
            _datasetService = datasetService;
            _identityService = identityService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddDataset(AddDatasetModel addDataset)
        {
            Guid? userIdentifier = _identityService.GetUserIdentifier();

            if (userIdentifier is null)
            {
                return Unauthorized();
            }

            Dataset dataset = new()
            {
                CreatedByUser = userIdentifier.Value,
                Name = addDataset.Name,
                IsDeleted = false
            };

            await _datasetService.Add(dataset);

            return Ok(new DatasetModel
            {
                Id = dataset.Id,
                Name = dataset.Name
            });
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int datasetId)
        {
            Dataset dataset = await _datasetService.GetById(datasetId);

            dataset.IsDeleted = true;

            await _datasetService.Update(dataset);

            return StatusCode(204);
        }

        [HttpGet("get-datasets")]
        public async Task<IActionResult> GetDatasets(int? pageNumber)
        {
            pageNumber ??= 1;

            int pageSize = int.Parse(_configuration["PageSize"] ?? "100");

            return Ok((await _datasetService.GetDatasets((pageNumber.Value * pageSize) - pageSize, pageSize))
                .Select(dataset => new DatasetModel
                {
                    Id = dataset.Id,
                    Name = dataset.Name,
                }));
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateDataset(DatasetModel dataset)
        {
            Dataset datasetRecord = await _datasetService.GetById(dataset.Id);
            
            datasetRecord.Name = dataset.Name;
            
            await _datasetService.Update(datasetRecord);

            return StatusCode(204);
        }
    }
}
