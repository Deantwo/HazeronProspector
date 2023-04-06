using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;

namespace HazeronProspector
{
    class HazeronStarMapReader
    {
        Dictionary<string, Galaxy> _galaxyList;
        public Dictionary<string, Galaxy> GalaxyList
        {
            get { return _galaxyList; }
            set { _galaxyList = value; }
        }

        Dictionary<string, Sector> _sectorList;
        public Dictionary<string, Sector> SectorList
        {
            get { return _sectorList; }
            set { _sectorList = value; }
        }

        Dictionary<string, HSystem> _systemList;
        public Dictionary<string, HSystem> SystemList
        {
            get { return _systemList; }
            set { _systemList = value; }
        }

        public HazeronStarMapReader()
        {
        }
        public HazeronStarMapReader(string filename)
        {
        }

        public void ReadXmlFile(string filename)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);

            _galaxyList = new Dictionary<string, Galaxy>();
            _sectorList = new Dictionary<string, Sector>();
            _systemList = new Dictionary<string, HSystem>();
            
            XmlNode starMapNode = xmlDoc.SelectSingleNode("/starmap");

            // Make sure there are actaully any ChildNodes before we continue.
            if (!starMapNode.HasChildNodes)
                return;

            XmlNodeList galaxyNodeList = starMapNode.SelectNodes("galaxy");
            foreach (XmlNode galaxyNode in galaxyNodeList)
            {
                Galaxy galaxy = new Galaxy(galaxyNode.Attributes["name"].Value);
                XmlNodeList sectorNodeList = galaxyNode.SelectNodes("sector");
                foreach (XmlNode sectorNode in sectorNodeList)
                {
                    Sector sector = new Sector(sectorNode.Attributes["sectorId"].Value, sectorNode.Attributes["name"].Value, sectorNode.Attributes["x"].Value, sectorNode.Attributes["y"].Value, sectorNode.Attributes["z"].Value);
                    XmlNodeList systemNodeList = sectorNode.SelectNodes("system[@eod='Surveyed']");
                    foreach (XmlNode systemNode in systemNodeList)
                    {
                        HSystem system = new HSystem(systemNode.Attributes["systemId"].Value, systemNode.Attributes["name"].Value, systemNode.Attributes["catalog"]?.Value, systemNode.Attributes["x"].Value, systemNode.Attributes["y"].Value, systemNode.Attributes["z"].Value, systemNode.Attributes["eod"].Value);
                        if (system.Surveyed) // This if statement is redundent because of the XPath search.
                        {
                            InitializeSystem(system, systemNode);
                            sector.AddSystem(system);
                            _systemList.Add(system.CatalogName, system);
                        }
                    }
                    if (sector.ContainSurvey)
                    {
                        galaxy.AddSector(sector);
                        _sectorList.Add(sector.CatalogName, sector);
                    }
                }
                if (galaxy.ContainSurvey)
                    _galaxyList.Add(galaxy.Name, galaxy);
            }

