using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HazeronProspector
{
    public class Sector
    {
        protected Dictionary<string, HSystem> _systems = new Dictionary<string, HSystem>();
        public Dictionary<string, HSystem> Systems
        {
            get { return _systems; }
        }

        protected Galaxy _hostGalaxy;
        public Galaxy HostGalaxy
        {
            get { return _hostGalaxy; }
            set { _hostGalaxy = value; }
        }

        protected string _id;
        public string ID
        {
            get { return _id; }
        }

        protected string _name;
        public string Name
        {
            get { return _name; }
        }

        protected string _catalogName;
        public string CatalogName
        {
            get { return _catalogName; }
        }

        protected Coordinate _coord;
        public Coordinate Coord
        {
            get { return _coord; }
        }

        public bool ContainSurvey
        {
            get { return _systems.Values.Any(x => x.Surveyed); }
        }

        public Sector(string id, string name, string x, string y, string z)
        {
            _id = id;
            _name = name;
            _catalogName = $"Sector ({x}, {y}, {z})";
            int nX = Convert.ToInt32(x, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            int nY = Convert.ToInt32(y, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            int nZ = Convert.ToInt32(z, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            _coord = new Coordinate(nX, nY, nZ);
        }

        public void AddSystem(HSystem system)
        {
            _systems.Add(system.ID, system);
            system.HostSector = this;
        }

        public Dictionary<ResourceType, Resource> BestResources()
        {
            List<Dictionary<ResourceType, Resource>> resourceLists = new List<Dictionary<ResourceType, Resource>>();
            foreach (HSystem system in _systems.Values)
            {
                foreach (CelestialBody body in system.CelestialBodies.Values)
                {
                    foreach (Zone zone in body.ResourceZones)
                    {
                        resourceLists.Add(body.BestResources());
                    }
                }
            }
            return resourceLists.SelectMany(dict => dict)
                         .ToLookup(pair => pair.Key, pair => pair.Value)
                         .ToDictionary(k => k.Key, v => v.Aggregate((i, j) => i.Quality > j.Quality ? i : j));
        }

        public override string ToString()
        {
            return _name + " (" + _coord.SectorX + ", " + _coord.SectorY + ", " + _coord.SectorZ + ")";
        }

        //public override bool Equals(object obj)
        //{
        //    Sector sector = obj as Sector;
        //    if (sector == null)
        //        return false;
        //    return Equals(sector);
        //}
        //public bool Equals(Sector sector)
        //{
        //    return _id == sector.ID;
        //}
    }
}
