namespace FinalProject.wwwroot.js1
{
    
    document.addEventListener("DOMContentLoaded", function () {
        const contentContainer = document.getElementById("content");
        const loadMoreButton = document.getElementById("load-more-btn");
        let currentPage = 1; // Initial page

        // Function to load more content
        function loadMore() {
            fetch(`data-page-${currentPage}.json`) // Assuming your data is paginated and stored in JSON files
                .then(response => response.json())
                .then(data => {
                    // Append loaded content to the container
                    data.forEach(item => {
                        const contentItem = document.createElement("div");
                        contentItem.classList.add("col-md-4"); // Assuming Bootstrap grid layout
                        contentItem.innerHTML = `
            <div class="card mb-3">
              <div class="card-body">
                <h5 class="card-title">${item.title}</h5>
                <p class="card-text">${item.description}</p>
              </div>
            </div>
          `;
                        contentContainer.appendChild(contentItem);
                    });

                    // Increment page number
                    currentPage++;

                    // If there's no more content, hide the load more button
                    if (data.length === 0) {
                        loadMoreButton.style.display = "none";
                    }
                })
                .catch(error => console.error('Error fetching data:', error));
        }

        // Load more content when the button is clicked
        loadMoreButton.addEventListener("click", loadMore);

        // Load initial content
        loadMore();
    });


}
