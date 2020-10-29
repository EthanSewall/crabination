using static Raylib_cs.Raylib;  
using static Raylib_cs.Color;   
using static Raylib_cs.Raymath;
using System.Diagnostics;
using System.Numerics; 
using System;
using System.Collections.Generic;
using System.IO;      

namespace Crabination
{
    public class Window
    {
        public static int Main()
        {
            InitWindow(1600, 900, "Project");
            SetTargetFPS(60);
            bool stillGoing = true;
            float accel = 0.75f;
            float drag = 0.95f;
            Game game = new Game();
            game.Init();

            SceneObject bigCrab = new SceneObject();
            SceneObject smallCrab = new SceneObject();
            SpriteObject bigSprite = new SpriteObject();
            SpriteObject smallSprite = new SpriteObject();

            {
                bigSprite.Load("crab.png");
                smallSprite.Load("smallcrab.png");

                smallCrab.AddChild(smallSprite);
                bigCrab.AddChild(bigSprite);
                bigCrab.AddChild(smallCrab);

                bigSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
                smallSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));

                bigCrab.SetPosition(800, 450);
                smallCrab.SetPosition(bigSprite.Height * 0.65f, -bigSprite.Width * 0.25f);

                bigCrab.Rotate((float)(Math.PI * 0.5f));
            }//crab initialization stuff


            while (!WindowShouldClose() && stillGoing)
            {
                BeginDrawing();
                game.Update();
                game.Draw();
                ClearBackground(DARKBLUE);

                Vector2 accelDirection = new Vector2();

                if (IsKeyDown(Raylib_cs.KeyboardKey.KEY_W))
                {
                    Vector3 facing = new Vector3( bigCrab.LocalTransform.m2, bigCrab.LocalTransform.m1, 1) * game.deltaTime * -150 * accel; 
                    accelDirection += new Vector2(facing.y, -facing.x);
                }
                if (IsKeyDown(Raylib_cs.KeyboardKey.KEY_A))
                {
                    bigCrab.Rotate(-game.deltaTime);
                }
                if (IsKeyDown(Raylib_cs.KeyboardKey.KEY_S))
                {
                    Vector3 facing = new Vector3(bigCrab.LocalTransform.m2, bigCrab.LocalTransform.m1, 1) * game.deltaTime * 150 * accel;
                    accelDirection += new Vector2(facing.y, -facing.x);
                }
                if (IsKeyDown(Raylib_cs.KeyboardKey.KEY_D))
                {
                    bigCrab.Rotate(game.deltaTime);
                }
                if (IsKeyDown(Raylib_cs.KeyboardKey.KEY_Q))
                {
                    smallCrab.Rotate(-game.deltaTime);
                }
                if (IsKeyDown(Raylib_cs.KeyboardKey.KEY_E))
                {
                    smallCrab.Rotate(game.deltaTime);
                }

                bigCrab.acceleration = accelDirection;
                bigCrab.velocity += bigCrab.acceleration;
                bigCrab.Translate(bigCrab.velocity.X, bigCrab.velocity.Y);
                bigCrab.velocity = new Vector2(bigCrab.velocity.X * drag, bigCrab.velocity.Y * drag);

                bigCrab.Draw();
                bigCrab.Update(game.deltaTime);

                EndDrawing();
            }

            CloseWindow();

            return 0;
        }
    }

    public class Game
    {
        Stopwatch stopwatch = new Stopwatch();
        private long currentTime = 0;
        private long lastTime = 0;
        private float timer = 0;
        private int fps = 1;
        private int frames;
        public float deltaTime = 0.005f;
        public void Init()
        {
            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds;
        }
        public void Shutdown()
        { 
        
        }
        public void Update()
        {
            currentTime = stopwatch.ElapsedMilliseconds;
            deltaTime = (currentTime - lastTime) / 1000.0f;
            timer += deltaTime;
            if (timer >= 1)
            {
                fps = frames;
                frames = 0;
                timer -= 1;
            }
            frames++;
            lastTime = currentTime;
        }
        public void Draw()
        {
            DrawText(fps.ToString() + " FPS", 10, 10, 20, RED);
            DrawText(deltaTime.ToString() + " deltaTime", 10, 30, 20, RED);
        }
    }



   
}