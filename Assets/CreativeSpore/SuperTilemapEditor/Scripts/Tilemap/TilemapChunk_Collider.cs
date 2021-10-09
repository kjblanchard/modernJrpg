// This is causing some errors with 2D colliders, enable it under your own responsibility
//#define ENABLE_MERGED_SUBTILE_COLLIDERS
#pragma warning disable 0436


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CreativeSpore.SuperTilemapEditor
{
    public partial class TilemapChunk
    {

        // Dispatch collision messages to the tilemap parent object
        #region Unity Collision Messages

        private void OnCollisionEnter(Collision collision)
        {
            ParentTilemap.SendMessage("OnCollisionEnter", collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ParentTilemap.SendMessage("OnCollisionEnter2D", collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnCollisionExit(Collision collision)
        {
            ParentTilemap.SendMessage("OnCollisionExit", collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            ParentTilemap.SendMessage("OnCollisionExit2D", collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnCollisionStay(Collision collision)
        {
            ParentTilemap.SendMessage("OnCollisionStay", collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            ParentTilemap.SendMessage("OnCollisionStay2D", collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnTriggerEnter(Collider other)
        {
            ParentTilemap.SendMessage("OnTriggerEnter", other, SendMessageOptions.DontRequireReceiver);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            ParentTilemap.SendMessage("OnTriggerEnter2D", collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnTriggerExit(Collider other)
        {
            ParentTilemap.SendMessage("OnTriggerExit", other, SendMessageOptions.DontRequireReceiver);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            ParentTilemap.SendMessage("OnTriggerExit2D", collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnTriggerStay(Collider other)
        {
            ParentTilemap.SendMessage("OnTriggerStay", other, SendMessageOptions.DontRequireReceiver);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            ParentTilemap.SendMessage("OnTriggerStay2D", collision, SendMessageOptions.DontRequireReceiver);
        }

        #endregion

        /// <summary>
        /// Next time UpdateColliderMesh is called, the collider mesh will be rebuild
        /// </summary>
        public void InvalidateMeshCollider()
        {
            m_needsRebuildColliders = true;
        }

        private bool m_needsRebuildColliders = false;
        public bool UpdateColliders()
        {
            if (ParentTilemap == null)
            {
                ParentTilemap = transform.parent.GetComponent<STETilemap>();
            }
            if (gameObject.layer != ParentTilemap.gameObject.layer)
                gameObject.layer = ParentTilemap.gameObject.layer;
            if (!gameObject.CompareTag(ParentTilemap.gameObject.tag))
                gameObject.tag = ParentTilemap.gameObject.tag;

            //+++ Free unused resources
            if (ParentTilemap.ColliderType != eColliderType._3D)
            {
                if (m_meshCollider != null)
                {
                    if (!s_isOnValidate)
                        DestroyImmediate(m_meshCollider);
                    else
                        m_meshCollider.enabled = false;
                }
            }

            //if (ParentTilemap.ColliderType != eColliderType._2D)
            if(m_needsRebuildColliders)
            {
                if (m_has2DColliders)
                {
                    m_has2DColliders = ParentTilemap.ColliderType == eColliderType._2D;
                    System.Type oppositeCollider2DType = ParentTilemap.Collider2DType != e2DColliderType.EdgeCollider2D ? typeof(EdgeCollider2D) : typeof(PolygonCollider2D);
                    System.Type collidersToDestroy = ParentTilemap.ColliderType != eColliderType._2D ? typeof(Collider2D) : oppositeCollider2DType;
                    var aCollider2D = GetComponents(collidersToDestroy);
                    for (int i = 0; i < aCollider2D.Length; ++i)
                    {
                        if (!s_isOnValidate) 
                            DestroyImmediate(aCollider2D[i]); 
                        else 
                            ((Collider2D)aCollider2D[i]).enabled = false;
                    }
                }
            }
            //---

            if (ParentTilemap.ColliderType == eColliderType._3D)
            {
                if (m_meshCollider == null)
                {
                    m_meshCollider = GetComponent<MeshCollider>();
                    if (m_meshCollider == null && ParentTilemap.ColliderType == eColliderType._3D)
                    {
                        m_meshCollider = gameObject.AddComponent<MeshCollider>();
                    }
                }

                if (ParentTilemap.IsTrigger)
                {
                    m_meshCollider.convex = true;
                    m_meshCollider.isTrigger = true;
                }
                else
                {
                    m_meshCollider.isTrigger = false;
                    m_meshCollider.convex = false;
                }
                m_meshCollider.sharedMaterial = ParentTilemap.PhysicMaterial;

                //NOTE: m_meshCollider.sharedMesh is equal to m_meshFilter.sharedMesh when the script is attached or reset
                if (m_meshCollider != null && (m_meshCollider.sharedMesh == null || m_meshCollider.sharedMesh == m_meshFilter.sharedMesh))
                {
                    m_meshCollider.sharedMesh = new Mesh();
                    m_meshCollider.sharedMesh.hideFlags = HideFlags.DontSave;
                    m_meshCollider.sharedMesh.name = ParentTilemap.name + "_collmesh";
                    m_needsRebuildColliders = true;
                }
            } 
            
            if (m_needsRebuildColliders)
            {
                m_needsRebuildColliders = false;
                bool isEmpty = FillColliderMeshData();
                if (ParentTilemap.ColliderType == eColliderType._3D)
                {
                    Mesh mesh = m_meshCollider.sharedMesh;
                    mesh.Clear();
#if UNITY_5_0 || UNITY_5_1
                    mesh.vertices = s_meshCollVertices.ToArray();
                    mesh.triangles = s_meshCollTriangles.ToArray();
#else
                    mesh.SetVertices(s_meshCollVertices);
                    mesh.SetTriangles(s_meshCollTriangles, 0);
#endif
                    mesh.RecalculateNormals(); // needed by Gizmos.DrawWireMesh
                    m_meshCollider.sharedMesh = null; // for some reason this fix showing the green lines of the collider mesh
                    m_meshCollider.sharedMesh = mesh;
                }
                return isEmpty;
            }
            return true;
        }

        private void DestroyColliderMeshIfNeeded()
        {
            MeshCollider meshCollider = GetComponent<MeshCollider>();
            if (meshCollider != null && meshCollider.sharedMesh != null
                && (meshCollider.sharedMesh.hideFlags & HideFlags.DontSave) != 0)
            {
                //Debug.Log("Destroy Mesh of " + name);
                DestroyImmediate(meshCollider.sharedMesh);
            }
        }        

        private static List<Vector2> s_fullCollTileVertices = new List<Vector2>() { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
        private static Vector2[] s_neighborSegmentMinMax = new Vector2[4];
        private static List<Vector2> s_vectorListLookup = new List<Vector2>();
        private static List<LinkedList<Vector2>> s_openEdges = new List<LinkedList<Vector2>>(50);
        private static TileColliderData[] s_neighborTileCollData = new TileColliderData[4];
        private static List<Component> s_colliders2DLookup = new List<Component>();
        private bool FillColliderMeshData()
        {
            //Debug.Log( "[" + ParentTilemap.name + "] FillColliderMeshData -> " + name);
            if (Tileset == null || ParentTilemap.ColliderType == eColliderType.None)
            {
                return false;
            }

            System.Type collider2DType = ParentTilemap.Collider2DType == e2DColliderType.EdgeCollider2D ? typeof(EdgeCollider2D) : typeof(PolygonCollider2D);
            if (ParentTilemap.ColliderType == eColliderType._3D)
            {
                int totalTiles = m_width * m_height;
                if (s_meshCollVertices == null)
                {
                    s_meshCollVertices = new List<Vector3>(totalTiles * 4);
                    s_meshCollTriangles = new List<int>(totalTiles * 6);
                }
                else
                {
                    s_meshCollVertices.Clear();
                    s_meshCollTriangles.Clear();
                }
            }
            else //if (ParentTilemap.ColliderType == eColliderType._2D)
            {
                m_has2DColliders = true;
                s_openEdges.Clear();
                s_colliders2DLookup.Clear();
                GetComponents(collider2DType, s_colliders2DLookup); 
            }
            
            bool isEmpty = true;

            if (ParentTilemap.ColliderGenerationMethod == STETilemap.eColliderGenerationMethod.Default)
            {
                UnityEngine.Profiling.Profiler.BeginSample("Collider Default");
                isEmpty = _ProcessTileColliders_Default();
                UnityEngine.Profiling.Profiler.EndSample();
            }
            else //if(ParentTilemap.ColliderGenerationMethod == STETilemap.eColliderGenerationMethod.Clipper)
            {
                UnityEngine.Profiling.Profiler.BeginSample("Collider Clipper");
                isEmpty = _ProcessTileColliders_Clipper();
                UnityEngine.Profiling.Profiler.EndSample();
            }

            return !isEmpty;
        }

        // Credits: Angus Johnson http://www.angusj.com/delphi/clipper.php for the Clipper C# implementation
        static ClipperLib.Clipper s_clipperInstance = new ClipperLib.Clipper();
        static List<List<ClipperLib.IntPoint>> s_clipper_path = new List<List<ClipperLib.IntPoint>>();
        static List<List<ClipperLib.IntPoint>> s_clipper_solution = new List<List<ClipperLib.IntPoint>>();
        static List<Vector2> s_collVerticesCache = new List<Vector2>();
        private bool _ProcessTileColliders_Clipper()
        {
            const float pointPrecission = 0.001f; // NOTE: use relation between tile pixel size and cell size value?
            float halvedCollDepth = ParentTilemap.ColliderDepth / 2f;
            bool isEmpty = true;
            int shapeCounter = 0;

            // Iterate tiles and populate clipper path with all the shapes found
            for (int ty = 0, tileIdx = 0; ty < m_height; ++ty)
            {
                for (int tx = 0; tx < m_width; ++tx, ++tileIdx)
                {
                    uint tileData = m_tileDataList[tileIdx];
                    if (tileData != Tileset.k_TileData_Empty)
                    {
                        int tileId = (int)(tileData & Tileset.k_TileDataMask_TileId);
                        Tile tile = Tileset.GetTile(tileId);
                        s_collVerticesCache.Clear();
#if ENABLE_MERGED_SUBTILE_COLLIDERS
                        TilesetBrush brush = ParentTilemap.Tileset.FindBrush(Tileset.GetBrushIdFromTileData(tileData));
                        if (brush)
                            brush.GetMergedSubtileColliderVertices(ParentTilemap, GridPosX + tx, GridPosY + ty, tileData, s_collVerticesCache);
#endif
                        if (tile != null)
                        {
                            bool hasMergedColliders = s_collVerticesCache.Count > 0;

                            if (tile.collData.type != eTileCollider.None || hasMergedColliders)
                            {                                
                                isEmpty = false;
                                // Populate path shapes
                                {
                                    float px0 = tx * CellSize.x;
                                    float py0 = ty * CellSize.y;
                                    List<Vector2> collVertices = s_collVerticesCache;
                                    if (!hasMergedColliders)
                                    {
                                        if(tile.collData.type == eTileCollider.Full)
                                            collVertices = s_fullCollTileVertices;
                                        else
                                        {
                                            collVertices.AddRange(tile.collData.vertices);
                                            tile.collData.ApplyFlippingFlags(tileData, collVertices);
                                        }
                                    }

                                    List<ClipperLib.IntPoint> shapePoints = null;
                                    if (shapeCounter >= s_clipper_path.Count)
                                    {
                                        shapePoints = new List<ClipperLib.IntPoint>();
                                        s_clipper_path.Add(shapePoints);
                                    }
                                    else
                                    {
                                        shapePoints = s_clipper_path[shapeCounter];
                                        shapePoints.Clear();
                                    }
                                    ++shapeCounter;

                                    for (int i = 0; i < collVertices.Count; ++i)
                                    {                                        
                                        Vector2 s0 = collVertices[i];
                                        if (hasMergedColliders) ++i; // add ++i; in this case to go 2 by 2 because the collVertices for merged colliders will have the segments in pairs
                                        
                                        // convert to local positions
                                        s0.x = px0 + CellSize.x * s0.x; s0.y = py0 + CellSize.y * s0.y;

                                        var fixedPoint = new ClipperLib.IntPoint(s0.x / pointPrecission, s0.y / pointPrecission);
                                        shapePoints.Add(fixedPoint);                                                                                
                                    }
                                }
                            }
                        }
                    }
                }
            }

            s_clipper_path.RemoveRange(shapeCounter, s_clipper_path.Count - shapeCounter);

            s_clipperInstance.Clear();

            s_clipperInstance.AddPaths(s_clipper_path, ClipperLib.PolyType.ptSubject, true);
            s_clipperInstance.Execute(ClipperLib.ClipType.ctUnion, s_clipper_solution, ClipperLib.PolyFillType.pftEvenOdd, ClipperLib.PolyFillType.pftEvenOdd);

            // Generate Unity Colliders
            if (ParentTilemap.ColliderType == eColliderType._3D)
            {
                foreach (var shape in s_clipper_solution)
                {
                    for (int i = 0, j = 1; i < shape.Count; i++, j++)
                    {
                        var p0 = shape[i];
                        var p1 = shape[j < shape.Count? j : 0];
                        Vector2 s0 = new Vector2(p0.X * pointPrecission, p0.Y * pointPrecission);
                        Vector2 s1 = new Vector2(p1.X * pointPrecission, p1.Y * pointPrecission);

                        int collVertexIdx = s_meshCollVertices.Count;
                        s_meshCollVertices.Add(new Vector3(s0.x, s0.y, -halvedCollDepth));
                        s_meshCollVertices.Add(new Vector3(s0.x, s0.y, halvedCollDepth));
                        s_meshCollVertices.Add(new Vector3(s1.x, s1.y, halvedCollDepth));
                        s_meshCollVertices.Add(new Vector3(s1.x, s1.y, -halvedCollDepth));

                        s_meshCollTriangles.Add(collVertexIdx + 0);
                        s_meshCollTriangles.Add(collVertexIdx + 1);
                        s_meshCollTriangles.Add(collVertexIdx + 2);
                        s_meshCollTriangles.Add(collVertexIdx + 2);
                        s_meshCollTriangles.Add(collVertexIdx + 3);
                        s_meshCollTriangles.Add(collVertexIdx + 0);
                    }
                }
            }
            else //if( ParentTilemap.ColliderType == eColliderType._2D )
            {
                Create2DColliders_Clipper(pointPrecission);
            }

            return isEmpty;
        }

        //Inline support method
        Func<TilemapChunk, int, System.Type, Collider2D> __GetCollider2D = delegate ( TilemapChunk chunk, int idx, System.Type collider2DType)
        {
            bool reuseCollider = idx < s_colliders2DLookup.Count;
            Collider2D collider2D = reuseCollider ? (Collider2D)s_colliders2DLookup[idx] : (Collider2D)chunk.gameObject.AddComponent(collider2DType);
            collider2D.enabled = true;
            collider2D.isTrigger = chunk.ParentTilemap.IsTrigger;
            collider2D.sharedMaterial = chunk.ParentTilemap.PhysicMaterial2D;
            return collider2D;
        };

        private bool _ProcessTileColliders_Default()
        {
            bool isEmpty = true;
            float halvedCollDepth = ParentTilemap.ColliderDepth / 2f;
            for (int ty = 0, tileIdx = 0; ty < m_height; ++ty)
            {
                for (int tx = 0; tx < m_width; ++tx, ++tileIdx)
                {
                    uint tileData = m_tileDataList[tileIdx];
                    if (tileData != Tileset.k_TileData_Empty)
                    {
                        int tileId = (int)(tileData & Tileset.k_TileDataMask_TileId);
                        Tile tile = Tileset.GetTile(tileId);
                        s_collVerticesCache.Clear();
#if ENABLE_MERGED_SUBTILE_COLLIDERS
                        TilesetBrush brush = ParentTilemap.Tileset.FindBrush(Tileset.GetBrushIdFromTileData(tileData));
                        if (brush)
                            brush.GetMergedSubtileColliderVertices(ParentTilemap, GridPosX + tx, GridPosY + ty, tileData, s_collVerticesCache);
#endif
                        if (tile != null)
                        {
                            bool hasMergedColliders = s_collVerticesCache.Count > 0;

                            if (tile.collData.type != eTileCollider.None || hasMergedColliders)
                            {
                                isEmpty = false;
                                List<Vector2> collVertices = s_collVerticesCache;
                                if (!hasMergedColliders)
                                {
                                    if (tile.collData.type == eTileCollider.Full)
                                        collVertices = s_fullCollTileVertices;
                                    else
                                    {                                        
                                        collVertices.AddRange(tile.collData.vertices);
                                        tile.collData.ApplyFlippingFlags(tileData, collVertices);
                                    }
                                }

                                // Special Case: for polygon colliders, add option to avoid connecting the tile edges
                                // NOTE: has no support for merged subtile colliders
                                if (ParentTilemap.Collider2DType == e2DColliderType.PolygonCollider2D && ParentTilemap.Collider2DOptimizationDisabled)
                                {
                                    float px0 = tx * CellSize.x;
                                    float py0 = ty * CellSize.y;
                                    s_vectorListLookup.Clear();
                                    for (int i = 0, size = collVertices.Count; i < size; i++)
                                    {
                                        Vector2 s0 = collVertices[i];
                                        // Update to world positions
                                        s0.x = px0 + CellSize.x * s0.x; s0.y = py0 + CellSize.y * s0.y;
                                        s_vectorListLookup.Add(s0);
                                    }

                                    s_openEdges.Add(new LinkedList<Vector2>(s_vectorListLookup));
                                    continue;
                                }
                                //---
                                int neighborCollFlags = 0; // don't remove, even using neighborTileCollData, neighborTileCollData is not filled if tile is empty
                                bool isSurroundedByFullColliders = true;
                                for (int i = 0; i < s_neighborSegmentMinMax.Length; ++i)
                                {
                                    s_neighborSegmentMinMax[i].x = float.MaxValue;
                                    s_neighborSegmentMinMax[i].y = float.MinValue;
                                }
                                System.Array.Clear(s_neighborTileCollData, 0, s_neighborTileCollData.Length);

                                if (!hasMergedColliders)
                                {
                                    for (int i = 0; i < 4; ++i)
                                    {
                                        uint neighborTileData;
                                        bool isTriggerOrPolygon = ParentTilemap.IsTrigger ||
                                            ParentTilemap.ColliderType == eColliderType._2D &&
                                            ParentTilemap.Collider2DType == e2DColliderType.PolygonCollider2D;
                                        switch (i)
                                        {
                                            case 0:  // Up Tile
                                                neighborTileData = (tileIdx + m_width) < m_tileDataList.Count ?
                                                m_tileDataList[tileIdx + m_width]
                                                :
                                                isTriggerOrPolygon ? Tileset.k_TileData_Empty : ParentTilemap.GetTileData(GridPosX + tx, GridPosY + ty + 1); break;
                                            case 1: // Right Tile
                                                neighborTileData = (tileIdx + 1) % m_width != 0 ? //(tileIdx + 1) < m_tileDataList.Count ? 
                                                m_tileDataList[tileIdx + 1]
                                                :
                                                isTriggerOrPolygon ? Tileset.k_TileData_Empty : ParentTilemap.GetTileData(GridPosX + tx + 1, GridPosY + ty); break;
                                            case 2: // Down Tile
                                                neighborTileData = tileIdx >= m_width ?
                                                m_tileDataList[tileIdx - m_width]
                                                :
                                                isTriggerOrPolygon ? Tileset.k_TileData_Empty : ParentTilemap.GetTileData(GridPosX + tx, GridPosY + ty - 1); break;
                                            case 3: // Left Tile
                                                neighborTileData = tileIdx % m_width != 0 ? //neighborTileId = tileIdx >= 1 ? 
                                                m_tileDataList[tileIdx - 1]
                                                :
                                                isTriggerOrPolygon ? Tileset.k_TileData_Empty : ParentTilemap.GetTileData(GridPosX + tx - 1, GridPosY + ty); break;
                                            default: neighborTileData = Tileset.k_TileData_Empty; break;
                                        }

                                        int neighborTileId = (int)(neighborTileData & Tileset.k_TileDataMask_TileId);
#if UNITY_EDITOR
                                        if (neighborTileId != Tileset.k_TileId_Empty && neighborTileId >= Tileset.Tiles.Count)
                                            Debug.LogWarning("Wrong neighbor tileId " + neighborTileId + " not found in the tileset!");
#endif
                                        if (neighborTileId != Tileset.k_TileId_Empty && neighborTileId < Tileset.Tiles.Count)
                                        {
                                            Vector2 segmentMinMax;
                                            TileColliderData neighborTileCollider;
                                            neighborTileCollider = Tileset.Tiles[neighborTileId].collData;
                                            if ((neighborTileData & (Tileset.k_TileFlag_FlipH | Tileset.k_TileFlag_FlipV | Tileset.k_TileFlag_Rot90)) != 0)
                                            {
                                                neighborTileCollider = neighborTileCollider.Clone();
                                                if ((neighborTileData & Tileset.k_TileFlag_FlipH) != 0) neighborTileCollider.FlipH();
                                                if ((neighborTileData & Tileset.k_TileFlag_FlipV) != 0) neighborTileCollider.FlipV();
                                                if ((neighborTileData & Tileset.k_TileFlag_Rot90) != 0) neighborTileCollider.Rot90();
                                            }
                                            s_neighborTileCollData[i] = neighborTileCollider;
                                            isSurroundedByFullColliders &= (neighborTileCollider.type == eTileCollider.Full);

                                            if (neighborTileCollider.type == eTileCollider.None)
                                            {
                                                segmentMinMax = new Vector2(float.MaxValue, float.MinValue); //NOTE: x will be min, y will be max
                                            }
                                            else if (neighborTileCollider.type == eTileCollider.Full)
                                            {
                                                segmentMinMax = new Vector2(0f, 1f); //NOTE: x will be min, y will be max
                                                neighborCollFlags |= (1 << i);
                                            }
                                            else
                                            {
                                                segmentMinMax = new Vector2(float.MaxValue, float.MinValue); //NOTE: x will be min, y will be max
                                                neighborCollFlags |= (1 << i);
                                                for (int j = 0; j < neighborTileCollider.vertices.Length; ++j)
                                                {
                                                    Vector2 v = neighborTileCollider.vertices[j];
                                                    {
                                                        if (i == 0 && v.y == 0 || i == 2 && v.y == 1) //Top || Bottom
                                                        {
                                                            if (v.x < segmentMinMax.x) segmentMinMax.x = v.x;
                                                            if (v.x > segmentMinMax.y) segmentMinMax.y = v.x;
                                                        }
                                                        else if (i == 1 && v.x == 0 || i == 3 && v.x == 1) //Right || Left
                                                        {
                                                            if (v.y < segmentMinMax.x) segmentMinMax.x = v.y;
                                                            if (v.y > segmentMinMax.y) segmentMinMax.y = v.y;
                                                        }
                                                    }
                                                }
                                            }
                                            s_neighborSegmentMinMax[i] = segmentMinMax;
                                        }
                                        else
                                        {
                                            isSurroundedByFullColliders = false;
                                        }
                                    }
                                }
                                // Create Mesh Colliders
                                if (isSurroundedByFullColliders && !hasMergedColliders)
                                {
                                    //Debug.Log(" Surrounded! " + tileIdx);
                                }
                                else
                                {
                                    float px0 = tx * CellSize.x;
                                    float py0 = ty * CellSize.y;

                                    for (int i = 0; i < collVertices.Count; ++i)
                                    {
                                        //Special case: Tile colliders with only two vertices
                                        if (collVertices.Count == 2 && i == 1)
                                            continue;

                                        Vector2 s0 = collVertices[i];
                                        Vector2 s1 = collVertices[i == (collVertices.Count - 1) ? 0 : i + 1];
                                        if (hasMergedColliders) ++i; // add ++i; in this case to go 2 by 2 because the collVertices for merged colliders will have the segments in pairs

                                        // full collider optimization
                                        if ((tile.collData.type == eTileCollider.Full) &&
                                            (
                                            (i == 0 && s_neighborTileCollData[3].type == eTileCollider.Full) || // left tile has collider
                                            (i == 1 && s_neighborTileCollData[0].type == eTileCollider.Full) || // top tile has collider
                                            (i == 2 && s_neighborTileCollData[1].type == eTileCollider.Full) || // right tile has collider
                                            (i == 3 && s_neighborTileCollData[2].type == eTileCollider.Full)  // bottom tile has collider
                                            )
                                        )
                                        {
                                            continue;
                                        }
                                        // polygon collider optimization
                                        else // if( tileCollData.type == eTileCollider.Polygon ) Or Full colliders if neighbor is not Full as well
                                        {
                                            Vector2 n, m;
                                            if (s0.y == 1f && s1.y == 1f) // top side
                                            {
                                                if ((neighborCollFlags & 0x1) != 0) // top tile has collider
                                                {
                                                    n = s_neighborSegmentMinMax[0];
                                                    if (n.x < n.y && n.x <= s0.x && n.y >= s1.x)
                                                    {
                                                        continue;
                                                    }
                                                }
                                            }
                                            else if (s0.x == 1f && s1.x == 1f) // right side
                                            {
                                                if ((neighborCollFlags & 0x2) != 0) // right tile has collider
                                                {
                                                    n = s_neighborSegmentMinMax[1];
                                                    if (n.x < n.y && n.x <= s1.y && n.y >= s0.y)
                                                    {
                                                        continue;
                                                    }
                                                }
                                            }
                                            else if (s0.y == 0f && s1.y == 0f) // bottom side
                                            {
                                                if ((neighborCollFlags & 0x4) != 0) // bottom tile has collider
                                                {
                                                    n = s_neighborSegmentMinMax[2];
                                                    if (n.x < n.y && n.x <= s1.x && n.y >= s0.x)
                                                    {
                                                        continue;
                                                    }
                                                }
                                            }
                                            else if (s0.x == 0f && s1.x == 0f) // left side
                                            {
                                                if ((neighborCollFlags & 0x8) != 0) // left tile has collider
                                                {
                                                    n = s_neighborSegmentMinMax[3];
                                                    if (n.x < n.y && n.x <= s0.y && n.y >= s1.y)
                                                    {
                                                        continue;
                                                    }
                                                }
                                            }
                                            else if (s0.y == 1f && s1.x == 1f) // top - right diagonal
                                            {
                                                if ((neighborCollFlags & 0x1) != 0 && (neighborCollFlags & 0x2) != 0)
                                                {
                                                    n = s_neighborSegmentMinMax[0];
                                                    m = s_neighborSegmentMinMax[1];
                                                    if ((n.x < n.y && n.x <= s0.x && n.y == 1f) && (m.x < m.y && m.x <= s1.y && m.y == 1f))
                                                    {
                                                        continue;
                                                    }
                                                }
                                            }
                                            else if (s0.x == 1f && s1.y == 0f) // right - bottom diagonal
                                            {
                                                if ((neighborCollFlags & 0x2) != 0 && (neighborCollFlags & 0x4) != 0)
                                                {
                                                    n = s_neighborSegmentMinMax[1];
                                                    m = s_neighborSegmentMinMax[2];
                                                    if ((n.x < n.y && n.x == 0f && n.y >= s0.y) && (m.x < m.y && m.x <= s1.x && m.y == 1f))
                                                    {
                                                        continue;
                                                    }
                                                }
                                            }
                                            else if (s0.y == 0f && s1.x == 0f) // bottom - left diagonal
                                            {
                                                if ((neighborCollFlags & 0x4) != 0 && (neighborCollFlags & 0x8) != 0)
                                                {
                                                    n = s_neighborSegmentMinMax[2];
                                                    m = s_neighborSegmentMinMax[3];
                                                    if ((n.x < n.y && n.x == 0f && n.y >= s0.x) && (m.x < m.y && m.x == 0f && m.y >= s1.y))
                                                    {
                                                        continue;
                                                    }
                                                }
                                            }
                                            else if (s0.x == 0f && s1.y == 1f) // left - top diagonal
                                            {
                                                if ((neighborCollFlags & 0x8) != 0 && (neighborCollFlags & 0x1) != 0)
                                                {
                                                    n = s_neighborSegmentMinMax[3];
                                                    m = s_neighborSegmentMinMax[0];
                                                    if ((n.x < n.y && n.x <= s0.y && n.y == 1f) && (m.x < m.y && m.x == 0f && m.y >= s1.x))
                                                    {
                                                        continue;
                                                    }
                                                }
                                            }
                                        }
                                        // Update s0 and s1 to world positions
                                        s0.x = px0 + CellSize.x * s0.x; s0.y = py0 + CellSize.y * s0.y;
                                        s1.x = px0 + CellSize.x * s1.x; s1.y = py0 + CellSize.y * s1.y;
                                        if (ParentTilemap.ColliderType == eColliderType._3D)
                                        {
                                            int collVertexIdx = s_meshCollVertices.Count;
                                            s_meshCollVertices.Add(new Vector3(s0.x, s0.y, -halvedCollDepth));
                                            s_meshCollVertices.Add(new Vector3(s0.x, s0.y, halvedCollDepth));
                                            s_meshCollVertices.Add(new Vector3(s1.x, s1.y, halvedCollDepth));
                                            s_meshCollVertices.Add(new Vector3(s1.x, s1.y, -halvedCollDepth));

                                            s_meshCollTriangles.Add(collVertexIdx + 0);
                                            s_meshCollTriangles.Add(collVertexIdx + 1);
                                            s_meshCollTriangles.Add(collVertexIdx + 2);
                                            s_meshCollTriangles.Add(collVertexIdx + 2);
                                            s_meshCollTriangles.Add(collVertexIdx + 3);
                                            s_meshCollTriangles.Add(collVertexIdx + 0);
                                        }
                                        else //if( ParentTilemap.ColliderType == eColliderType._2D )
                                        {
                                            int linkedSegments = 0;
                                            int segmentIdxToMerge = -1;
                                            for (int edgeIdx = s_openEdges.Count - 1; edgeIdx >= 0 && linkedSegments < 2; --edgeIdx)
                                            {
                                                LinkedList<Vector2> edgeSegments = s_openEdges[edgeIdx];
                                                if (edgeSegments.First.Value == edgeSegments.Last.Value)
                                                    continue; //skip closed edges
                                                if (edgeSegments.Last.Value == s0)
                                                {
                                                    if (segmentIdxToMerge >= 0)
                                                    {
                                                        LinkedList<Vector2> segmentToMerge = s_openEdges[segmentIdxToMerge];
                                                        if (s0 == segmentToMerge.First.Value)
                                                        {
                                                            for (LinkedListNode<Vector2> node = segmentToMerge.First.Next; node != null; node = node.Next)
                                                                edgeSegments.AddLast(node.Value);
                                                            s_openEdges.RemoveAt(segmentIdxToMerge);
                                                        }
                                                        /* Cannot join head with head or tail with tail, it will change the segment normal
                                                        else
                                                            for (LinkedListNode<Vector2> node = segmentToMerge.Last.Previous; node != null; node = node.Previous)
                                                                edgeSegments.AddLast(node.Value);*/
                                                    }
                                                    else
                                                    {
                                                        segmentIdxToMerge = edgeIdx;
                                                        edgeSegments.AddLast(s1);
                                                    }
                                                    ++linkedSegments;
                                                }
                                                /* Cannot join head with head or tail with tail, it will change the segment normal
                                                else if( edgeSegments.Last.Value == s1 )                                                
                                                else if (edgeSegments.First.Value == s0)*/
                                                else if (edgeSegments.First.Value == s1)
                                                {
                                                    if (segmentIdxToMerge >= 0)
                                                    {
                                                        LinkedList<Vector2> segmentToMerge = s_openEdges[segmentIdxToMerge];
                                                        if (s1 == segmentToMerge.Last.Value)
                                                        {
                                                            for (LinkedListNode<Vector2> node = edgeSegments.First.Next; node != null; node = node.Next)
                                                                segmentToMerge.AddLast(node.Value);
                                                            s_openEdges.RemoveAt(edgeIdx);
                                                        }
                                                        /* Cannot join head with head or tail with tail, it will change the segment normal
                                                        else
                                                            for (LinkedListNode<Vector2> node = edgeSegments.First.Next; node != null; node = node.Next)
                                                                segmentToMerge.AddFirst(node.Value);*/
                                                    }
                                                    else
                                                    {
                                                        segmentIdxToMerge = edgeIdx;
                                                        edgeSegments.AddFirst(s0);
                                                    }
                                                    ++linkedSegments;
                                                }
                                            }
                                            if (linkedSegments == 0)
                                            {
                                                LinkedList<Vector2> newEdge = new LinkedList<Vector2>();
                                                newEdge.AddFirst(s0);
                                                newEdge.AddLast(s1);
                                                s_openEdges.Add(newEdge);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (ParentTilemap.ColliderType == eColliderType._2D)
            {
                //+++ Process Edges
                //(NOTE: this was added to fix issues related with lighting, otherwise leave this commented)                
                if (ParentTilemap.Collider2DType != e2DColliderType.PolygonCollider2D || !ParentTilemap.Collider2DOptimizationDisabled)
                {
                    // Remove vertex inside a line
                    RemoveRedundantVertices(s_openEdges);

                    // Split segments (NOTE: This is not working with polygon colliders)
                    /*/ commented unless necessary for performance reasons
                    if (ParentTilemap.Collider2DType == e2DColliderType.EdgeCollider2D)
                    {
                        openEdges = SplitSegments(openEdges);
                    }
                    //*/
                }
                //---

                Create2DColliders_Default();
            }

            return isEmpty;
        }

        void Create2DColliders_Clipper(float pointPrecission)
        {
            System.Type collider2DType = ParentTilemap.Collider2DType == e2DColliderType.EdgeCollider2D ? typeof(EdgeCollider2D) : typeof(PolygonCollider2D);
            int colliderCount;
            if (ParentTilemap.Collider2DType == e2DColliderType.EdgeCollider2D)
            {
                colliderCount = s_clipper_solution.Count;
                for (int i = 0; i < s_clipper_solution.Count; i++)
                {
                    var shape = s_clipper_solution[i];
                    EdgeCollider2D collider2D = __GetCollider2D(this, i, collider2DType) as EdgeCollider2D;
                    collider2D.points = shape
                        .Concat(new ClipperLib.IntPoint[] { shape[0] }) // edges need to close the shape with the last point
                        .Select(p => new Vector2(p.X * pointPrecission, p.Y * pointPrecission)).ToArray();
                }
            }
            else
            {
                colliderCount = 1;
                PolygonCollider2D collider2D = __GetCollider2D(this, 0, collider2DType) as PolygonCollider2D;
                collider2D.enabled = false; // improves performance by reducing the calls to Physics2D.ColliderCleanup
                if (collider2D.pathCount != s_clipper_solution.Count)
                    collider2D.pathCount = s_clipper_solution.Count;
                for (int i = 0; i < s_clipper_solution.Count; i++)
                {
                    collider2D.SetPath(i, s_clipper_solution[i].Select(p => new Vector2(p.X * pointPrecission, p.Y * pointPrecission)).ToArray());
                }
                collider2D.enabled = true;
            }

            //Destroy/Disable unused colliders
            for (int i = colliderCount; i < s_colliders2DLookup.Count; ++i)
            {
                if (!s_isOnValidate)
                    DestroyImmediate(s_colliders2DLookup[i]);
                else
                    ((Collider2D)s_colliders2DLookup[i]).enabled = false;
            }
        }

        void Create2DColliders_Default()
        {
            System.Type collider2DType = ParentTilemap.Collider2DType == e2DColliderType.EdgeCollider2D ? typeof(EdgeCollider2D) : typeof(PolygonCollider2D);
            int colliderCount;
            if (ParentTilemap.Collider2DType == e2DColliderType.EdgeCollider2D)
            {
                colliderCount = s_openEdges.Count;
                for (int i = 0; i < s_openEdges.Count; i++)
                {
                    var shape = s_openEdges[i];
                    EdgeCollider2D collider2D = __GetCollider2D(this, i, collider2DType) as EdgeCollider2D;
                    collider2D.points = shape.ToArray();
                }
            }
            else
            {
                colliderCount = 1;
                PolygonCollider2D collider2D = __GetCollider2D(this, 0, collider2DType) as PolygonCollider2D;
                collider2D.enabled = false; // improves performance by reducing the calls to Physics2D.ColliderCleanup
                if (collider2D.pathCount != s_openEdges.Count)
                    collider2D.pathCount = s_openEdges.Count;
                for (int i = 0; i < s_openEdges.Count; i++)
                {
                    collider2D.SetPath(i, s_openEdges[i].ToArray());
                }
                collider2D.enabled = true;
            }

            //Destroy/Disable unused colliders
            for (int i = colliderCount; i < s_colliders2DLookup.Count; ++i)
            {
                if (!s_isOnValidate)
                    DestroyImmediate(s_colliders2DLookup[i]);
                else
                    ((Collider2D)s_colliders2DLookup[i]).enabled = false;
            }
        }

        void RemoveRedundantVertices(List<LinkedList<Vector2>> edgeList)
        {
            for (int i = 0; i < edgeList.Count; ++i)
            {
                RemoveRedundantVertices(edgeList[i]);
            }
        }
        void RemoveRedundantVertices(LinkedList<Vector2> edgeVertices)
        {
            LinkedListNode<Vector2> iter = edgeVertices.First;
            while (iter != edgeVertices.Last)
            {
                float perpDot;
                //Special case for first node if this is a closed edge
                if (iter == edgeVertices.First)
                {
                    if (iter.Value == edgeVertices.Last.Value)
                    {
                        perpDot = PerpDot(iter.Value, edgeVertices.Last.Previous.Value, iter.Next.Value);
                        if (Mathf.Abs(perpDot) <= Vector2.kEpsilon)
                        {
                            edgeVertices.RemoveFirst();
                            edgeVertices.Last.Value = edgeVertices.First.Value;
                            iter = edgeVertices.First;
                        }
                        else
                        {
                            iter = iter.Next;
                        }
                    }
                    else
                    {
                        iter = iter.Next;
                    }
                }
                else
                {
                    perpDot = PerpDot(iter.Value, iter.Previous.Value, iter.Next.Value);
                    iter = iter.Next;
                    if (Mathf.Abs(perpDot) <= Vector2.kEpsilon)
                    {
                        edgeVertices.Remove(iter.Previous);
                    }
                }
            }
            //Remove the last vertex in polygon mode (it is redundant in this case)
            if (ParentTilemap.Collider2DType == e2DColliderType.PolygonCollider2D)
            {
                if( edgeVertices.First.Value == edgeVertices.Last.Value) // FIXED: sometimes due optimization, last vertex don't need to be removed
                    edgeVertices.RemoveLast();
            }
        }

        List<LinkedList<Vector2>> SplitSegments(List<LinkedList<Vector2>> edges)
        {
            List<LinkedList<Vector2>> separatedSegmentList = new List<LinkedList<Vector2>>();
            for (int i = 0; i < edges.Count; ++i)
            {
                LinkedList<Vector2> edgeVertices = edges[i];
                LinkedListNode<Vector2> iter = edgeVertices.First;
                while (iter.Next != null)
                {
                    LinkedList<Vector2> segment = new LinkedList<Vector2>();
                    segment.AddFirst(iter.Value);
                    segment.AddLast(iter.Next.Value);
                    separatedSegmentList.Add(segment);
                    iter = iter.Next;
                }
            }
            return separatedSegmentList;
        }

        float PerpDot(Vector2 p, Vector2 a, Vector2 b)
        {
            Vector2 v0 = a - p;
            Vector2 v1 = b - p;
            return PerpDot(v0, v1);
        }

        float PerpDot(Vector2 v0, Vector2 v1)
        {
            return v0.x * v1.y - v0.y * v1.x;
        }
    }
}
