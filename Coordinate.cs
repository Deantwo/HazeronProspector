using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HazeronProspector
{
    public class Coordinate
    {
        protected double _x;
        public double X
        {
            get { return _x; }
        }

        protected double _y;
        public double Y
        {
            get { return _y; }
        }

        protected double _z;
        public double Z
        {
            get { return _z; }
        }

        public int SectorX
        {
            get { return (int)Math.Round(_x) / 10; }
        }

        public int SectorY
        {
            get { return (int)Math.Round(_y) / 10; }
        }

        public int SectorZ
        {
            get { return (int)Math.Round(_z) / 10; }
        }

        public Coordinate()
        {
            _x = 0;
            _y = 0;
            _z = 0;
        }
        public Coordinate(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
        public Coordinate(int x, int y, int z)
        {
            _x = (x * 10);
            _y = (y * 10);
            _z = (z * 10);
        }

        public double Distance(Coordinate dist)
        {
            return Math.Sqrt(Math.Pow(_x - dist.X, 2) + Math.Pow(_y - dist.Y, 2) + Math.Pow(_z - dist.Z, 2));
        }

        public override string ToString()
        {
            return Math.Round(_x, 3).ToString(Hazeron.NumberFormat) + ", " + Math.Round(_y, 3).ToString(Hazeron.NumberFormat) + ", " + Math.Round(_z, 3).ToString(Hazeron.NumberFormat);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            Coordinate coordinate = (obj as Coordinate);
            return Equals(coordinate);
        }
        public bool Equals(Coordinate coordinate)
        {
            if (coordinate == null)
                return false;
            return (coordinate.X == _x && coordinate.Y == _y && coordinate.Z == _z);
        }
    }
}
