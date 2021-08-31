using DungeonGame.Other;
using DungeonGame.Constants;
using DungeonGame.Dungeons.DungeonClasses;
using System.Collections.Generic;

using System;

namespace DungeonGame.Dungeons.Generators
{
    /*
     *  This class will generate a level and in turn generate rooms for it 
     */
    class LevelGenerator
    {       
        /*
         * This method will generate a level given a height and a width
         */
        public LevelGenerator()
        {
        }

        public Level GenerateLevel(int depth, int height, int width, int spaceMin, int spaceMax, int spaceSizeMin, int spaceSizeMax, int minRegSize)
        {
            //room array
            int[,] roomArray = GenerateRoomArray(height, width, spaceMin, spaceMax, spaceSizeMin, spaceSizeMax);
            //clean the room array and get cleanRoomArray and regionArray
            var clean = CleanRoomArray(roomArray, minRegSize);
            int[,] cleanRoomArray = clean.roomArray;
            int[,] regionArray = clean.regionArray;
            //generate a weight array
            int[,] weightArray = GenerateWeightArray(cleanRoomArray);
            //generate rooms
            Room[,] rooms = addRooms(cleanRoomArray, weightArray);
            //create an array of the room dimensions
            var dim = new int[] { width, height };
            
            //create new level passing in all the arrays and the size and depth
            Level level = new Level(dim,1,rooms, cleanRoomArray, regionArray, weightArray);

            return level;
        }

        /*
         * This method will generate an array of size, width and heigth,
         * with two values 1 and 0, where 1 is a room and 0 is not a room,
         * spaceMin to spaceMaX determins the amount of times to remove space from a level,
         * spaceSize will determin how much room is removed each time
         */
        private int[,] GenerateRoomArray(int height, int width, int spaceMin, int spaceMax, int spaceSizeMin, int spaceSizeMax)
        {
            //create the empty array
            int[,] roomArray = new int[height, width];

            //fill the entire array with 1's
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    roomArray[y, x] = 1;
                }
            }

            //genereate number of times to take away space
            int timesToTakeSpace = RandomFunctions.randomRangeWithMax(spaceMin, spaceMax);

