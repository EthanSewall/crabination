using static Raylib_cs.Raylib;  
using static Raylib_cs.Color;   
using static Raylib_cs.Raymath; 
using System.Numerics; 
using System;
using System.Collections.Generic;
using System.IO;      

namespace Crabination
{
    public class Window
    {
        public static Raylib_cs.Texture2D crab;
        public static int Main()
        {
            InitWindow(1600, 900, "Project");
            SetTargetFPS(60);
            bool stillGoing = true;
            crab = LoadTexture("crab.png"); //figure out how to put this in the output directory
            

            while (!WindowShouldClose() && stillGoing)
            {
                BeginDrawing();
                DrawTexture(crab, 500, 300, WHITE);
                ClearBackground(DARKBLUE);
                EndDrawing();
            }

            CloseWindow();

            return 0;
        }
    }

   
}