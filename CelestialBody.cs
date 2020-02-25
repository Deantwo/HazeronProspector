using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HazeronProspector
{
    public enum CelestialBodyType
    {
        ERROR,
        Star,
        NeutronStar,
        BlackHole,
        Planet,
        Moon,
        GasGiant,
        Titan,
        Ring,
        Planetoid,
        RingworldArc
    }

    public enum CelestialBodyOrbit
    {
        ERROR,
        Star,
        Inferno,
        Inner,
        Habitable,
        Outer,
        Frigid
    }

    public class CelestialBody
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

        protected int _diameter;
        public int Diameter
        {
            get { return _diameter; }
        }

        public bool IsHabitable
        {
            get
            {
                return _orbit == CelestialBodyOrbit.Habitable
                && (_type == CelestialBodyType.Planet
                 || _type == CelestialBodyType.Titan
                 || _type == CelestialBodyType.RingworldArc
                    );
            }
        }

        public CelestialBody(string id, string name, string type)
            : this(id, name, type, "n/a", "n/a")
        {
        }
        public CelestialBody(string id, string name, string type, string orbit, string size)
            : this(id, name, type, orbit, size, new Zone[1])
        {
        }
        public CelestialBody(string id, string name, string type, string orbit, string size, string numZones)
            : this(id, name, type, orbit, size, Convert.ToInt32(numZones))
        {
        }
        public CelestialBody(string id, string name, string type, string orbit, string size, int numZones)
            : this(id, name, type, orbit, size, new Zone[numZones])
        {
        }
        public CelestialBody(string id, string name, string type, string orbit, string size, Zone[] zones)
        {
            _id = id;
            _name = name;
            _resourceZones = zones;
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
                case "Photosphere":
                    _type = CelestialBodyType.Star;
                    break;
                case "Photon Sphere":
                    _type = CelestialBodyType.NeutronStar;
                    break;
                case "Neutron Sphere":
                    _type = CelestialBodyType.BlackHole;
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
                case "Titan":
                    _type = CelestialBodyType.Titan;
                    break;
                case "Ring":
                    _type = CelestialBodyType.Ring;
                    break;
                case "Asteroid": // Minor backward compatibility.
                case "Planetoid":
                    _type = CelestialBodyType.Planetoid;
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
                case "Ringworld Arc 21":
                    _type = CelestialBodyType.RingworldArc;
                    break;
                default:
                    throw new Exception($"Unknown type: '{type}'");
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
                default:
                    throw new Exception($"Unknown orbit: '{orbit}'");
            }

            // Planet size.
            if (_type == CelestialBodyType.RingworldArc)
                _diameter = 36135; // 229,814 x 17,850m in spherical diameter.
            else
            {
                int pos = size.IndexOf("m ");
                if (pos >= 0)
                    size = size.Remove(pos);
                size = size.Replace(".", "").Replace(",", "");
                int.TryParse(size, out _diameter);
            }
        }

        public void AddResource(int zone, Resource resource)
        {
            _resourceZones[zone].AddResource(resource);
        }

        public Dictionary<ResourceType, Resource> BestResources(bool excludeUncolonizable = false)
        {
            if (excludeUncolonizable
             && (_type == CelestialBodyType.Star
              || _type == CelestialBodyType.Ring
                 ))
                return new Dictionary<ResourceType, Resource>();
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

        public override bool Equals(object obj)
        {
            CelestialBody celestialBody = obj as CelestialBody;
            if (celestialBody == null)
                return false;
            return Equals(celestialBody);
        }
        public bool Equals(CelestialBody celestialBody)
        {
            return _id == celestialBody.ID;
        }
    }
}
