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
        SacredFire.fireLit.AddListener(BakeNavMesh);
        SacredFire.fireDoused.AddListener(BakeNavMesh);

    }
    private void OnDisable()
    {
        LevelGenerator.levelGenerated -= BakeNavMesh;
        SacredFire.fireLit.RemoveListener(BakeNavMesh);
        SacredFire.fireDoused.RemoveListener(BakeNavMesh);
    }

    void BakeNavMesh()
    {
        Debug.Log("Baking NavMesh for surface: " + navMeshSurface.name);
        navMeshSurface.BuildNavMesh();
        
    }

}
