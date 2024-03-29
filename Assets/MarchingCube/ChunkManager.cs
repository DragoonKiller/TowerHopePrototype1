using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace MarchingCube
{
    /// Manage and synchronize player position and mesh.
    /// Dynamically generate or delete meshes.
    [ExecuteAlways]
    [Serializable]
    public class ChunkManager : MonoBehaviour
    {
        public static ChunkManager inst;
        
        ChunkManager() => inst = this;
        
        /// Whether I shall refresh the mesh.
        public bool refresh;
        
        /// Remove all chunks and rebuild then.
        public bool reset;
        
        /// Configuration object, specified in unity editor inspector.
        public ChunkConfig config;
        
        /// Position tracer.
        public Protagonist protagonist;
        
        Vector2 curPos => protagonist.transform.position;
        
        Vector2 cachedCurPos;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Chunk ChunkOfId(Vector2Int loc)
        {
            var cur = GetCurrentChunks();
            if(cur.TryGetValue(loc, out var val)) return val.GetComponent<Chunk>();
            return null;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Chunk ChunkOfWorldPos(Vector2 pos) => ChunkOfId(config.ChunkBelong(pos));
        
        void Update()
        {
            if(reset)
            {
                void DestroyGameObject(Transform x) => DestroyImmediate(x.gameObject);
                this.transform.ForeachChild(DestroyGameObject);
                refresh = true;
                reset = false;
            }
            UpdateChunks(refresh);
            refresh = false;
        }
        
        [ThreadStatic] HashSet<Vector2Int> deadChunksCache;
        void UpdateChunks(bool ignoreLimits)
        {
            var cur = GetCurrentChunks();
            var nxt = GetRequiredChunksInOrder();
            
            // // Remove all chunks that shouldn't esist anymore.
            if(deadChunksCache == null) deadChunksCache = new HashSet<Vector2Int>();
            deadChunksCache.Clear();
            foreach(var i in cur) deadChunksCache.Add(i.Key);
            deadChunksCache.ExceptWith(nxt);
            foreach(var i in deadChunksCache) DestroyImmediate(cur[i]);
            
            // Bring up required chunks that previously not exist.
            int cc = 0;
            foreach(var i in nxt) if(!cur.ContainsKey(i))
            {
                Instantiate(config.chunkTemplate, this.transform).GetComponent<Chunk>().Setup(config, i);
                cc += 1;
                if(!ignoreLimits && cc >= config.maxUpdateChunk) break;
            }
        }
        
        // Find all currently exist chunk objects.
        // All chunk objects must be stored as sub-object of this component's GameObject.
        // All chunk objects are named by their grid location in syntax "x.y" e.g. "12.23".
        [ThreadStatic] Dictionary<Vector2Int, GameObject> currentChunksCache;
        Dictionary<Vector2Int, GameObject> GetCurrentChunks()
        {
            if(currentChunksCache == null) currentChunksCache = new Dictionary<Vector2Int, GameObject>();
            currentChunksCache.Clear();
            void AddChunk(GameObject x) => currentChunksCache[x.name.DecodeChunkOffset()] = x;
            this.gameObject.ForeachChild(AddChunk);
            return currentChunksCache;
        }
        
        List<Vector2Int> GetRequiredChunksInOrder()
        {
            var reqChunks = config.ChunksNearby(curPos);
            
            // Here we sort then by distance to the center for further use.
            // The more close the more important (firstly considered) the chunk is.
            reqChunks.Sort((a, b) =>
            {
                float da = curPos.To(config.OffsetCenter(a)).magnitude;
                float db = curPos.To(config.OffsetCenter(b)).magnitude;
                return da < db ? -1 : da > db ? 1 : 0;
            });
            
            return reqChunks;
        }
        
    }
    
    
}
