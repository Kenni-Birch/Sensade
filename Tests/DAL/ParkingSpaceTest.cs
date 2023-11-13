using MODEL;
using DAL;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.EntityFrameworkCore.Query;

namespace Tests.DAL
{
    public class ParkingSpaceTest
    {
        ParkingSpaceDAO parkingSpaceDAO = default!;
        List<int> parkingSpaceIds = new List<int>();
        private string connectionString = "Host=localhost;Port=5432;Database=Sensade;Username=postgres;Password=lol123";


        [SetUp]
        public void Setup()
        {
            parkingSpaceDAO = new ParkingSpaceDAO();
            insertTestData();
        }

        [Category("Database")]
        [Description("Integration test for creating a parking space when all fields are valid")]
        [Test]
        public void CreateParkingSpace_WhenAllFieldsAreValid_ReturnsTrue()
        {
            //Arrange
            ParkingSpace parkingSpace = new ParkingSpace(false, 1, 12);
            //Act
            int actualId = parkingSpaceDAO.addParkingSpace(parkingSpace);
            parkingSpaceIds.Add(actualId);
            //Assert
            int expectedId = parkingSpaceDAO.getParkingSpaceById(actualId);
            string errorMessage = $"Test failed: actualID should be equal to expectedID: actualID {actualId}, expectedID {expectedId}";
            Assert.That(actualId, Is.EqualTo(expectedId), errorMessage);
        }

        [Test]
        public void DeleteParkingSpace_WhenIdIsValid_returnsTrue()
        {
            //arrange 
            int parkingSpaceIdToDelete = parkingSpaceIds[0];
            //act
            bool expectedTrue = parkingSpaceDAO.deleteParkingSpaceById(parkingSpaceIdToDelete);
            //assert
            Assert.That(expectedTrue, Is.True);

        }


        [TearDown]
        public void Teardown() 
        {
            foreach (int id in parkingSpaceIds) { parkingSpaceDAO.deleteParkingSpaceById(id);}
            parkingSpaceIds.Clear();
        }
      

        public void insertTestData()
        {
            List<ParkingSpace> parkingSpaces = CreateParkingSpaceTestData();
            foreach (ParkingSpace parking in parkingSpaces) 
            {
                int id = parkingSpaceDAO.addParkingSpace(parking);
                parkingSpaceIds.Add(id);
            }
        }

        public List<ParkingSpace> CreateParkingSpaceTestData()
        {
            List<ParkingSpace> data = new List<ParkingSpace> {
            new ParkingSpace(false, 3, 7),
            new ParkingSpace(true, 4, 9),
            new ParkingSpace (false, 5, 11)
            };

            return data;
        }
    }
}


