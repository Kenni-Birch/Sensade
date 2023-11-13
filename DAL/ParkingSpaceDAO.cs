using Dapper;
using Npgsql;
using MODEL;
using Interface;
using DAL.Exceptions;

namespace DAL;

public class ParkingSpaceDAO : IParkingSpaceDao<ParkingSpace>
{
    private string connectionString = default!;

    private const string insertParkingSpace = "INSERT INTO parking_space (status, space_number, parking_area_id_fk ) VALUES (@Status, @SpaceNumber, @ParkingArea) RETURNING parking_space_id_pk";

    private const string deleteParkingSpace = "DELETE FROM parking_space WHERE parking_space_id_pk = @id";

    private const string selectParkingSpaceById = "SELECT parking_space_id_pk AS Id, status AS Status, space_number AS SpaceNumber, parking_area_id_fk AS ParkingArea FROM parking_space WHERE parking_space_id_pk = @id";

    private const string _updateParkingArea = "Update parking_space SET status = @Status, space_number = @SpaceNumber WHERE parking_space_id_pk = @id";

    private const string _selectAllParkingSpacesForParkingArea = "SELECT parking_space_id_pk AS Id, status AS Status, space_number AS SpaceNumber, parking_area_id_fk AS ParkingArea from parking_space WHERE parking_area_id_fk = @id";

    private const string _updateStatus = "UPDATE parking_space SET status = @Status WHERE parking_space_id_pk = @id";

    public ParkingSpaceDAO(string connectionString)
    {
        this.connectionString = connectionString;
    }

 
    public async Task<IEnumerable<ParkingSpace>> GetAllParkingSpacesForParkingAreaAsync(int id)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {

            try
            {
                connection.Open();
                IEnumerable<ParkingSpace> parkingSpace = await connection.QueryAsync<ParkingSpace>(_selectAllParkingSpacesForParkingArea, new { id });
                return parkingSpace;
            }
            catch (NpgsqlException ex)
            {
               throw new NpgsqlException($"A server related issue occured, please try again later");
            }
        }
    }

    public async Task<int> AddAsync(ParkingSpace entity)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int parkingSpaceId = await connection.QuerySingleAsync<int>(insertParkingSpace, entity, transaction);

                        transaction.Commit();
                        return parkingSpaceId;
                    }
                    catch(PostgresException ex) when (ex.SqlState == "23503")
                    {
                        transaction.Rollback();
                        throw new ForeignKeyConstraintException("The parking area does not exist");
                    }

                    catch (PostgresException ex)
                    {
                        transaction.Rollback();
                        throw new DuplicateItemException($"The parking space already exist");
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new NpgsqlException($"A server related issue occured, please try again later");
            }
        }
    }

    public async Task<bool> UpdateAsync(ParkingSpace entity)
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

                int rows = await connection.ExecuteAsync(deleteParkingSpace, new { id });
                if (rows == 0)
                {
                    return false;
                }
                return true;
            }
            catch (NpgsqlException ex) {throw new NpgsqlException($"A server related issue occured, please try again later");}
        }
    }

    public async Task<ParkingSpace> GetAsync(int id)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                {
                    try
                    {
                        ParkingSpace parkingSpace = await connection.QueryFirstAsync<ParkingSpace>(selectParkingSpaceById, new { id });
                        return parkingSpace;
                    }
                    catch (InvalidOperationException ex)
                    {
                        throw new InvalidOperationException($"The parking space does not exist");
                    }
                }
            }
            catch (NpgsqlException ex) { throw new NpgsqlException($"A server related issue occured, please try again later"); }
        }
    }
   

    public async Task<bool> updateStatusAsync(bool status, int id)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                int rows = await connection.ExecuteAsync(_updateStatus, new { status, id });

                if (rows == 0)
                {
                    return false;
                }

                return true;
            }
            catch (NpgsqlException ex) { throw new NpgsqlException($"A server related issue occured, please try again later"); }
        }
    }

    public async Task<ParkingSpace> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}