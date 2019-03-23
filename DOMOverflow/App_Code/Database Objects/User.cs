using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOMOverflow {
    public class User {
        public readonly string username, email;
        public readonly Guid id;
        public readonly EnumUserGroup group;
        
        public User(string username, string email, Guid id, EnumUserGroup group) {
            this.username   = username;
            this.email      = email;
            this.id         = id;
            this.group      = group;
        }
    }
}