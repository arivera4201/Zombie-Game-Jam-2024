using System;
using System.Collections.Generic;

public class Point
{
    public float X { get; set; }
    public float Y { get; set; }

    public Point(float x, float y)
    {
        X = x;
        Y = y;
    }
}

public class SpreadOutPointGenerator
{
    private Random random = new Random();

    public List<Point> GeneratePoints(int numPoints, float minX, float maxX, float minY, float maxY, float minDistance)
    {
        List<Point> points = new List<Point>();
        int maxAttempts = numPoints * 10;

        while (points.Count < numPoints && maxAttempts > 0)
        {
            float x = (float)random.NextDouble() * (maxX - minX) + minX;
            float y = (float)random.NextDouble() * (maxY - minY) + minY;
            Point newPoint = new Point(x, y);

            if (IsPointValid(newPoint, points, minDistance))
            {
                points.Add(newPoint);
            }
            else
            {
                maxAttempts--;
            }
        }

        return points;
    }

    private bool IsPointValid(Point newPoint, List<Point> existingPoints, double minDistance)
    {
        foreach (var point in existingPoints)
        {
            double distance = CalculateDistance(newPoint, point);
            if (distance < minDistance)
            {
                return false;
            }
        }
        return true;
    }

    private double CalculateDistance(Point p1, Point p2)
    {
        return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
    }
}