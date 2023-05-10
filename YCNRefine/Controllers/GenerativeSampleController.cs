using Microsoft.AspNetCore.Mvc;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;
using YCNRefine.Models;

namespace YCNRefine.Controllers;

[ApiController]
[Route("generative-sample")]
public class GenerativeSampleController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IGenerativeSampleService _generativeSampleService;
    private readonly IIdentityService _identityService;

    public GenerativeSampleController(IConfiguration configuration, IGenerativeSampleService generativeSampleService, IIdentityService identityService)
    {
        _configuration = configuration;
        _generativeSampleService = generativeSampleService;
        _identityService = identityService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(AddGeneratveSampleModel addGenerativeSample)
    {
        Guid? userIdentifier = _identityService.GetUserIdentifier();

        if(userIdentifier is null)
        {
            return Unauthorized();
        }

        GenerativeSample genarativeSample = new ()
        {
            Name = addGenerativeSample.Input[..Math.Min(addGenerativeSample.Input.Length, 30)],
            DatasetId = addGenerativeSample.DatasetId,
            IsDeleted = false,
            Context = addGenerativeSample.Context,
            Input = addGenerativeSample.Input,
            Output = addGenerativeSample.Output,
            UserIdentifier = userIdentifier.Value
        };

        await _generativeSampleService.Add(genarativeSample);

        return Ok(new GenerativeSampleModel
        {
            Id = genarativeSample.Id,
            Name = genarativeSample.Name
        });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        GenerativeSample generativeSample = await _generativeSampleService.GetByIdWithDataset(id);

        generativeSample.IsDeleted = true;

        await _generativeSampleService.Update(generativeSample);

        return StatusCode(204);
    }

    [HttpGet("get-by-dataset/{datasetId}")]
    public async Task<IActionResult> GetByDataset(int datasetId, int? pageNumber)
    {
        pageNumber ??= 1;

        int pageSize = int.Parse(_configuration["PageSize"] ?? "100");

        return Ok((await _generativeSampleService.GetByDataset(datasetId, (pageSize * pageNumber.Value) - pageSize, pageSize))
            .Select(sample => new GenerativeSampleModel
            {
                Id = sample.Id,
                Name = sample.Name,
                Context = sample.Context,
                DatasetId = sample.DatasetId,
                Input = sample.Input,
                Output = sample.Output,
            }));
    }

    [HttpGet("get-sample-by-id/{sampleId}")]
    public async Task<IActionResult> GetSampleById(int sampleId)
    {
        GenerativeSample generativeSample = await _generativeSampleService.GetByIdWithDataset(sampleId);

        return Ok(new GenerativeSampleModel
        {
            Id = generativeSample.Id,
            Name = generativeSample.Name,
            Context = generativeSample.Context,
            DatasetId = generativeSample.DatasetId,
            Dataset = new DatasetModel
            {
                Id = generativeSample.DatasetId,
                Name = generativeSample.Dataset.Name,
            },
            Input = generativeSample.Input,
            Output = generativeSample.Output,
        });        
    }

    [HttpPatch("update")]
    public async Task<IActionResult> Update(GenerativeSampleModel generativeSampleModel)
    {
        await _generativeSampleService.Update(new GenerativeSample
        {
            Id = generativeSampleModel.Id,
            DatasetId = generativeSampleModel.DatasetId,
            IsDeleted = false,
            Context = generativeSampleModel.Context,
            Input = generativeSampleModel.Input,
            Output = generativeSampleModel.Output,
            Name = generativeSampleModel.Name
        });

        return StatusCode(204);
    }
}
