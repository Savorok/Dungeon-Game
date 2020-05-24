using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;

/// <summary>
/// A scalable approach to generating levels where all of the rooms are conected
///                 
/// #~~~~~~~~~~~~~~~ Made by ~~~~~~~~~~~~~~~~#
/// ~~~~~~~~~~ Oisin cassidy Dawson ~~~~~~~~~~
/// #~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
/// 
/// #~~~~~~~~~~~~~~~~~~~~~~~~ Visualiser ~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
/// ~~~~~~~~~~~~~~~~~~~~~~~~ By Owen Quinn ~~~~~~~~~~~~~~~~~~~~~~~~~~~
/// #~ https://editor.p5js.org/oisincassidydawson179/full/gzvS9YcIs ~#
/// 
/// Created: 16/05/2020
/// Last edited: 21/05/2020
/// 
/// #################### Known Bugs ####################
/// # A tunnel can be created between the same region  #
/// #                                                  #
/// # levels can still have loops as there is a chance #
/// # that not all the regions will connect up         #
/// #                                                  #
/// # tunnels are not created in the correct position  #
/// # and wont allow multiple tunnels to the same cell #
/// #                                                  #
/// # Arrays are fucked                                #
/// ####################################################
/// 
/// #################### Important ####################
/// # Arrays have been made with x and y switched the #
/// # whole time i need to go back and fix this, this #
/// # message will be removed once done               #
/// ###################################################
/// 
/// </summary>

