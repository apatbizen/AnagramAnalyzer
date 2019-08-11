using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramConsole {
    public class StationModel {
        public StationModel(string name, string ruby) {
            Name = name;
            Ruby = ruby;
        }

        public string Name { get; private set; }
        public string Ruby { get; private set; }
    }
}
