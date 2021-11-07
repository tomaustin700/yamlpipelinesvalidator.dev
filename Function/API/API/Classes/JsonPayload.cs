using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Classes
{
    public class JsonPayload
    {
        public bool PreviewRun { get; set; } = true;
        public string YamlOverride { get; set; }
    }
}
