using NavMeshPlus.Components;
using Unity.AI.Navigation;
using UnityEngine;

public class NavBaker : MonoBehaviour
{
    
    NavMeshPlus.Components.NavMeshSurface navMeshSurface;

    private void Awake()
    {
        navMeshSurface = GetComponent<NavMeshPlus.Components.NavMeshSurface>();
        LevelGenerator.levelGenerated += BakeNavMesh;

    }

    void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }

}
