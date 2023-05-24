using System;
using System.Text;
using GPS;

public class Program
{
    public static void Main()
    {
        List<GPS.GPS> data = GPSDataReader.readGPSJson("2019-07.json");
        Dictionary<int, int> sat = new Dictionary<int, int>();
        sat = GPS.GPSDataReceiver.GetSatellites(data);
        Histogram.Histogram hist = new Histogram.Histogram("Satellite histogram", 10, sat, 0);
        sat = GPS.GPSDataReceiver.GetSpeed(data);
        Histogram.Histogram hist2 = new Histogram.Histogram("Speed histogram", 10, sat, 10);
        hist.drawHistogram();
        Console.WriteLine();
        hist2.drawHistogram();
    }
}