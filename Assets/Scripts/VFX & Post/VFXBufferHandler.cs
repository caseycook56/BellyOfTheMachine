using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.VFX;

public class VFXBufferHandler : MonoBehaviour
{
    public int maxColliders = 10;
    public Vector3[] TenticlePoints;
    private VisualEffect VisualEffectGraph;
    public GraphicsBuffer gfxBuffer;
    private int m_BufferPropertyID = Shader.PropertyToID("BufferData");

    [VFXType(VFXTypeAttribute.Usage.GraphicsBuffer)]
    struct CustomVFXData
    {
        public Vector3 Position;
        public uint IsActive;
    }

    private List<CustomVFXData> VFXData;

    void Reallocate(int newSize)
    {
        //Debug.Log("Resize Buffer: " + newSize);

        if (gfxBuffer != null)
            gfxBuffer.Release();

        gfxBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, newSize, Marshal.SizeOf(typeof(CustomVFXData)));
        gfxBuffer.SetData(new CustomVFXData[newSize]);
    }

    void Start()
    {
        TenticlePoints = new Vector3[maxColliders];
        int MaxSize = 10;
        Reallocate(MaxSize);
        VisualEffectGraph = GetComponent<VisualEffect>();
        VisualEffectGraph.SetGraphicsBuffer(m_BufferPropertyID, gfxBuffer);
    }

    private void ZeroInitialiseList()
    {
        VFXData = new List<CustomVFXData>(10);
        for (int i = 0; i < 10; i++)
        {
            CustomVFXData EmptyData = new CustomVFXData();
            EmptyData.Position = Vector3.zero;
            EmptyData.IsActive = 0;
            VFXData.Add(EmptyData);
        }
    }

    void Update()
    {
        VisualEffectGraph.SetVector3("WorldCurrentPosition", GetComponent<PhysicsBody>().transform.position);
        FillBuffer();
        gfxBuffer.SetData(VFXData);
    }
    void FillBuffer()
    {
        PhysicsBody body = GetComponent<PhysicsBody>();
        List<Vector3> GrapplePoints = body.points;
        TenticlePoints = GrapplePoints.ToArray();

        ZeroInitialiseList();

        //if (GrapplePoints.Count == 0)
        //{
        //    CustomVFXData newData = new CustomVFXData();
        //    newData.Position = body.transform.position;
        //    newData.IsActive = 0;
        //    //VFXData.Add(newData);
        //}
        if (GrapplePoints.Count != 0)
        {
            for (int i = 0; i < GrapplePoints.Count; i++)
            {
                CustomVFXData newData = new CustomVFXData();
                newData.Position = GrapplePoints[i];
                newData.IsActive = 1;
                VFXData[i] = (newData);
            }
        }

    }

    #region Dispose
    public void OnDisable()
    {
        Release();
    }

    public void OnDestroy()
    {
        Release();
    }

    void Release()
    {
        if (gfxBuffer != null)
        {
            gfxBuffer.Release();
            gfxBuffer = null;
        }
    }
    #endregion
}

//    using System;
//using System.Linq;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;
//using UnityEngine;
//using UnityEngine.VFX;

//public class VFXBufferHandler : MonoBehaviour
//{
//    [Header("Scenario Variables")]
//    public float radius = 5f;
//    public int maxColliders = 5;
//    public Collider[] hitColliders;
//    public Collider[] newhitsDebug;
//    public Vector3[] closestPos;

//    private VisualEffect vfx;
//    public GraphicsBuffer gfxBuffer;
//    private int m_BufferPropertyID = Shader.PropertyToID("BufferData");

//    [VFXType(VFXTypeAttribute.Usage.GraphicsBuffer)]
//    struct CustomVFXData
//    {
//        public Vector3 position;
//        public Vector4 color;
//        public uint isActive;
//    }

//    private List<CustomVFXData> m_CustomVFXData;

//    void Reallocate(int newSize)
//    {
//        if (gfxBuffer != null)
//            gfxBuffer.Release();

//        gfxBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, newSize, Marshal.SizeOf(typeof(CustomVFXData)));
//        gfxBuffer.SetData(new CustomVFXData[newSize]);
//    }

//    void Start()
//    {
//        hitColliders = new Collider[maxColliders];
//        closestPos = new Vector3[maxColliders];

