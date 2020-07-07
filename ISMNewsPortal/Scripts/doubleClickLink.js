let links = document.querySelectorAll(".doubleClickLink")

for (let i = 0; i < links.length; i++) {
    links[i].addEventListener("click", function (e) {
        if (links[i].classList.contains("doubleClickLink")) {
            e.preventDefault();
        }
        links[i].classList.remove("doubleClickLink");
    });
}