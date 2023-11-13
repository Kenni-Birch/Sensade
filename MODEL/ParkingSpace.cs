namespace MODEL
{
    public class ParkingSpace
    {
        public int? Id { get; set; }
        public bool? Status { get; set; }
        public int? SpaceNumber { get; set; }
        public int? ParkingArea { get; set; }

        public ParkingSpace(bool status, int spaceNumber, int parkingArea)
        {
            this.Status = status;
            this.SpaceNumber = spaceNumber;
            this.ParkingArea = parkingArea;
        }

        public ParkingSpace(int id, bool status, int spaceNumber, int parkingArea)
        {
            this.Id = id;
            this.Status = status;
            this.SpaceNumber = spaceNumber;
        }

        public ParkingSpace() { }
    }
}