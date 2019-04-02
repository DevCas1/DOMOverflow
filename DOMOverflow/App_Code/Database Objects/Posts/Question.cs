using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOMOverflow {
    public class Question : Post {
        public readonly Guid? Answer;
        public readonly string Title;
        public readonly List<Topic> Topics;

        public Question(Guid id, Guid poster, DateTime date, string title, string content, int rating, Guid? answer, List<Topic> topics)
            : base (id, poster, PostType.QUESTION, date, content, rating) {
            this.Answer = answer;
            this.Title = title;
            this.Topics = topics;
        }
    }
}