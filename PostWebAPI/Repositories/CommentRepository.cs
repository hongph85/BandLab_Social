using System;
using System.Linq;

namespace PostWebAPI.Controllers
{
    public class CommentRepository
    {
        private CosmosDBContext _context;

        public CommentRepository(CosmosDBContext context)
        {
            _context = context;
        }

        internal void Add(Guid postId, string comment, Guid author)
        {
            _context.Comments.Add(new Comment(postId, comment, author));
        }

        internal void Delete(Guid id)
        {
            var item = _context.Comments.Where(x => x.CommentId == id).FirstOrDefault();
            _context.Comments.Remove(item);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}