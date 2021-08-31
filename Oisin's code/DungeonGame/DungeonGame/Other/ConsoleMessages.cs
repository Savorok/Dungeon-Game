using System;
using DungeonGame.Constants;

namespace DungeonGame.Other
{
    /*
     * This class contains methods for printing values and showing errors in console
     */
    class ConsoleMessages
    {
        /*
         * Print an error to console in red
         */
        public static void PrintErrorMessage(string loc, string err)
        {
            //set console color to red
            Console.ForegroundColor = ConsoleColours.colours[1];
            //print the error message
            Console.WriteLine("!!!!! ERROR !!!!!");
            Console.WriteLine("Loc: " + loc);
            Console.WriteLine("Err: " + err);
            Console.ResetColor();

        }

        /*
         * This method will print data to the console in a chosen colour
         */
        public static void PrintMessage(int col, string title, string exp)
        {
            //set console color to a colour
            Console.ForegroundColor = ConsoleColours.colours[col];
            //print the message
            Console.WriteLine("::::: " + title + " :::::");
            if (exp == "" || exp == null){}
            else { Console.WriteLine("exp: " + exp); }           
            Console.ResetColor();
        }
    }
}
