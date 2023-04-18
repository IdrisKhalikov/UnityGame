using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace BSPTreeGeneration
{
    public class Partition
    {
        public Partition Neighbour { get; private set; }
        public readonly Rectangle Bounds;
        public readonly int Iteration;

        public Partition(Rectangle bounds, int iteration)
        {
            Bounds = bounds;
            Iteration = iteration;
        }

        public void SetNeighbour(Partition other)
        {
            Neighbour = other;
            other.Neighbour = this;
        }
    }

    public class BSPAlgorithm
    {
        private int width;
        private int height;
        const double MaxRatio = 0.65;
        const double MinRatio = 0.45;
        private Random random;

        public BSPAlgorithm(int levelWidth, int levelHeight)
        {
            width = levelWidth;
            height = levelHeight;
            random = new Random();
        }

        public List<Partition> GetMap(int iterations)
        {
            var partitions = new List<Partition>();
            var stack = new Stack<Partition>();
            var initialBounds = new Rectangle(0, 0, width, height);
            stack.Push(new Partition(initialBounds, 0));
            while(stack.Count > 0)
            {
                var partition = stack.Pop();
                partitions.Add(partition);
                var splitted = Split(partition);
                if (partition.Iteration == iterations)    
                    continue;

                stack.Push(splitted.Item1);
                stack.Push(splitted.Item2);
            }

            return partitions;
        }

        public (Partition, Partition) Split(Partition partition)
        {
            var pair = SplitRect(partition.Bounds);
            var firstPart = new Partition(pair.Item1, partition.Iteration + 1);
            var secondPart = new Partition(pair.Item2, partition.Iteration + 1);
            firstPart.SetNeighbour(secondPart);
            return (firstPart, secondPart);
        }

        private (Rectangle, Rectangle) SplitRect(Rectangle rect)
        {
            var split = random.NextDouble() * (MaxRatio - MinRatio) + MinRatio;
            if (rect.Height > rect.Width)
            {
                var r1 = new Rectangle(rect.X, rect.Y, rect.Width, (int)(rect.Height * split));
                var r2 = new Rectangle(rect.X, rect.Y + r1.Height, rect.Width, rect.Height - r1.Height);
                return (r1, r2);
            }
            else
            {
                var r1 = new Rectangle(rect.X, rect.Y, (int)(rect.Width * split), rect.Height);
                var r2 = new Rectangle(rect.X + r1.Width, rect.Y, rect.Width - r1.Width, rect.Height);
                return (r1, r2);
            }
        }


    }
}