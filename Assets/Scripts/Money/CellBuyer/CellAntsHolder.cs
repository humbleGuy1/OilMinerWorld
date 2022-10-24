using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class CellAntsHolder : MonoBehaviour
    {
        private List<Digger> _diggers = new List<Digger>();
        private List<Loader> _loaders = new List<Loader>();

        public int DiggersCount => _diggers.Count;
        public int LoadersCount => _loaders.Count;

        public void AddDigger(Digger digger) => _diggers.Add(digger);
        public void AddLoader(Loader loader) => _loaders.Add(loader);
        public void RemoveDigger(Digger digger) => _diggers.Remove(digger);
        public void RemoveLoader(Loader loader) => _loaders.Remove(loader);
        public bool ContainsDigger(Digger digger) => _diggers.Contains(digger);
        public bool ContainsLoader(Loader loader) => _loaders.Contains(loader);

    }
}
