using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Megumin
{
    [Serializable]
    public class GameObjectFilter
    {
        public Enableable<LayerMask> LayerMask = new Enableable<LayerMask>(false, 0);
        public Enableable<TagMask> TagMask = new Enableable<TagMask>(false, new TagMask());

        public bool Check(Component component)
        {
            if (component)
            {
                return Check(component.gameObject);
            }
            else
            {
                return false;
            }
        }

        public bool Check(GameObject gameObject)
        {
            if (gameObject)
            {
                if (LayerMask.Enabled)
                {
                    if ((1 << gameObject.layer & LayerMask.Value) != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (TagMask.Enabled)
                {
                    return TagMask.Value.HasFlag(gameObject.tag);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckTag(GameObject gameObject)
        {
            if (gameObject)
            {
                if (TagMask.Enabled)
                {
                    return TagMask.Value.HasFlag(gameObject.tag);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckLayer(GameObject gameObject)
        {
            if (gameObject)
            {
                if (LayerMask.Enabled)
                {
                    if ((1 << gameObject.layer & LayerMask.Value) != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }

}

