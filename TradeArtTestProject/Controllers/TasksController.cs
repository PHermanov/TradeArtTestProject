using Microsoft.AspNetCore.Mvc;
using TradeArtTestProject.Resources;
using TradeArtTestProject.Services.Interfaces;

namespace TradeArtTestProject.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    private readonly IShaService _shaService;
    private readonly IPricesService _pricesService;
    private readonly IProcessDataService _processDataService;
    
    public TasksController(IShaService shaService, IPricesService pricesService, IProcessDataService processDataService)
    {
        _shaService = shaService;
        _pricesService = pricesService;
        _processDataService = processDataService;
    }


    /// <summary>
    /// Task #1: Invert the text
    /// </summary>
    [HttpGet("GetInvertedLoremIpsum")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<string> GetInvertedLoremIpsum()
    {
        // Since the text is not supposed to be changed, we can just have an inverted version in Resources
        return Ok(StringResources.InvertedLoremIpsum);
    }


    /// <summary>
    /// Task #2: Publish and process messages
    /// </summary>
    [HttpGet("ProcessData")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string>> ProcessData()
    {
        var result = await _processDataService.StartProcess();
        return result.Success
            ? Ok(result.Data)
            : Problem(result.ErrorMessage, statusCode: StatusCodes.Status500InternalServerError);
    }


    /// <summary>
    /// Task #3: Calculate a SHA hash
    /// </summary>
    [HttpGet("CalculateSha")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> CalculateSha([FromQuery] string filePath = @"Files/100MB.bin")
    {
        var result = await _shaService.CalculateShaAsync(filePath);

        return result.Success
            ? Ok(result.Data)
            : Problem(result.ErrorMessage, statusCode: StatusCodes.Status400BadRequest);
    }
    

    /// <summary>
    /// Task #4: Query Prices
    /// </summary>
    [HttpGet("QueryPrices")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string>> QueryPrices()
    {
        var result = await _pricesService.GetPricesAsync();

        return result.Success
            ? Ok(result.Data)
            : Problem(result.ErrorMessage, statusCode: StatusCodes.Status500InternalServerError);
    }

}
