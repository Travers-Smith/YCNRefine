using Microsoft.AspNetCore.Mvc;
using YCNRefine.Core.Services;
using YCNRefine.Models;

namespace YCNRefine.Controllers
{
    [ApiController]
    [Route("original-source")]
    public class OriginalSourceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IOriginalSourceService _originalSourceService;

        public OriginalSourceController(IConfiguration configuration, IOriginalSourceService originalSourceService)
        {
            _configuration = configuration;
            _originalSourceService = originalSourceService;
        }

        [HttpGet("get-by-dataset/{datasetId}")]
        public async Task<IActionResult> GetByDatasetId(int datasetId, int? pageNumber)
        {
            pageNumber ??= 1;

            int pageSize = int.Parse(_configuration["PageSize"] ?? "100");

            return Ok((await _originalSourceService
                .GetIdAndNamesByDatasetIdWithDataset(datasetId, (pageSize * pageNumber.Value) - pageSize, pageSize))
                .Select(originalSource => new OriginalSourceModel
                {
                    Id = originalSource.Item1,
                    Name = originalSource.Item2,
                }));
        }
    }
}
