using System.Collections;
using System.Data;
using System.Globalization;
using System.Text;

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
        public static List<GPS> readGPSJson(string path)
        {
            List<GPS>? jsonData = new List<GPS>();
            try
            {
                List<string> data = new List<string>();
                bool inBrackets = false;
                bool inQuotes = false;
                int index = 0;
                string json = File.ReadAllText(path);
                foreach (char c in json)
                {
                    //CHECK IF THE READ SYMBOL IS IN QUOTES
                    if (c == '"')
                    {
                        if (inQuotes) inQuotes = false; inQuotes = true;
                    }
                    //IGNORE UNNECESSARY SYMBOLS
                    if (c != ':' && c != '[' && c != ']' && c != '{' && c != '}' && c != '"' && c != ',' && data.Count > 0)
                    {
                        if (c != 'T') data[index] += c;
                        else data[index] += ' ';
                    }
                    //DON'T IGNORE ':' SYMBOL FOR DIFFER
                    if (c == ':' && inQuotes)
                    {
                        data[index] += c;
                    }
                    if(c == '{')
                    {
                        data.Add("");
                        inBrackets = true;
                    }
                    if ((c == ':' && !inQuotes) || (c == ',' && inBrackets))
                    {
                        data.Add("");
                        index++;
                    }
                    if(c == '}')
                    {
                        inBrackets = false;

                        //REMOVE JSON KEY VALUES (e.g. Latitude:)
                        for(int i = 0; i < data.Count; i++)
                        {
                            data[i] = data[i].Substring(data[i].IndexOf(':') + 1);
                        }

                        GPS temp = new GPS(Convert.ToDouble(data[0]), Convert.ToDouble(data[1]), data[2], Convert.ToInt32(data[3]), Convert.ToInt32(data[4]), Convert.ToInt32(data[5]), Convert.ToInt32(data[6]));
                        jsonData.Add(temp);

                        data.Clear();
                        index = 0;
                    }
                }
            }
            catch
            {
                Console.WriteLine("ERROR: Can't read the file on " + path);
            }
            return jsonData;
        }
        public static List<GPS> readGPSCsv(string path)
        {
            List<GPS>? csvData = new List<GPS>();
            try
            {
                IEnumerable<string> csv = File.ReadLines(path);
                foreach(var line in csv)
                {
                    //STORING EVERY COLUMN OF A ROW IN CSV FILE
                    List<string> data = new List<string>();
                    int index = 0;
                    data.Add("");
                    foreach(char c in line)
                    {
                        if (c != ',') data[index] += c;
                        if (c == ',')
                        {
                            data.Add("");
                            index++;
                        }
                    }
                    GPS temp = new GPS(Convert.ToDouble(data[0]), Convert.ToDouble(data[1]), data[2], Convert.ToInt32(data[3]), Convert.ToInt32(data[4]), Convert.ToInt32(data[5]), Convert.ToInt32(data[6]));
                    csvData.Add(temp);
                }
            }
            catch
            {
                Console.WriteLine("ERROR: Can't read the file on " + path);
            }
            return csvData;
        }
        public static List<GPS> readGPSBin(string path)
        {
            List<GPS>? binData = new List<GPS>();
            try
            {
                int index = 0;
                byte[] fileBytes = File.ReadAllBytes(path);
                List<byte> tempArray = new List<byte>();
                DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                
                int lat = 0;
                int lon = 0;
                long gpsT = 0;
                short spd = 0;
                short agl = 0;
                short alt = 0;
                byte sat = 0;
                List<string> data = new List<string>();

                foreach (byte b in fileBytes)
                {
                    tempArray.Add(b);
                    if(index == 3)
                    {
                        byte[] byteArray = tempArray.ToArray();
                        Array.Reverse(byteArray);
                        lat = BitConverter.ToInt32(byteArray, 0);
                        tempArray.Clear();
                    }
                    else if (index == 7)
                    {
                        byte[] byteArray = tempArray.ToArray();
                        Array.Reverse(byteArray);
                        lon = BitConverter.ToInt32(byteArray, 0);
                        tempArray.Clear();
                    }
                    else if (index == 15)
                    {
                        byte[] byteArray = tempArray.ToArray();
                        Array.Reverse(byteArray);
                        gpsT = BitConverter.ToInt64(byteArray, 0);
                        tempArray.Clear();
                    }
                    else if (index == 17)
                    {
                        byte[] byteArray = tempArray.ToArray();
                        Array.Reverse(byteArray);
                        spd = BitConverter.ToInt16(byteArray, 0);
                        tempArray.Clear();
                    }
                    else if (index == 19)
                    {
                        byte[] byteArray = tempArray.ToArray();
                        Array.Reverse(byteArray);
                        agl = BitConverter.ToInt16(byteArray, 0);
                        tempArray.Clear();
                    }
                    else if (index == 21)
                    {
                        byte[] byteArray = tempArray.ToArray();
                        Array.Reverse(byteArray);
                        alt = BitConverter.ToInt16(byteArray, 0);
                        tempArray.Clear();
                    }
                    else if (index == 22)
                    {
                        sat = b;
                        DateTime tempDate = unixEpoch.AddMilliseconds(gpsT);
                        GPS temp = new GPS
                        (
                            Convert.ToDouble(lat / 10000000),
                            Convert.ToDouble(lon / 10000000),
                            tempDate.ToString("yyyy-MM-dd HH:mm:ss"),
                            spd,
                            agl,
                            alt,
                            sat
                        );
                        binData.Add(temp);
                        index = -1;
                        tempArray.Clear();
                    }
                    index++;
                }
                
            }
            catch
            {
                Console.WriteLine("ERROR: Can't read the file on " + path);
            }

            return binData;
        }
    }
    public class GPSDataReceiver
    {
        public static Dictionary<double, int> GetLatitude(List<GPS> data)
        {
            Dictionary<double, int> sat = new Dictionary<double, int>();
            foreach (GPS gps in data)
            {
                if(!sat.ContainsKey(gps.latitude))
                {
                    sat.Add(gps.latitude, 1);
                } else
                {
                    sat[gps.latitude]++;
                }
            }
            return sat;
        }
        public static Dictionary<double, int> GetLongitude(List<GPS> data)
        {
            Dictionary<double, int> sat = new Dictionary<double, int>();
            foreach (GPS gps in data)
            {
                if (!sat.ContainsKey(gps.longitude))
                {
                    sat.Add(gps.longitude, 1);
                }
                else
                {
                    sat[gps.longitude]++;
                }
            }
            return sat;
        }
        public static Dictionary<string, int> GPSTime(List<GPS> data)
        {
            Dictionary<string, int> sat = new Dictionary<string, int>();
            foreach (GPS gps in data)
            {
                if (!sat.ContainsKey(gps.gpsTime))
                {
                    sat.Add(gps.gpsTime, 1);
                }
                else
                {
                    sat[gps.gpsTime]++;
                }
            }
            return sat;
        }
        public static Dictionary<int, int> GetSpeed(List<GPS> data)
        {
            Dictionary<int, int> sat = new Dictionary<int, int>();
            foreach (GPS gps in data)
            {
                if (!sat.ContainsKey(gps.speed))
                {
                    sat.Add(gps.speed, 1);
                }
                else
                {
                    sat[gps.speed]++;
                }
            }
            return sat;
        }
        public static Dictionary<int, int> GetAltitude(List<GPS> data)
        {
            Dictionary<int, int> sat = new Dictionary<int, int>();
            foreach (GPS gps in data)
            {
                if (!sat.ContainsKey(gps.altitude))
                {
                    sat.Add(gps.altitude, 1);
                }
                else
                {
                    sat[gps.altitude]++;
                }
            }
            return sat;
        }
        public static Dictionary<int, int> GetSatellites(List<GPS> data)
        {
            Dictionary<int, int> sat = new Dictionary<int, int>();
            foreach (GPS gps in data)
            {
                if (!sat.ContainsKey(gps.satellites))
                {
                    sat.Add(gps.satellites, 1);
                }
                else
                {
                    sat[gps.satellites]++;
                }
            }
            return sat;
        }
    }
    public class GPSDataAnalyzer
    {
        public static void AnalyzeGPS(List<GPS> data)
        {
            string[] formats = { "yyyy-MM-dd HH:mm:ss.fff", "yyyy-MM-dd HH:mm:ss" };
            double minTime = 99999999;
            double minDistance = 0;
            int startPos=0, endPos=0;
            double minTimeInHours = 0;
            for (int i = 0; i < data.Count; i++)
            {
                double distance = 0;
                for (int j = i; j < data.Count; j++)
                {
                    if (i == j) continue;
                    distance += Geolocation.GeoCalculator.GetDistance(data[j].latitude, data[j].longitude, data[j - 1].latitude, data[j - 1].longitude, 2, Geolocation.DistanceUnit.Kilometers);
                    if (distance >= 100)
                    {
                        double time = 0;
                        double timeInHours = 0;
                        foreach (string format in formats)
                        {
                            DateTime dateFrom;
                            DateTime dateTo;
                            if(DateTime.TryParseExact(data[i].gpsTime, format, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dateFrom)
                                &&
                               DateTime.TryParseExact(data[j].gpsTime, format, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTo))
                            {
                                dateFrom = DateTime.ParseExact(data[i].gpsTime, format, System.Globalization.CultureInfo.InvariantCulture);
                                dateTo = DateTime.ParseExact(data[j].gpsTime, format, System.Globalization.CultureInfo.InvariantCulture);
                                time = (dateTo - dateFrom).TotalSeconds;
                                timeInHours = (dateTo - dateFrom).TotalHours;
                                break;
                            }
                        }
                        if (time < minTime && time != 0)
                        {
                            startPos = i;
                            endPos = j;
                            minDistance = distance;
                            minTime = time;
                            minTimeInHours = timeInHours;
                        }
                        break;
                    }
                }
            }
            Console.WriteLine("Fastest road section of at least 100 km was driven over " + minTime.ToString("0.000") + "s and was " + minDistance.ToString("0.000") + "km long.");
            Console.WriteLine("Start position " + data[startPos].latitude.ToString("0.0000000") + "; " + data[startPos].longitude.ToString("0.0000000"));
            Console.WriteLine("Start gps time " + data[startPos].gpsTime);
            Console.WriteLine("End   position " + data[endPos].latitude.ToString("0.0000000") + "; " + data[endPos].longitude.ToString("0.0000000"));
            Console.WriteLine("End   gps time " + data[endPos].gpsTime);
            Console.WriteLine("Average speed: " + Convert.ToDouble(minDistance / minTimeInHours).ToString("0.0") + "km/h");
        }
    }
}
