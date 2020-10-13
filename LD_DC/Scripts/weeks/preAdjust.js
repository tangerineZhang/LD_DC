function adjustRem() {
if (window.innerWidth < 750) {
	document.getElementsByTagName("html")[0].style.fontSize = (100 * (window.innerWidth / 750)) + "px";
} else { 
	document.getElementsByTagName("html")[0].style.fontSize = "100px";	
}
}

adjustRem();
window.onresize = function(){
	adjustRem();
}