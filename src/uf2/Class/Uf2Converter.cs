using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace uf2
{
    public static class Uf2Converter
    {
        public static byte[] BytesToUf2(uint address, byte[] data)
        {
            var numBlocks = (uint)(data.Length / CHUNK_SIZE);
            if (data.Length % CHUNK_SIZE >= 1) numBlocks++;

            var uf2 = new List<byte>();
            var payload = new byte[476];
            for (uint blockNo = 0; blockNo < numBlocks; blockNo++)
            {
                var offset = CHUNK_SIZE * blockNo;
                var targetAddr = address + offset;
                var payloadSize = (uint)Math.Min(data.Length - offset, CHUNK_SIZE);
                Array.Copy(data, offset, payload, 0, payloadSize);
                for (uint i = payloadSize; i < payload.Length; i++) payload[i] = 0;

                uf2.AddRange(BitConverter.GetBytes(FIRST_MAGIC_NUMBER));
                uf2.AddRange(BitConverter.GetBytes(SECOND_MAGIC_NUMBER));
                uf2.AddRange(BitConverter.GetBytes((uint)0));
                uf2.AddRange(BitConverter.GetBytes(targetAddr));
                uf2.AddRange(BitConverter.GetBytes(CHUNK_SIZE));    // payloadSize?
                uf2.AddRange(BitConverter.GetBytes(blockNo));
                uf2.AddRange(BitConverter.GetBytes(numBlocks));
                uf2.AddRange(BitConverter.GetBytes((uint)0));
                uf2.AddRange(payload);
                uf2.AddRange(BitConverter.GetBytes(FINAL_MAGIC_NUMBER));
            }

            return uf2.ToArray();
        }

        public static byte[] BytesToUf2(IEnumerable<BinaryBlock> blocks)
        {
            var uf2 = new List<byte>();

            foreach (var block in blocks)
            {
                uf2.AddRange(BytesToUf2(block.Address, block.Data));
            }

            return uf2.ToArray();
        }

        private const int CHUNK_SIZE = 256;

        private const uint FIRST_MAGIC_NUMBER = 0x0a324655;
        private const uint SECOND_MAGIC_NUMBER = 0x9e5d5157;
        private const uint FINAL_MAGIC_NUMBER = 0x0ab16f30;

    }
}
