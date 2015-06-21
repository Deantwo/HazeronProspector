using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HazeronProspector
{
    class Galaxy
    {
        protected Dictionary<string, Sector> _sectors = new Dictionary<string, Sector>();
        public Dictionary<string, Sector> Sectors
        {
            get { return _sectors; }
        }

        protected string _name;
        public string Name
        {
            get { return _name; }
        }

        public bool ContainSurvey
        {
            get { return _sectors.Values.Any(x => x.ContainSurvey); }
        }

        public Galaxy(string name)
        {
            _name = name;
        }

        public void AddSector(Sector sector)
        {
            _sectors.Add(sector.ID, sector);
            sector.HostGalaxy = this;
        }

        public Dictionary<ResourceType, Resource> BestResources()
        {
            List<Dictionary<ResourceType, Resource>> resourceLists = new List<Dictionary<ResourceType, Resource>>();
            foreach (Sector sector in _sectors.Values)
            {
                foreach (HSystem system in sector.Systems.Values)
                {
                    foreach (CelestialBody body in system.CelestialBodies.Values)
                    {
                        foreach (Zone zone in body.ResourceZones)
                        {
                            resourceLists.Add(body.BestResources());
                        }
                    }
                }
            }
            return resourceLists.SelectMany(dict => dict)
                         .ToLookup(pair => pair.Key, pair => pair.Value)
                         .ToDictionary(k => k.Key, v => v.Aggregate((i, j) => i.Quality > j.Quality ? i : j));
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
