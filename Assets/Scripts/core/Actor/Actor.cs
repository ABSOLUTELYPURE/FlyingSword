using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    public class Actor : MonoBehaviour
    {
        private float m_hp;
        private float m_def;
        private float m_atk;
        private string m_name;

        public virtual string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }
        public virtual float Atk
        {
            get
            {
                return m_atk;
            }

            set
            {
                m_atk = value;
            }
        }
        public virtual float HP
        {
            get
            {
                return m_hp;
            }

            set
            {
                m_hp = value;
            }
        }
        public virtual float Def
        {
            get
            {
                return m_def;
            }

            set
            {
                m_def = value;
            }
        }
    }
}
