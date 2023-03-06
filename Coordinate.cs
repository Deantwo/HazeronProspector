using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace HazeronProspector
{
    // Heavily based on: https://markheath.net/post/coord-performance-versus-readability
    public struct Coordinate : IEnumerable<double>, IEquatable<Coordinate>
    {
        private readonly double _x;
        private readonly double _y;
        private readonly double _z;
        public double X { get => _x; }
        public double Y { get => _y; }
        public double Z { get => _z; }

        public int SectorX { get { return (int)Math.Round(_x) / 10; } }
        public int SectorY { get { return (int)Math.Round(_y) / 10; } }
        public int SectorZ { get { return (int)Math.Round(_z) / 10; } }

        public double this[double index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _x;
                    case 1:
                        return _y;
                    case 2:
                        return _z;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public Coordinate(double x, double y)
        {
            this._x = x;
            this._y = y;
            _z = 0;
        }
        public Coordinate(double x, double y, double z)
        {
            this._x = x;
            this._y = y;
            this._z = z;
        }
        public Coordinate(int sectorX, int sectorY, int sectorZ)
        {
            _x = (sectorX * 10);
            _y = (sectorY * 10);
            _z = (sectorZ * 10);
        }

        public static implicit operator (double, double)(Coordinate c) => (c._x, c._y);
        public static implicit operator Coordinate((double X, double Y) c) => new Coordinate(c.X, c.Y);
        public static implicit operator (double, double, double)(Coordinate c) => (c._x, c._y, c._z);
        public static implicit operator Coordinate((double X, double Y, double Z) c) => new Coordinate(c.X, c.Y, c.Z);

        public void Deconstruct(out double x, out double y)
        {
            x = this._x;
            y = this._y;
        }

        public void Deconstruct(out double x, out double y, out double z)
        {
            x = this._x;
            y = this._y;
            z = this._z;
        }

        public double Distance(Coordinate dist)
        {
            return Math.Sqrt(Math.Pow(_x - dist.X, 2) + Math.Pow(_y - dist.Y, 2) + Math.Pow(_z - dist.Z, 2));
        }

        public static Coordinate operator +(Coordinate a, Coordinate b)
        {
            return new Coordinate(a._x + b._x, a._y + b._y, a._z + b._z);
        }

        public override bool Equals(object other) =>
            other is Coordinate c
                && c._x.Equals(_x)
                && c._y.Equals(_y)
                && c._z.Equals(_z);

        // Implement IEquatable<T> https://stackoverflow.com/a/8952026/7532
#if CSHARP8_OR_GREATER
        public bool Equals([AllowNull] Coordinate other) => x == other.x && y == other.y && z == other.z;
#else
        public bool Equals(Coordinate other)
        {
            //if (other is null)
            //    return false;
            return _x == other._x && _y == other._y && _z == other._z;
        }
#endif


        public override int GetHashCode()
        //            => HashHelpers.Combine(HashHelpers.Combine(HashHelpers.Combine(HashHelpers.RandomSeed, _x), _y), _z);
        {
            // based on Jon Skeet - hashcode of an int is just its value
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + (int)_x;
                hash = hash * 23 + (int)_y;
                hash = hash * 23 + (int)_z;
                return hash;
            }
        }
        // => _x.GetHashCode() ^ _y.GetHashCode() ^ _z.GetHashCode();

        public override string ToString() => $"({_x},{_y},{_z})";

        public IEnumerator<double> GetEnumerator()
        {
            yield return _x;
            yield return _y;
            yield return _z;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}