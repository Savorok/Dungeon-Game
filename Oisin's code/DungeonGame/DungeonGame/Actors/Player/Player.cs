using System;
using System.Collections.Generic;
using System.Text;
using DungeonGame.Other;

namespace DungeonGame.Actors.Player
{
    /*
     * This is the player class and represents the main playable character
     */
    class Player
    {
        //class variables
        public int X { get; private set; }       
        public int Y { get; private set; }

        //player attributes
        public string Name { get; private set; }
        public int Health { get; private set; }
        public int Stamina { get; private set; }
        public int Strength { get; private set; }
        public int Constitution { get; private set; }
        public int Dexterity { get; private set; }
        public int Intelligence { get; private set; }
        public int Wisdom { get; private set; }
        public int Charisma { get; private set; }
        public int Luck { get; private set; }



        /*
         * This is the public constructor for the player class
         */
        public Player(string name, int[] attList)
        {
            this.Name = name;
            this.Health = attList[0];
            this.Stamina = attList[1];
            this.Strength = attList[2];
            this.Constitution = attList[3];
            this.Dexterity = attList[4];
            this.Intelligence = attList[5];
            this.Wisdom = attList[6];
            this.Charisma = attList[7];
            this.Luck = attList[8];
        }    

        public void setPos(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public void printPos()
        {
            ConsoleMessages.PrintMessage(2, "Player Position", "X: " + this.X.ToString() + " Y: " + this.Y.ToString());
        }


    }
}
