let imagePathInput = document.querySelector("#imagePath");
let imageElement = document.querySelector("#imageElement");

function UpdateImage() {
    imageElement.src = window.URL.createObjectURL(imagePathInput.files[0]);
}

imagePathInput.addEventListener("change", UpdateImage);
UpdateImage();