function Filter(callback, clickedPage) {
    const nameValue = document.getElementById("name").value;
    

    let xhr = new XMLHttpRequest();

    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {

            callback(this);

        }
    }

    xhr.open("GET", "https://localhost:7241/Admin/Contact/Filter?name=" + nameValue + "&page=" + clickedPage, true);

    xhr.send();
}

function FilterByPagination() {
    const paginations = document.querySelectorAll(".pagination a");

    for (let pagination of paginations) {
        if (typeof pagination.onclick == "function") {
            continue;
        }
        pagination.addEventListener("click", function (e) {
            e.preventDefault();

            const clickedPage = this.innerText;

            Filter((res) => {

                const previousActive = document.querySelector(".pagination li.active");

                previousActive.classList.remove("active");

                pagination.parentElement.classList.add("active");

                const result = JSON.parse(res.responseText);

                document.querySelector("#table-area").innerHTML = result.data;

                FilterByPagination();


            }, clickedPage)



        })

    }
}