const writeBtn = document.querySelector(".write-btn");
const writeDropdown = document.querySelector(".write-dropdown");

writeBtn.addEventListener("click", () => {
  writeDropdown.classList.toggle("show");
});

document.addEventListener("click", (e) => {
  if (!e.target.closest(".write-wrapper")) {
    writeDropdown.classList.remove("show");
  }
});
