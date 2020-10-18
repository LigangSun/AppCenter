using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.BlockPuzzle.Data
{
    public class PuzzleMxNLib
    {
        public class CPuzzleMxN
        {
            public class PlaceList
            {
                // 0-3 bit: 0, 4-7 bit: 1, 8-11 bit: 2, 12-15 bit: 3
                // ......
                // 28-31 bit: 15
                public int[] Place;
                public PlaceList Left;
                public PlaceList Right;
 
                public PlaceList()
                {
                    Place = new int[2];
                }
            }

            public class Scanbuf
            {
                public int[] Place;
                public int ScanID;

                public Scanbuf()
                {
                    Place = new int[2];
                    ScanID = 0;
                }
            }

            public class PathList
            {
                public int[] Path;

                public PathList()
                {
                    Path = new int[2];
                }
            }

            public PlaceList m_pPlaceList;
            public List<Scanbuf> m_pScanbuf = new List<Scanbuf>();

            public int m_iPathsize;
            public int m_iStepCount;
            public byte[] m_iTargetPuzzle = new byte[16];
            public byte[] m_iPuzzle = new byte[16];
            public List<PathList> m_pPathList = new List<PathList>();
            public bool m_bAutoRun;

            public struct POINT
            {
                public int x;
                public int y;
            };

            public void Compress(byte[] array, ref int[] data, int M, int N)
            {
                int i;
                data[0] = 0;
                data[1] = 0;

                if (M * N <= 8)
                {
                    for (i = 0; i < M * N; i++)
                    {
                        data[0] += (int)((int)array[i] << ((8 - 1 - i) * 4));
                    }
                }
                else
                {
                    for (i = 0; i < 8; i++)
                    {
                        data[0] += (int)((int)array[i] << ((8 - 1 - i) * 4));
                    }

                    for (i = 8; i < M * N; i++)
                    {
                        data[1] += (int)((int)array[i] << (((16 - 1 - i)) * 4));
                    }
                }
            }

            public void Decompress(int[] data, ref byte[] array, int M, int N)
            {
                int i;

                if (M * N <= 8)
                {
                    for (i = 0; i < M * N; i++)
                    {
                        array[i] = (byte)((data[0] >> ((8 - 1 - i) * 4)) & 0xf);
                    }
                }
                else
                {
                    for (i = 0; i < 8; i++)
                    {
                        array[i] = (byte)((data[0] >> ((8 - 1 - i) * 4)) & 0xf);
                    }

                    for (i = 8; i < M * N; i++)
                    {
                        array[i] = (byte)((data[1] >> ((16 - 1 - i) * 4)) & 0xf);
                    }
                }
            }

            public bool AddTree(int[] place, ref PlaceList parent)
            {
                int i;

                if (parent == null)
                {
                    parent = new PlaceList();
                    parent.Place[0] = place[0];
                    parent.Place[1] = place[1];

                    return true;
                }

                for (i = 0; i < 2; i++)
                {
                    if (parent.Place[i] > place[i])
                    {
                        return AddTree(place, ref parent.Right);
                    }
                    else if (parent.Place[i] < place[i])
                    {
                        return AddTree(place, ref parent.Left);
                    }
                }

                return false;
            }

            public bool Move(ref byte[] array, int way, int M, int N)
            {
                int i;
                POINT pnt;
                bool moveok = false;

                for (i = 0; i < M * N; i++)
                {
                    if (array[i] == 0)
                    {
                        break;
                    }
                }

                pnt.x = i % M;
                pnt.y = (int)(i / M);

                switch (way)
                {
                    case 0: //down
                        if (pnt.y + 1 < N)
                        {
                            array[i] = array[i + M];
                            array[i + M] = 0;
                            moveok = true;
                        }
                        break;
                    case 1: //up
                        if (pnt.y - 1 > -1)
                        {
                            array[i] = array[i - M];
                            array[i - M] = 0;
                            moveok = true;
                        }
                        break;
                    case 2: //right
                        if (pnt.x + 1 < M)
                        {
                            array[i] = array[i + 1];
                            array[i + 1] = 0;
                            moveok = true;
                        }
                        break;
                    case 3: //left
                        if (pnt.x - 1 > -1)
                        {
                            array[i] = array[i - 1];
                            array[i - 1] = 0;
                            moveok = true;
                        }
                        break;
                }

                if (moveok && !m_bAutoRun)
                {
                    m_iStepCount++;
                }

                return moveok;
            }

            public void GetPath(int depth, int M, int N)
            {
                int now = 0, maxpos = 100;
                int parentid;

                //m_pPathList = new PathList[maxpos];
                parentid = m_pScanbuf[depth].ScanID;

                //Decompress(m_pScanbuf[depth].Place , m_pPathList[++now].Path);
                ++now;
                //memcpy(m_pPathList[now].Path, m_pScanbuf[depth].Place, sizeof(int) * 2);

                CPuzzleMxN.PathList pathList0 = new CPuzzleMxN.PathList();
                m_pPathList.Add(pathList0);
                CPuzzleMxN.PathList pathList1 = new CPuzzleMxN.PathList();
                m_pPathList.Add(pathList1); 
                m_pPathList[now].Path[0] = m_pScanbuf[depth].Place[0];
                m_pPathList[now].Path[1] = m_pScanbuf[depth].Place[1];

                while (parentid != -1)
                {
                    //if (now == maxpos)
                    //{
                    //    maxpos += 10;
                    //    public List<PathList> m_pPathList = new List<PathList>();


                    //    PathList * temlist = new PathList[maxpos];
                    //    memcpy(temlist , m_pPathList , sizeof(PathList) * (maxpos - 10));
                    //    delete[] m_pPathList;
                    //    m_pPathList = temlist;
                    //}

                    ++now;
                    //Decompress(m_pScanbuf[parentid].Place, m_pPathList[now].Path, M, N);
                    //memcpy(m_pPathList[now].Path, m_pScanbuf[parentid].Place, sizeof(int) * 2);
                    CPuzzleMxN.PathList pathListX = new CPuzzleMxN.PathList();
                    m_pPathList.Add(pathListX);
                    m_pPathList[now].Path[0] = m_pScanbuf[parentid].Place[0];
                    m_pPathList[now].Path[1] = m_pScanbuf[parentid].Place[1];

                    parentid = m_pScanbuf[parentid].ScanID;
                }

                m_iPathsize = now;
            }

            public bool IsSame(byte[] pSrc, byte[] pDst, int M, int N)
            {
                int i;

                for (i = 0; i < M * N; i++)
                {
                    if (pSrc[i] != pDst[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        static CPuzzleMxN g_Cpuzzle = new CPuzzleMxN();

        //2
        // Return -1 is no answer, n is answer and total move paths number
        public static int PGGetAnswer(byte[] pSrcData, ref byte[] pDstData, int iPuzzleCols, int iPuzzleRows)
        {
            int num_movepaths = -1;
            bool flag = false;
            int M = iPuzzleCols;
            int N = iPuzzleRows;

            if (M > 4 || N > 4)
            {
                return -1;
            }

            CPuzzleMxN p_Cpuzzle = g_Cpuzzle;

            int i;
            byte[] temparray = new byte[16];

            for (i = 0; i < 16; i++)
            {
                temparray[i] = 0xff;
            }

            int parentID = 0, child = 1;
            int[] sum_total = new int[2];
            int sum_MxN;

            // Check place 
            sum_total[0] = 0;
            sum_total[1] = 0;
            sum_MxN = 0;
            int number = 0;
            for (i = 0; i < M * N; i++)
            {
                sum_total[0] += pSrcData[i];
                sum_total[1] += pDstData[i];
                sum_MxN += number++;
            }

            if (sum_total[0] != sum_MxN)
            {
                // Initial place error!
                return num_movepaths;
            }

            if (sum_total[1] != sum_MxN)
            {
                // Target place error!
                return num_movepaths;
            }

            for (i = 0; i < M * N; i++)
            {
                p_Cpuzzle.m_iPuzzle[i] = pSrcData[i];
                p_Cpuzzle.m_iTargetPuzzle[i] = pDstData[i];
            }

            p_Cpuzzle.m_bAutoRun = true;

            if (p_Cpuzzle.IsSame(p_Cpuzzle.m_iPuzzle, p_Cpuzzle.m_iTargetPuzzle, M, N))
            {
                num_movepaths = 0;
                return num_movepaths;
            }

            int[] target = new int[2];
            int[] fountain = new int[2];
            int[] parentL = new int[2];

            p_Cpuzzle.Compress(p_Cpuzzle.m_iTargetPuzzle, ref target, M, N);
            p_Cpuzzle.Compress(p_Cpuzzle.m_iPuzzle, ref fountain, M, N);

            p_Cpuzzle.m_pPlaceList = null;
            p_Cpuzzle.AddTree(fountain, ref p_Cpuzzle.m_pPlaceList);

            CPuzzleMxN.Scanbuf pathList0 = new CPuzzleMxN.Scanbuf();
            p_Cpuzzle.m_pScanbuf.Add(pathList0);
            p_Cpuzzle.m_pScanbuf[0].Place[0] = fountain[0];
            p_Cpuzzle.m_pScanbuf[0].Place[1] = fountain[1];
            p_Cpuzzle.m_pScanbuf[0].ScanID = -1;
            long MAX_PATH = 362880;// 362880;

            while (parentID < MAX_PATH && child < MAX_PATH && parentID < child)
            {
                parentL[0] = p_Cpuzzle.m_pScanbuf[parentID].Place[0];
                parentL[1] = p_Cpuzzle.m_pScanbuf[parentID].Place[1];

                for (i = 0; i < 4; i++)
                {
                    p_Cpuzzle.Decompress(parentL, ref temparray, M, N);
            
                    flag = p_Cpuzzle.Move(ref temparray, i, M, N);

                    if (flag)
                    {
                        p_Cpuzzle.Compress(temparray, ref fountain, M, N);

                        if (p_Cpuzzle.AddTree(fountain, ref p_Cpuzzle.m_pPlaceList))
                        {
                            CPuzzleMxN.Scanbuf pathChild = new CPuzzleMxN.Scanbuf();
                            p_Cpuzzle.m_pScanbuf.Add(pathChild);
                            p_Cpuzzle.m_pScanbuf[child].Place[0] = fountain[0];
                            p_Cpuzzle.m_pScanbuf[child].Place[1] = fountain[1];
                            p_Cpuzzle.m_pScanbuf[child].ScanID = parentID;

                            if (fountain[0] == target[0] && fountain[1] == target[1])
                            {
                                p_Cpuzzle.GetPath(child, M, N);

                                num_movepaths = p_Cpuzzle.m_iPathsize;

                                return num_movepaths;
                            }
                            child++;
                        }
                    }
                } // for i
                parentID++;
            }

            p_Cpuzzle.m_bAutoRun = false;

            return num_movepaths;
        }

        //3 
        public static void PGGetNextMovePath(int iPuzzleCols, int iPuzzleRows, int curPath, ref byte[] pDstData)
        {
            g_Cpuzzle.Decompress(g_Cpuzzle.m_pPathList[curPath].Path, ref pDstData, iPuzzleCols, iPuzzleRows);
        }
    }
}
