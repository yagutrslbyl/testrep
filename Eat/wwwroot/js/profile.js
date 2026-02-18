const profileBtn = document.querySelector(".profile");
const dropdown = document.querySelector(".dropdown");

profileBtn.addEventListener("click", () => {
  dropdown.classList.toggle("show");
});

document.addEventListener("click", (e) => {
  if (!e.target.closest(".profile-wrapper")) {
    dropdown.classList.remove("show");
  }
});
