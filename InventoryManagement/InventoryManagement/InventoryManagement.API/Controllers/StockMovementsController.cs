using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockMovementsController : ControllerBase
{
    private readonly IStockMovementService _stockMovementService;

    public StockMovementsController(IStockMovementService stockMovementService)
        => _stockMovementService = stockMovementService;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _stockMovementService.GetAllAsync(ct));

    [HttpGet("product/{productId:guid}")]
    public async Task<IActionResult> GetByProduct(Guid productId, CancellationToken ct)
        => Ok(await _stockMovementService.GetByProductAsync(productId, ct));

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(CancellationToken ct)
        => Ok(await _stockMovementService.GetSummaryAsync(ct));

    [HttpPost]
    public async Task<IActionResult> RegisterMovement([FromBody] CreateStockMovementDto dto, CancellationToken ct)
    {
        var result = await _stockMovementService.RegisterMovementAsync(dto, ct);
        return StatusCode(201, result);
    }
}
