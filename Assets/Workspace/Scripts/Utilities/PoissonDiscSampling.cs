using System.Collections.Generic;
using UnityEngine;

public static class PoissonDiscSampling
{
    public static List<Vector2> Generate(float radius, Vector2 regionSize, int samples = 32)
    {
        float cellSize = radius / Mathf.Sqrt(2);

        int[,] grid = new int[
            Mathf.CeilToInt(regionSize.x / cellSize),
            Mathf.CeilToInt(regionSize.y / cellSize)
        ];

        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawns = new List<Vector2>();

        spawns.Add(regionSize / 2);

        while (spawns.Count > 0)
        {
            int index = Random.Range(0, spawns.Count);
            Vector2 center = spawns[index];
            bool accepted = false;

            for (int i = 0; i < samples; i++)
            {
                float angle = Random.value * Mathf.PI * 2;
                Vector2 direction = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 canidate = center + direction * Random.Range(radius, 2 * radius);

                if (IsValidPoint(canidate, regionSize, cellSize, points, grid))
                {
                    accepted = true;

                    points.Add(canidate);
                    spawns.Add(canidate);

                    grid[(int)(canidate.x / cellSize), (int)(canidate.y / cellSize)] = points.Count;

                    break;
                }
            }

            if (!accepted)
                spawns.RemoveAt(index);
        }

        return points;
    }

    public static bool IsValidPoint(Vector2 canidate, Vector2 regionSize, float cellSize, List<Vector2> points, int[,] grid)
    {
        float radius = cellSize * Mathf.Sqrt(2);

        if (canidate.x < 0 || canidate.x >= regionSize.x || canidate.y < 0 || canidate.y >= regionSize.y)
            return false;

        int cx = (int)(canidate.x / cellSize);
        int cy = (int)(canidate.y / cellSize);

        for (int x = Mathf.Max(0, cx - 2); x <= Mathf.Min(cx + 2, grid.GetLength(0) - 1); x++)
        {
            for (int y = Mathf.Max(0, cy - 2); y <= Mathf.Min(cy + 2, grid.GetLength(1) - 1); y++)
            {
                int index = grid[x, y] - 1;

                if (index == -1)
                    continue;

                float sdist = (canidate - points[index]).sqrMagnitude;

                if (sdist < radius * radius)
                    return false;
            }
        }

        return true;
    }
}