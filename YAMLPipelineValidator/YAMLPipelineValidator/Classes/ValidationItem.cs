using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YAMLPipelineValidator.Classes
{
    public class ValidationItem
    {
        public string YAML { get; set; }
        public string PAT { get; set; }
        public string BuildDefinitionId { get; set; }
        public string ProjectUrl { get; set; }

    }
}
