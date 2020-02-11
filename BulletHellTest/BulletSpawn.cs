using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellTest
{
    class BulletSpawn
    {
        public Vector2 SpawnPosition { get; set; }

        public Vector2 SpawnVelocity { get; set; } // probably replace with SpawnPattern instead 

        private string BULLETSPAWN_DATA_LAYOUT = "{0},{1}.{2},{3}";
        public string ParsedData { get => string.Format(BULLETSPAWN_DATA_LAYOUT, SpawnPosition.X, SpawnPosition.Y, SpawnVelocity.X, SpawnVelocity.Y); }
        public BulletSpawn(Vector2 spawnPosition, Vector2 spawnVelocity)
        {
            SpawnPosition = spawnPosition;
            SpawnVelocity = spawnVelocity;
        }
    }
}
