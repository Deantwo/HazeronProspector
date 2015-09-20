using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HazeronProspector
{
    public class Zone
    {
        protected CelestialBody _hostCelestialBody;
        public CelestialBody HostCelestialBody
        {
            get { return _hostCelestialBody; }
            set { _hostCelestialBody = value; }
        }

        protected Dictionary<ResourceType, Resource> _resources = new Dictionary<ResourceType, Resource>();
        public Dictionary<ResourceType, Resource> Resources
        {
            get { return _resources; }
        }

        protected int _id;
        public int ID
        {
            get { return _id; }
        }
        public string Name
        {
            get { return "z" + (_id + 1); }
        }

        public Zone()
        {
            _id = 0;
        }
        public Zone(int id)
        {
            _id = id;
        }

        public void AddResource(Resource resource)
        {
            _resources.Add(resource.Type, resource);
            resource.HostZone = this;
        }

        //protected List<HCity> _cities = new List<HCities>();
        //public List<HCity> Cities
        //{
        //    get { return _cities; }
        //}

        public override string ToString()
        {
            if (_hostCelestialBody.ResourceZones.Length > 1)
                return (_id + 1).ToString();
            else
                return "";
        }
    }
}
