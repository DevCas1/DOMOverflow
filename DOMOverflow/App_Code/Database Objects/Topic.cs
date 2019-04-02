using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOMOverflow {
    public class Topic {
        public readonly Guid UUID;
        public readonly string name, description;

        public Topic(Guid id, string name, string desc) {
            this.UUID = id;
            this.name = name;
            this.description = desc;
        }
    }
}