namespace LevelGen
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            do
            {
                Console.WriteLine("//////////////////// Level Generator ////////////////////\n");
                Console.WriteLine("1 - Generate a level");
                Console.WriteLine("2 - Generate level and connectors recursivley");
                Console.WriteLine("3 - Generate a level from a level code");
                Console.WriteLine("4 - Distribution and probability calculators");
                Console.WriteLine("5 - Exit application");
                Console.Write("\n\nEnter Choice:");

                int input = 0;
                int.TryParse(Console.ReadLine(), out input);
                Console.WriteLine();

                switch (input)
                {
                    //generate level
                    case 1: levelGen();
                        break;

                    //generate level recursivley
                    case 2: //wip
                        break;

                    //generate level from code
                    case 3: //tba
                        break;

                    //calc distributions
                    case 4:
                        probMenu();
                        break;

                    //exit
                    case 5: exit = true;
                        break;
                }
            }
            while (!exit);

        }

        #region//////////Level Generation//////////

        #region~~~~~~~~~~Generation stuff~~~~~~~~~~

        //levelGen
        public static void levelGen()
        {
            //init variables
            int x, y;

            //get no of rows
            Console.Write("Width of array(x): ");
            x = int.Parse(Console.ReadLine());

            //get no of col
            Console.Write("Height of array(y): ");
            y = int.Parse(Console.ReadLine());

            //generate arrays and calculate generation time
            var GenerationTime = Stopwatch.StartNew();
            string[,] posArray = generatePositionalArray(x, y);
            string[,] dirArray1 = getDirArray(posArray);
            string[,] dirArray2 = ensrueTwoWayConectivity(posArray, dirArray1);
            int[,] regionArray = getRegionArray(dirArray2);
            var connectedRegionOutput = connectRegions(dirArray2, posArray, regionArray, 1, 1);
            string[,] connectRegionsArray = connectedRegionOutput.connectionArray;
            string[,] dirArray3 = connectedRegionOutput.dirArray;                                                                                                                               
            GenerationTime.Stop();         

            //print arrays
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            
            //visualize array        
            visualiseArray(dirArray2);

            //array componants
            Console.WriteLine("\nPossition array:\n");
            printArray(posArray);
            Console.WriteLine("\nDirection array pass 1:\n");
            printArray(dirArray1);
            Console.WriteLine("\nDirection array pass 2:\n");           
            printArray(dirArray2);
            Console.WriteLine("\nRegion Array:\n");
            printArray(regionArray);
            Console.WriteLine();
            printRegionArray(regionArray);
            Console.WriteLine("\nRegion connection array:\n");
            printArray(connectRegionsArray);
            Console.WriteLine("\nDirection array pass 3\n");
            printArray(dirArray3);
           
            //array info
            Console.WriteLine("\nArray Size: [{0},{1}]",x,y);
            Console.WriteLine("Number of rooms: " + x * y);
            Console.WriteLine("Number of regions: " + convertRegionArrayToList(regionArray).Count);
            printLevelCode(dirArray3);
                      
            //print time taken to generate
            Console.WriteLine("\nTime taken to generate: {0}ms", GenerationTime.ElapsedMilliseconds);
            Console.Write("\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n\n");
        }

        //generate an array with labled cells
        public static string[,] generatePositionalArray(int Width, int Height)
        {
            string[,] posArray = new string[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {                  
                    posArray[x, y] = getCellPos(Width, Height, x, y);
                }
            }         
            return posArray;
        }

        //convert from type to dir
        public static string[,] getDirArray(string[,] posArray)
        {
            int width = posArray.GetLength(0);
            int height = posArray.GetLength(1);

            string[,] dirArray = new string[width,height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    dirArray[x, y] = assignPathDir(posArray[x,y]);
                }
            }
            return dirArray;
        }

        //print string array 
        public static void printArray(string[,] array)
        {
            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    Console.Write("[" + array[y, x] + "]");
                }
                Console.WriteLine();
            }
        }
       
        //print integer array
        public static void printArray(int[,] array)
        {
            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    Console.Write("[" + array[y, x] + "]");
                }
                Console.WriteLine();
            }
        }
       
        //detect cell position
        public static string getCellPos(int Width,int Height, int x, int y)
        {
            //////////check to see if it 1*x or x*1//////////
            //1*x
            if(Height == 1)
            {
                //x is 0 and y is 0 (left most piece)
                if (x == 0 && y == 0) { return "LP"; }
                //x is max and y is 0 (right peice)
                else if (x == Width-1) { return "RP"; }
                //middle peices left over
                else { return "HP"; }
            }
            //x*1
            if(Width == 1)
            {
                //row is 0 and col is 0 (top most piece)
                if (x == 0 && y == 0) { return "TP"; }
                //row is 0 and col is max (bottom peice)
                else if (x == 0 && y == Height-1) { return "BP"; }
                //middle peices left over
                else { return "VP"; }
            }

            /*//////////Corner//////////
             * is a corner if
             * col is 0 and row is 0 (top left)
             * col is 0 and row is max (bottom left)
             * col is max and row is 0 (top right)
             * col is max and row is max (bottom right)
             */

            //top left
            if (x == 0 && y == 0) { return "TL"; }
            //bottom left
            else if (x == 0 && y == Height-1) { return "BL"; }
            //top right
            else if (x == Width-1 && y == 0) { return "TR"; }
            //bottom right
            else if (x == Width-1 && y == Height-1) { return "BR"; }

            /*//////////Side//////////
             * is side if not a corner
             * //////////top side//////////
             *   if row is 0
             * //////////bottom side//////////
             *   if row is max
             * //////////right side//////////
             *   if col is max
             * //////////left side//////////
             *   if col is 0
             */

            //top side
            else if (y == 0) { return "TS"; }
            //bottom side
            else if (y == Height-1) { return "BS"; }
            //right side
            else if (x == Width-1) { return "RS"; }
            //left side
            else if (x == 0) { return "LS"; }

            //////////centre piece//////////
            else { return "CP"; }
        }
      
        //assign directions to cells
        public static string assignPathDir(string typeOfCell)
        {
            /*//////////Types of cell and thier possible exit dir//////////
             *            
             * //////////Directions//////////
             * North : N
             * South : S
             * East : E
             * West : W
             *                   
             * //////////corner peices / end pieces//////////
             * Top left = "TL" : S E
             * Top Right = "TR" : S W
             * Bottom Left = "BL" : N E
             * Bottom Right = "BR" : N W
             * Top piece = "TP" : S
             * Bottom piece = "BP" : N
             * Right piece = "RP" : W
             * Left Piece = "LP" : E
             * 
             * //////////Side Pieces//////////
             * Top Side = "TS" : S E W
             * Bottom Side = "BS" : N E W
             * Right Side = "RS" : N S W
             * Left Side = "LS" : N S E
             * 
             * //////////Centre Pieces//////////
             * Center piece = "CP" = N S E W
             * Vertical piece = "VP" = N S
             * Horizontal piece = "HP" = E W
             * 
             * ~~~~~~~~~~Important~~~~~~~~~~
             * Because of the way the generation on the
             * side pieces works to ensure all rooms are
             * accessable the backup assigned direcions 
             * for them must contain a connection to a 
             * centre piece
             * 
             */

            ////////////////////Assign exit directions to cells////////////////////

            //chances
            Random r = new Random();
            double twoExitChance = 0.5;
            double threeExitChance = 0.333;
            double fourExitChance = 0.25;

            //////////corner pieces//////////
            //top left
            if (typeOfCell == "TL")
            {
                string dirs = "";
                string[] options = new string[] { "S", "E" }; 

                if (randomChance(twoExitChance)) { dirs += "S"; }
                if (randomChance(twoExitChance)) { dirs += "E"; }
                if (dirs == "") { int rNum = r.Next(2); dirs += options[rNum]; }                        
                return dirs;
            }
            //top right
            if (typeOfCell == "TR")
            {
                string dirs = "";
                string[] options = new string[] { "S", "W" };

                if (randomChance(twoExitChance)) { dirs += "S"; }
                if (randomChance(twoExitChance)) { dirs += "W"; }
                if (dirs == "") { int rNum = r.Next(2); dirs += options[rNum]; }
                return dirs;
            }
            //bottom left
            if (typeOfCell == "BL")
            {
                string dirs = "";
                string[] options = new string[] { "N", "E" };

                if (randomChance(twoExitChance)) { dirs += "N"; }
                if (randomChance(twoExitChance)) { dirs += "E"; }
                if (dirs == "") { int rNum = r.Next(2); dirs += options[rNum]; }
                return dirs;
            }
            //bottom right
            if (typeOfCell == "BR")
            {
                string dirs = "";
                string[] options = new string[] { "N", "W" };

                if (randomChance(twoExitChance)) { dirs += "N"; }
                if (randomChance(twoExitChance)) { dirs += "W"; }
                if (dirs == "") { int rNum = r.Next(2); dirs += options[rNum]; }
                return dirs;
            }

            //////////end pieces//////////
            //top piece
            if (typeOfCell == "TP") { return "S"; }
            //bottom piece
            if (typeOfCell == "BP") { return "N"; }
            //right piece
            if (typeOfCell == "RP") { return "W"; }
            //left piece
            if (typeOfCell == "LP") { return "E"; }

            //////////Side pieces//////////
            //top side
            if (typeOfCell == "TS")
            {
                string dirs = "";
                string[] options = new string[] { "S", "SE", "SW" };

                if (randomChance(threeExitChance)) { dirs += "S"; }
                if (randomChance(threeExitChance)) { dirs += "E"; }
                if (randomChance(threeExitChance)) { dirs += "W"; }

                if (dirs == "") { int rNum = r.Next(3); dirs += options[rNum]; }
                return dirs;
            }
            //bottom side
            if (typeOfCell == "BS")
            {
                string dirs = "";
                string[] options = new string[] { "N", "NE", "NW" };

                if (randomChance(threeExitChance)) { dirs += "N"; }
                if (randomChance(threeExitChance)) { dirs += "E"; }
                if (randomChance(threeExitChance)) { dirs += "W"; }

                if (dirs == "") { int rNum = r.Next(3); dirs += options[rNum]; }
                return dirs;
            }
            //right side
            if (typeOfCell == "RS")
            {
                string dirs = "";
                string[] options = new string[] { "NW", "SW", "W" };

                if (randomChance(threeExitChance)) { dirs += "N"; }
                if (randomChance(threeExitChance)) { dirs += "S"; }
                if (randomChance(threeExitChance)) { dirs += "W"; }

                if (dirs == "") { int rNum = r.Next(3); dirs += options[rNum]; }
                return dirs;
            }
            //left side
            if (typeOfCell == "LS")
            {
                string dirs = "";
                string[] options = new string[] { "NE", "SE", "E" };

                if (randomChance(threeExitChance)) { dirs += "N"; }
                if (randomChance(threeExitChance)) { dirs += "S"; }
                if (randomChance(threeExitChance)) { dirs += "E"; }

                if (dirs == "") { int rNum = r.Next(3); dirs += options[rNum]; }
                return dirs;
            }

            //////////centre pieces//////////
            //centre piece
            if (typeOfCell == "CP")
            {
                string dirs = "";
                string[] options = new string[] { "N", "S", "E", "W" };

                if (randomChance(fourExitChance)) { dirs += "N"; }
                if (randomChance(fourExitChance)) { dirs += "S"; }
                if (randomChance(fourExitChance)) { dirs += "E"; }
                if (randomChance(fourExitChance)) { dirs += "W"; }

                if (dirs == "") { int rNum = r.Next(4); dirs += options[rNum]; }
                return dirs;
            }
            //vertical piece
            if (typeOfCell == "VP") { return "NS"; }
            //horizontal piece
            if (typeOfCell == "HP") { return "EW"; }

            return "Err";
        }

        //will return true if the generated random number meets the chance given
        public static bool randomChance(double chance)
        {
            Random random = new Random();
            if(random.NextDouble() <= chance)
            {
                return true;
            }
            return false;
        }

        //get and print the level code
        public static void printLevelCode(string[,] array)

        {
            int xi = array.GetLength(0);
            int yi = array.GetLength(1);
            string xs = "", ys = "";

            //get the array sizes and convert to code format
            if (1 <= xi && xi <= 9) { xs = "0" + xi.ToString(); }
            else { xs = xi.ToString(); }
            if (1 <= yi && yi <= 9) { ys = "0" + yi.ToString(); }
            else { ys = yi.ToString(); }

            //print raw
            Console.Write("Raw array: ");
            string rawArray = "";
            foreach (string dir in array)
            {
                rawArray += dir + ",";
            }
            rawArray += (xs + "," + ys);
            Console.WriteLine(rawArray);
            //Clipboard.SetText(rawArray); Lags somtimes

            //print compressed code
            string compLevelCode = compressLevelCode(array);
            Console.WriteLine("Compressed code: " + compLevelCode);

                        
            //print compressed after decompression
            Console.Write("Uncompressed compressed code: ");
            string rawArray3 = "";
            string[,] uncompressed = decompressLevelCode(compLevelCode);

            int xic = uncompressed.GetLength(0);
            int yic = uncompressed.GetLength(1);
            string xsc = "", ysc = "";

            //get the array sizes and convert to code format
            if (1 <= xic && xic <= 9) { xsc = "0" + xic.ToString(); }
            else { xsc = xic.ToString(); }
            if (1 <= yic && yic <= 9) { ysc = "0" + yic.ToString(); }
            else { ysc = yic.ToString(); }

            foreach (string dir in uncompressed)
            {
                rawArray3 += dir + ",";
            }
            rawArray3 += (xsc + "," + ysc);
            Console.WriteLine(rawArray3);
            
        }

        //second pass to assure room conectivity
        public static string[,] ensrueTwoWayConectivity(string[,] posArray, string[,] dirArray)
        {
            //get the array size
            int width = posArray.GetLength(0);
            int height = posArray.GetLength(1);

            //current tile position and direction 
            string pos;
            string dir;

            string[,] newDirArray = new string[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //get the current tile position
                    pos = posArray[x, y];
                    dir = dirArray[x, y];

                    //the dirrections to add to the current pos
                    string append = "";

                    //for every direction 
                    for (int i = 0; i < dir.Length; i++)
                    {
                        switch (pos)
                        {
                            //left piece
                            case "LP":
                                if (checkRight(x,y)) { append += "E"; }
                                break;

                            //right piece
                            case "RP":
                                if (checkLeft(x, y)) { append += "W"; }
                                break;

                            //horizontal piece
                            case "HP":
                                if (checkUp(x, y)) { append += "N"; }
                                if (checkDown(x, y)) { append += "S"; }
                                break;

                            //top piece
                            case "TP":
                                if (checkDown(x, y)) { append += "S"; }
                                break;

                            //bottom piece
                            case "BP":
                                if (checkUp(x, y)) { append += "N"; }
                                break;

                            //veritcal piece
                            case "VP":
                                if (checkUp(x, y)) { append += "N"; }
                                if (checkDown(x, y)) { append += "S"; }
                                break;

                            //top left piece
                            case "TL":
                                if (checkRight(x, y)) { append += "E"; }
                                if (checkDown(x, y)) { append += "S"; }
                                break;

                            //bottom left piece
                            case "BL":
                                if (checkUp(x, y)) { append += "N"; }
                                if (checkRight(x, y)) { append += "E"; }
                                break;

                            //top right piece
                            case "TR":
                                if (checkLeft(x, y)) { append += "W"; }
                                if (checkDown(x, y)) { append += "S"; }
                                break;
                            
                            //bottom right piece
                            case "BR":
                                if (checkUp(x, y)) { append += "N"; }
                                if (checkLeft(x, y)) { append += "W"; }
                                break;
                           
                            //top side piece
                            case "TS":
                                if (checkRight(x, y)) { append += "E"; }
                                if (checkLeft(x, y)) { append += "W"; }
                                if (checkDown(x, y)) { append += "S"; }
                                break;

                            //bottom side piece
                            case "BS":
                                if (checkRight(x, y)) { append += "E"; }
                                if (checkLeft(x, y)) { append += "W"; }
                                if (checkUp(x, y)) { append += "N"; }
                                break;

                            //right side piece
                            case "RS":
                                if (checkUp(x, y)) { append += "N"; }
                                if (checkDown(x, y)) { append += "S"; }
                                if (checkLeft(x, y)) { append += "W"; }
                                break;

                            //left side piece
                            case "LS":
                                if (checkUp(x, y)) { append += "N"; }
                                if (checkDown(x, y)) { append += "S"; }
                                if (checkRight(x, y)) { append += "E"; }
                                break;

                            //centre piece
                            case "CP":
                                if (checkLeft(x, y)) { append += "W"; }
                                if (checkUp(x, y)) { append += "N"; }
                                if (checkDown(x, y)) { append += "S"; }
                                if (checkRight(x, y)) { append += "E"; }
                                break;

                            default: Console.WriteLine("Unable to add new dir");
                                break;
                        }
                    }

                    //remove the duplicates from the new dir
                    string newDir = removeDuplicates(dirArray[x, y] + append);

                    //add the directions to the dir array
                    newDirArray[x, y] = newDir;                  
                }
            }

            return newDirArray;

            #region//////////Methods for detection and remove duplicates//////////

            //check the cell to the right of the current one
            bool checkRight(int currentX,int currentY)
            {
                if(dirArray[currentX + 1,currentY].Contains('W'))
                {
                    return true;
                }
                return false;
            }

            //check the cell to the left of the current one
            bool checkLeft(int currentX, int currentY)
            {
                if (dirArray[currentX - 1, currentY].Contains('E'))
                {
                    return true;
                }
                return false;
            }

            //check the cell above the current one
            bool checkUp(int currentX, int currentY)
            {
                if (dirArray[currentX, currentY - 1].Contains('S'))
                {
                    return true;
                }
                return false;
            }

            //check the cell bellow the current one
            bool checkDown(int currentX, int currentY)
            {
                if (dirArray[currentX, currentY + 1].Contains('N'))
                {
                    return true;
                }
                return false;
            }

            //remove duplicates from a dir
            string removeDuplicates(string cellDir)
            {
                return new string(cellDir.ToCharArray().Distinct().ToArray());
            }

            //remove 
            #endregion
        }

        //get the regions in each array
        public static int[,] getRegionArray(string[,] dirArray)
        {
            //get width and height
            int width = dirArray.GetLength(0);
            int height = dirArray.GetLength(1);

            //make an array the same size as input arrays
            int[,] regionArray = new int[width, height];

            //set all values of region array to 0
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    regionArray[x, y] = 0;
                }
            }

            doRecursion();
                                 
            return regionArray;

            #region//////////recursive step method and check for zero method//////////
            //recursive step for regioning
            void recursiveStep(int x, int y, int regionNumber, string[,] directionArray)
            {
                //set the cell to be the region number or break
                if (regionArray[x, y] == regionNumber) { return; }
                else { regionArray[x, y] = regionNumber; }
            
                //get the exit paths from cell
                string directions = directionArray[x, y];

                

                //check north
                if (directions.Contains("N") || directions.Contains("!")) { recursiveStep(x, y - 1, regionNumber, directionArray); }

                //check south
                if (directions.Contains("S") || directions.Contains("$")) { recursiveStep(x, y + 1, regionNumber, directionArray); }

                //check east
                if (directions.Contains("E") || directions.Contains("%")) { recursiveStep(x + 1, y, regionNumber, directionArray); }

                //check west
                if (directions.Contains("W") || directions.Contains("£")) { recursiveStep(x - 1, y, regionNumber, directionArray); }
            }

            //do the recursion until all reagions are marked
            void doRecursion()
            {
                //init variables
                int region = 0;
                bool zeroInArray = true;
                do
                {
                    region++;
                                    
                    //check for a zero in the array
                    (int xPos, int yPos, bool noZero) getZeroPosition()
                    {
                        for (int x = 0; x < regionArray.GetLength(0); x++)
                        {
                            for (int y = 0; y < regionArray.GetLength(1); y++)
                            {
                                if (regionArray[x, y] == 0) { return (x, y, false); }
                            }
                        }
                        return (0, 0, true);
                    }

                    //if there is no zero exit method
                    if (getZeroPosition().noZero == true) { zeroInArray = false; return;}

                    //call recursive step
                    var getZeroPositionMehtod = getZeroPosition();
                    recursiveStep(getZeroPositionMehtod.xPos, getZeroPositionMehtod.yPos, region, dirArray);
                }
                while (zeroInArray);
                return;
            }
            #endregion
        }
       
        //connect regions 
        public static (string[,] dirArray, string[,] connectionArray, int timesToGo) connectRegions(string[,] dirArray, string[,] posArray, int[,] regionArray, double chanceOfSecretPassages, int tunnelsPerConnectionMax)
        {
            //get width and height
            int width = dirArray.GetLength(0);
            int height = dirArray.GetLength(1);

            //create a new dir array and make it the same as the old one
            string[,] outDirArray = new string[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    outDirArray[x, y] = dirArray[x,y];
                }
            }

            //create a blank array and assign 0 values
            string[,] connectionArray = new string[width,height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    connectionArray[x, y] = "-";
                }
            }

            //get number of regions
            List<int> regionNumbers = new List<int>();
            regionNumbers = convertRegionArrayToList(regionArray);

            //get array of number of tunnels per region connection
            int[] numberOfTunnelsPerRegion = new int[regionNumbers.Count];
            Random r = new Random();
            for (int i = 0; i < regionNumbers.Count; i++)
            {
                numberOfTunnelsPerRegion[i] = r.Next(1, tunnelsPerConnectionMax + 1);
            }

            //loop through regions and connect them
            for (int j = 0; j < regionNumbers.Count; j++)
            {
                int timesToRun = numberOfTunnelsPerRegion[j]-1;
                int timesRan = 0;
                do
                {
                    //get the needed variables
                    var tempHold = findAdjacentRegionTile(regionNumbers[j]);
                    int xPos = tempHold.xPos;
                    int yPos = tempHold.yPos;
                    char dir = tempHold.dir;

                    //should this be a secret passage
                    bool secretPassage = randomChance(chanceOfSecretPassages);

                    if(secretPassage)
                    {
                        switch(dir)
                        {
                            //secret passage to right
                            case 'E':
                                connectionArray[xPos, yPos] = "%";
                                connectionArray[xPos + 1 , yPos] = "£";
                                outDirArray[xPos, yPos] = dirArray[xPos, yPos] + "%";
                                outDirArray[xPos + 1, yPos] = dirArray[xPos, yPos] + "£";
                                break;

                            //secret passage to left
                            case 'W':
                                connectionArray[xPos, yPos] = "£";
                                connectionArray[xPos - 1, yPos] = "%";
                                outDirArray[xPos, yPos] = dirArray[xPos, yPos] + "£";
                                outDirArray[xPos - 1, yPos] = dirArray[xPos, yPos] + "%";
                                break;

                            //secret passage up
                            case 'N':
                                connectionArray[xPos, yPos] = "!";
                                connectionArray[xPos, yPos - 1] = "$";
                                outDirArray[xPos, yPos] = dirArray[xPos, yPos] + "!";
                                outDirArray[xPos, yPos - 1] = dirArray[xPos, yPos] + "$";
                                break;

                            //secret passage down
                            case 'S':
                                connectionArray[xPos, yPos] = "$";
                                connectionArray[xPos, yPos + 1] = "!";
                                outDirArray[xPos, yPos] = dirArray[xPos, yPos] + "$";
                                outDirArray[xPos, yPos + 1] = dirArray[xPos, yPos] + "!";
                                break;
                        }                                                                                                             
                    }
                    else
                    {
                        switch (dir)
                        {
                            //passage to right
                            case 'E':
                                connectionArray[xPos, yPos] = "E";
                                connectionArray[xPos + 1, yPos] = "W";
                                outDirArray[xPos, yPos] = dirArray[xPos, yPos] + "E";
                                outDirArray[xPos + 1, yPos] = dirArray[xPos, yPos] + "W";
                                break;

                            //passage to left
                            case 'W':
                                connectionArray[xPos, yPos] = "W";
                                connectionArray[xPos - 1, yPos] = "E";
                                outDirArray[xPos, yPos] = dirArray[xPos, yPos] + "W";
                                outDirArray[xPos - 1, yPos] = dirArray[xPos, yPos] + "E";
                                break;

                            //passage up
                            case 'N':
                                connectionArray[xPos, yPos] = "N";
                                connectionArray[xPos, yPos - 1] = "S";
                                outDirArray[xPos, yPos] = dirArray[xPos, yPos] + "N";
                                outDirArray[xPos, yPos - 1] = dirArray[xPos, yPos] + "S";
                                break;

                            //passage down
                            case 'S':
                                connectionArray[xPos, yPos] = "S";
                                connectionArray[xPos, yPos + 1] = "N";
                                outDirArray[xPos, yPos] = dirArray[xPos, yPos] + "S";
                                outDirArray[xPos, yPos + 1] = dirArray[xPos, yPos] + "N";
                                break;
                        }
                    }
                }
                while (timesRan < timesToRun);
            }

            //loop through dir array and remove any doubles
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    outDirArray[x, y] = new string(outDirArray[x, y].ToCharArray().Distinct().ToArray());
                }
            }
           
            return (outDirArray, connectionArray, regionNumbers.Count);

            #region//////////find adjacent tiles method - check allowed dir method - print array method//////////

            //find adjecent region tiles
            (int xPos, int yPos, char dir) findAdjacentRegionTile(int region)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        string cellPos = posArray[x, y];
                        bool right = checkDirAllowd(cellPos).right;
                        bool left = checkDirAllowd(cellPos).left;
                        bool up = checkDirAllowd(cellPos).up;
                        bool down = checkDirAllowd(cellPos).down;

                        //check right
                        if (right && (regionArray[x, y] == region) && (regionArray[x + 1, y] != region))
                        {                          
                            return (x, y, 'E');
                        }
                        //check left
                        if (left && (regionArray[x, y] == region) && (regionArray[x - 1, y] != region))
                        {
                            return (x, y, 'W');
                        }
                        //check up
                        if (up && (regionArray[x, y] == region) && (regionArray[x, y - 1] != region))
                        {
                            return (x, y, 'N');
                        }
                        //check down
                        if (down && (regionArray[x, y] == region) && (regionArray[x, y + 1] != region))
                        {
                            return (x, y, 'S');
                        }
                    }
                }
                return (0, 0, 'X');
            }

            //see which directions the current cell can check in 
            (bool right, bool left, bool up , bool down) checkDirAllowd(string pos)
            {
                if (pos == "LP") { return (true, false, false, false); }
                else if (pos == "RP") { return (false, true, false, false); }
                else if (pos == "HP") { return (true, true, false, false); }
                else if (pos == "TP") { return (false, false, false, true); }
                else if (pos == "BP") { return (false, false, true, false); }
                else if (pos == "VP") { return (false, false, true, true); }
                else if (pos == "TL") { return (true, false, false, true); }
                else if (pos == "BL") { return (true, false, true, false); }
                else if (pos == "TR") { return (false, true, false, true); }
                else if (pos == "BR") { return (false, true, true, false); }
                else if (pos == "TS") { return (true, true, false, true); }
                else if (pos == "BS") { return (true, true, true, false); }
                else if (pos == "RS") { return (false, true, true, true); }
                else if (pos == "LS") { return (true, false, true, true); }
                else if (pos == "CP") { return (true, true, true, true); }
                else { Console.WriteLine("Err"); return (false, false, false, false); }
            }

            //print the region array
            void printArray(int[,] regionArrayToPrint, int passNo)
            {
                Console.Write("Region array pass {0}:",passNo);
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Console.WriteLine("[{0}]", regionArrayToPrint[x,y]);
                    }
                }
                Console.WriteLine();
            }
           
           
            #endregion
        }

        //check for full conectivity
        public static bool fullyConnected(int[,] regionArray)
        {
            int width = regionArray.GetLength(0);
            int height = regionArray.GetLength(1);
       
            List<int> outputList = new List<int>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //check if first addition
                    if (outputList.Count == 0) { outputList.Add(regionArray[x, y]); }
                    //add if not in list
                    if (outputList.Contains(regionArray[x, y])) { }
                    else { outputList.Add(regionArray[x, y]); }
                }
            }

            if (outputList.Count == 1) { return true; }
            else { return false; }
            
        }

        //print region array to console in colour if 16 or less regions
        public static void printRegionArray(int[,] regionArray)
        {
            //convert to a list of unique regions
            List<int> listOfRegionNumbers = convertRegionArrayToList(regionArray);

            //array of colours
            ConsoleColor[] colours = new ConsoleColor[]
            {
                ConsoleColor.Black,
                ConsoleColor.Red, 
                ConsoleColor.Green, 
                ConsoleColor.Blue,
                ConsoleColor.Yellow,
                ConsoleColor.White,
                ConsoleColor.Magenta,
                ConsoleColor.Cyan,
                ConsoleColor.Gray,
                ConsoleColor.DarkRed,
                ConsoleColor.DarkGreen,
                ConsoleColor.DarkBlue,
                ConsoleColor.DarkYellow,
                ConsoleColor.DarkMagenta,
                ConsoleColor.DarkMagenta,
                ConsoleColor.DarkCyan,
                ConsoleColor.DarkGray
            }; 
            
            //print the array in colour if less than 16
            if(listOfRegionNumbers.Count <= 16 && listOfRegionNumbers.Count > 1)
            {                
                    for (int x = 0; x < regionArray.GetLength(0); x++)
                    {
                        for (int y = 0; y < regionArray.GetLength(1); y++)
                        {
                            Console.Write("[");
                            Console.ForegroundColor = colours[regionArray[y,x]];
                            Console.Write(regionArray[y,x]);
                            Console.ResetColor();
                            Console.Write("]");
                        }
                        Console.WriteLine();
                    }
            }
            else
            {
                printArray(regionArray);
            }
        }

        //convert region array to a list
        public static List<int> convertRegionArrayToList(int[,] regionArrayToConvert)
        {
            //get width and height
            int width = regionArrayToConvert.GetLength(0);
            int height = regionArrayToConvert.GetLength(1);

            List<int> outputList = new List<int>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //check if first addition
                    if (outputList.Count == 0) { outputList.Add(regionArrayToConvert[x, y]); }
                    //add if not in list
                    if (outputList.Contains(regionArrayToConvert[x, y])) { }
                    else { outputList.Add(regionArrayToConvert[x, y]); }
                }
            }

            return outputList;
        }

        //visulise array
        public static void visualiseArray(string[,] array)
        {
            //get the length and width
            int width = array.GetLength(0);
            int height = array.GetLength(1);

            //init the list of arrays
            List<int[]> vConnectors = new List<int[]>();
            List<int[]> hConnectors = new List<int[]>();

            //get the vertical arrays
            for (int y = 0; y < height - 1; y++)
            {
                int[] tempArray = new int[width];
                for (int x = 0; x < width; x++)
                {
                    if (array[x, y].Contains("S")) { tempArray[x] = 1; }
                    else if(array[x, y].Contains("$")) { tempArray[x] = 2; }
                    else { tempArray[x] = 0; }
                }

                vConnectors.Add(tempArray.Clone() as int[]);
            }

            //get the horizontal arrays
            for (int y = 0; y < height; y++)
            {
                int[] tempArray = new int[width - 1];
                for (int x = 0; x < width-1; x++)
                {
                    if (array[x, y].Contains("E")) { tempArray[x] = 1; }
                    else if (array[x, y].Contains("%")) { tempArray[x] = 2; }
                    else { tempArray[x] = 0; }
                }

                hConnectors.Add(tempArray.Clone() as int[]);
            }

            //init strings for drawing boxes
            string topAndBottomBox = "[][][]  ";
            string middle = "[]  []  ";
            string middelCon = "[]  []##";
            string bottomCon = "  ##    ";
            string bottomGap = "        ";

            //draw for as many times as needed
            for (int i = 0; i < hConnectors.Count; i++)
            {
                drawHBoxes(hConnectors[i]);
                if(i < hConnectors.Count - 1) { drawVConnectors(vConnectors[i]); }
            }

            #region//////////drawBoxes//////////

            //draw horizontal boxes
            void drawHBoxes(int[] inArray)
            {           
                //draw top
                for (int a = 0; a < inArray.Length; a++)
                {
                    Console.Write(topAndBottomBox);
                }
                Console.Write(topAndBottomBox);
                Console.WriteLine();
                //draw middle
                for (int b = 0; b < inArray.Length; b++)
                {
                    if (inArray[b] == 1) { Console.Write(middelCon); }
                    else { Console.Write(middle); }
                }
                Console.Write(middle);
                Console.WriteLine();
                //draw bottom
                for (int b = 0; b < inArray.Length; b++)
                {
                    Console.Write(topAndBottomBox);
                }
                Console.Write(topAndBottomBox);
                Console.WriteLine();
            }

            //draw vertical connectors
            void drawVConnectors(int[] inArray)
            {
                for (int i = 0; i < inArray.Length; i++)
                {
                    if (inArray[i] == 1) { Console.Write(bottomCon); }
                    else { Console.Write(bottomGap); }
                }
                Console.WriteLine();
            }

            #endregion

        }

        #endregion
   
        #region~~~~~~~~~~Probability stuff~~~~~~~~~~

        //probability menu
        public static void probMenu()
        {
            bool exit = false;
            do
            {
                Console.WriteLine("//////////////////// Probability and distriution ////////////////////\n");
                Console.WriteLine("1 - Show direction distribution");
                Console.WriteLine("2 - Show compressed code distribution");
                Console.WriteLine("3 - Return to main menu");
                Console.Write("\n\nEnter Choice:");

                int input = 0;
                int.TryParse(Console.ReadLine(), out input);
                Console.WriteLine();

                switch (input)
                {
                    //get dir distribution
                    case 1:
                        callCalcDirDistribution();
                        break;

                    //get code distribution
                    case 2:
                        callCalcCodeDistributions();
                        break;

                    //exit
                    case 3:
                        exit = true;
                        break;
                }
            }
            while (!exit);
        }

        //call prob calc
        public static void callCalcDirDistribution()
        {
            Console.Write("How many arrays you like to analyse: ");
            int noOfArray = int.Parse(Console.ReadLine());
            calcDirDistribution(noOfArray);
        }

        //calculate probability of directions
        public static void calcDirDistribution(int arrayNo)
        {
            Random r = new Random();

            int N = 0, S = 0, E = 0, W = 0, SE = 0, SW = 0, NE = 0, NW = 0, NS = 0, EW = 0, SEW = 0, NEW = 0, NSW = 0, NSE = 0, NSEW = 0, other = 0;
            int dirCount = 0;
            int avgCodeLength = 0;
            List<string> others = new List<string>();
            List<string> compressedLevels = new List<string>();

            //get number of each dir
            for (int i = 0; i < arrayNo; i++)
            {
                //random number between 1 and 10
                int xr = r.Next(2, 11);
                int yr = r.Next(2, 11);

                string[,] posArray = generatePositionalArray(xr, yr);
                string[,] dirArray = getDirArray(posArray);
                compressedLevels.Add(compressLevelCode(dirArray));

                foreach (string dir in dirArray)
                {
                    dirCount++;
                    if (dir == "N") { N++; }
                    else if (dir == "S") { S++; }
                    else if (dir == "E") { E++; }
                    else if (dir == "W") { W++; }
                    else if (dir == "SE") { SE++; }
                    else if (dir == "SW") { SW++; }
                    else if (dir == "NE") { NE++; }
                    else if (dir == "NW") { NW++; }
                    else if (dir == "NS") { NS++; }
                    else if (dir == "EW") { EW++; }
                    else if (dir == "SEW") { SEW++; }
                    else if (dir == "NEW") { NEW++; }
                    else if (dir == "NSW") { NSW++; }
                    else if (dir == "NSE") { NSE++; }
                    else if (dir == "NSEW") { NSEW++; }

                    else { other++; others.Add(dir); }
                }
            }

            //write porbabilitys along with amount to console
            Console.WriteLine("Total number of dirs Analysed: " + dirCount);
            Console.WriteLine("N: " + N + "\t" + "N%: " + String.Format("{0:.##}", ((Convert.ToDouble(N) / dirCount) * 100)) + "%");
            Console.WriteLine("S: " + S + "\t" + "S%: " + String.Format("{0:.##}", ((Convert.ToDouble(S) / dirCount) * 100)) + "%");
            Console.WriteLine("E: " + E + "\t" + "E%: " + String.Format("{0:.##}", ((Convert.ToDouble(E) / dirCount) * 100)) + "%");
            Console.WriteLine("W: " + W + "\t" + "W%: " + String.Format("{0:.##}", ((Convert.ToDouble(W) / dirCount) * 100)) + "%");
            Console.WriteLine("SE: " + SE + "\t" + "SE%: " + String.Format("{0:.##}", ((Convert.ToDouble(SE) / dirCount) * 100)) + "%");
            Console.WriteLine("SW: " + SW + "\t" + "SW%: " + String.Format("{0:.##}", ((Convert.ToDouble(SW) / dirCount) * 100)) + "%");
            Console.WriteLine("NE: " + NE + "\t" + "NE%: " + String.Format("{0:.##}", ((Convert.ToDouble(NE) / dirCount) * 100)) + "%");
            Console.WriteLine("NW: " + NW + "\t" + "NW%: " + String.Format("{0:.##}", ((Convert.ToDouble(NW) / dirCount) * 100)) + "%");
            Console.WriteLine("NS: " + NW + "\t" + "NS%: " + String.Format("{0:.##}", ((Convert.ToDouble(NW) / dirCount) * 100)) + "%");
            Console.WriteLine("EW: " + NW + "\t" + "EW%: " + String.Format("{0:.##}", ((Convert.ToDouble(NW) / dirCount) * 100)) + "%");
            Console.WriteLine("SEW: " + SEW + "\t" + "SEW%: " + String.Format("{0:.##}", ((Convert.ToDouble(SEW) / dirCount) * 100)) + "%");
            Console.WriteLine("NEW: " + NEW + "\t" + "NEW%: " + String.Format("{0:.##}", ((Convert.ToDouble(NEW) / dirCount) * 100)) + "%");
            Console.WriteLine("NSW: " + NSW + "\t" + "NSW%: " + String.Format("{0:.##}", ((Convert.ToDouble(NSW) / dirCount) * 100)) + "%");
            Console.WriteLine("NSE: " + NSE + "\t" + "NSE%: " + String.Format("{0:.##}", ((Convert.ToDouble(NSE) / dirCount) * 100)) + "%");
            Console.WriteLine("NSEW: " + NSEW + "\t" + "NSEW%: " + String.Format("{0:.##}", ((Convert.ToDouble(NSEW) / dirCount) * 100)) + "%");
            Console.WriteLine("Other: " + other + "\t" + "Other%: " + String.Format("{0:.##}", ((Convert.ToDouble(other) / dirCount) * 100)) + "%");
            Console.WriteLine("\n");
            foreach (string s in others)
            {
                Console.WriteLine(s);
            }

            //print list of compressed codes and avg length
            foreach (string code in compressedLevels)
            {
                Console.WriteLine(code);
                avgCodeLength += code.Length;
            }
            Console.WriteLine("Averge code length: " + Convert.ToDouble(avgCodeLength / arrayNo));
        }

        //calculate the distributions of the codes
        public static void callCalcCodeDistributions()
        {
            //init variables and code list
            int codesToAnalyse = 0;
            List<string> codeList = new List<string>();
            List<string> codeSegments = new List<string>();
            List<int> codeSegmentCount = new List<int>();

            //get how many codes to analyse
            Console.Write("How many codes would you like to analyse (done with 100x100 matrixes): ");
            codesToAnalyse = int.Parse(Console.ReadLine());

            //get the codes and add to a list
            for (int i = 0; i < codesToAnalyse; i++)
            {
                string[,] posArray = generatePositionalArray(100, 100);
                string[,] dirArray = getDirArray(posArray);
                string codeWithSize = compressLevelCode(dirArray);
                string codeWithoutSize = Regex.Replace(codeWithSize, @"[\d-]", string.Empty);
                codeList.Add(codeWithoutSize);
            }

            //call and run the distribution check
            var returnLists = calcCodeDistributions(codeList);
            codeSegments = returnLists.CodeSegments;
            codeSegmentCount = returnLists.CodeSegmentCount;

            //sort the results
            var sortedResults = bubbleSortForCodeLists(codeSegments, codeSegmentCount);
            codeSegments = sortedResults.sortedCodeSegmentList;
            codeSegmentCount = sortedResults.sortedCodeSegmentCountList;

            //calculate percentages
            string[] percentages = new string[codeSegments.Count];

            Console.Write("Calculating: ");
            using (var progress = new ProgressBar())
            {
                for (int i = 0; i < percentages.Length; i++)
                {
                    percentages[i] = String.Format("{0:.##}", (Convert.ToDouble(codeSegmentCount[i]) / (codesToAnalyse * 1000)) * 100);
                    progress.Report((double)i / 100);
                    Thread.Sleep(20);
                }
            }
            Console.WriteLine("Done");

            //print the results
            Console.WriteLine("\nTotal number of codes Analysed: " + codeList.Count);
            Console.WriteLine("Total number of unique segments: " + codeSegments.Count);

            for (int j = 0; j < codeSegments.Count; j++)
            {
                Console.WriteLine("{0}: {1}     {0}: {2}%", codeSegments[j], codeSegmentCount[j], percentages[j]);
            }

            Console.WriteLine();
        }

        //calc distributions of codes
        public static (List<string> CodeSegments, List<int> CodeSegmentCount) calcCodeDistributions(List<string> codes)
        {
            //list of code segments and count
            List<string> codeSegments = new List<string>();
            List<int> codeSegmentCount = new List<int>();
            List<string> inputCodes = codes;

            foreach (string code in inputCodes)
            {
                for (int i = 0; i < code.Length; i += 2)
                {
                    //assign the segment as 2 chars
                    string segment = code[i].ToString() + code[i + 1].ToString();
                    bool added = false;

                    //check if it is the first element being added
                    if (codeSegments.Count == 0)
                    {
                        codeSegments.Add(segment);
                        codeSegmentCount.Add(1);
                        added = true;
                    }

                    //check if its in the list
                    var inListReturnData = checkList(codeSegments, segment);
                    bool inList = inListReturnData.inList;
                    int index = inListReturnData.index;

                    //is in the list
                    if (inList && added == false)
                    {
                        //add 1 to count
                        codeSegmentCount[index] += 1;
                    }

                    //not in the list
                    else if (added == false && !inList)
                    {
                        codeSegments.Add(segment);
                        codeSegmentCount.Add(1);
                    }
                }
            }
            return (codeSegments, codeSegmentCount);
        }

        //check if a code segment is in the list
        public static (bool inList, int index) checkList(List<string> segmentList, string segment)
        {
            //check if in list and if it is return index
            for (int i = 0; i < segmentList.Count; i++)
            {
                if (segment == segmentList[i])
                {
                    return (true, i);
                }
            }

            return (false, 0);
        }

        //bubble sort for list highest to lowest
        public static (List<string> sortedCodeSegmentList, List<int> sortedCodeSegmentCountList) bubbleSortForCodeLists(List<string> codeSegentList, List<int> codeSegmentCountList)
        {
            string[] segments = codeSegentList.ToArray();
            int[] counts = codeSegmentCountList.ToArray();

            int tempNum;
            string tempSeg;
            Console.WriteLine();
            for (int k = 0; k <= counts.Length - 2; k++)
            {
                for (int l = 0; l <= counts.Length - 2; l++)
                {
                    if (counts[l] < counts[l + 1])
                    {
                        //move count list
                        tempNum = counts[l + 1];
                        counts[l + 1] = counts[l];
                        counts[l] = tempNum;

                        //move segment list
                        tempSeg = segments[l + 1];
                        segments[l + 1] = segments[l];
                        segments[l] = tempSeg;
                    }
                }
            }
            return (segments.ToList(), counts.ToList());
        }
        #endregion

        #region~~~~~~~~~~Compression and Decompression~~~~~~~~~~

        //compress code to a more useable version 
        public static string compressLevelCode(string[,] array)
        {
            string code = "";
            int x = array.GetLength(0);
            int y = array.GetLength(1);

            //pass 1
            foreach (string dir in array)
            {
                //Console.WriteLine(dir);
                if (dir == "N") { code += "A"; } //N
                else if (dir == "S") { code += "B"; } //S
                else if (dir == "E") { code += "C"; } //E
                else if (dir == "W") { code += "D"; } //W
                else if (dir.Contains("S") && dir.Contains("E") && !dir.Contains("N") && !dir.Contains("W")) { code += "E"; } //SE
                else if (dir.Contains("S") && dir.Contains("W") && !dir.Contains("N") && !dir.Contains("E")) { code += "F"; } //SW
                else if (dir.Contains("N") && dir.Contains("E") && !dir.Contains("S") && !dir.Contains("W")) { code += "G"; } //NE
                else if (dir.Contains("N") && dir.Contains("W") && !dir.Contains("S") && !dir.Contains("E")) { code += "H"; } //NW
                else if (dir.Contains("N") && dir.Contains("S") && !dir.Contains("E") && !dir.Contains("W")) { code += "I"; } //NS
                else if (dir.Contains("E") && dir.Contains("W") && !dir.Contains("N") && !dir.Contains("S")) { code += "J"; } //EW
                else if (dir.Contains("S") && dir.Contains("E") && dir.Contains("W") && !dir.Contains("N")) { code += "K"; } //SEW
                else if (dir.Contains("N") && dir.Contains("E") && dir.Contains("W") && !dir.Contains("S")) { code += "L"; } //NEW
                else if (dir.Contains("N") && dir.Contains("S") && dir.Contains("W") && !dir.Contains("E")) { code += "M"; } //NSW
                else if (dir.Contains("N") && dir.Contains("S") && dir.Contains("E") && !dir.Contains("W")) { code += "N"; } //NSE
                else if (dir.Contains("N") && dir.Contains("S") && dir.Contains("E") && dir.Contains("N")) { code += "O"; } //NSEW

                if (dir.Contains("!")) { code += "!"; }
                if (dir.Contains("%")) { code += "%"; }
                if (dir.Contains("$")) { code += "$"; }
                if (dir.Contains("£")) { code += "£"; }

            }

            return x + code + y;

        }

        //decompresser
        public static string[,] decompressLevelCode(string compresedCode)
        {
            int Width = 0;
            int Height = 0;

            //pull width and height from the code
            //width
            int frontNoCount = 0;
            if (Char.IsNumber(compresedCode[0])) { frontNoCount++; }
            if (Char.IsNumber(compresedCode[1])) { frontNoCount++; }

            if (frontNoCount > 1)
            {
                Width = int.Parse(compresedCode.Substring(0, 2));
            }
            else
            {
                Width = (int)Char.GetNumericValue(compresedCode[0]);
            }
            //height
            int backNoCount = 0;
            if (Char.IsNumber(compresedCode[compresedCode.Length - 1])) { backNoCount++; }
            if (Char.IsNumber(compresedCode[compresedCode.Length - 2])) { backNoCount++; }

            if (backNoCount > 1)
            {
                Height = int.Parse(compresedCode.Substring(compresedCode.Length - 2, 2));
            }
            else
            {
                Height = (int)Char.GetNumericValue(compresedCode[compresedCode.Length - 1]);
            }

            //get a clean version of the string of letters
            string cleanCode = Regex.Replace(compresedCode, @"[\d-]", string.Empty);
            int cleanPos = 0;

            string[,] levelArray = new string[Width, Height];
            List<string> codeSegments = new List<string>();
            // Console.WriteLine("Clean code: " +cleanCode);
            codeSegments = decode(cleanCode);

            Console.WriteLine("Code segments: ");
            foreach (string str in codeSegments)
            {
                Console.WriteLine(str);
            }

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    levelArray[x, y] = codeSegments[cleanPos];
                    cleanPos++;
                }
            }
            return levelArray;
        }

        //decode compression
        public static List<string> decode(string s)
        {
            Console.WriteLine("Clean code: " + s);

            //init needed array and list
            string[] code = new string[s.Length];
            List<string> codeList = new List<string>();

            //get each part of the code
            bool addToPrevious = false;
            for (int i = 0; i < s.Length; i++)
            {
                //Console.WriteLine(s[i]);
                addToPrevious = false;
                string codeSegment = "";
                if (s[i] == 'A') { codeSegment += "N"; }
                else if (s[i] == 'B') { codeSegment += "S"; }
                else if (s[i] == 'C') { codeSegment += "E"; }
                else if (s[i] == 'D') { codeSegment += "W"; }
                else if (s[i] == 'E') { codeSegment += "SE"; }
                else if (s[i] == 'F') { codeSegment += "SW"; }
                else if (s[i] == 'G') { codeSegment += "NE"; }
                else if (s[i] == 'H') { codeSegment += "NW"; }
                else if (s[i] == 'I') { codeSegment += "NS"; }
                else if (s[i] == 'J') { codeSegment += "EW"; }
                else if (s[i] == 'K') { codeSegment += "SEW"; }
                else if (s[i] == 'L') { codeSegment += "NEW"; }
                else if (s[i] == 'M') { codeSegment += "NSW"; }
                else if (s[i] == 'N') { codeSegment += "NSE"; }
                else if (s[i] == 'O') { codeSegment += "NSEW"; }

                if (i <= s.Length - 2)
                {
                    if (s[i + 1] == '!') { addToPrevious = true; codeSegment += "!"; }
                    if (s[i + 1] == '%') { addToPrevious = true; codeSegment += "%"; }
                    if (s[i + 1] == '$') { addToPrevious = true; codeSegment += "$"; }
                    if (s[i + 1] == '£') { addToPrevious = true; codeSegment += "£"; }
                }

                codeList.Add(codeSegment);
            }
            return codeList;

        }

        #endregion

        #endregion

        #region/////////Level generation recursivley//////////



        #endregion




    }
}


