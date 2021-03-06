﻿using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using MicroLikeAppFramework.PooledMemoryStreams.Pools.Array;
using Xunit;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.Pools.Array
{
    public class StreamManagerArrayPoolTest: StreamManagerPoolTestBase
    {
        [Fact]
        public void ReuseInstanceTest()
        {
            StreamManagerPool l_StreamManagerPool = CreatePool();
            MemoryBlock l_Block = l_StreamManagerPool.GetBlock();
            l_Block.ReturnBlock();

            MemoryBlock l_Block2 = l_StreamManagerPool.GetBlock();

            Assert.Equal(l_Block, l_Block2);
        }


        public override StreamManagerPool CreatePool()
        {
            return new StreamManagerArrayPool("Test", 1, 1);
        }
    }
}
