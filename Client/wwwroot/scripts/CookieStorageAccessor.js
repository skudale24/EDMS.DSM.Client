export function get() {
    return document.cookie;
}

export function getByName(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

export function set(key, value) {
    document.cookie = `${key}=${value}`;
}

export function writelog(value) {
    console.log('EDM Grid Log:');
    console.log(value);
}