(function () {
    var d = document, s = d.createElement('script');

    s.src = 'https://nirojdahal.disqus.com/embed.js';

    s.setAttribute('data-timestamp', +new Date());
    (d.head || d.body).appendChild(s);
})();


$(document).ready(function () {
    lazyLoadImages();
    setReadingTime();
});

function setReadingTime() {
    const text = document.getElementById("blog-content").innerText;
    const wpm = 225;
    const words = text.trim().split(/\s+/).length;
    const time = Math.ceil(words / wpm);
    document.getElementById("time").innerText = `${time} min read`;
}