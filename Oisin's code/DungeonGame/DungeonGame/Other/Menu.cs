using System;
using System.Collections.Generic;

namespace DungeonGame.Other
{
    class Menu
    {
        //class variables
        private List<string> Options;   //the menu options
        private string Title;       //the menus title

        public Menu(string title, List<string> options)
        {
            this.Title = title;
            this.Options = options;
        }

        /*
         * Get the chosen menu option
         */
        public int getChoice()
        {
            //init return variable
            int choice = -1;
            bool finished = false;
            do
            {
                try
                {
                    Console.Write("Enter choice: ");
                    choice = int.Parse(Console.ReadLine());
                    finished = true;
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Please enter a number" + ex);
                }
            }
            while (!finished);

            return choice;
        }

        /*
         * This method will display the menu
         */
        public void display()
        {
            //print the menu title
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~ " + Title + "~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine();
            //display the menu options
            int count = 1;
            foreach(string str in this.Options)
            {
                Console.WriteLine(count + ". " + str);
                count++;
            }

            //print bottom of menu 
            Console.WriteLine();
            Console.Write("~~~~~~~~~~~~~~~~~~~~");
            for (int i = 0; i < Title.Length; i++)
            {
                Console.Write("~");
            }
            Console.Write("~~~~~~~~~~~~~~~~~~~~\n");
        }
    }
}
