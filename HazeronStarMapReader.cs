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
        protected XmlDocument _xmlDoc = new XmlDocument();

        protected string _filename;
        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        public HazeronStarMapReader()
        {
        }
        public HazeronStarMapReader(string filename)
        {
            _filename = filename;
            _xmlDoc.Load(_filename);
        }

        public void ReadSectorsAndSystems(ref Dictionary<string, Galaxy> galaxyList, ref Dictionary<string, Sector> sectorList, ref Dictionary<string, HSystem> systemList)
        {
            galaxyList.Clear();
            sectorList.Clear();
            systemList.Clear();

            XmlNode starMapNode = _xmlDoc.SelectSingleNode("/starmap");

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
                        HSystem system = new HSystem(systemNode.Attributes["systemId"].Value, systemNode.Attributes["name"].Value, systemNode.Attributes["x"].Value, systemNode.Attributes["y"].Value, systemNode.Attributes["z"].Value, systemNode.Attributes["eod"].Value);
                        if (system.Surveyed) // This if statement is redundent because of the XPath search.
                        {
                            sector.AddSystem(system);
                            systemList.Add(system.ID, system);
                        }
                    }
                    if (sector.ContainSurvey)
                    {
                        galaxy.AddSector(sector);
                        sectorList.Add(sector.ID, sector);
                    }
                }
                if (galaxy.ContainSurvey)
                    galaxyList.Add(galaxy.Name, galaxy);
            }

            WormholeLinking(ref systemList);
        }

        protected void WormholeLinking(ref Dictionary<string, HSystem> systemList)
        {
            // Wormholes
            XmlNodeList systemNodeList = _xmlDoc.SelectNodes("/starmap/galaxy/sector/system");
            foreach (XmlNode systemNode in systemNodeList)
            {
                string systemId = systemNode.Attributes["systemId"].Value;
                if (systemList.ContainsKey(systemId))
                {
                    XmlNodeList wormholeNodeList = systemNode.SelectNodes("wormhole");
                    foreach (XmlNode wormholeNode in wormholeNodeList)
                    {
                        if (wormholeNode.Attributes["destSystemId"] != null)
                        {
                            string destSystemId = wormholeNode.Attributes["destSystemId"].Value;
                            if (systemList.ContainsKey(destSystemId))
                            {
                                systemList[systemId].AddWormholeLink(systemList[destSystemId]);
                                systemList[destSystemId].AddWormholeLink(systemList[systemId]);
                            }
                        }
                    }
                }
            }
        }

        public void InitializeSystems(List<HSystem> systems)
        {
            foreach (HSystem system in systems)
            {
                XmlNode systemNode = _xmlDoc.SelectSingleNode("/starmap/galaxy/sector/system[@systemId='" + system.ID + "']");

                // Make sure there are actaully any ChildNodes before we continue.
                if (!systemNode.HasChildNodes)
                    continue; // AKA skip this system

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
}