const editor = document.getElementById("editor");
const imageInput = document.getElementById("imageInput");
const videoInput = document.getElementById("videoInput");

imageInput.addEventListener("change", () => {
  const file = imageInput.files[0];
  if (!file) return;

  const reader = new FileReader();
  reader.onload = () => {
    const img = document.createElement("img");
    img.src = reader.result;
    editor.appendChild(img);
  };
  reader.readAsDataURL(file);
});

videoInput.addEventListener("change", () => {
  const file = videoInput.files[0];
  if (!file) return;

  const url = URL.createObjectURL(file);
  const video = document.createElement("video");
  video.src = url;
  video.controls = true;
  editor.appendChild(video);
});
