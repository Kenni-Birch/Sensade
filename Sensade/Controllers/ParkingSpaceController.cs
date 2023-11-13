using DAL;
using MODEL;
using Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Npgsql;
using DAL.Exceptions;

namespace Sensade.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParkingSpacesController : ControllerBase
{
    private IParkingSpaceDao<ParkingSpace> _parkingSpaceDAO;

    public ParkingSpacesController(IParkingSpaceDao<ParkingSpace> parkingSpaceDAO)
    {
        _parkingSpaceDAO = parkingSpaceDAO;
    }

    [HttpPost] 
    public async Task<ActionResult> AddParkingSpace(ParkingSpace parkingSpace)
    {
        try
        {
            int id = await _parkingSpaceDAO.AddAsync(parkingSpace);
            return Ok(id);
        }
        catch(ForeignKeyConstraintException ex) { return NotFound(ex.Message); }
        catch(DuplicateItemException ex) { return Conflict(ex.Message); }
        catch (NpgsqlException ex){return StatusCode(500, ex.Message);}
    }

    [HttpDelete]

    public async Task<ActionResult> DeleteParkingSpace(int id) 
    {
        try
        {
            var parkingSpace = await _parkingSpaceDAO.GetAsync(id);
           
            if (await _parkingSpaceDAO.DeleteAsync(id))
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Parking space was not deleted");
            }
        }
        catch (InvalidOperationException ex) { return NotFound(ex.Message); }
        catch (NpgsqlException ex) { return StatusCode(500, ex.Message); }
    }

    [HttpGet]
    public async Task<ActionResult> GetParkingSpace(int id) 
    {
        try
        {
            ParkingSpace parkingSpace = await _parkingSpaceDAO.GetAsync(id);
            return Ok(parkingSpace);    
        }
        catch (InvalidOperationException ex) { return NotFound(ex.Message); }
        catch (NpgsqlException ex) { return StatusCode(500, ex.Message); }
    }

    [HttpPut]
    public async Task<ActionResult> UpdateParkingSpace(ParkingSpace newparkingSpace) 
    {
        try
        {
            ParkingSpace parkingSpace = await _parkingSpaceDAO.GetAsync((int)newparkingSpace.Id);
            if (await _parkingSpaceDAO.UpdateAsync(newparkingSpace))
            {
                return Ok();
            }
            else { return BadRequest("Parking space was not updated"); }
        }
        catch(DuplicateItemException ex) { return Conflict(ex.Message); }
        catch (InvalidOperationException ex) { return NotFound(ex.Message); }
        catch (NpgsqlException ex) { return StatusCode(500, ex.Message); }

    }

    [HttpPut]
    [Route("Status")]
    public async Task<ActionResult> UpdateStatusParkingSpace(bool status, int id)
    {
        try
        {
            ParkingSpace parkingSpace = await _parkingSpaceDAO.GetAsync(id);
            if (await _parkingSpaceDAO.updateStatusAsync(status, id))
            {
                return Ok();
            }
            else { return BadRequest("Parking space was not updated"); }
        }
        catch (InvalidOperationException ex) { return NotFound(ex.Message); }
        catch (NpgsqlException ex) { return StatusCode(500, ex.Message); }
    }

}
