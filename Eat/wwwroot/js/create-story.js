const input = document.getElementById("coverInput");
const preview = document.getElementById("coverPreview");

input.addEventListener("change", function () {
    const file = this.files[0];

    if (file) {
        const reader = new FileReader();

        reader.onload = function (e) {

            // placeholder-i sil
            preview.innerHTML = "";

            const img = document.createElement("img");
            img.src = e.target.result;

            preview.appendChild(img);
        };

        reader.readAsDataURL(file);
    }
});
const tagInput = document.getElementById("tagInput");
const addTagBtn = document.getElementById("addTagBtn");
const tagList = document.getElementById("tagList");
const tagsHidden = document.getElementById("tagsHidden");

let tags = [];

addTagBtn?.addEventListener("click", function () {
    const value = tagInput.value.trim();

    if (value && !tags.includes(value)) {
        tags.push(value);

        const span = document.createElement("span");
        span.textContent = value;
        span.style.marginRight = "8px";
        span.style.background = "#eee";
        span.style.padding = "5px 10px";
        span.style.borderRadius = "20px";

        tagList.appendChild(span);

        tagsHidden.value = tags.join(","); // 🔥 Controller-a gedəcək data

        tagInput.value = "";
    }
});

const characterInput = document.getElementById("characterInput");
const addCharacterBtn = document.getElementById("addCharacterBtn");
const characterList = document.getElementById("characterList");
const charactersHidden = document.getElementById("charactersHidden");

let characters = [];

addCharacterBtn?.addEventListener("click", function () {
    const value = characterInput.value.trim();

    if (value && !characters.includes(value)) {
        characters.push(value);

        const span = document.createElement("span");
        span.textContent = value;
        span.style.marginRight = "8px";
        span.style.background = "#eee";
        span.style.padding = "5px 10px";
        span.style.borderRadius = "20px";

        characterList.appendChild(span);

        charactersHidden.value = characters.join(",");

        characterInput.value = "";
    }
});