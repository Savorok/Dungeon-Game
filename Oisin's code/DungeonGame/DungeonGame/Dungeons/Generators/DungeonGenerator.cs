using DungeonGame.Dungeons.DungeonClasses;
using DungeonGame.Other;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace DungeonGame.Dungeons.Generators
{
    class DungeonGenerator
    {
        //Level variables
        public int MaxHeight { get; private set; }  //maximum height a level can be
        public int MinHeight { get; private set; }  //minimum height a level can be
        public int MaxWidth { get; private set; }   //maximum width a level can be 
        public int MinWidth { get; private set; }   //minimum width a level can be
        public int SpaceMinMax { get; private set; } //The upper range of what the minimum amount of times to take space from a level can be 
        public int SpaceMinMin { get; private set; } //The lower range of what the minimum amount of times to take space from a level can be 
        public int SpaceMaxMax { get; private set; } //The upper range of what the maximum amount of times to take space from a level can be
        public int SpaceMaxMin { get; private set; } //The lower range of what the maximum amount of times to take space from a level can be
        public int SpaceSizeMinMax { get; private set; } //The upper range of what the Minimum space size can be 
        public int SpaceSizeMinMin { get; private set; } //The lower range of what the Minimum space size can be 
        public int SpaceSizeMaxMax { get; private set; } //The upper range of what the Maximum space size can be 
        public int SpaceSizeMaxMin { get; private set; } //The lower range of what the Maximum space size can be 
        public int MinRegSizeMin { get; private set; } //The Maximum size a region can be
        public int MinRegSizeMax { get; private set; } //The minimum size a region can be 



        /*
         * This will create a dungeon generator and load 
         * its configuration settings from a given config file
         */
        public DungeonGenerator(string configFile)
        {
            loadConfig(configFile);
        }

        /*
         * This will create a dungeon generator and make the user enter 
         * its configuration settings
         */
        public DungeonGenerator()
        {

        }

        /*
         * This method will generate a dungeon with a given depth,
         * it will also generate levels for the dungeon based on the loaded
         * config file
         */
        public Dungeon GenerateDungeon(int depth)
        {
            //init empty list of levels
            List<Level> levels = new List<Level>();
            //create a level generator 
            LevelGenerator lg = new LevelGenerator();
            //create 'depth' many levels
            for (int i = 0; i < depth; i++)
            {
                //generate the level settings
                int[] settings = generateRandomLevelSettings();
                //generate the level
                var level = lg.GenerateLevel(i, settings[0], settings[1], settings[2], settings[3], settings[4], settings[5], settings[6]);
                //add the level to the list of levels
                levels.Add(level);
            }
            //create the dungeon and return           
            Dungeon dungeon = new Dungeon(depth, levels);
            return dungeon;
        }

        /*
         * This method will generate random properties for a room given the config file loaded
         */
        public int[] generateRandomLevelSettings()
        {
            //init return value
            int[] levelSettings = new int[7];

            //generate level height
            levelSettings[0] = RandomFunctions.randomRangeWithMax(this.MinHeight, this.MaxHeight);
            //generate level width
            levelSettings[1] = RandomFunctions.randomRangeWithMax(this.MinWidth, this.MaxWidth);
            //generate level space min
            levelSettings[2] = RandomFunctions.randomRangeWithMax(this.SpaceMinMin,this.SpaceMinMax);
            //generate level space max
            levelSettings[3] = RandomFunctions.randomRangeWithMax(this.SpaceMaxMin, this.SpaceMaxMax);
            //generate level space size min
            levelSettings[4] = RandomFunctions.randomRangeWithMax(this.SpaceSizeMinMin, this.SpaceSizeMinMax);
            //generate level space size max
            levelSettings[5] = RandomFunctions.randomRangeWithMax(this.SpaceSizeMaxMin, this.SpaceSizeMaxMax);
            //generate level region size
            levelSettings[6] = RandomFunctions.randomRangeWithMax(this.MinRegSizeMin, this.MinRegSizeMax);

            //return settings
            return levelSettings;
        }

        /*
         * This Method will load the config file for the dungeon generator
         */
        public bool loadConfig(string configFile)
        {
            try
            {
                string filePath = "../../../Configs/" + configFile;
                string defaultPath = "../../../Configs/defaultConfig.txt";

                //get current path                  
                if (File.Exists(filePath))
                {
                    Console.WriteLine("Config file found loading settings...");
                    //get and set the config settings
                    string[] settings = File.ReadAllLines(filePath);
                    //set the configurations
                    setConfig(settings);
                }
                else if((File.Exists(defaultPath)))
                {
                    Console.WriteLine("Config file has not been found loading default settings...");
                    //get and set the config settings
                    string[] settings = File.ReadAllLines(defaultPath);
                    //set the configurations
                    setConfig(settings);
                }
                else
                {
                    ConsoleMessages.PrintErrorMessage("DungeonGenerator:LoadConfig", "No default config file found, cannot load config!");
                    return false;
                }
            }
            catch(Exception ex)
            {
                ConsoleMessages.PrintErrorMessage("DungeonGenerator:LoadConfig", "Exception:" + ex);
                return false;
            }

            Console.WriteLine("Config File sucessfully loaded");
            return true;
        }

        /*
         * Given the list of all the configs in the form of a string array set them 
         */
        public void setConfig(string[] configs)
        {
            //MaxHeight
            if (configs[0].Contains("MaxHeight")) { this.MaxHeight = int.Parse(configs[0].Split("=")[1]); }
            else { ConsoleMessages.PrintErrorMessage("DungeonGenerator:setConfig - file line:1", "MaxHeight variable was expected, but is not here!"); }
            //MinHeight
            if (configs[1].Contains("MinHeight")) { this.MinHeight = int.Parse(configs[1].Split("=")[1]); }
            else { ConsoleMessages.PrintErrorMessage("DungeonGenerator:setConfig - file line:2", "MinHeight variable was expected, but is not here!"); }
            //MaxWidth
            if (configs[2].Contains("MaxWidth")) { this.MaxWidth = int.Parse(configs[2].Split("=")[1]); }
            else { ConsoleMessages.PrintErrorMessage("DungeonGenerator:setConfig - file line:3", "MaxWidth variable was expected, but is not here!"); }
            //MinWidth 
            if (configs[3].Contains("MinWidth")) { this.MinWidth = int.Parse(configs[3].Split("=")[1]); }
            else { ConsoleMessages.PrintErrorMessage("DungeonGenerator:setConfig - file line:4", "MinWidth variable was expected, but is not here!"); }
            //SpaceMinMax
            if (configs[4].Contains("SpaceMinMax")) { this.SpaceMinMax = int.Parse(configs[4].Split("=")[1]); }
            else { ConsoleMessages.PrintErrorMessage("DungeonGenerator:setConfig - file line:5", "SpaceMinMax variable was expected, but is not here!"); }
            //SpaceMinMin
            if (configs[5].Contains("SpaceMinMin")) { this.SpaceMinMin = int.Parse(configs[5].Split("=")[1]); }
            else { ConsoleMessages.PrintErrorMessage("DungeonGenerator:setConfig - file line:6", "SpaceMinMin variable was expected, but is not here!"); }
            //SpaceMaxMax
            if (configs[6].Contains("SpaceMaxMax")) { this.SpaceMaxMax = int.Parse(configs[6].Split("=")[1]); }
            else { ConsoleMessages.PrintErrorMessage("DungeonGenerator:setConfig - file line:7", "SpaceMaxMax variable was expected, but is not here!"); }
            //SpaceMaxMin
            if (configs[7].Contains("SpaceMaxMin")) { this.SpaceMaxMin = int.Parse(configs[7].Split("=")[1]); }
            else { ConsoleMessages.PrintErrorMessage("DungeonGenerator:setConfig - file line:8", "SpaceMaxMin variable was expected, but is not here!"); }
            //SpaceSizeMinMax
            if (configs[8].Contains("SpaceSizeMinMax")) { this.SpaceSizeMinMax = int.Parse(configs[8].Split("=")[1]); }
            else { ConsoleMessages.PrintErrorMessage("DungeonGenerator:setConfig - file line:9", "SpaceSizeMinMax variable was expected, but is not here!"); }
            //SpaceSizeMinMin
            if (configs[9].Contains("SpaceSizeMinMin")) { this.SpaceSizeMinMin = int.Parse(configs[9].Split("=")[1]); }
            else { ConsoleMessages.PrintErrorMessage("DungeonGenerator:setConfig - file line:10", "SpaceSizeMinMin variable was expected, but is not here!"); }
            //SpaceSizeMaxMax
            if (configs[10].Contains("SpaceSizeMaxMax")) { this.SpaceSizeMaxMax = int.Parse(configs[10].Split("=")[1]); }
            else { ConsoleMessages.PrintErrorMessage("DungeonGenerator:setConfig - file line:11", "SpaceSizeMaxMax variable was expected, but is not here!"); }
            //SpaceSizeMaxMin
            if (configs[11].Contains("SpaceSizeMaxMin")) { this.SpaceSizeMaxMin = int.Parse(configs[11].Split("=")[1]); }
            else { ConsoleMessages.PrintErrorMessage("DungeonGenerator:setConfig - file line:12", "SpaceSizeMaxMin variable was expected, but is not here!"); }
            //MinRegSizeMin
            if (configs[12].Contains("MinRegSizeMin")) { this.MinRegSizeMin = int.Parse(configs[12].Split("=")[1]); }
            else { ConsoleMessages.PrintErrorMessage("DungeonGenerator:setConfig - file line:13", "MinRegSizeMin variable was expected, but is not here!"); }
            //MinRegSizeMax
            if (configs[13].Contains("MinRegSizeMax")) { this.MinRegSizeMax = int.Parse(configs[13].Split("=")[1]); }
            else { ConsoleMessages.PrintErrorMessage("DungeonGenerator:setConfig - file line:14", "MinRegSizeMax variable was expected, but is not here!"); }
        }

        /*
         * This method will print the current configuration values
         */
        public void printConfig()
        {
            //print MaxHeight
            Console.WriteLine("MaxHeight = " + this.MaxHeight);
            //print MinHeight
            Console.WriteLine("MinHeight = " + this.MinHeight);
            //print MaxWidth
            Console.WriteLine("MaxWidth = " + this.MaxWidth);
            //print MinWidth
            Console.WriteLine("MinWidth = " + this.MinWidth);
            //print SpaceMinMax
            Console.WriteLine("SpaceMinMax = " + this.SpaceMinMax);
            //print SpaceMinMin
            Console.WriteLine("SpaceMinMin = " + this.SpaceMinMin);
            //print SpaceMaxMax
            Console.WriteLine("SpaceMaxMax = " + this.SpaceMaxMax);
            //print SpaceMaxMin
            Console.WriteLine("SpaceMaxMin = " + this.SpaceMaxMin);
            //print SpaceSizeMinMax
            Console.WriteLine("SpaceSizeMinMax = " + this.SpaceSizeMinMax);
            //print SpaceSizeMinMin
            Console.WriteLine("SpaceSizeMinMin = " + this.SpaceSizeMinMin);
            //print SpaceSizeMaxMax
            Console.WriteLine("SpaceSizeMaxMax = " + this.SpaceSizeMaxMax);
            //print SpaceSizeMaxMin
            Console.WriteLine("SpaceSizeMaxMin = " + this.SpaceSizeMaxMin);
            //print MinRegSizeMin
            Console.WriteLine("MinRegSizeMin = " + this.MinRegSizeMin);
            //print MinRegSizeMax
            Console.WriteLine("MinRegSizeMax = " + this.MinRegSizeMax);
        }

    }
}
