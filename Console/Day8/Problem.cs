using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Console.Day8
{
    class Problem
    {
        private const int layerSize = 150;
        static int[] Digits = File.ReadAllText($"Day8/{Program.Config}Day8.txt")
            .Select(c => int.Parse(c.ToString()))
            .ToArray();

        public static int PartA()
        {
            int[] counts = null;
            int minZero = int.MaxValue;
            int[] minZeroCounts = new[] { 0, 0, 0 };
            for (int i = 0; i < Digits.Length; i += layerSize)
            {
                counts = new[] { 0, 0, 0 };
                for (int j = i; j < i + layerSize; j++)
                {
                    switch (Digits[j])
                    {
                        case 0:
                            counts[0]++;
                            break;
                        case 1:
                            counts[1]++;
                            break;
                        case 2:
                            counts[2]++;
                            break;
                        default:
                            throw new System.Exception($"Unhandled value {Digits[j]}");
                    }
                }
                if (counts[0] < minZero)
                {
                    minZero = counts[0];
                    minZeroCounts[0] = counts[0];
                    minZeroCounts[1] = counts[1];
                    minZeroCounts[2] = counts[2];
                }
            }

            return minZeroCounts[1] * minZeroCounts[2];
        }

        public static void PartB()
        {
            List<int[]> layers = new List<int[]>();
            for (int i = 0; i < Digits.Length; i += layerSize)
            {
                int[] layer = new int[layerSize];
                for (int j = i; j < i + layerSize; j++)
                {
                    layer[j - i] = Digits[j];
                }
                layers.Add(layer);
            }

            int[] pixels = Enumerable.Range(0, 150).Select(_ => 2).ToArray();

            for (int i = 0; i < layers.Count; i++)
            {
                for (int j = 0; j < 150; j++)
                {
                    if (pixels[j] == 2) pixels[j] = layers[i][j];
                }
            }

            using (var stream = File.OpenWrite("img.png"))
            using (var image = new Image<Rgba32>(25, 6))
            {
                for (int i = 0; i < 25; i++)
                    for (int j = 0; j < 6; j++)
                    {
                        var pixel = pixels[j * 25 + i];
                        if (pixel == 0) image[i, j] = Rgba32.Black;
                        else if (pixel == 2) image[i, j] = Rgba32.White;
                        else image[i, j] = Rgba32.Transparent;
                    }
                image.SaveAsPng(stream);
            }
        }
    }
}
