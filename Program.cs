using System;
using GPS;

public class Program
{
    public static void Main()
    {
        List<GPS.GPS> data = GPSDataReader.readGPSJson("2019-07.json");
        /*
        List<GPS.GPS> data = GPSDataReader.readGPSCsv("2019-08.csv");
        */
        foreach(GPS.GPS gps in data)
        {
            Console.WriteLine("{0} {1} {2} {3} {4} {5} {6}", gps.latitude, gps.longitude, gps.gpsTime, gps.speed, gps.angle, gps.altitude, gps.satellites);
        }
    }
}