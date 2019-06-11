$("#catalog_search_input").on("keyup", catalogSearch);

function catalogSearch() {
    var phrase = document.getElementById('catalog_search_input');
    var elems = document.getElementsByClassName('catalog_item');
    var regPhrase = new RegExp(phrase.value, 'i');
    var flag = false;
    
    for(let i=0; i<elems.length; i++) {
        let text = elems[i].querySelector(".catalog_item_title").textContent;
        flag = regPhrase.test(text);
        if (flag) {
            elems[i].style.display = "";
        } else {
            elems[i].style.display = "none";
        }
    }
}