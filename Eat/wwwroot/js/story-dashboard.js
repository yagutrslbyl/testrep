const newPartBtn = document.getElementById("newPartBtn");
const storyTitle = document.getElementById("storyTitle");

// + New Part → write editor
newPartBtn.addEventListener("click", () => {
  // Yeni bölüm için editor açılır
  window.location.href = "write-story.html?part=new";
});

// Story title → dashboard refresh (zaten buradayız ama mantık bu)
storyTitle.addEventListener("click", () => {
  window.location.href = "story-dashboard.html";
});
