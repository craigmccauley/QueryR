using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryR.Examples.ConsoleApp.MenuSystem
{
    public interface IMenuItem
    {
        string? Description { get; }
        string? Response { get; }
        Func<bool> Run { get; }
    }
}
