using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace Project.Encryption
{
    class EncryptionBase<TBlockCipher, TDigest>
        where TBlockCipher : IBlockCipher, new()
        where TDigest : IDigest, new()
    {
        private Encoding encoding;
        private IBlockCipher blockCipher;
        private BufferedBlockCipher cipher;
        private HMac mac;
        private byte[] key;
    }
}
