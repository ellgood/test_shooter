﻿namespace CommonLayer.UserInterface.Utility
{
    internal class SafeProxy
    {
        private const uint Poly = 0xedb88320u;

        private readonly uint[] _table = new uint[16 * 256];

        internal SafeProxy()
        {
            Init(Poly);
        }

        public uint Append(uint crc, byte[] input, int offset, int length)
        {
            uint crcLocal = uint.MaxValue ^ crc;

            uint[] table = _table;
            while (length >= 16)
            {
                uint a = table[3 * 256 + input[offset + 12]]
                         ^ table[2 * 256 + input[offset + 13]]
                         ^ table[1 * 256 + input[offset + 14]]
                         ^ table[0 * 256 + input[offset + 15]];

                uint b = table[7 * 256 + input[offset + 8]]
                         ^ table[6 * 256 + input[offset + 9]]
                         ^ table[5 * 256 + input[offset + 10]]
                         ^ table[4 * 256 + input[offset + 11]];

                uint c = table[11 * 256 + input[offset + 4]]
                         ^ table[10 * 256 + input[offset + 5]]
                         ^ table[9 * 256 + input[offset + 6]]
                         ^ table[8 * 256 + input[offset + 7]];

                uint d = table[15 * 256 + ((byte) crcLocal ^ input[offset])]
                         ^ table[14 * 256 + ((byte) (crcLocal >> 8) ^ input[offset + 1])]
                         ^ table[13 * 256 + ((byte) (crcLocal >> 16) ^ input[offset + 2])]
                         ^ table[12 * 256 + ((crcLocal >> 24) ^ input[offset + 3])];

                crcLocal = d ^ c ^ b ^ a;
                offset += 16;
                length -= 16;
            }

            while (--length >= 0)
            {
                crcLocal = table[(byte) (crcLocal ^ input[offset++])] ^ (crcLocal >> 8);
            }

            return crcLocal ^ uint.MaxValue;
        }

        protected void Init(uint poly)
        {
            uint[] table = _table;
            for (uint i = 0; i < 256; i++)
            {
                uint res = i;
                for (var t = 0; t < 16; t++)
                {
                    for (var k = 0; k < 8; k++)
                    {
                        res = (res & 1) == 1 ? poly ^ (res >> 1) : res >> 1;
                    }

                    table[t * 256 + i] = res;
                }
            }
        }
    }
}