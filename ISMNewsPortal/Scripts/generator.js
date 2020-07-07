let inputList = document.querySelectorAll(".generateCharacterCount");

for (let i = 0; i < inputList.length; i++) {
    let element = inputList[i];
    CreateCountDisplay(element);
}

function CreateCountDisplay(element) {
    let elementParent = element.parentNode;
    let target = document.createElement("P");
    target.innerHTML = 0 + "/" + element.attributes['data-val-maxlength-max'].value;
    elementParent.append(target);
    element.addEventListener("input", function () {
        let maxLength = element.attributes['data-val-maxlength-max'].value;
        target.innerHTML = element.value.length + "/" + maxLength;
    })
}