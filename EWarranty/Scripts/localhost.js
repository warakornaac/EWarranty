 function getlocalhost(){

    
     var url;
    //console.log("Output;");
    //console.log(location.hostname);
    //console.log(document.domain);
    ////alert(window.location.hostname)

    //console.log("document.URL : " + document.URL);
    //console.log("document.location.href : " + document.location.href);
    //console.log("document.location.origin : " + document.location.origin);
    //console.log("document.location.hostname : " + document.location.hostname);
    //console.log("document.location.host : " + document.location.host);
    //console.log("document.location.pathname : " + document.location.pathname);
    var array = document.location.pathname.split("/");
    if (document.domain == "localhost") {

        url = document.location.origin;
    } else {
        url = document.location.origin +"/" +array[1];
    }
    return url;
    // console.log(result);
    //  alert(result)
}