(function () {
    var d = document, s = d.createElement('script');

    s.src = 'https://nirojdahal.disqus.com/embed.js';

    s.setAttribute('data-timestamp', +new Date());
    (d.head || d.body).appendChild(s);
})();


//$(document).ready(function () {
//    var disqusPublicKey = "YOUR_PUBLIC_KEY";
//    var disqusShortname = "thenextweb";

//    var urlArray = [];
//    $('.count-comment').each(function () {
//        var url = $(this).attr('data-disqus-url');
//        urlArray.push('thread:link=' + url);
//    });
//});