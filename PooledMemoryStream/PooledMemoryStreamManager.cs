﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PooledMemoryStreams.PoolPolicies;
using PooledMemoryStreams.Pools;

namespace PooledMemoryStreams
{
    public class PooledMemoryStreamManager
    {
        private IPoolChooserPolicy m_ChooserPolicy;

        public PooledMemoryStreamManager(IPoolChooserPolicy p_ChooserPolicy)
        {
            m_ChooserPolicy = p_ChooserPolicy;
        }

        protected internal virtual List<MemoryBlock> GetBlock(long p_Capacity, long p_TargetCapacity)
        {
            List<MemoryBlock> l_Blocks = new List<MemoryBlock>();

            long l_NeededBytes = p_TargetCapacity - p_Capacity;

            if (l_NeededBytes <= 0)
                return l_Blocks;

            // Find Best Pool
            StreamManagerPool l_Pool = m_ChooserPolicy.FindBestPool(p_Capacity, p_TargetCapacity);

            if (l_Pool == null)
                throw new Exception($"No Pool found. Capacity {p_Capacity}, TargetCapacity {p_TargetCapacity}");

            // Allocated all nesseary blockes
            while (l_NeededBytes > 0)
            {
                MemoryBlock l_MemoryBlock = l_Pool.GetBlock();
                l_NeededBytes = l_NeededBytes - l_MemoryBlock.GetLength();
                l_Blocks.Add(l_MemoryBlock);
            }

            return l_Blocks;
        }

        public Stream GetStream(int p_Capacity)
        {
            return new PooledMemoryStream(p_Capacity, this);
        }

        public Stream GetStream()
        {
            return new PooledMemoryStream(0, this);
        }
    }
}
