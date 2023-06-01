﻿function SaveFileAsCSV(fileContent, fileName) {
    var link = document.createElement('a');
    link.download = fileName;
    link.href = "data:text/csv;charset=utf-8,%EF%BB%BF" + encodeURIComponent(fileContent);
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function SaveFileAsPDF(contentStreamReference, fileName) {
    const blob = new Blob([contentStreamReference], { type: 'application/pdf;base64' });

    var url = URL.createObjectURL(blob);
    window.open(url, "_blank");
}

function OpenFileAsPDF(contentStreamReference, fileName) {
      
    const blob = new Blob([contentStreamReference], { type: 'application/pdf;base64' });
    var url = URL.createObjectURL(blob);
    window.open(url, "_blank");
    
}

function convertStreamToDataURL(stream) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = () => resolve(reader.result);
        reader.onerror = reject;
        reader.readAsDataURL(stream);
    });
}

async function downloadFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    triggerFileDownload(fileName, url);
    URL.revokeObjectURL(url);
}

function triggerFileDownload(fileName, url) {
    const anchorElement = document.createElement("a");
    anchorElement.href = url;
    anchorElement.target = "_blank";

    if (fileName) {
        anchorElement.download = fileName;
    }
    document.body.appendChild(anchorElement);
    anchorElement.click();
    document.body.removeChild(anchorElement);

    //anchorElement.remove();
}

function copyToClipboard(text) {
    navigator.clipboard.writeText(text);
 }

// START Loading indicator mechanism
function hasClass(el, className) {
    if (el.classList)
        return el.classList.contains(className);
    return !!el.className.match(new RegExp('(\\s|^)' + className + '(\\s|$)'));
}

function addClass(el, className) {
    if (el.classList)
        el.classList.add(className)
    else if (!hasClass(el, className))
        el.className += " " + className;
}

function removeClass(el, className) {
    if (el.classList)
        el.classList.remove(className)
    else if (hasClass(el, className)) {
        var reg = new RegExp('(\\s|^)' + className + '(\\s|$)');
        el.className = el.className.replace(reg, ' ');
    }
}

function showLoadingIndicator() {
    addClass(document.getElementById("tva-loading"), "show");
    addClass(document.getElementById("TVAAppMainBody"), "disable-body");
}

function hideLoadingIndicator() {
    removeClass(document.getElementById("tva-loading"), "show");
    removeClass(document.getElementById("TVAAppMainBody"), "disable-body");
}

function DownladFileAsCSV(contentStreamReference, fileName)
{
    const blob = new Blob([contentStreamReference], { type: 'text/csv;charset=utf-8' });    
    var url = URL.createObjectURL(blob);
    var a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();  
}

function OpenNewTab(URL)
{
    window.open(URL, '_blank');
}
// END Loading indicator mechanism

function writelog(value) {
    console.log('EDM Grid Log:');
    console.log(value);
}

// JavaScript interop
window.addEventListener('message', event => {
    const data = event.data;
    if (data.token) {
        localStorage.setItem('_z', data.token);
    }
});

window.setTopFrameUrl = function (url) {
    if (window.top && window.top !== window.self) {
        window.top.location.href = url;
    } else {
        window.location.href = url;
    }
};
