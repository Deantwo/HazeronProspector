using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HazeronProspector
{
    enum CelestialBodyType
    {
        ERROR,
        Star,
        Planet,
        Moon,
        GasGiant,
        LargeMoon,
        Ring,
        RingworldArc
    }

    enum CelestialBodyOrbit
    {
        ERROR,
        Star,
        Inferno,
        Inner,
        Habitable,
        Outer,
        Frigid
    }

    class CelestialBody
    {
        protected HSystem _hostSystem;
        public HSystem HostSystem
        {
            get { return _hostSystem; }
            set { _hostSystem = value; }
        }

        protected CelestialBodyType _type;
        public CelestialBodyType Type
        {
            get { return _type; }
        }

        protected CelestialBodyOrbit _orbit;
        public CelestialBodyOrbit Orbit
        {
            get { return _orbit; }
        }

        protected string _name;
        public string Name
        {
            get { return _name; }
        }

        protected string _id;
        public string ID
        {
            get { return _id; }
        }

        protected Zone[] _resourceZones;
        public Zone[] ResourceZones
        {
            get { return _resourceZones; }
        }

        protected int _populationLimit;
        public int PopulationLimit
        {
            get { return _populationLimit; }
        }

        public CelestialBody(string id, string name, string type)
        {
            _id = id;
            _name = name;
            _resourceZones = new Zone[1];
            Initialize(type, "n/a", "n/a");
        }
        public CelestialBody(string id, string name, string type, string orbit, string size)
        {
            _id = id;
            _name = name;
            _resourceZones = new Zone[1];
            Initialize(type, orbit, size);
        }
        public CelestialBody(string id, string name, string type, string orbit, string size, int numZones)
        {
            _id = id;
            _name = name;
            _resourceZones = new Zone[numZones];
            Initialize(type, orbit, size);
        }
        public CelestialBody(string id, string name, string type, string orbit, string size, string numZones)
        {
            _id = id;
            _name = name;
            _resourceZones = new Zone[Convert.ToInt32(numZones)];
            Initialize(type, orbit, size);
        }
        
        public void Initialize(string type, string orbit, string size)
        {
            for (int i = 0; i < _resourceZones.Length; i++)
            {
                _resourceZones[i] = new Zone(i);
                _resourceZones[i].HostCelestialBody = this;
            }

            switch (type)
            {
                case "Star":
                    _type = CelestialBodyType.Star;
                    break;
                case "Planet":
                    _type = CelestialBodyType.Planet;
                    break;
                case "Moon":
                    _type = CelestialBodyType.Moon;
                    break;
                case "Gas Giant":
                    _type = CelestialBodyType.GasGiant;
                    break;
                case "Large Moon":
                    _type = CelestialBodyType.LargeMoon;
                    break;
                case "Ring":
                    _type = CelestialBodyType.Ring;
                    break;
                case "Ringworld Arc 1":
                case "Ringworld Arc 2":
                case "Ringworld Arc 3":
                case "Ringworld Arc 4":
                case "Ringworld Arc 5":
                case "Ringworld Arc 6":
                case "Ringworld Arc 7":
                case "Ringworld Arc 8":
                case "Ringworld Arc 9":
                case "Ringworld Arc 10":
                case "Ringworld Arc 11":
                case "Ringworld Arc 12":
                case "Ringworld Arc 13":
                case "Ringworld Arc 14":
                case "Ringworld Arc 15":
                case "Ringworld Arc 16":
                case "Ringworld Arc 17":
                case "Ringworld Arc 18":
                case "Ringworld Arc 19":
                case "Ringworld Arc 20":
                    _type = CelestialBodyType.RingworldArc;
                    break;
            }

            switch (orbit)
            {
                case "n/a":
                    _orbit = CelestialBodyOrbit.Star;
                    break;
                case "Inferno Zone":
                    _orbit = CelestialBodyOrbit.Inferno;
                    break;
                case "Inner Zone":
                    _orbit = CelestialBodyOrbit.Inner;
                    break;
                case "Habitable Zone":
                    _orbit = CelestialBodyOrbit.Habitable;
                    break;
                case "Outer Zone":
                    _orbit = CelestialBodyOrbit.Outer;
                    break;
                case "Frigid Zone":
                    _orbit = CelestialBodyOrbit.Frigid;
                    break;
            }

            // Planet size to population limit
            if (_type == CelestialBodyType.RingworldArc)
                _populationLimit = 1000;
            else if (_type == CelestialBodyType.Planet || _type == CelestialBodyType.Moon || _type == CelestialBodyType.LargeMoon)
            {
                _populationLimit = Convert.ToInt32(size.Remove(size.LastIndexOf(' ') - 1));
                _populationLimit = Convert.ToInt32(100 * Math.Floor((float)_populationLimit / 1800));
            }
        }

        public void AddResource(int zone, Resource resource)
        {
            _resourceZones[zone].AddResource(resource);
        }

        public Dictionary<ResourceType, Resource> BestResources()
        {
            List<Dictionary<ResourceType, Resource>> resourceLists = new List<Dictionary<ResourceType, Resource>>();
            foreach (Zone zone in _resourceZones)
            {
                resourceLists.Add(zone.Resources);
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
