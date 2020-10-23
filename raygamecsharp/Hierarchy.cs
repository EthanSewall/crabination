using System;
using System.Collections.Generic;
using System.Text;

namespace Crabination
{
    class SceneObject
    {
        protected SceneObject parent = null;
        protected List<SceneObject> children = new List<SceneObject>();

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

    }
}
