using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationBLL.Services;

public interface IPasswordHasher
{
    bool Verify(string passwordHash, string inputPassword);
    string Hash(string inputPassword);
}
