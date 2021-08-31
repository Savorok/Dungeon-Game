using System;
using DungeonGame.Constants;
using DungeonGame.Dungeons.DungeonClasses;
using System.Collections.Generic;

namespace DungeonGame.Other
{
    /*
     * This class handles printing arrays to console
     */
    class PrintArrays
    {
        /*
         * this method will print 2d integer arrays
         * suports up to 100*100 arrays (0 indexed)
         */
        public static void printArray(int[,] array, string arrName, string arrDesc)
        {
            //print title
            ConsoleMessages.PrintMessage(2, arrName, arrDesc);

            for (int y = 0; y < array.GetLength(0); y++)
            {
                //print Y axis
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (y > 9){ Console.Write(" " + y.ToString() + " "); }
                else{ Console.Write(" " + y.ToString() + "  "); }
                Console.ResetColor();

                for (int x = 0; x < array.GetLength(1); x++)
                {
                    Console.Write("[" + array[y, x] + "]");
                }
                Console.WriteLine();
            }

            //print x axis
            Console.ForegroundColor = ConsoleColor.Red;       
            Console.Write("    ");

            for (int xCord = 0; xCord < array.GetLength(1); xCord++)
            {
                if (xCord > 9) { Console.Write(" " + xCord.ToString()); }
                else{ Console.Write(" " + xCord.ToString() + " ");}
            }
            Console.WriteLine();
            Console.ResetColor();
            
            
        }