            WormholeLinking(xmlDoc);
        }

        public void AppendXmlFile(string filename)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);

            XmlNode starMapNode = xmlDoc.SelectSingleNode("/starmap");

            // Make sure there are actaully any ChildNodes before we continue.
            if (!starMapNode.HasChildNodes)
                return;

            XmlNodeList galaxyNodeList = starMapNode.SelectNodes("galaxy");
            foreach (XmlNode galaxyNode in galaxyNodeList)
            {
                Galaxy galaxy;
                if (_galaxyList.ContainsKey(galaxyNode.Attributes["name"].Value))
                    galaxy = _galaxyList[galaxyNode.Attributes["name"].Value];
                else
                    galaxy = new Galaxy(galaxyNode.Attributes["name"].Value);
                XmlNodeList sectorNodeList = galaxyNode.SelectNodes("sector");
                foreach (XmlNode sectorNode in sectorNodeList)
                {
                    string sectorCatalogName = $"Sector ({sectorNode.Attributes["x"].Value}, {sectorNode.Attributes["y"].Value}, {sectorNode.Attributes["z"].Value})";
                    Sector sector;
                    if (_sectorList.ContainsKey(sectorCatalogName))
                        sector = _sectorList[sectorCatalogName];
                    else
                        sector = new Sector(sectorNode.Attributes["sectorId"].Value, sectorNode.Attributes["name"].Value, sectorNode.Attributes["x"].Value, sectorNode.Attributes["y"].Value, sectorNode.Attributes["z"].Value);
                    XmlNodeList systemNodeList = sectorNode.SelectNodes("system[@eod='Surveyed']");
                    foreach (XmlNode systemNode in systemNodeList)
                    {
                        string systemCatalogName = systemNode.Attributes["catalog"]?.Value ?? systemNode.Attributes["name"].Value;
                        HSystem system;
                        if (_sectorList.ContainsKey(systemCatalogName))
                            system = _systemList[systemCatalogName];
                        else
                            system = new HSystem(systemNode.Attributes["systemId"].Value, systemNode.Attributes["name"].Value, systemNode.Attributes["catalog"]?.Value, systemNode.Attributes["x"].Value, systemNode.Attributes["y"].Value, systemNode.Attributes["z"].Value, systemNode.Attributes["eod"].Value);
                        if (!_systemList.ContainsKey(system.CatalogName) && system.Surveyed) // This if statement is redundent because of the XPath search.
                        {
                            InitializeSystem(system, systemNode);
                            sector.AddSystem(system);
                            _systemList.Add(system.CatalogName, system);
                        }
                    }
                    if (!_sectorList.ContainsKey(sector.CatalogName) && sector.ContainSurvey)
                    {
                        galaxy.AddSector(sector);
                        _sectorList.Add(sector.CatalogName, sector);
                    }
                }
                if (!_galaxyList.ContainsKey(galaxy.Name) && galaxy.ContainSurvey)
                    _galaxyList.Add(galaxy.Name, galaxy);
            }

            WormholeLinking(xmlDoc);
        }

        protected void WormholeLinking(XmlDocument xmlDoc)
        {
            // Wormholes
            XmlNodeList systemNodeList = xmlDoc.SelectNodes("/starmap/galaxy/sector/system");
            foreach (XmlNode systemNode in systemNodeList)
            {
                string systemCatalogName = systemNode.Attributes["catalog"]?.Value ?? systemNode.Attributes["name"].Value;
                if (_systemList.ContainsKey(systemCatalogName))
                {
                    XmlNodeList wormholeNodeList = systemNode.SelectNodes("wormhole");
                    foreach (XmlNode wormholeNode in wormholeNodeList)
                    {
                        if (wormholeNode.Attributes["polarity"].Value == "neutral") // Ignore intergalactic wormholes.
                            continue;

                        if (wormholeNode.Attributes["destSystemId"] != null)
                        {
                            Coordinate destSystemCoordinates = new Coordinate(double.Parse(wormholeNode.Attributes["destX"].Value, System.Globalization.CultureInfo.InvariantCulture),
                                                                              double.Parse(wormholeNode.Attributes["destY"].Value, System.Globalization.CultureInfo.InvariantCulture),
                                                                              double.Parse(wormholeNode.Attributes["destZ"].Value, System.Globalization.CultureInfo.InvariantCulture));
                            HSystem destSystem = _systemList.Values.FirstOrDefault(sys => sys.Coord.Equals(destSystemCoordinates));
                            if (destSystem is object)
                            {
                                HSystem system = _systemList[systemCatalogName];
                                system.AddWormholeLink(destSystem);
                                destSystem.AddWormholeLink(system);
                            }
                        }
                    }
                }
            }
        }

        private void InitializeSystem(HSystem system, XmlDocument xmlDoc)
        {
            XmlNode systemNode = xmlDoc.SelectSingleNode("/starmap/galaxy/sector/system[@systemId='" + system.ID + "']");

            InitializeSystem(system, systemNode);
        }
        private void InitializeSystem(HSystem system, XmlNode systemNode)
        {
            // Make sure there are actaully any ChildNodes before we continue.
            if (!systemNode.HasChildNodes)
                return; // AKA skip this system

            // Stars
            XmlNodeList starNodeList = systemNode.SelectNodes("star");
            foreach (XmlNode starNode in starNodeList)
            {
                CelestialBody star = new CelestialBody(starNode.Attributes["starId"].Value, starNode.Attributes["name"].Value, starNode.Attributes["shell"].Value);
                XmlNodeList resourceNodeList = starNode.SelectNodes("resource");
                foreach (XmlNode resourceNode in resourceNodeList)
                {
                    Resource resource = new Resource(resourceNode.Attributes["name"].Value, resourceNode.Attributes["quality"].Value, resourceNode.Attributes["abundance"].Value);
                    star.AddResource(0, resource);
                }
                system.AddCelestialBody(star);
            }
            // Planets
            XmlNodeList planetNodeList = systemNode.SelectNodes("planet");
            foreach (XmlNode planetNode in planetNodeList)
            {
                CelestialBody planet = new CelestialBody(planetNode.Attributes["planetId"].Value, planetNode.Attributes["name"].Value, planetNode.Attributes["bodyType"].Value, planetNode.Attributes["zone"].Value, planetNode.FirstChild.Attributes["diameter"].Value, planetNode.FirstChild.Attributes["resourceZones"].Value);
                XmlNodeList resourceNodeList = planetNode.SelectNodes(".//resource");
                foreach (XmlNode resourceNode in resourceNodeList)
                {
                    for (int zone = 0; zone < planet.ResourceZones.Length; zone++)
                    {
                        Resource resource;
                        if (resourceNode.Attributes["qualityZone" + (zone + 1)] != null)
                            resource = new Resource(resourceNode.Attributes["name"].Value, resourceNode.Attributes["qualityZone" + (zone + 1)].Value, resourceNode.Attributes["abundanceZone" + (zone + 1)].Value);
                        else
                            resource = new Resource(resourceNode.Attributes["name"].Value, resourceNode.Attributes["qualityZone1"].Value, resourceNode.Attributes["abundanceZone1"].Value);
                        planet.AddResource(zone, resource);
                    }
                }
                system.AddCelestialBody(planet);
            }

            system.Initialized = true;
        }
    }
}