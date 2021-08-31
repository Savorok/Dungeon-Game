using DungeonGame.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonGame.Actors.Player
{
    /*
     * This is the player controller class it will handle all player-game 
     * interactions moving,combat, etc...
     */
    class PlayerController
    {
        //this is the player the controller will manage
        public Player player { get; set; }

        /*
         * The constructor for a player controller given a player
         */
        public PlayerController(Player player)
        {
            
        }

        /*
         * The constructor for a player controller will create new player
         */
        public PlayerController()
        {

        }

        /*
         * This is the method used for moving the player
         * dir values: north:0, east:1, south:2, west:3
         */
        public bool MovePlayer(int dir)
        {
            switch (dir)
            {
                //north
                case 0:
                    this.player.setPos(player.X,player.Y-1);
                    return true;
                //east
                case 1:
                    this.player.setPos(player.X + 1, player.Y);
                    return true;
                //south
                case 2:
                    this.player.setPos(player.X, player.Y + 1);
                    return true;
                //west
                case 3:
                    this.player.setPos(player.X - 1, player.Y);
                    return true;
                //incorrect dir
                default:
                    ConsoleMessages.PrintErrorMessage("Player:Move", "Incorrect dir: " + dir.ToString());
                    return false;
            }
        }

        /*
         * This is the method used to create a new player 
         */
        public Player CreateNewPlayer()
        {
            //temp player attributes
            string name = "default";
            int[] attList = new int[]{ -1, -1 , -1 , -1 , -1 , -1 , -1 , -1 , -1 };

            Console.Out.Write("~~~~~~~~~~~~~~~~~~~~ Create New Player ~~~~~~~~~~~~~~~~~~~~\n\n");
            bool gotName = false;
            do
            {
                Console.Out.Write("Enter Player Name: ");
                name = Console.ReadLine();
                //if name contains only letters and is less than 24 chars
                if(!name.Any(char.IsDigit) && name.Length < 24)
                {
                    gotName = true;
                }
                else
                {
                    Console.Out.WriteLine("Invalid Player name!");
                    name = "Invalid";
                }
            }
            while(!gotName);

            //todo attribute generator

            Player newPlayer = new Player(name, attList);
            return newPlayer;

        }

    }
}