        /*
        * this method will print a 2d representation of the levels boundries
        * suports up to 100*100 arrays (0 indexed)
        */
        public static void printRoomArray(int[,] array, string arrName, string arrDesc)
        {
            //print title
            ConsoleMessages.PrintMessage(2,arrName, arrDesc);

            for (int y = 0; y < array.GetLength(0); y++)
            {
                //print Y axis
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (y > 9){ Console.Write(" " + y.ToString() + " "); }
                else{ Console.Write(" " + y.ToString() + "  "); }
                Console.ResetColor();

                for (int x = 0; x < array.GetLength(1); x++)
                {
                    //print rooms and space
                    if(array[y,x] == 0){ Console.Write("   ");}else{Console.Write("[ ]"); }                  
                }
                Console.WriteLine();
            }
            //print x axis
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("    ");
            
            for (int xCord = 0; xCord < array.GetLength(1); xCord++)
            {
                if(xCord > 9){ Console.Write(" " + xCord.ToString());}
                else{ Console.Write(" " + xCord.ToString() + " "); }               
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        /*
         * this method will print a 2d representation of the levels regions
         * can print up to 15 diffrent regions + blank space region
         */
        public static void printRegionArray(int[,] array, string arrName, string arrDesc)
        {
            //print title
            ConsoleMessages.PrintMessage(2,arrName, arrDesc);
        
            //get amount of regions
            int largestRegion = 0;
            for (int y = 0; y < array.GetLength(0); y++)
            {
                for (int x = 0; x < array.GetLength(1); x++)
                {
                    int temp = array[y, x];
                    if (temp > largestRegion) { largestRegion = temp; }
                }
            }

            //print regions for up to 15 regions
            if(largestRegion <= 15)
            {
                for (int y = 0; y < array.GetLength(0); y++)
                {
                    //print Y axis
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if (y > 9)
                    {
                        Console.Write(" " + y.ToString() + " ");
                    }
                    else
                    {
                        Console.Write(" " + y.ToString() + "  ");
                    }
                    Console.ResetColor();

                    for (int x = 0; x < array.GetLength(1); x++)
                    {                      
                        //other numbers
                        Console.Write("[");
                        if (array[y, x] == 0) { Console.ForegroundColor = ConsoleColor.DarkGray; }
                        else { Console.ForegroundColor = ConsoleColours.colours[array[y, x]]; }                     
                        Console.Write(array[y, x]);
                        Console.ResetColor();
                        Console.Write("]");
                    }
                    Console.WriteLine();
                }
                //print x axis
                Console.ForegroundColor = ConsoleColor.Red;            
                Console.Write("    ");              

                for (int xCord = 0; xCord < array.GetLength(1); xCord++)
                {
                    if (xCord > 9)
                    {
                        Console.Write(" " + xCord.ToString());
                    }
                    else
                    {
                        Console.Write(" " + xCord.ToString() + " ");
                    }

                }
                Console.WriteLine();
                Console.ResetColor();
            }
            else
            {
                ConsoleMessages.PrintErrorMessage("PrintArrays:PrintRegionArray", "To many regions to print");
            }
            
        }

        /*
         * this method will print a 2d representation of the room positions
         */
        public static void printRoomPosArray(string[,] array, string arrName, string arrDesc)
        {
            //print title
            ConsoleMessages.PrintMessage(2, arrName, arrDesc);

            for (int y = 0; y < array.GetLength(0); y++)
            {
                //print Y axis
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (y > 9) { Console.Write(" " + y.ToString() + " "); }
                else { Console.Write(" " + y.ToString() + "  "); }
                Console.ResetColor();

                for (int x = 0; x < array.GetLength(1); x++)
                {
                    if(array[y,x] != null)
                    {
                        Console.Write("[" + array[y, x] + "]");
                    }
                    else
                    {
                        Console.Write("     ");
                    }                  
                }
                Console.WriteLine();
            }

            //print x axis
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("     ");

            for (int xCord = 0; xCord < array.GetLength(1); xCord++)
            {
                if (xCord > 9) { Console.Write(" " + xCord.ToString() + "  "); }
                else { Console.Write(" " + xCord.ToString() + "   "); }
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        /*
        * this method will print a 2d representation of the levels 
        * where each colour coresponds to the position of the room 
        */
        public static void printRoomWeightArray(int[,] array, string arrName, string arrDesc)
        {
            //print title
            ConsoleMessages.PrintMessage(2, arrName, arrDesc);

            for (int y = 0; y < array.GetLength(0); y++)
            {
                //print Y axis
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (y > 9)
                {
                    Console.Write(" " + y.ToString() + " ");
                }
                else
                {
                    Console.Write(" " + y.ToString() + "  ");
                }
                Console.ResetColor();

                for (int x = 0; x < array.GetLength(1); x++)
                {
                    //other numbers                           
                    if (array[y, x] == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("    ");
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColours.colours[array[y, x]];
                        if (array[y, x] <= 9)
                        {
                            Console.Write("0" + array[y, x]);
                        }
                        else
                        {
                            Console.Write(array[y, x]);
                        }
                        Console.ResetColor();
                        Console.Write("]");
                    }
                    
                }
                Console.WriteLine();
            }
            //print x axis
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("    ");

            for (int xCord = 0; xCord < array.GetLength(1); xCord++)
            {
                if (xCord > 9)
                {
                    Console.Write(" " + xCord.ToString() + " ");
                }
                else
                {
                    Console.Write(" " + xCord.ToString() + "  ");
                }

            }
            Console.WriteLine();
            Console.ResetColor();
            
           

        }

        /*
         * this method will print the rooms along with their exit directions
         */
        public static void printExitsArray(Room[,] array, string arrName, string arrDesc)
        {
            //print title
            ConsoleMessages.PrintMessage(2, arrName, arrDesc);

            for (int y = 0; y < array.GetLength(0); y++)
            {
                //print Y axis
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (y > 9) { Console.Write(" " + y.ToString() + " "); }
                else { Console.Write(" " + y.ToString() + "  "); }
                Console.ResetColor();

                for (int x = 0; x < array.GetLength(1); x++)
                {
                    if(array[y,x] == null)
                    {
                        Console.Write("[    ]");
                    }
                    else
                    {
                        string outputStr = "";
                        List<Direction> exits = new List<Direction>();
                        exits = array[y, x].Exits;

                        if (exits.Contains(Direction.N)) { outputStr += "N"; }
                        if (exits.Contains(Direction.S)) { outputStr += "S"; }
                        if (exits.Contains(Direction.E)) { outputStr += "E"; }
                        if (exits.Contains(Direction.W)) { outputStr += "W"; }

                        if (outputStr.Length == 0) { outputStr += "    "; }
                        if (outputStr.Length == 1) { outputStr += "   "; }
                        if (outputStr.Length == 2) { outputStr += "  "; }
                        if (outputStr.Length == 3) { outputStr += " "; }


                        Console.Write("[" + outputStr + "]");
                    }                  
                }
                Console.WriteLine();
            }

            //print x axis
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("    ");

            for (int xCord = 0; xCord < array.GetLength(1); xCord++)
            {
                if (xCord > 9) { Console.Write(" " + xCord.ToString() + "   ");  }
                else { Console.Write(" " + xCord.ToString() + "    "); }
            }
            Console.WriteLine();
            Console.ResetColor();


        }
    }
}
