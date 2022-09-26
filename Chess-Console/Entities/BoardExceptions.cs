using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Console.Entities
{
    internal class BoardExceptions : ApplicationException
    {
        public BoardExceptions(string message): base (message)
        {

        }
    }
}
