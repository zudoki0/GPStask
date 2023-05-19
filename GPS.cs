using System.Data;

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
                        data[index] += c;
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
    }
}
