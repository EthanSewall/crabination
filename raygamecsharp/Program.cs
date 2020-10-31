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
            SceneObject collisionCircle = new SceneObject();

            List<SceneObject> projectileCrabs = new List<SceneObject>();
            List<SpriteObject> projectileSprites = new List<SpriteObject>();

            List<SceneObject> incomingCrabs = new List<SceneObject>();
            List<SpriteObject> incomingSprites = new List<SpriteObject>();
            float spawnDelay = 0f;

            {
                bigSprite.Load("crab.png");
                smallSprite.Load("smallcrab.png");

                smallCrab.AddChild(smallSprite);
                bigCrab.AddChild(bigSprite);
                bigCrab.AddChild(smallCrab);
                bigCrab.AddChild(collisionCircle);

                bigSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
                smallSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
                bigSprite.Translate(-bigSprite.Height * 0.5f, bigSprite.Width * 0.5f);
                bigCrab.SetPosition(800, 450);
                smallSprite.Translate(-smallSprite.Height * 0.5f, smallSprite.Width * 0.5f);
                smallCrab.Translate(smallSprite.Height * 0.5f, 0);

                bigCrab.Rotate((float)(Math.PI * 0.5f));
            }//crab initialization stuff


            while (!WindowShouldClose() && stillGoing)
            {
                BeginDrawing();
                game.Update();
                game.Draw();
                ClearBackground(DARKBLUE);

                Vector2 accelDirection = new Vector2();

                {
                    if (IsKeyDown(Raylib_cs.KeyboardKey.KEY_W) && bigCrab.IsInBounds())
                    {
                        Vector3 facing = new Vector3(bigCrab.LocalTransform.m2, bigCrab.LocalTransform.m1, 1) * game.deltaTime * -150 * accel;
                        accelDirection += new Vector2(facing.y, -facing.x);
                    }
                    if (IsKeyDown(Raylib_cs.KeyboardKey.KEY_A))
                    {
                        bigCrab.Rotate(-game.deltaTime * 2);
                    }
                    if (IsKeyDown(Raylib_cs.KeyboardKey.KEY_S) && bigCrab.IsInBounds())
                    {
                        Vector3 facing = new Vector3(bigCrab.LocalTransform.m2, bigCrab.LocalTransform.m1, 1) * game.deltaTime * 150 * accel;
                        accelDirection += new Vector2(facing.y, -facing.x);
                    }
                    if (IsKeyDown(Raylib_cs.KeyboardKey.KEY_D))
                    {
                        bigCrab.Rotate(game.deltaTime * 2);
                    }
                    if (IsKeyDown(Raylib_cs.KeyboardKey.KEY_Q))
                    {
                        smallCrab.Rotate(-game.deltaTime * 2);
                    }
                    if (IsKeyDown(Raylib_cs.KeyboardKey.KEY_E))
                    {
                        smallCrab.Rotate(game.deltaTime * 2);
                    }
                    bigCrab.acceleration = accelDirection;
                    bigCrab.velocity += bigCrab.acceleration;
                    bigCrab.Translate(bigCrab.velocity.X, bigCrab.velocity.Y);
                    bigCrab.velocity = new Vector2(bigCrab.velocity.X * drag, bigCrab.velocity.Y * drag);
                    if (bigCrab.GlobalTransform.m6 < 0)
                    {
                        bigCrab.velocity.Y = MathF.Abs(bigCrab.velocity.Y);
                    }
                    if (bigCrab.GlobalTransform.m6 > 900)
                    {
                        bigCrab.velocity.Y = MathF.Abs(bigCrab.velocity.Y);
                        bigCrab.velocity.Y = -bigCrab.velocity.Y;
                    }
                    if (bigCrab.GlobalTransform.m3 > 1600)
                    {
                        bigCrab.velocity.X = MathF.Abs(bigCrab.velocity.X);
                        bigCrab.velocity.X = -bigCrab.velocity.X;
                    }
                    if (bigCrab.GlobalTransform.m3 < 0)
                    {
                        bigCrab.velocity.X = MathF.Abs(bigCrab.velocity.X);
                    }
                }//movement stuff

                foreach(SceneObject crab in projectileCrabs)
                {
                    crab.Draw();
                    crab.Translate(crab.velocity.X, crab.velocity.Y);
                }
                for (int i = 0; i < projectileCrabs.Count; i++)
                {
                     if(!projectileCrabs[i].IsInBounds())
                     {
                         projectileCrabs.RemoveAt(i);
                     }
                }
                if (IsKeyPressed(Raylib_cs.KeyboardKey.KEY_ENTER))
                {
                    SceneObject newProjectile = new SceneObject();
                    SpriteObject newSprite = new SpriteObject();
                    newSprite.Load("weapon_crab.png");
                    newProjectile.AddChild(newSprite);
                    newProjectile.SetPosition(bigCrab.GlobalTransform.m3, bigCrab.GlobalTransform.m6);
                    newSprite.Translate(-smallSprite.Width * 0.4f, 0);
                    Vector3 facing = new Vector3(smallCrab.GlobalTransform.m2, smallCrab.GlobalTransform.m1, 1) * game.deltaTime * 1500;
                    newProjectile.velocity = new Vector2(facing.y, -facing.x);

                    projectileCrabs.Add(newProjectile);
                    projectileSprites.Add(newSprite);
                }

                if(spawnDelay > 2f)
                {
                    SceneObject newIncoming = new SceneObject();
                    SpriteObject newSprite = new SpriteObject();
                    newSprite.Load("smallcrab.png");
                    newIncoming.AddChild(newSprite);
                    newSprite.Translate(-newSprite.Width * 0.5f, 0);
                    Random rngX = new Random();
                    Random rngY = new Random(rngX.Next());
                    newIncoming.SetPosition(rngX.Next(0,1600), rngY.Next(0, 900));
                    incomingCrabs.Add(newIncoming);
                    incomingSprites.Add(newSprite);
                    spawnDelay = 0;
                }

                foreach (SceneObject crab in incomingCrabs)
                {
                    if(MathF.Abs(crab.GlobalTransform.m3 - bigCrab.GlobalTransform.m3) > 5)
                    {
                        if(crab.GlobalTransform.m3 > bigCrab.GlobalTransform.m3)
                        {
                            crab.Translate(-50 * game.deltaTime, 0);
                        }
                        else if (crab.GlobalTransform.m3 < bigCrab.GlobalTransform.m3)
                        {
                            crab.Translate(50 * game.deltaTime, 0);
                        }
                    }
                    if (MathF.Abs(crab.GlobalTransform.m6 - bigCrab.GlobalTransform.m6) > 5)
                    {
                        if (crab.GlobalTransform.m6 > bigCrab.GlobalTransform.m6)
                        {
                            crab.Translate(0, -50 * game.deltaTime);
                        }
                        else if (crab.GlobalTransform.m6 < bigCrab.GlobalTransform.m6)
                        {
                            crab.Translate(0, 50 * game.deltaTime);
                        }
                    }
                    crab.Draw();
                }

                for (int i = 0; i < incomingCrabs.Count; i++)
                {
                    if (incomingCrabs[i].DistanceFrom(new Vector2(collisionCircle.GlobalTransform.m3, collisionCircle.GlobalTransform.m6)) < 60)
                    {
                        incomingCrabs.RemoveAt(i);
                    }
                }

                for (int h = 0; h < projectileCrabs.Count; h++)
                {
                    for (int i = 0; i < incomingCrabs.Count; i++)
                    {
                        if (incomingCrabs[i].DistanceFrom(new Vector2(projectileCrabs[h].GlobalTransform.m3, projectileCrabs[h].GlobalTransform.m6)) < 20)
                        {
                            incomingCrabs.RemoveAt(i);
                        }
                    }
                }

                bigCrab.Draw();
                bigCrab.Update(game.deltaTime);
                spawnDelay += game.deltaTime;

                EndDrawing();
            }

            CloseWindow();

            return 1;
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