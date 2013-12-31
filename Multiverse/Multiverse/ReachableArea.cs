using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Multiverse
{
    // Creates a grid of the squares which are reachable or not in the play area

    class ReachableArea
    {
        public int width;
        public int height;
        private int[,] reachable;

        private List<City> cityList;

        // The public instance class

        public ReachableArea(IsoGrid grid, List<City> citiesList)
        {
            
            this.width  = (int)grid.size.X;
            this.height = (int)grid.size.Y;

            this.reachable = new int[width, height];

            for (int x = 0; x < (width); x++)
            {
                for (int y = 0; y < (height); y++)
                {
                    reachable[x, y] = 0;
                }
            }

            this.cityList = citiesList;

            foreach (City city in cityList)
            {
                reachable[(int)city.gridposition.X - 1, (int)city.gridposition.Y - 1] = 1;

            }
        }

        // return value from array

        public int Value(int x, int y)
        {

            try
            {
                if (reachable[x-1, y-1] > 0)
                {
                    return 1;
                }

                else
                {
                    return 0;
                }
            }

            catch (IndexOutOfRangeException)
            {
                return 1;
            }


        }

        public void Update(List<City> citiesList)
        {
            for (int x = 0; x < (width); x++)
            {
                for (int y = 0; y < (height); y++)
                {
                    reachable[x, y] = 0;
                }
            }

            this.cityList = citiesList;

            foreach (City city in cityList)
            {
                for (int x = 0; x < city.footprint.X; x++)
                {
                    for (int y = 0; y < city.footprint.Y; y++)
                    {
                        reachable[(int)city.gridposition.X - 1 + x, (int)city.gridposition.Y - 1 + y] = 1;
                    }
                }

            }

        }
        
    }
    
}
