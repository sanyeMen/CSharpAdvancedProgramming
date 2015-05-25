using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAdvancedProgramming.Linq
{
    [Serializable]
    public class Racer : IComparable<Racer>, IFormattable
    {

        public Racer(string firstName = null, string lastName = null, string country = null, int starts = 0, int wins = 0, IEnumerable<int> years = null, IEnumerable<string> cars = null)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Country = country;
            this.Starts = starts;
            this.Wins = wins;
            var yearList = new List<int>();
            foreach (var year in years)
            {
                yearList.Add(year);
            }
            this.Years = yearList.ToArray();
            var carList = new List<string>();
            foreach (var car in cars)
            {
                carList.Add(car);
            }
            this.Cars = carList.ToArray();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Wins { get; set; }

        public string Country { get; set; }

        public int Starts { get; set; }

        public string[] Cars { get; set; }

        public int[] Years { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1}", this.FirstName, this.LastName);
        }

        public int CompareTo(Racer other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            return this.LastName.CompareTo(other.LastName);
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case null:
                    {
                        return null;
                    }
                case "N":
                    {
                        return ToString();
                    }
                case "F":
                    {
                        return this.FirstName;
                    }
                case "L":
                    {
                        return this.LastName;
                    }
                case "C":
                    {
                        return this.Country;
                    }
                case "S":
                    {
                        return this.Starts.ToString();
                    }
                case "W":
                    {
                        return this.Wins.ToString();
                    }
                case "A":
                    {
                        return String.Format("{0} {1} {2}; starts: {3}, wins: {4}", this.FirstName, this.LastName, this.Country, this.Starts, this.Wins);
                    }
                default:
                    {
                        throw new FormatException(String.Format("Format {0} not supported", format));
                    }
            }
        }
    }
}
