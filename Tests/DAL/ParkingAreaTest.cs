using MODEL;
using DAL;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.EntityFrameworkCore.Query;

namespace Tests.DAL
{
    public class ParkingAreaTest
    {
        ParkingAreaDAO parkingAreaDAO = default!;
        List<int> parkingAreaIds = new List<int>();
        private string connectionString = "Host=localhost;Port=5432;Database=Sensade;Username=postgres;Password=lol123";


        [SetUp]
        public void Setup()
        {
            parkingAreaDAO = new ParkingAreaDAO();
            //insertTestData();
        }

        [Category("Database")]
        [Description("Integration test for creating a parking area when all fields are valid")]
        [Test]
        public void CreateParkingArea_WhenAllFieldsAreValid_ReturnsTrue()
        {
            //Arrange
            ParkingArea parkingArea = new ParkingArea("Gadevej 55", "Fantasiby", "7900", 50.7128m, -20.0060m);
            //Act
            int actualId = parkingAreaDAO.CreateParkingArea(parkingArea);
            parkingAreaIds.Add(actualId);
            //Assert
            ParkingArea selectedParkingArea = parkingAreaDAO.SelectParkingAreaById(actualId);
            int expectedId = selectedParkingArea.Id;
            string errorMessage = $"Test failed: actualID should be equal to expectedID: actualID {actualId}, expectedID {expectedId}";
            Assert.That(actualId, Is.EqualTo(expectedId), errorMessage);
        }


        //[Test]
        //public void DeleteParkingSpace_WhenIdIsValid_returnsTrue()
        //{
        //    //arrange 
        //    int parkingSpaceIdToDelete = parkingAreaIds[0];
        //    //act
        //    bool expectedTrue = parkingAreaDAO.DeleteParkingSpaceById(parkingSpaceIdToDelete);
        //    //assert
        //    Assert.That(expectedTrue, Is.True);

        //}

        //public void SelectParkingSpace_WhenSpaceNumberIsValid_returnsId()
        //{
        //    //Arrange 
        //    int parkingSpaceNumberToSelect = 1;
        //}


        //[TearDown]
        //public void Teardown()
        //{
        //    foreach (int id in parkingSpaceIds) { parkingSpaceDAO.DeleteParkingSpaceById(id); }
        //    parkingSpaceIds.Clear();
        //}


        //public void insertTestData()
        //{
        //    List<ParkingSpace> parkingSpaces = CreateParkingSpaceTestData();
        //    foreach (ParkingSpace parking in parkingSpaces)
        //    {
        //        int id = parkingSpaceDAO.InsertParkingSpace(parking);
        //        parkingSpaceIds.Add(id);
        //    }
        //}

        //public List<ParkingArea> CreateParkingSpaceTestData()
        //{
        //    List<ParkingArea> data = new List<ParkingArea> {
        //    new ParkingArea(false, 3),
        //    new ParkingArea(true, 4),
        //    new ParkingArea (false, 5)
        //    };

        //    return data;
        //}
    }
}