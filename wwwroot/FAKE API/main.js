let section = document.querySelector("section");
let apiUrl = "https://fakestoreapi.com/products";

fetch(apiUrl)
.then((response) => response.json())
.then((response) => htmlRenderer(response)); 

function htmlRenderer(list) { 
    console.log(list);
for (let i = 0; i < list.length; i++) {
    section.innerHTML += `
    <div class="card" style="width: 30rem;">
    <img src="${list[i].image}" class="img" alt="photo">
    <p class="id">${list[i].id}</p>
    <h5 class="title" style=color:blue>${list[i].title}</h5>
    <span class="price" style = 'display:block'>${list[i].price}</span>
    <p class= "description">${list[i].description}</p>
    <h6 class="category">${list[i].category}<h/6>
    <p class="rate">${list[i].rating.rate}</p>
    <p class="count"> ${list[i].rating.count}</p>
    <a href="https://www.trendyol.com/en/select-country" target="_blank" class="btn btn-primary">Go somewhere</a>
    </div>
    `
}}

// <a href="${list[i].url}" target="_blank" class="btn btn-primary">Go somewhere</a>