using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class HitParameters
    {
        public GameObject Source { get; set; }
        public Collider2D Collider2D { get; set; }
        public GameObject Destination { get; set; }

        public HitParameters(GameObject source, 
            Collider2D collider2D, GameObject destination)
        {
            Source = source;
            Collider2D = collider2D;
            Destination = destination;
        }
    }
}
