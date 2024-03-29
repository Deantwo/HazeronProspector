﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HazeronProspector
{
    public enum ResourceType
    {
        ERROR,
        AnimalCarcass,
        Beans,
        Cheese,
        Eggs,
        Fertilizer,
        Fish,
        Fruit,
        Grain,
        Grapes,
        Herbs,
        Hops,
        Hay,
        Log,
        Milk,
        Nuts,
        PlantFiber,
        Spices,
        Vegetable,
        Adamantite,
        Bolite,
        Coal,
        Crystals,
        Eludium,
        Gems,
        GoldOre,
        Ice,
        Lumenite,
        Minerals,
        Ore,
        Radioactives,
        Stone,
        Vulcanite,
        Air,
        Cryozine,
        Hydrogen,
        Ioplasma,
        Oil,
        Phlogiston,
        Polytaride,
        Viathol,
        Flomentum,
        Magmex,
        Myrathane,
        Water,
        AntifluxParticles,
        BorexinoPrecipitate,
        Preons
    }

    public enum ResourceCategory
    {
        ERROR,
        Geosphere,
        Hydrophere,
        Biosphere,
        Atmosphere,
        Photophere
    }

    public class Resource
    {
        public static ResourceCategory GetCategory(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.AnimalCarcass:
                case ResourceType.Beans:
                case ResourceType.Cheese:
                case ResourceType.Eggs:
                case ResourceType.Fertilizer:
                case ResourceType.Fish:
                case ResourceType.Fruit:
                case ResourceType.Grain:
                case ResourceType.Grapes:
                case ResourceType.Herbs:
                case ResourceType.Hops:
                case ResourceType.Hay:
                case ResourceType.Log:
                case ResourceType.Milk:
                case ResourceType.Nuts:
                case ResourceType.PlantFiber:
                case ResourceType.Spices:
                case ResourceType.Vegetable:
                    return ResourceCategory.Biosphere;
                case ResourceType.Adamantite:
                case ResourceType.Bolite:
                case ResourceType.Coal:
                case ResourceType.Crystals:
                case ResourceType.Eludium:
                case ResourceType.Gems:
                case ResourceType.GoldOre:
                case ResourceType.Ice:
                case ResourceType.Lumenite:
                case ResourceType.Minerals:
                case ResourceType.Ore:
                case ResourceType.Radioactives:
                case ResourceType.Stone:
                case ResourceType.Vulcanite:
                    return ResourceCategory.Geosphere;
                case ResourceType.Oil:
                case ResourceType.Phlogiston:
                case ResourceType.Polytaride:
                case ResourceType.Viathol:
                    return ResourceCategory.Geosphere;
                case ResourceType.Air:
                case ResourceType.Cryozine:
                case ResourceType.Hydrogen:
                case ResourceType.Ioplasma:
                case ResourceType.AntifluxParticles:
                case ResourceType.BorexinoPrecipitate:
                    return ResourceCategory.Atmosphere;
                case ResourceType.Flomentum:
                case ResourceType.Magmex:
                case ResourceType.Myrathane:
                case ResourceType.Water:
                    return ResourceCategory.Hydrophere;
                case ResourceType.Preons:
                    return ResourceCategory.Photophere;
                default:
                    throw new Exception($"Unknown resource type: '{type}'");
            }
        }

        public static bool IsAcrossZones(ResourceType type)
        {
            return GetCategory(type) == ResourceCategory.Hydrophere || GetCategory(type) == ResourceCategory.Atmosphere;
        }

        protected Zone _hostZone;
        public Zone HostZone
        {
            get { return _hostZone; }
            set { _hostZone = value; }
        }

        protected ResourceType _type;
        public ResourceType Type
        {
            get { return _type; }
        }

        protected string _name;
        public string Name
        {
            get { return _name; }
        }

        protected byte _quality;
        public byte Quality
        {
            get { return _quality; }
        }

        protected byte _abundance;
        public byte Abundance
        {
            get { return _abundance; }
        }

        public System.Drawing.Color TechLevelColor
        {
            get
            {
                if (_abundance == 0)
                    return System.Drawing.Color.Gray;

                if (_quality < 104)
                    return System.Drawing.Color.Red;
                else if (_quality < 184)
                    return System.Drawing.Color.Orange;
                else if (_quality < 248)
                    return System.Drawing.Color.Green;
                else
                    return System.Drawing.Color.Blue;
            }
        }

        public ResourceCategory Category => GetCategory(_type);

        public bool AcrossZones => IsAcrossZones(_type);

        public Resource(string name, byte quality, byte abundance)
        {
            _name = name;
            _quality = quality;
            _abundance = abundance;
            Initialize();
        }
        public Resource(string name, string quality, string abundance)
        {
            _name = name;
            _quality = Convert.ToByte(quality);
            _abundance = Convert.ToByte(abundance);
            Initialize();
        }

        protected void Initialize()
        {
            if (_abundance == 0)
                _quality = 0;

            switch (_name)
            {
                case "Animal Carcass":
                    _type = ResourceType.AnimalCarcass;
                    break;
                case "Beans":
                    _type = ResourceType.Beans;
                    break;
                case "Cheese":
                    _type = ResourceType.Cheese;
                    break;
                case "Eggs":
                    _type = ResourceType.Eggs;
                    break;
                case "Fertilizer":
                    _type = ResourceType.Fertilizer;
                    break;
                case "Fish":
                    _type = ResourceType.Fish;
                    break;
                case "Fruit":
                    _type = ResourceType.Fruit;
                    break;
                case "Grain":
                    _type = ResourceType.Grain;
                    break;
                case "Grapes":
                    _type = ResourceType.Grapes;
                    break;
                case "Herbs":
                    _type = ResourceType.Herbs;
                    break;
                case "Hops":
                    _type = ResourceType.Hops;
                    break;
                case "Vegetation Density":
                    _type = ResourceType.Hay;
                    break;
                case "Log":
                    _type = ResourceType.Log;
                    break;
                case "Milk":
                    _type = ResourceType.Milk;
                    break;
                case "Nuts":
                    _type = ResourceType.Nuts;
                    break;
                case "Plant Fiber":
                    _type = ResourceType.PlantFiber;
                    break;
                case "Spices":
                    _type = ResourceType.Spices;
                    break;
                case "Vegetable":
                    _type = ResourceType.Vegetable;
                    break;
                case "Adamantite":
                    _type = ResourceType.Adamantite;
                    break;
                case "Bolite":
                    _type = ResourceType.Bolite;
                    break;
                case "Coal":
                    _type = ResourceType.Coal;
                    break;
                case "Crystals":
                    _type = ResourceType.Crystals;
                    break;
                case "Eludium":
                    _type = ResourceType.Eludium;
                    break;
                case "Gems":
                    _type = ResourceType.Gems;
                    break;
                case "Gold Ore":
                    _type = ResourceType.GoldOre;
                    break;
                case "Ice":
                    _type = ResourceType.Ice;
                    break;
                case "Lumenite":
                    _type = ResourceType.Lumenite;
                    break;
                case "Minerals":
                    _type = ResourceType.Minerals;
                    break;
                case "Ore":
                    _type = ResourceType.Ore;
                    break;
                case "Radioactives":
                    _type = ResourceType.Radioactives;
                    break;
                case "Stone":
                    _type = ResourceType.Stone;
                    break;
                case "Vulcanite":
                    _type = ResourceType.Vulcanite;
                    break;
                case "Air":
                    _type = ResourceType.Air;
                    break;
                case "Cryozine":
                    _type = ResourceType.Cryozine;
                    break;
                case "Hydrogen":
                    _type = ResourceType.Hydrogen;
                    break;
                case "Ioplasma":
                    _type = ResourceType.Ioplasma;
                    break;
                case "Oil":
                    _type = ResourceType.Oil;
                    break;
                case "Phlogiston":
                    _type = ResourceType.Phlogiston;
                    break;
                case "Polytaride":
                    _type = ResourceType.Polytaride;
                    break;
                case "Viathol":
                    _type = ResourceType.Viathol;
                    break;
                case "Flomentum":
                    _type = ResourceType.Flomentum;
                    break;
                case "Magmex":
                    _type = ResourceType.Magmex;
                    break;
                case "Myrathane":
                    _type = ResourceType.Myrathane;
                    break;
                case "Water in the Environment":
                    _type = ResourceType.Water;
                    break;
                case "Antiflux Particles":
                    _type = ResourceType.AntifluxParticles;
                    break;
                case "Borexino Precipitate":
                    _type = ResourceType.BorexinoPrecipitate;
                    break;
                case "Type O Preons":
                case "Type B Preons":
                case "Type A Preons":
                case "Type F Preons":
                case "Type G Preons":
                case "Type K Preons":
                case "Type M Preons":
                    _type = ResourceType.Preons;
                    break;
                default:
                    throw new Exception($"Unknown resource: '{_name}'");
            }
        }

        public override string ToString()
        {
            string text;
            if (_abundance != 0)
                text = _quality.ToString() + " (" + _abundance + "%)";
            else
                text = "None";
            return text;
        }
    }
}
