using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellTest
{
    class LevelKeypoint
    {
        public float TimePosition { get; set; }
        public string[] KeypointData { get => keypointDataList.ToArray(); }

        private List<string> keypointDataList;
        public LevelKeypoint(float timePosition, string initialData)
        {
            keypointDataList = new List<string>();
            TimePosition = timePosition;
            AddData(initialData);
        }

        public void AddData(string someData)
        {
            keypointDataList.Add(someData);
        }

        public void RemoveData(string someData)
        {
            keypointDataList.Remove(someData);
        }
    }
}
