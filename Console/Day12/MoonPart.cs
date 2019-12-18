using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Console.Day12
{
    public class MoonParts
    {
        public int[] Positions { get; set; } 
        public int[] Velocities { get; set; } 

        public void ApplyGravity()
        {
            for (int i = 0; i < Positions.Length; i++)
            {
                for (int j = i+1; j < Positions.Length; j++)
                {
                    if (Positions[i] > Positions[j])
                    {
                        Velocities[i]--;
                        Velocities[j]++;
                    }
                    else if (Positions[i] < Positions[j])
                    {
                        Velocities[i]++;
                        Velocities[j]--;
                    }
                }
            }
        }

        public void ApplyVelocity()
        {
            for(int i = 0; i < Positions.Length; i++)
            {
                Positions[i] += Velocities[i];
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is MoonParts moon)
            {
                return Positions.SequenceEqual(moon.Positions) &&
                    Velocities.SequenceEqual(moon.Velocities);
            }
            return false;
        }

        public override int GetHashCode() =>
            GetArrayHash(Positions) + GetArrayHash(Velocities);
         
        static int GetArrayHash(int[] arr)
        {
            int hash = arr.Length;
            for(int i = 0; i < arr.Length; i++)
            {
                hash = hash * 17 + arr[i];
            }
            return hash;
        }

        public MoonParts Copy()
        {
            return new MoonParts
            {
                Positions = Positions.ToArray(),
                Velocities = Velocities.ToArray()
            };
        }
        public static MoonParts FromMoons(DeathStar[] moons, Func<Vector, int> getCoordinate)
        {
            var positions = new int[4];
            var velocities = new int[4];

            for(int i = 0; i < 4; i++)
            {
                positions[i] = getCoordinate(moons[i].Position);
                velocities[i] = getCoordinate(moons[i].Velocity);
            }

            return new MoonParts
            {
                Positions = positions,
                Velocities = velocities
            };
        }

    }
}
