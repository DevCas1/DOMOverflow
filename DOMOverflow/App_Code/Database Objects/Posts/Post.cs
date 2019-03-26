using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOMOverflow {
    public abstract class Post {
        public readonly Guid UUID;
        public readonly Guid PosterID;
        public readonly PostType Type;
        public readonly DateTime PostDate;
        public readonly string Content;
        public readonly int Rating;


        public Post(Guid id, Guid poster, PostType type, DateTime date, string content, int rating) {
            this.UUID = id;
            this.PosterID = poster;
            this.Type = type;
            this.PostDate = date;
            this.Content = content;
            this.Rating = rating;
        }
    }
}