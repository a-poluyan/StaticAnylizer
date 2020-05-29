using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticAnalyzer
{
    public class ListViewErrors
    {
        public string FilePath { get; set; }
        public List<Error> Errors { get; set; } = new List<Error>();
        public enum Criticality
        {
            Низкий,
            Средний,
            Высокий,
            Критический,
        };

        public class Error
        {
            public string Location { get; set; }
            public string Description { get; set; }
            public Criticality Criticality { get; set; }
            public string Suggestions { get; set; }
        }
    }
}
