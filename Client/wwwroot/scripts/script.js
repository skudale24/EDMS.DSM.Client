function SaveFileAsCSV(fileContent, fileName) {
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

    if (fileName) {
        anchorElement.download = fileName;
    }

    anchorElement.click();
    anchorElement.remove();
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
    addClass(document.getElementById("tiva-loading"), "show");
    addClass(document.getElementById("TivaAppMainBody"), "disable-body");
}

function hideLoadingIndicator() {
    removeClass(document.getElementById("tiva-loading"), "show");
    removeClass(document.getElementById("TivaAppMainBody"), "disable-body");
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