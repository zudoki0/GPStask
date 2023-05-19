using System;
using GPS;

public class Program
{
    public static void Main()
    {
        List<GPS.GPS> data = GPSDataReader.readGPSData("2019-07.json");
        foreach(GPS.GPS gps in data)
        {
            Console.WriteLine("{0} {1} {2} {3} {4} {5} {6}", gps.latitude, gps.longitude, gps.gpsTime, gps.speed, gps.angle, gps.altitude, gps.satellites);
        }
    }
}