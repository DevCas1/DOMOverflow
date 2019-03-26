using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOMOverflow {
    public class Answer : Post {
        public readonly Guid Question;

        public Answer(Guid id, Guid poster, Guid question, DateTime date, string content, int rating)
            : base(id, poster, PostType.ANSWER, date, content, rating) {
            this.Question = question;
        }
    }
}