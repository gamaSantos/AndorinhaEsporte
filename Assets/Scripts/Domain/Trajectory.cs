using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AndorinhaEsporte.Domain
{
    public class Trajectory : IEnumerable
    {
        public Trajectory()
        {
            _maxIndex = 0;
            points= new SortedList<int, Vector3>();
        }
        private SortedList<int, Vector3> points;
        private int _maxIndex;

        public void Add(Vector3 point)
        {
            _maxIndex = points.Count;
            points.Add(_maxIndex, point);
        }
        public Vector3 Last => points[_maxIndex];
        
        public IReadOnlyList<Vector3> Points => points.Values.ToList();

        public IEnumerator GetEnumerator()
        {
            return points.GetEnumerator();
        }
    }
}
