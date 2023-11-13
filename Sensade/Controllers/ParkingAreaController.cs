using Microsoft.AspNetCore.Mvc;
using MODEL;
using DAL;
using Npgsql;
using Interface;
using System.Reflection;
using System.Linq.Expressions;
using DAL.Exceptions;

namespace Sensade.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParkingAreasController : ControllerBase
{
    private IParkingAreaDao<ParkingArea> _parkingAreaDAO;

    public ParkingAreasController(IParkingAreaDao<ParkingArea> parkingAreDAO)
    {
        _parkingAreaDAO = parkingAreDAO;
    }

    [HttpPost]
    public async Task<IActionResult> AddParkingArea(ParkingArea parkingArea)
    {
        try
        {
            int id = await _parkingAreaDAO.AddAsync(parkingArea);
            return Ok(id);
        }
        catch (DuplicateItemException ex){return Conflict(ex.Message);}
        catch(NpgsqlException ex){return StatusCode(500, ex.Message);}
    }

    [HttpGet]
    public async Task<ActionResult> GetParkingAreaById(int id) 
    {
        try
        {
            var parkingArea = await _parkingAreaDAO.GetAsync(id);
            return Ok(parkingArea);
        }

        catch (InvalidOperationException ex){return NotFound(ex.Message);}
        catch(NpgsqlException e){return StatusCode(500, e.Message);}
    }


    [HttpDelete]
    public async Task<ActionResult> DeleteParkingAreaById(int id) 
    {
        try
        {
            var parkingArea = await _parkingAreaDAO.GetAsync(id);

            if (await _parkingAreaDAO.DeleteAsync(id))
            {
                return NoContent();

            } else{

                return BadRequest("Parking area was not deleted");
            }
        }
        catch (InvalidOperationException ex){return NotFound(ex.Message);}
        catch (NpgsqlException ex) {return StatusCode(500, ex.Message);}
    }

    [HttpPut]
    public async Task<ActionResult> UpdateParkingArea(ParkingArea newparkingArea) 
    {
        try
        {
            var parkingArea = await _parkingAreaDAO.GetAsync((int)newparkingArea.Id);

            if (await _parkingAreaDAO.UpdateAsync(newparkingArea))
            {
                return Ok();
            }

            else { return BadRequest("Parking area was not deleted"); }
        }
        catch (InvalidOperationException ex) { return NotFound(ex.Message); }
        catch(DuplicateItemException ex) { return Conflict(ex.Message); }
        catch (NpgsqlException ex){ return StatusCode(500, ex.Message); }
    }

    [HttpGet]
    [Route("FreeAndTotalSpaces")]
    public async Task<ActionResult> GetFreeAndTotalAmountOfParkingSpaceInParkingArea(int id) 
    {
        try
        {
            ParkingArea parkingArea = await _parkingAreaDAO.GetTotalAndFreeAmountOfSpace(id);

            return Ok(parkingArea);
        }

        catch (InvalidOperationException ex){ return NotFound(ex.Message); }
        catch (NpgsqlException ex){return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);}
    }
}