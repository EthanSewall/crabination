using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.Raymath;
using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Crabination
{
    public class SceneObject
    {
        protected SceneObject parent = null;
        protected List<SceneObject> children = new List<SceneObject>();
        protected Matrix3 localTransform = new Matrix3();
        protected Matrix3 globalTransform = new Matrix3();
        public Vector2 velocity = new Vector2();
        public Vector2 acceleration = new Vector2();
        
        public Matrix3 LocalTransform
        {
            get { return localTransform; }
        }
        public Matrix3 GlobalTransform
        {
            get { return globalTransform; }
        }
        //accessors

        public SceneObject()
        {

        }
        ~SceneObject()
        {
            if (parent != null)
            {
                parent.RemoveChild(this);
            }
            foreach (SceneObject so in children)
            {
                so.parent = null;
            }
        }

        public void UpdateTransform()
        {
            if (parent != null)
            {
                globalTransform = parent.globalTransform * localTransform;
            }
            else
            {
                globalTransform = localTransform;
            }
            foreach (SceneObject child in children)
            {
                child.UpdateTransform();
            }
        }

        public void SetPosition(float x, float y)
        {
            localTransform.SetTranslate(x, y);
            UpdateTransform();
        }
        public void SetRotate(float radians)
        {
            localTransform.SetRotateZ(radians);
            UpdateTransform();
        }
        public void SetScale(float width, float height)
        {
            localTransform.SetScaled(width, height, 1);
            UpdateTransform();
        }
        public void Translate(float x, float y)
        {
            localTransform.Translate(x, y);
            UpdateTransform();
        }
        public void Rotate(float radians)
        {
            localTransform.RotateZ(radians);
            UpdateTransform();
        }
        public void Scale(float width, float height)
        {
            localTransform.Scale(width, height, 1);
            UpdateTransform();
        }

        public virtual void OnUpdate(float deltaTime)
        {

        }
        public virtual void OnDraw()
        {

        }

        public void Update(float deltaTime)
        {
            OnUpdate(deltaTime);
            foreach (SceneObject child in children)
            {
                child.Update(deltaTime);
            }
        }
        //these don't actually update or draw, but instead call this function in all children, and call the On() methods
        public void Draw()
        {
            OnDraw();
            foreach (SceneObject child in children)
            {
                child.Draw();
            }
        }

        public SceneObject Parent
        {
            get { return parent; }
        }
        public int GetChildCount()
        {
            return children.Count;
        }
        public SceneObject GetChild(int index)
        {
            return children[index];
        }
        public void AddChild(SceneObject child)
        {
            if (child.parent == null)
            {
                child.parent = this;
                children.Add(child);
            }
        }
        public void RemoveChild(SceneObject child)
        {
            if (children.Remove(child) == true)
            {
                child.parent = null;
            }
        }

        public bool IsInBounds()
        {
            bool h = true;

            if(GlobalTransform.m6 < 0)
            {
                h = false;
            }
            if (GlobalTransform.m6 > 900)
            {
                h = false;
            }
            if (GlobalTransform.m3 < 0)
            {
                h = false;
            }
            if (GlobalTransform.m3 > 1600)
            {
                h = false;
            }

            return h;
        }
    }

    public class SpriteObject : SceneObject
    {
        Raylib_cs.Texture2D texture = new Raylib_cs.Texture2D();

        public void Load(string path)
        {
            texture = LoadTexture(path);
        }
        public float Width
        {
            get { return texture.width; }
        }
        public float Height
        {
            get { return texture.height; }
        }

        public SpriteObject()
        {
            
        }

        public override void OnDraw()
        {
            float rotation = (float)Math.Atan2(globalTransform.m4, globalTransform.m1);

            DrawTextureEx(texture, new Vector2(globalTransform.m3, globalTransform.m6), rotation * (float)(180.0f / Math.PI),1, WHITE);

        }
    }
}
