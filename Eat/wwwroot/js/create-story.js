const coverInput = document.getElementById("coverInput");
const coverPreview = document.getElementById("coverPreview");

coverInput.addEventListener("change", () => {
  const file = coverInput.files[0];
  if (!file) return;

  const reader = new FileReader();
  reader.onload = () => {
    coverPreview.innerHTML = `<img src="${reader.result}" />`;
  };
  reader.readAsDataURL(file);
});
