using NavMeshPlus.Components;
using Unity.AI.Navigation;
using UnityEngine;

public class NavBaker : MonoBehaviour
{
    
    NavMeshPlus.Components.NavMeshSurface navMeshSurface;

    private void OnEnable()
    {
        navMeshSurface = GetComponent<NavMeshPlus.Components.NavMeshSurface>();
        LevelGenerator.levelGenerated += BakeNavMesh;

    }
    private void OnDisable()
    {
        LevelGenerator.levelGenerated -= BakeNavMesh;
    }

    void BakeNavMesh()
    {
        
        navMeshSurface.BuildNavMesh();
    }

}
