using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HazeronProspector
{
    enum ResourceType
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
        Bolite,
        Coal,
        Crystals,
        Eludium,
        Gems,
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
        NaturalGas,
        Oil,
        Phlogiston,
        Polytaride,
        Magmex,
        Myrathane,
        Water,
        AntifluxParticles,
        BorexinoPrecipitate,
        Preons
    }

    class Resource
    {
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

        public int TechLevel
        {
            get { return 1 + (_quality / 8); }
        }

        public System.Drawing.Color TechLevelColor
        {
            get
            {
                if ((Type == ResourceType.Cryozine && TechLevel < 4)
                 || (Type == ResourceType.Phlogiston && TechLevel < 5)
                 || (Type == ResourceType.Ioplasma && TechLevel < 7)
                 || (Type == ResourceType.Magmex && TechLevel < 10)
                 || (Type == ResourceType.Myrathane && TechLevel < 13)
                 || (Type == ResourceType.Polytaride && TechLevel < 16)
                 || (Type == ResourceType.Vulcanite && TechLevel < 19)
                 || (Type == ResourceType.AntifluxParticles && TechLevel < 24)
                 || (Type == ResourceType.Bolite && TechLevel < 24)
                 || (Type == ResourceType.BorexinoPrecipitate && TechLevel < 30)
                   )
                    return System.Drawing.Color.Gray;
                if (TechLevel < 14)
                    return System.Drawing.Color.Red;
                else if (TechLevel < 24)
                    return System.Drawing.Color.Orange;
                else if (TechLevel < 30)
                    return System.Drawing.Color.Green;
                else
                    return System.Drawing.Color.Blue;
            }
        }

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
                case "Natural Gas":
                    _type = ResourceType.NaturalGas;
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
