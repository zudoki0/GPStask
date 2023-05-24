using System;
using System.Text;
using GPS;

public class Program
{
    public static void Main()
    {
        List<GPS.GPS> data = GPSDataReader.readGPSJson("2019-07.json");
        data.AddRange(GPSDataReader.readGPSCsv("2019-08.csv"));

        Dictionary<int, int> sat = new Dictionary<int, int>();

        sat = GPS.GPSDataReceiver.GetSatellites(data);
        Histogram.Histogram hist = new Histogram.Histogram("Satellite histogram", 10, sat, 0);
        sat = GPS.GPSDataReceiver.GetSpeed(data);
        Histogram.Histogram hist2 = new Histogram.Histogram("Speed histogram", 10, sat, 10);

        hist2.drawHistogram();
        hist.drawHistogram();
        GPS.GPSDataAnalyzer.AnalyzeGPS(data);
    }
}