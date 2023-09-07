using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vPlusDrawing.Models;
public class DrawingWithCommand
{
    public Command command { get; set; }
    public Geometry geometry { get; set; }
}
