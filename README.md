# Imagegram Social

## Requirement
User stories (where the user is an API consumer):
- As a user, I should be able to create posts with images (1 post - 1 image)
- As a user, I should be able to set a text caption when I create a post
- As a user, I should be able to comment on a post
- As a user, I should be able to delete my comments (one by one)
- As a user, I should be able to get the list of all posts along with the last 2 comments
to each post

## Functional requirements
- RESTful Web API (JSON)
- Maximum image size - 100MB
- Allowed image formats: .png, .jpg, .bmp.
- Save uploaded images in the original format
- Convert uploaded images to .jpg format
- Serve images only in .jpg format
- Posts should be sorted by the number of comments (desc)
- Retrieve posts via a cursor-based pagination

## Non-functional requirements
- Maximum response time for any API call except uploading image files - 50 ms
- Minimum throughput handled by the system - 100 RPS
- Users have a slow and unstable internet connection

## Usage forecast
- 1k uploaded images per 1h
- 100k new comments per 1h

## Designer
![Imagegram](/designer.png "Imagegram")

## Components
- Mobile app use Xamarin can process image and posts in offline mode and continue processing data when online.
- The web app accesses the back end service data and images via Traffic Manager. The web app can work in offline mode and continue processing data when online.
- App service is used like as a backend (Restfull service) which process the posts/comments, store non-image data to the Cosmos database and store the origin image to the blob storage. This app is configured to auto scale.
- Azure Functions used to converting any images to JPEG and output to the storage.
- Azure Cosmos DB is used to store non-image data to make sure high throughput and low latency
- Azure Redis Cache is used with cache-aside pattern to optimize the performance.
- Blob storage is used to store the origin images and trigger events to the event grid. Block blobs are optimized for uploading large amounts of data efficiently
- Azure Traffic Manager controls the distribution of user traffic for service endpoints in different datacenters in order to deliver a highly responsive and available application.

## Roadmap
- [X] Create designer
- [X] Backend service
- [X] 'ImageToJpg' function app
- [ ] Create deployment script
- [ ] Apply Traffic Manager & configure app scale for backend
- [ ] Mobile + web front end
- [ ] Rename project to Imagegram
- [ ] Apply redis cache and cache-aside pattern
## Bugs
- Do not remove recent comments after remove comment.

## Author
**Hong Pham (Gary)**

- [Profile](https://github.com/hongph85 "Hong Pham")
- [Email](mailto:hongph85@gmail.com?subject=Hi "Hi!")
- [Linkedin](https://www.linkedin.com/in/hongph85 "Linkedin")

## 🤝 Support

Contributions, issues, and feature requests are welcome!

Give a ⭐️ if you like this project!