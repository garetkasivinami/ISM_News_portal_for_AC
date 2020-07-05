let imagePathInput = document.querySelector("#imagePath");
let imageElement = document.querySelector("#imageElement");

function UpdateImage () {
    imageElement.src = imagePathInput.value;
}

imagePathInput.addEventListener("input", UpdateImage);
UpdateImage();