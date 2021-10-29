using System;

namespace PostWebAPI
{
    public class UserRepository
    {
        CosmosDBContext context;

        public UserRepository(CosmosDBContext context)
        {
            this.context = context;
        }

        public void Add(User user)
        {
            context.Users.Add(user);
        }

        public void Commit()
        {
            context.SaveChanges();
        }
    }
}