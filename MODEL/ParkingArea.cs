using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MODEL;

public class ParkingArea
{
    public int? Id { get; set; }
    public string? StreetAddress { get; set; }

    public string? City { get; set; }

    public string? ZipCode { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public int? FreeSpaces { get; set; }

    public int? TotalSpace { get; set; }

    public IEnumerable<ParkingSpace?> parkingSpaces { get; set; }

    public ParkingArea(string address, string city, string zipcode, decimal latitude, decimal longitude, IEnumerable<ParkingSpace> parkingSpaces)
    {
        StreetAddress = address;
        City = city;
        ZipCode = zipcode;
        Latitude = latitude;
        Longitude = longitude;
        this.parkingSpaces = parkingSpaces;
    }

    public ParkingArea(int Id, string StreetAddress, string City, string ZipCode, decimal Latitude, decimal Longitude, IEnumerable<ParkingSpace> parkingSpaces) 
    { 
        this.Id = Id;
        this.StreetAddress= StreetAddress;
        this.City = City;
        this.ZipCode = ZipCode;
        this.Latitude = Latitude;
        this.Longitude = Longitude;
        this.parkingSpaces = parkingSpaces;
    }
    public ParkingArea() { }    
}
