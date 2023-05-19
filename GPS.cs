using Newtonsoft.Json;

namespace GPS
{
    public class GPS
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string gpsTime { get; set; }
        public int speed { get; set; }
        public int angle { get; set; }
        public int altitude { get; set; }
        public int satellites { get; set; }
        public GPS(double latitude, double longitude, string gpsTime, int speed, int angle, int altitude, int satellites)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.gpsTime = gpsTime;
            this.speed = speed;
            this.angle = angle;
            this.altitude = altitude;
            this.satellites = satellites;
        }
        public GPS()
        {
            this.latitude = 0;
            this.longitude = 0;
            this.gpsTime = "";
            this.speed = 0;
            this.angle = 0;
            this.altitude = 0;
            this.satellites = 0;
        }
    }

    public class GPSDataReader
    {
        public static List<GPS> readGPSData(string path)
        {
            List<GPS>? jsonData = new List<GPS>();
            try
            {
                string json = File.ReadAllText(path);
                jsonData = JsonConvert.DeserializeObject<List<GPS>>(json);
            }
            catch
            {
                Console.WriteLine("ERROR: Can't read the file on " + path);
            }
            return jsonData;
        }
    }
}
