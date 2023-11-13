using DAL.Exceptions;
using Dapper;
using Interface;
using MODEL;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DAL;

public class ParkingAreaDAO : IParkingAreaDao<ParkingArea>
{
    private string connectionString = default!;
    private IParkingSpaceDao<ParkingSpace> _parkingSpaceDAO;

    private const string _insertParkingArea = "INSERT INTO parking_area (street_address, city, zip_code, latitude, longitude) VALUES (@StreetAddress, @City, @ZipCode, @Latitude, @Longitude) RETURNING parking_area_id_pk";
    private const string _selectParkingAreaById = "SELECT parking_area_id_pk AS Id, street_address AS StreetAddress, city AS City, zip_code as ZipCode, latitude AS Latitude, longitude as Longitude FROM parking_area WHERE parking_area_id_pk = @id";
    private const string _deleteParkingArea = "DELETE FROM parking_area WHERE parking_area_id_pk = @id";
    private const string _updateParkingArea = "Update parking_area SET street_address = @StreetAddress, city = @City, zip_code = @ZipCode, latitude = @Latitude, longitude = @Longitude WHERE parking_area_id_pk = @id";
    private const string _getTotalAndFreeAmount = "SELECT parking_area_id_pk AS id, COALESCE(SUM(CASE WHEN status = 'true' THEN 1 ELSE 0 END), 0) AS freeSpaces,COALESCE(COUNT(parking_space_id_pk), 0) AS TotalSpace FROM parking_area LEFT JOIN parking_space ON parking_area_id_pk = parking_area_id_fk WHERE parking_area_id_pk = @id GROUP BY parking_area_id_pk";


    public ParkingAreaDAO(string connectionString)
    {
        this.connectionString = connectionString;
        _parkingSpaceDAO = new ParkingSpaceDAO(connectionString);
    }

    public async Task<ParkingArea> GetTotalAndFreeAmountOfSpace(int id)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                ParkingArea parkingArea = await connection.QueryFirstAsync<ParkingArea>(_getTotalAndFreeAmount, new { id });
                return parkingArea;
            }

            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The requested parking area was not found");
            }
            catch (NpgsqlException ex)
            {
                throw new Exception("An error occured when tyring to retrive data");
            }
        }
    }

    public async Task<int> AddAsync(ParkingArea entity)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            try
            {
                int parkingAreaId = await connection.QuerySingleAsync<int>(_insertParkingArea, entity);
                return parkingAreaId;
            }
            catch (PostgresException ex)
            {
                throw new DuplicateItemException("Parking area already exist");
            }

            catch (NpgsqlException ex)
            {
                throw new NpgsqlException($"A server related issue occured, please try again later");
            }
        }
    }

    public async Task<bool> UpdateAsync(ParkingArea entity)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                int rows = await connection.ExecuteAsync(_updateParkingArea, entity);

                if (rows == 0)
                {
                    return false;
                }

                return true;
            }
            catch (PostgresException ex) 
            {
                throw new DuplicateItemException("The parking area already exist");
            }
            catch (NpgsqlException ex)
            {
                throw new NpgsqlException($"A server related issue occured, please try again later");
            }
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                int rows = await connection.ExecuteAsync(_deleteParkingArea, new { id });
                if (rows == 0)
                {
                    return false;
                }
                return true;
            }
            catch (NpgsqlException ex)
            {
                throw new NpgsqlException($"A server related issue occured, please try again later");
            }
        }
    }

    public async Task<ParkingArea> GetAsync(int id)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                ParkingArea? parkingArea = await connection.QueryFirstAsync<ParkingArea>(_selectParkingAreaById, new { id });

                var parkingSpaces = await _parkingSpaceDAO.GetAllParkingSpacesForParkingAreaAsync(id);

                if(!parkingSpaces.Any<ParkingSpace>())
                {
                    return parkingArea;
                }
                
                parkingArea.parkingSpaces = parkingSpaces;

                return parkingArea;
            }

            catch(InvalidOperationException ex)
            {
                throw new InvalidOperationException("The parking area was not found");
            }
            catch (NpgsqlException ex)
            {
                throw new NpgsqlException($"A server related issue occured, please try again later");
            }
        }
    }

    public Task<ParkingArea> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}
