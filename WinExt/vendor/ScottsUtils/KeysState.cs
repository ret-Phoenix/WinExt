using System;
using System.Collections.Generic;
using System.Text;

namespace ScottsUtils
{
    class KeysState
    {
        public int Variable { get; set; }

        class LoopInfo
        {
            public int LoopPos;
            public int Iterations;
        }

        List<LoopInfo> m_loops;

        public KeysState()
        {
            Variable = 0;
            Reset();
        }

        public void Reset()
        {
            m_loops = new List<LoopInfo>();
        }

        public void StartLoop(int pos)
        {
            foreach (var cur in m_loops)
            {
                if (cur.LoopPos == pos)
                {
                    return;
                }
            }

            LoopInfo info = new LoopInfo();
            info.LoopPos = pos;
            info.Iterations = -1;
            m_loops.Add(info);
        }

        public bool IterateLoop(int count, ref int targetPos)
        {
            if (m_loops.Count > 0)
            {
                LoopInfo info = m_loops[m_loops.Count - 1];
                if (info.Iterations == -1)
                {
                    info.Iterations = count;
                }

                info.Iterations--;
                if (info.Iterations <= 0)
                {
                    m_loops.RemoveAt(m_loops.Count - 1);
                    return false;
                }
                else
                {
                    targetPos = info.LoopPos;
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
