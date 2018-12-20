using System;
using MPK.Connect.Model.Business;

namespace MPK.Connect.Model.Graph
{
    public class City : LocalizableEntity<string>
    {
        public override string Id { get; set; }
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public City()
        {
        }

        public City(string id)
        {
            Id = id;
        }

        public override double GetDistanceTo(LocalizableEntity<string> otherEntity)
        {
            if (otherEntity is City)
            {
                if (otherEntity == this)
                {
                    return 0;
                }

                var city = otherEntity as City;

                if (Id == "S" && city.Id == "G" || Id == "G" && city.Id == "S")
                {
                    return 11;
                }

                if (Id == "S" && city.Id == "A" || Id == "A" && city.Id == "S")
                {
                    return 3;
                }
                if (Id == "S" && city.Id == "B" || Id == "B" && city.Id == "A")
                {
                    return 5;
                }
                if (Id == "B" && city.Id == "A" || Id == "A" && city.Id == "B")
                {
                    return 3;
                }
                if (Id == "D" && city.Id == "A" || Id == "A" && city.Id == "D")
                {
                    return 3;
                }
                if (Id == "G" && city.Id == "D" || Id == "D" && city.Id == "G")
                {
                    return 5;
                }
                if (Id == "B" && city.Id == "C" || Id == "C" && city.Id == "B")
                {
                    return 3;
                }
                if (Id == "C" && city.Id == "E" || Id == "E" && city.Id == "C")
                {
                    return 6;
                }
                if (Id == "C" && city.Id == "G" || Id == "G" && city.Id == "C")
                {
                    return 7.5;
                }
                if (Id == "B" && city.Id == "G" || Id == "G" && city.Id == "B")
                {
                    return 6;
                }
                if (Id == "A" && city.Id == "G" || Id == "G" && city.Id == "A")
                {
                    return 7.5;
                }

                return Double.MaxValue;
            }

            return Double.MaxValue;
        }

        public override double GetDistanceTo(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Id;
        }
    }
}