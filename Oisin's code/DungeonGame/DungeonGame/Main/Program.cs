using DungeonGame.Dungeons.DungeonClasses;
using DungeonGame.Dungeons.Generators;
using DungeonGame.Actors;
using DungeonGame.Other;
using System;
using System.Collections.Generic;
using DungeonGame.Actors.Player;

namespace DungeonGame
{
    class Program
    {
        /*
         * This is the main starting point for the game
         */
        static void Main(string[] args)
        {
        
            DungeonGenerator DG = new DungeonGenerator("defaultConfig.txt");
            Dungeon D = DG.GenerateDungeon(7);

          
            foreach(Level level in D.Levels)
            {               
                level.printLevel();               
            }

            
            //PlayerController pc = new PlayerController();
            //Player player = pc.CreateNewPlayer();
            //pc.player = player;
            
            /*

            int x, y;
            Console.Write("Enter player X: ");
            x = int.Parse(Console.ReadLine());
            Console.Write("Enter player Y: ");
            y = int.Parse(Console.ReadLine());

            player.setPos(x, y);
            Console.Clear();
            player.printPos();

            List<string> options = new List<string>()
            {
                "Go North",
                "Go East",
                "Go South",
                "Go West"
            };

            Menu mainMenu = new Menu("Movement Menu",options);

            do
            {
                level.printLevel();
                mainMenu.display();
                player.printPos();
                Console.WriteLine("Room contains" );
                foreach (Constants.Direction dir in level.Rooms[player.Y, player.X].Exits)
                {
                    Console.WriteLine(dir.ToString());
                }


                switch (mainMenu.getChoice())
                {
                    case 1:
                        if(level.Rooms[player.Y, player.X].Exits.Contains(Constants.Direction.N))
                        {                          
                            pc.MovePlayer(0);                                                
                        }
                        else
                        {
                            Console.WriteLine("Cant move north");
                        }
                        break;
                    case 2:
                        if (level.Rooms[player.Y, player.X].Exits.Contains(Constants.Direction.E))
                        {
                            pc.MovePlayer(1);
                        }
                        else
                        {
                            Console.WriteLine("Cant move east");
                        }
                        break;
                    case 3:
                        if (level.Rooms[player.Y, player.X].Exits.Contains(Constants.Direction.S))
                        {
                            pc.MovePlayer(2);
                        }
                        else
                        {
                            Console.WriteLine("Cant move south");
                        }
                        break;
                    case 4:
                        if (level.Rooms[player.Y, player.X].Exits.Contains(Constants.Direction.W))
                        {
                            pc.MovePlayer(3);
                        }
                        else
                        {
                            Console.WriteLine("Cant move west");
                        }
                        break;
                    default:
                        Console.WriteLine("Err");
                        break;

                }

                Console.ReadKey();
                Console.Clear();
            }
            while
            (true);

            */
                                        
        }
    }
}
