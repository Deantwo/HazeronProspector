using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HazeronProspector
{
    public class HSystem
    {
        protected Dictionary<string, CelestialBody> _celestialBodies = new Dictionary<string, CelestialBody>();
        public Dictionary<string, CelestialBody> CelestialBodies
        {
            get { return _celestialBodies; }
        }

        protected Sector _hostSector;
        public Sector HostSector
        {
            get { return _hostSector; }
            set { _hostSector = value; }
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

        protected string _id;
        public string ID
        {
            get { return _id; }
        }

        protected Coordinate _coord;
        public Coordinate Coord
        {
            get { return _coord; }
        }

        protected string _eod;
        public string EOD
        {
            get { return _eod; }
        }
        public bool Unexplored
        {
            get { return EOD == "Unexplored"; }
        }
        public bool Explored
        {
            get { return EOD == "Explored"; }
        }
        public bool Surveyed
        {
            get { return EOD == "Surveyed"; }
        }

        protected bool _initialized = false;
        public bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
        }

        protected List<HSystem> _wormholeLinks = new List<HSystem>();
        public List<HSystem> WormholeLinks
        {
            get { return _wormholeLinks; }
        }

        public HSystem(string id, string name, string catalog, string x, string y, string z, string eod)
        {
            _id = id;
            _name = name;
            _catalogName = catalog is null ? name : catalog;
            double nX = Convert.ToDouble(x, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            double nY = Convert.ToDouble(y, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            double nZ = Convert.ToDouble(z, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            _coord = new Coordinate(nX, nY, nZ);
            _eod = eod;
        }

        public void AddCelestialBody(CelestialBody celestialBody)
        {
            _celestialBodies.Add(celestialBody.ID, celestialBody);
            celestialBody.HostSystem = this;
        }

        public void AddWormholeLink(HSystem destination)
        {
            if (!_wormholeLinks.Contains(destination))
                _wormholeLinks.Add(destination);
        }

        public Dictionary<ResourceType, Resource> BestResources(bool excludeUncolonizable = false)
        {
            List<Dictionary<ResourceType, Resource>> resourceLists = new List<Dictionary<ResourceType, Resource>>();
            foreach (CelestialBody body in _celestialBodies.Values)
            {
                foreach (Zone zone in body.ResourceZones)
                {
                    resourceLists.Add(body.BestResources(excludeUncolonizable));
                }
            }
            return resourceLists.SelectMany(dict => dict)
                         .ToLookup(pair => pair.Key, pair => pair.Value)
                         .ToDictionary(k => k.Key, v => v.Aggregate((i, j) => i.Quality > j.Quality ? i : j));
        }

        public int HabitbleCount()
        {
            return _celestialBodies.Values.Count(x => x.IsHabitable);
        }

        public override string ToString()
        {
            return _name;
        }

        public override bool Equals(object obj)
        {
            HSystem system = obj as HSystem;
            if (system is null)
                return false;
            return Equals(system);
        }
        public bool Equals(HSystem system)
        {
            return _id == system.ID;
        }
    }
}
