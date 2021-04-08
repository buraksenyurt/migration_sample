using System;
using System.Collections.Generic;
using System.Text;

namespace Analysis
{
    public interface IAlienistService
    {
        Report GetReport(string content);
    }
}
