using System;
using System.Collections.Generic;
using System.Text;

namespace BombDiscovery
{
    class RandomArray
    {
        Random r = new Random();
        int bombs = 10;
     static  public int raws = 0, columns = 0;
        public int[][] Places ;
      public  RandomArray(int r,int c,int b)
        {
            bombs = b;
            raws = r;
            columns = c;
            Places = new int[raws][];
            for (int i = 0; i < Places.Length; i++)
            {
                Places[i] = new int[columns];

            }

            makeRandoms();
            calculateRoundBombs();
        }
        void makeRandoms()
        {
            int ind1, ind2 = 0;
            for (int i = 0; i < bombs; i++)
            {
                ind1 = r.Next(0, Places.Length);
                ind2 = r.Next(0, Places[0].Length);
                if (Places[ind1][ind2] != -1)
                    Places[ind1][ind2] = -1;
                else
                    i--;

            }

        }
      public  bool check(int x, int y)
        {
            if (x >= 0 && x <= Places.Length - 1)
                if (y >= 0 && y <= Places[x].Length - 1)
                    return true;
            return false;

        }

        void calculateRoundBombs()
        {
            for (int i = 0; i < Places.Length; i++)
            {
                for (int j = 0; j < Places[i].Length; j++)
                {

                    if (check(i - 1, j - 1) && Places[i - 1][j - 1] == -1 && Places[i ][j]!=-1) Places[i][j]++;
                    if (check(i - 1, j) && Places[i - 1][j] == -1 && Places[i][j] != -1) Places[i][j]++;
                    if (check(i - 1, j + 1) && Places[i - 1][j + 1] == -1 && Places[i][j] != -1) Places[i][j]++;
                    if (check(i, j - 1) && Places[i][j - 1] == -1 && Places[i][j] != -1) Places[i][j]++;
                    if (check(i, j + 1) && Places[i][j + 1] == -1 && Places[i][j] != -1) Places[i][j]++;
                    if (check(i + 1, j - 1) && Places[i + 1][j - 1] == -1 && Places[i][j] != -1) Places[i][j]++;
                    if (check(i + 1, j) && Places[i + 1][j] == -1 && Places[i][j] != -1) Places[i][j]++;
                    if (check(i + 1, j + 1) && Places[i + 1][j + 1] == -1 && Places[i][j] != -1) Places[i][j]++;


                }


            }



        }




    }



    }

