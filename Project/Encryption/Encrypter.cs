using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;

namespace Project.Encryption
{
    class Encrypter<TBlockCipher, TDigest> : EncryptionBase<TBlockCipher, TDigest>
        where TBlockCipher : IBlockCipher, new()
        where TDigest : IDigest, new()
    {
        protected String EncryptPassword(String plainText)
        {
            return "";
        }
    }
}