//        Reallocate(hitColliders.Length);
//        vfx = GetComponent<VisualEffect>();
//        Debug.Log(vfx.name);
//        vfx.SetGraphicsBuffer(m_BufferPropertyID, gfxBuffer);
//    }

//    void Update()
//    {
//        //GetColliders();
//        //FillBuffer();
//        //if (m_CustomVFXData.Count > gfxBuffer.count)
//        //{
//        //    int newCount = gfxBuffer.count;
//        //    while (newCount < m_CustomVFXData.Count)
//        //        newCount *= 2;
//        //    Reallocate(newCount);
//        //}

//        //gfxBuffer.SetData(m_CustomVFXData);

//        FillBuffer();
//        if (m_CustomVFXData.Count != gfxBuffer.count)
//        {
//            Reallocate(m_CustomVFXData.Count);
//        }
//        gfxBuffer.SetData(m_CustomVFXData);
//    }

//    void FillBuffer()
//    {
//        PhysicsBody body = GetComponent<PhysicsBody>();
//        List<Vector3> GrapplePoints = body.points;
//        m_CustomVFXData = new List<CustomVFXData>(GrapplePoints.Count);

//        if (GrapplePoints.Count == 0)
//        {
//            CustomVFXData newData = new CustomVFXData();
//            newData.position = body.transform.position;
//            newData.color = Color.black;
//            newData.isActive = 1;
//            m_CustomVFXData.Add(newData);
//        }
//        else
//        {
//            for (int i = 0; i < GrapplePoints.Count; i++)
//            {
//                CustomVFXData newData = new CustomVFXData();
//                newData.position = GrapplePoints[i];
//                newData.color = Color.cyan;
//                newData.isActive = 1;
//                m_CustomVFXData.Add(newData);
//            }
//        }
//    }

//    void GetColliders()
//    {
//        Collider[] newHits = new Collider[maxColliders];
//        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, radius, newHits);
//        m_CustomVFXData = new List<CustomVFXData>(numColliders);
//        newhitsDebug = new Collider[newHits.Length];
//        newhitsDebug = newHits;



//        // Remove colliders which are too far
//        for (int i = 0; i < hitColliders.Length; i++)
//        {
//            if (hitColliders[i] == null)
//                continue;

//            if (Vector3.Distance(hitColliders[i].transform.position, transform.position) > radius)
//            {
//                hitColliders[i] = null;
//            }
//        }

//        // Check if collider is already present
//        for (int i = 0; i < newHits.Length; i++)
//        {
//            if (!hitColliders.Contains(newHits[i]))
//            {
//                for (int j = 0; j < hitColliders.Length; j++)
//                {
//                    if (hitColliders[j] == null)
//                    {
//                        hitColliders[j] = newHits[i];
//                        closestPos[j] = hitColliders[j].ClosestPoint(transform.position);
//                        break;
//                    }
//                }
//            }
//        }

//        for (int i = 0; i < hitColliders.Length; i++)
//        {
//            CustomVFXData newData = new CustomVFXData();

//            if (hitColliders[i] != null)
//            {
//                // Add new data
//                newData.position = closestPos[i];
//                newData.color = Color.cyan;
//                newData.isActive = 1;

//                Color debugColor = Color.black;

//                switch (i)
//                {
//                    case 0:
//                        debugColor = Color.red;
//                        break;

//                    case 1:
//                        debugColor = Color.green;
//                        break;

//                    case 2:
//                        debugColor = Color.blue;
//                        break;

//                    case 3:
//                        debugColor = Color.yellow;
//                        break;

//                    case 4:
//                        debugColor = Color.cyan;
//                        break;

//                    case 5:
//                        debugColor = Color.grey;
//                        break;
//                }



//                Debug.DrawLine(transform.position, closestPos[i], debugColor);
//            }
//            else
//            {
//                // Add new data
//                newData.position = transform.position;
//                newData.color = Color.black;
//                newData.isActive = 0;
//            }

//            m_CustomVFXData.Add(newData);
//        }
//    }

//    #region Dispose
//    public void OnDisable()
//    {
//        Release();
//    }

//    public void OnDestroy()
//    {
//        Release();
//    }

//    void Release()
//    {
//        if (gfxBuffer != null)
//        {
//            gfxBuffer.Release();
//            gfxBuffer = null;
//        }
//    }
//    #endregion

//}
