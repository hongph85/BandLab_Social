using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostWebAPI.Repositories
{
    public class ReadingPostRepository : IPagination, IDisposable
    {
        CosmosClient client;
        private readonly string cosmosDatabaseId = "BandlabDB";
        private readonly string containerId = "Posts";

        public ReadingPostRepository(string cosmosDatabaseId, string containerId, string endpoint, string token)
        {
            this.cosmosDatabaseId = cosmosDatabaseId;
            this.containerId = containerId;
            client = new CosmosClient(endpoint, token);
        }

        private async Task<Container> GetOrCreateContainerAsync(Database database, string containerId)
        {
            ContainerProperties containerProperties = new ContainerProperties(id: containerId, partitionKeyPath: "/PostId");

            return await database.CreateContainerIfNotExistsAsync(
                containerProperties: containerProperties,
                throughput: 400);
        }

        public async Task<PostResponse> GetPosts(int pageSize, string continuationToken)
        {
            var postResponse = new PostResponse();

            var cosmosDatabase = await client.CreateDatabaseIfNotExistsAsync(cosmosDatabaseId);
            var _container = await GetOrCreateContainerAsync(cosmosDatabase, containerId);
            var posts = new List<Post>();
            var queryDef = new QueryDefinition("select * from Post p Order by p.TotalComments Desc");
            string token = null;
            if (!string.IsNullOrWhiteSpace(continuationToken))
                token = continuationToken;

            using FeedIterator<Post> resultSet = _container.GetItemQueryIterator<Post>(queryDefinition: queryDef, continuationToken: token, new QueryRequestOptions { MaxItemCount = pageSize });
            {
                var items = await resultSet.ReadNextAsync();
                foreach (Post item in items)
                {
                    posts.Add(item);
                }

                postResponse.Posts = posts.OrderByDescending(x => x.RecentComments?.Count);
                postResponse.ContinuationToken = items.ContinuationToken;
            }

            return postResponse;
        }

        public virtual void Dispose()
        {
            if (client != null)
            {
                client.Dispose();
            }
        }
    }
}

