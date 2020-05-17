using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YAMLPipelineValidator.Classes
{
    public class JsonPayload
    {
        public bool PreviewRun { get; set; } = true;
        public string YamlOverride { get; set; }
    }
}
