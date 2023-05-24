using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Histogram
{
    public class Histogram
    {
        public string name;
        public int height;
        public Dictionary<int, int> data;
        public int divisor;
        private int findMaxOfData()
        {
            int max = 0;
            foreach(var item in data)
            {
                if(item.Value > max)
                {
                    max = item.Value;
                }
            }
            return max;
        }
        private int findMaxOfKeys()
        {
            int max = 0;
            foreach (var item in data)
            {
                if (item.Key > max)
                {
                    max = item.Key;
                }
            }
            return max;
        }
        public Histogram(string name, int height, Dictionary<int, int> data) {
            this.name = name;
            this.height = height;
            this.data = data;
            this.divisor = 10;
        }
        public Histogram(string name, int height, Dictionary<int, int> data, int divisor)
        {
            this.name = name;
            this.height = height;
            this.data = data;
            this.divisor = divisor;
        }
        public void drawHistogram()
        {
            char element = '\u2591';
            int max = findMaxOfData();
            int maxKey = findMaxOfKeys();
            int index = 0;
            if (maxKey > 50)
            {
                int[] dividedData = new int[(maxKey / divisor) + 1];
                int maxOfArray = 0;
                foreach(var item in data)
                {
                    dividedData[item.Key / divisor] += item.Value;
                }
                foreach (var item in dividedData)
                {
                    if (item > maxOfArray)
                    {
                        maxOfArray = item;
                    }
                }
                int temp;
                int numX, numY;
                int digit, maxDigit = Convert.ToInt32(Math.Floor(Math.Log10(maxKey) + 1));
                for(int i = 0; i < maxDigit*2+6; i++) Console.Write(' ');
                Console.Write("| ");
                for (int i = 0; i < height; i++) Console.Write('-');
                Console.Write(" | hits");
                Console.WriteLine();
                Console.SetCursorPosition(0, Console.GetCursorPosition().Top-1);
                Console.WriteLine(name + ' ');
                for (int i = 0; i < dividedData.Length; i++)
                {
                    temp = Convert.ToInt32(Math.Ceiling((double)dividedData[i] / maxOfArray * height));
                    numX = i * divisor;
                    numY = (i + 1) * divisor - 1;
                    if (numX != 0) digit = Convert.ToInt32(Math.Floor(Math.Log10(numX) + 1)); else digit = 1;
                    Console.Write("[");
                    for (int j = 0; j < maxDigit - digit; j++) Console.Write(' ');
                    Console.Write(numX + " - ");
                    if (numY != 0) digit = Convert.ToInt32(Math.Floor(Math.Log10(numY) + 1)); else digit = 1;
                    for (int j = 0; j < maxDigit - digit; j++) Console.Write(' ');
                    Console.Write(numY + "] | ");
                    for(int j = 0; j < temp; j++) Console.Write(element);
                    for (int j = 0; j < height - temp; j++) Console.Write(' ');
                    Console.Write(" | " + dividedData[i]);
                    Console.WriteLine();
                }
                Console.WriteLine();
            } else
            {
                Console.WriteLine(name);
                int cursorY = Console.GetCursorPosition().Top;
                for (int i = 0; i <= maxKey; i++)
                {
                    if (data.ContainsKey(i))
                    {
                        double temp = Math.Ceiling((double)data[i] / max * height);
                        if (temp == 0) continue;
                        for (int j = height; j >= height - Convert.ToInt32(temp); j--)
                        {
                            Console.SetCursorPosition(index, cursorY + j);
                            Console.Write(element);
                            Console.Write(element);
                        }
                    }
                
                    Console.SetCursorPosition(index, cursorY + height);
                    if (i < height)
                    {
                        Console.Write(0);
                        Console.Write(i);
                    }
                    else
                    {
                        Console.Write(i);
                    }

                    index += 3;
                }
                
                Console.SetCursorPosition(index, cursorY + height - 1);
                Console.Write(0 + " hits");
                Console.SetCursorPosition(index, cursorY);
                Console.Write(max + " hits");
                Console.SetCursorPosition(0, cursorY + height + 1);
                Console.WriteLine();
                
            }
        }
    }
}