            //take away space 
            for (int i = 0; i < timesToTakeSpace; i++)
            {
                //room start pos
                int ranX, ranY;

                //pick a random place with a room slot to start from
                bool roomSelected = false;
                do
                {
                    //pick a random room to start from
                    ranX = RandomFunctions.randomRange(0, width);
                    ranY = RandomFunctions.randomRange(0, height);
            
                    //if this is a room start here
                    if (roomArray[ranY, ranX] == 1) 
                    {
                        roomSelected = true; 
                    }
                }
                while (!roomSelected);

                //current cell position
                int curX = ranX;
                int curY = ranY;
                       
                //calculate size of space
                int spaceSize = RandomFunctions.randomRangeWithMax(spaceSizeMin, spaceSizeMax);
                //ConsoleMessages.PrintLogMessageBlue("Space Size", spaceSize.ToString());

                //take away space size rooms
                for (int j = 0; j < spaceSize; j++)
                { 
                    //print current room
                    //ConsoleMessages.PrintLogMessageYellow("Current Cell pos", "X:" + curX + " Y:" + curY);
                    //make space empty
                    
                    if(curY < 0 || curY > height)
                    {
                        ConsoleMessages.PrintErrorMessage("CurY is out of bounds", curY.ToString());
                    }
                    else if (curX < 0 || curX > width)
                    {
                        ConsoleMessages.PrintErrorMessage("CurX is out of bounds", curX.ToString());
                    }
                    else
                    {
                        //PrintArrays.printArray(roomArray);
                        roomArray[curY, curX] = 0;
                    }


                    bool hasMoved = false;
                    do
                    {
                        //move random direction
                        // 0 = up
                        // 1 = right
                        // 2 = down
                        // 3 = left
                        int ranDir = RandomFunctions.randomRangeWithMax(0, 3);
                        //ConsoleMessages.PrintLogMessageGreen("ranDir", ranDir.ToString());

                        //move up
                        if (ranDir == 0)
                        {
                            if ((curY - 1) >= 0) 
                            { 
                                curY -= 1; 
                                hasMoved = true; 
                            }
                        }                                                   
                        //move right
                        else if(ranDir == 1)
                        {
                            if ((curX + 1) <= width-1) 
                            { 
                                curX += 1; 
                                hasMoved = true; 
                            }
                        }
                        //move down
                        else if (ranDir == 2)
                        {
                            if ((curY + 1) <= height-1) 
                            { 
                                curY += 1; 
                                hasMoved = true; 
                            }
                        }
                        //move left
                        else if(ranDir == 3)
                        {
                            if ((curX - 1) >= 0) 
                            { 
                                curX -= 1; 
                                hasMoved = true; 
                            }
                        }
                        else
                        {
                            string location = "LevelGenerator:AllowedRoomArray:switch(ranDir)";
                            string error = "Default case when switching ranDir";
                            ConsoleMessages.PrintErrorMessage(location, error);
                        }
                    }
                    while (!hasMoved);
                }
            }
            //return the room array
            return roomArray;
        }

        /*
         * This method will take a roomArray and create regions for each independant 
         * 'island', regions with less than n rooms will be deleted.
         * It has 2 returns, the roomArray which consists of 1 for room and 0 for space,
         * and the regionArray which has x for region number where (x>0) and 0 for space
         */
        private (int[,] roomArray, int[,] regionArray) CleanRoomArray(int[,] arr, int minRegSize)
        {
            //get width and height of the cell
            int height = arr.GetLength(0);
            int width = arr.GetLength(1);

            //create new arrays for output
            int[,] roomArray = arr;
            int[,] regionArray = new int[height, width];

            //region the array
            regionArray = RegionRecursion(arr);

            //PrintArrays.printRegionArray(regionArray, "Region Array", "description");

            //at this point all the 0's in the room array 

            //remove any regions that have less than 3 rooms
            //first get number of regions
            //get amount of regions
            int largestRegion = 0;
            for (int y = 0; y < regionArray.GetLength(0); y++)
            {
                for (int x = 0; x < regionArray.GetLength(1); x++)
                {
                    int temp = regionArray[y, x];
                    if (temp > largestRegion) { largestRegion = temp; }
                }
            }

            //now go through each region and remove the ones that have less than n members
            for (int i = 1; i < largestRegion + 1; i++)
            {
                //amount of rooms in region
                int roomsCount = 0;
                for (int y = 0; y < regionArray.GetLength(0); y++)
                {
                    for (int x = 0; x < regionArray.GetLength(1); x++)
                    {
                        if (regionArray[y, x] == i) { roomsCount++; }
                    }
                }

                //if less than n rooms remove the rooms
                if(roomsCount < minRegSize)
                {
                    for (int y = 0; y < regionArray.GetLength(0); y++)
                    {
                        for (int x = 0; x < regionArray.GetLength(1); x++)
                        {
                            if (regionArray[y, x] == i) 
                            { 
                                regionArray[y, x] = 0;
                                roomArray[y, x] = 0;
                            }
                        }
                    }
                }
            }

            
            //go through the room array and revert all the -1's to 0
            for (int y = 0; y < roomArray.GetLength(0); y++)
            {
                for (int x = 0; x < roomArray.GetLength(1); x++)
                {
                    if (roomArray[y, x] == -1)
                    {
                        roomArray[y, x] = 1;
                    }
                }
            }
            

            //return the new arrays
            return (roomArray, regionArray);
                                       
        }

        /*
         * will perform a recursive check and region connected parts of a level
         */
        private int[,] RegionRecursion(int[,] arr)
        {
            //get width and height of the cell
            int height = arr.GetLength(0);
            int width = arr.GetLength(1);

            //create new array for output
            int[,] regionArray = new int[height, width];

            //region number
            int region = 0;
            //if there is still rooms in the array
            bool stillRooms = true;
            do
            {
                region++;

                //check to see for 1's in the array (rooms)
                (int xPos, int yPos, bool stillRooms) getRoomPosition()
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            //if you find a room return its co-ords and true
                            if (arr[y, x] == 1) { return (x, y, true); }
                        }
                    }
                    //if you dont return 0,0 and false
                    return (0, 0, false);
                }

                //if there are no rooms stop regioning
                if (getRoomPosition().stillRooms == false)
                {
                    stillRooms = false; 
                }
                else
                {
                    //call the recursive step
                    var gRP = getRoomPosition();
                    recursiveStep(gRP.xPos, gRP.yPos, region, arr);
                }

            }
            while (stillRooms);

            //recursive step to map out regions
            void recursiveStep(int x, int y, int regionNumber, int[,] roomArray)
            {
                //check to see if this cell has already been regioned
                if (regionArray[y, x] == regionNumber) { return; }
                else 
                { 
                    regionArray[y, x] = regionNumber;
                    roomArray[y, x] = -1;
                }

                //check north
                if((y - 1) >= 0)
                {
                    if (roomArray[y - 1, x] == 1) { recursiveStep(x, y - 1, regionNumber, roomArray); }
                }
             
                //check east
                if((x + 1) <= roomArray.GetLength(1)-1)
                {
                    if (roomArray[y, x + 1] == 1) { recursiveStep(x + 1, y, regionNumber, roomArray); }
                }
                
                //check south
                if((y + 1) <= roomArray.GetLength(0)-1)
                {
                    if (roomArray[y + 1, x] == 1) { recursiveStep(x, y + 1, regionNumber, roomArray); }
                }
                
                //check west
                if((x - 1) >= 0)
                {
                    if (roomArray[y, x - 1] == 1) { recursiveStep(x - 1, y, regionNumber, roomArray); }
                }               
            }

            //return the region array
            return regionArray;
        }

        /*
         * Given a room array generate a negative version of it
         */
        private int[,] generateRoomArrayNegative(int[,] roomArray)
        {
            //return array
            int[,] roomArrayNegative = roomArray;

            //create the -ve
            for (int y = 0; y < roomArrayNegative.GetLength(0); y++)
            {
                for (int x = 0; x < roomArrayNegative.GetLength(1); x++)
                {
                    if (roomArrayNegative[y, x] == 0) { roomArrayNegative[y, x] = 1; }
                    else if (roomArrayNegative[y, x] == 1) { roomArrayNegative[y, x] = 0; }
                    else { ConsoleMessages.PrintErrorMessage("LevelGenerator:GenerateRoomArrayNegative", "Invalid value in matrix: " + roomArrayNegative[y, x]); }
                }
            }

            //return the -ve
            return roomArrayNegative;
        }
    
        /*
         * This method will produce an array with values 0-15 indicating 
         * the positional type of a room
         */
        private int[,] GenerateWeightArray(int[,] arr)
        {
            //get width and height of the cell
            int height = arr.GetLength(0);
            int width = arr.GetLength(1);

            //create new array for output
            int[,] weightArray = new int[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (arr[y, x] == 1)
                    {                      
                        weightArray[y, x] = assignWeight(x, y);
                    }
                }
            }

            int  assignWeight(int x, int y)
            {
                int weight = 0;

                //check north
                if ((y - 1) >= 0)
                {
                    if (arr[y - 1, x] == 1) { weight += 1; }
                }

                //check east
                if ((x + 1) <= width - 1)
                {
                    if (arr[y, x + 1] == 1) { weight += 2; }
                }

                //check south
                if ((y + 1) <= height - 1)
                {
                    if (arr[y + 1, x] == 1) { weight += 4; }
                }

                //check west
                if ((x - 1) >= 0)
                {
                    if (arr[y, x - 1] == 1) { weight += 8; }
                }

                return weight;
            }

            return weightArray;
        }
    
        /*
         * This array will take a clean roomArray and a roomWeightArray
         * and create an array of rooms with assigned exit directions
         */
        private Room[,] addRooms(int[,] roomArray,int[,] weightArray)
        { 
            //get width and height of the array
            int height = roomArray.GetLength(0);
            int width = roomArray.GetLength(1);

            //create the room object array
            Room[,] rooms = new Room[height, width];

            //go through each position and make a room
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (roomArray[y, x] == 1)
                    {
                        //create a room
                        int roomX = x;
                        int roomY = y;
                        int roomWeight = weightArray[y, x];
                        List<Direction> exits = assignExits(roomWeight);
                        Room room = new Room(roomX, roomY, roomWeight, exits);
                        rooms[y, x] = room;
                    }
                    else
                    {
                        rooms[y, x] = null;
                    }
                }
            }

            interConnectRooms(rooms);

            return rooms;
        }

        /*
         * This method will take a weight and return a List of directions 
         * that represents the exits of the room
         */
        private List<Direction> assignExits(int weight)
        {
            //chances
            double twoExitChance = 0.5;
            double threeExitChance = 0.333;
            double fourExitChance = 0.25;

            //return lisy
            List<Direction> exits = new List<Direction>();

            ////////// Cell type 1-15 //////////
            // 1 - bottom cap
            if (weight == 1)
            {
                exits.Add(Direction.N);
                return exits;
            }
            // 2 - left cap
            if(weight == 2)
            {
                exits.Add(Direction.E);
                return exits;
            }
            // 3 - bottom left corner
            if(weight == 3)
            {
                List<Direction> options = new List<Direction> { Direction.N, Direction.E };

                if (RandomFunctions.randomChance(twoExitChance)) { exits.Add(Direction.N); }
                if (RandomFunctions.randomChance(twoExitChance)) { exits.Add(Direction.E); }
                if (exits.Count == 0) { int rNum = RandomFunctions.randomRange(0,2); exits.Add(options[rNum]); }
                return exits;
            }
            // 4 - top cap
            if (weight == 4)
            {
                exits.Add(Direction.S);
                return exits;
            }
            // 5 - vertical passage
            if (weight == 5)
            {
                exits.Add(Direction.N);
                exits.Add(Direction.S);
                return exits;
            }
            // 6 - top left corner
            if (weight == 6)
            {
                List<Direction> options = new List<Direction> { Direction.E, Direction.S };

                if (RandomFunctions.randomChance(twoExitChance)) { exits.Add(Direction.E); }
                if (RandomFunctions.randomChance(twoExitChance)) { exits.Add(Direction.S); }
                if (exits.Count == 0) { int rNum = RandomFunctions.randomRange(0, 2); exits.Add(options[rNum]); }
                return exits;
            }
            // 7 - left side piece
            if (weight == 7)
            {
                List<Direction> options = new List<Direction> { Direction.N, Direction.S, Direction.E };

                if (RandomFunctions.randomChance(threeExitChance)) { exits.Add(Direction.N); }
                if (RandomFunctions.randomChance(threeExitChance)) { exits.Add(Direction.S); }
                if (RandomFunctions.randomChance(threeExitChance)) { exits.Add(Direction.E); }
                if (exits.Count == 0) { int rNum = RandomFunctions.randomRange(0, 3); exits.Add(options[rNum]); }
                return exits;
            }
            // 8 - right cap
            if (weight == 8)
            {
                exits.Add(Direction.W);
                return exits;
            }
            // 9 - bottom right corner
            if (weight == 9)
            {
                List<Direction> options = new List<Direction> { Direction.N, Direction.W };

                if (RandomFunctions.randomChance(twoExitChance)) { exits.Add(Direction.N); }
                if (RandomFunctions.randomChance(twoExitChance)) { exits.Add(Direction.W); }
                if (exits.Count == 0) { int rNum = RandomFunctions.randomRange(0, 2); exits.Add(options[rNum]); }
                return exits;
            }
            // 10 - horizontal passage
            if (weight == 10)
            {
                exits.Add(Direction.E);
                exits.Add(Direction.W);
                return exits;
            }
            // 11 - bottom side peice
            if (weight == 11)
            {
                List<Direction> options = new List<Direction> { Direction.N, Direction.E, Direction.W };

                if (RandomFunctions.randomChance(threeExitChance)) { exits.Add(Direction.N); }
                if (RandomFunctions.randomChance(threeExitChance)) { exits.Add(Direction.E); }
                if (RandomFunctions.randomChance(threeExitChance)) { exits.Add(Direction.W); }
                if (exits.Count == 0) { int rNum = RandomFunctions.randomRange(0, 3); exits.Add(options[rNum]); }
                return exits;
            }
            // 12 - top right corner
            if (weight == 12)
            {
                List<Direction> options = new List<Direction> { Direction.S, Direction.W };

                if (RandomFunctions.randomChance(twoExitChance)) { exits.Add(Direction.S); }
                if (RandomFunctions.randomChance(twoExitChance)) { exits.Add(Direction.W); }
                if (exits.Count == 0) { int rNum = RandomFunctions.randomRange(0, 2); exits.Add(options[rNum]); }
                return exits;
            }
            // 13 - right side piece
            if (weight == 13)
            {
                List<Direction> options = new List<Direction> { Direction.N, Direction.S, Direction.W };

                if (RandomFunctions.randomChance(threeExitChance)) { exits.Add(Direction.N); }
                if (RandomFunctions.randomChance(threeExitChance)) { exits.Add(Direction.S); }
                if (RandomFunctions.randomChance(threeExitChance)) { exits.Add(Direction.W); }
                if (exits.Count == 0) { int rNum = RandomFunctions.randomRange(0, 3); exits.Add(options[rNum]); }
                return exits;
            }
            // 14 - top side piece
            if (weight == 14)
            {
                List<Direction> options = new List<Direction> { Direction.S, Direction.E, Direction.W };

                if (RandomFunctions.randomChance(threeExitChance)) { exits.Add(Direction.S); }
                if (RandomFunctions.randomChance(threeExitChance)) { exits.Add(Direction.E); }
                if (RandomFunctions.randomChance(threeExitChance)) { exits.Add(Direction.W); }
                if (exits.Count == 0) { int rNum = RandomFunctions.randomRange(0, 3); exits.Add(options[rNum]); }
                return exits;
            }
            // 15 - centre piece
            if (weight == 15)
            {
                List<Direction> options = new List<Direction> { Direction.N, Direction.S, Direction.E, Direction.W };

                if (RandomFunctions.randomChance(fourExitChance)) { exits.Add(Direction.N); }
                if (RandomFunctions.randomChance(fourExitChance)) { exits.Add(Direction.S); }
                if (RandomFunctions.randomChance(fourExitChance)) { exits.Add(Direction.E); }
                if (RandomFunctions.randomChance(fourExitChance)) { exits.Add(Direction.W); }
                if (exits.Count == 0) { int rNum = RandomFunctions.randomRange(0, 4); exits.Add(options[rNum]); }
                return exits;
            }

            ConsoleMessages.PrintErrorMessage("LevelGenerator:assignedExits", "Invalid weight number: " + weight.ToString());
            return null;
        }

        /*
         * This method will take a 2d array of rooms and interconnect them
         * so if a room has a south exit the room to its south should have a north exit
         */
        private void interConnectRooms(Room[,] rooms)
        {
            //get width and height of the cell
            int height = rooms.GetLength(0);
            int width = rooms.GetLength(1);

            //go through each room and interconnect it 
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if(rooms[y,x] != null)
                    {
                        List<Direction> exits = rooms[y, x].Exits;

                        //north
                        if (exits.Contains(Direction.N) && rooms[y - 1,x] != null)
                        {
                            rooms[y - 1, x].addRemoveExits(0, 0, 1, 0);
                        }
                        //south
                        if (exits.Contains(Direction.S) && rooms[y + 1, x] != null)
                        {
                            rooms[y + 1, x].addRemoveExits(1, 0, 0, 0);
                        }
                        //east
                        if (exits.Contains(Direction.E) && rooms[y, x + 1] != null)
                        {
                            rooms[y, x + 1].addRemoveExits(0, 0, 0, 1);
                        }
                        //west
                        if (exits.Contains(Direction.W) && rooms[y, x - 1] != null)
                        {
                            rooms[y, x - 1].addRemoveExits(0, 1, 0, 0);
                        }
                    }                 
                }
            }

        }



    }
}
