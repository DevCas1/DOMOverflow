using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOMOverflow {
    public enum UserGroup {
        NOT_VERIFIED            = 0,
        PROJECT_PARTICIPANT     = 1,
        STUDENT                 = 2,
        TEACHER                 = 3,
        ADMIN                   = 4
    };

    
    public static class UserGroupMethods {
        public static bool CanAskQuestion(this UserGroup group) {
            return (
                group == UserGroup.PROJECT_PARTICIPANT  ||
                group == UserGroup.TEACHER              ||
                group == UserGroup.ADMIN
            );
        }


        public static bool CanGiveAnswer(this UserGroup group) {
            return (
                group == UserGroup.PROJECT_PARTICIPANT  ||
                group == UserGroup.STUDENT              ||
                group == UserGroup.TEACHER              ||
                group == UserGroup.ADMIN
            );
        }


        public static bool CanMakeTopic(this UserGroup group) {
            return (
                group == UserGroup.PROJECT_PARTICIPANT  ||
                group == UserGroup.TEACHER              ||
                group == UserGroup.ADMIN
            );
        }
    }
}