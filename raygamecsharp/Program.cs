using static Raylib_cs.Raylib;  
using static Raylib_cs.Color;   
using static Raylib_cs.Raymath; 
using System.Numerics; 
using System;
using System.Collections.Generic;
using System.IO;

namespace MissileCommand
{
    public class Window
    {
        const int screenWidth = 1600;
        const int screenHeight = 900;

        public static int Width()
        {
            return screenWidth;
        }
        public static int Height()
        {
            return screenHeight;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        public static int Main()
        {
            InitWindow(screenWidth, screenHeight, "Project");
            SetTargetFPS(60);

            //declaring objects to exist

            while (!WindowShouldClose())
            {

            }
            CloseWindow();

            return 0;
        }
    }//contains Main

   
}