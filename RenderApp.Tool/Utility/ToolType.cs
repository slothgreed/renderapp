namespace RenderApp.Tool
{
    /// <summary>
    /// コントローラのモード
    /// </summary>
    public enum CONTROL_MODE
    {
        SelectTriangle,
        SelectLine,
        SelectPoint,
        EdgeFlips,
        Dijkstra,
        ShapeMatching,
        Geodesic
    }

    public enum AnalyzeCommand
    {
        WireFrame,
        HalfEdge,
        Curvature,
        HalfEdgeWireFrame,
        ConvexHull,
        MarchingCube,
        Subdivision,
        IsoLine,
        AdaptiveMesh,
        QEM,
        Perceptron,
        FeatureLine,
        Voxelize,
        Kmeans,
        Smoothing
    }
}
