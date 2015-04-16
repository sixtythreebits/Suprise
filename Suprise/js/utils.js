function Decode(encoded) {
    var keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
    var output = "";
    var chr1, chr2, chr3;
    var enc1, enc2, enc3, enc4;
    var i = 0;

    do {
        enc1 = keyStr.indexOf(encoded.charAt(i++));
        enc2 = keyStr.indexOf(encoded.charAt(i++));
        enc3 = keyStr.indexOf(encoded.charAt(i++));
        enc4 = keyStr.indexOf(encoded.charAt(i++));

        chr1 = (enc1 << 2) | (enc2 >> 4);
        chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
        chr3 = ((enc3 & 3) << 6) | enc4;

        output = output + String.fromCharCode(chr1);

        if (enc3 != 64) {
            output = output + String.fromCharCode(chr2);
        }
        if (enc4 != 64) {
            output = output + String.fromCharCode(chr3);
        }
    } while (i < encoded.length);

    return output;
}

function Encode(data) {
    var keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
    var o1, o2, o3, h1, h2, h3, h4, bits, i = 0, enc = '';

    do { // pack three octets into four hexets
        o1 = data.charCodeAt(i++);
        o2 = data.charCodeAt(i++);
        o3 = data.charCodeAt(i++);

        bits = o1 << 16 | o2 << 8 | o3;

        h1 = bits >> 18 & 0x3f;
        h2 = bits >> 12 & 0x3f;
        h3 = bits >> 6 & 0x3f;
        h4 = bits & 0x3f;

        // use hexets to index into b64, and append result to encoded string
        enc += keyStr.charAt(h1) + keyStr.charAt(h2) + keyStr.charAt(h3) + keyStr.charAt(h4);
    } while (i < data.length);

    switch (data.length % 3) {
        case 1:
            enc = enc.slice(0, -2) + '==';
            break;
        case 2:
            enc = enc.slice(0, -1) + '=';
            break;
    }

    return enc;
}

function gup(name, url) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    if (url == undefined || url == null) {
        url = window.location.href;
    }
    var results = regex.exec(url);
    if (results == null)
        return null;
    else
        return results[1];
}

function HMSToSeconds(hms) {
    try {
        var a = hms.split(':');
        if ((+a[0]) > 12 || (+a[1] > 59) || (+a[2] > 59)) {
            return -1;
        }
        var res = parseInt(seconds = (+a[0]) * 60 * 60 + (+a[1]) * 60 + (+a[2]));
        return isNaN(res) ? -1 : res;
    } catch (ex) {
        return -1;
    }
}

function NewID() {
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}

function s4() {
    return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
};

function SecondsToHMS(TotalSeconds, option) {
    if (TotalSeconds >= 0) {
        switch (option) {
            case 0:
                {
                    var sec_num = TotalSeconds;
                    var hours = Math.floor(sec_num / 3600);
                    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
                    var seconds = sec_num - (hours * 3600) - (minutes * 60);

                    if (hours < 10) { hours = "0" + hours; }
                    if (minutes < 10) { minutes = "0" + minutes; }
                    if (seconds < 10) { seconds = "0" + seconds; }
                    var time = hours + ':' + minutes + ':' + seconds;
                    return time;
                }
            default:
                {
                    TotalSeconds = parseInt(TotalSeconds);
                    var hours = Math.floor(TotalSeconds / 3600);
                    TotalSeconds -= hours * 3600;
                    var minutes = Math.floor(TotalSeconds / 60);
                    TotalSeconds -= minutes * 60;

                    return result = (hours < 10 ? "0" + hours : hours) + "h " + (minutes < 10 ? "0" + minutes : minutes) + "m " + (TotalSeconds < 10 ? "0" + TotalSeconds : TotalSeconds) + "s";
                }
        }
    }
    else {
        return "&mdash;";
    }
}

// call example $(["path to image1","path to image2", "..."]).preload();
$.fn.preload = function () {
    this.each(function () {
        $('<img/>')[0].src = this;
    });
}

$.fn.extend({

    Show: function () {
        this.removeClass("hidden");
    },
    Hide: function () {
        this.addClass("hidden");
    },
    Toggle: function () {
        if (this.hasClass("hidden")) {
            this.removeClass("hidden");
        }
        else {
            this.addClass("hidden");
        }
    },

    Disable: function () {
        this.addClass("disabled");
    },
    Enable: function () {
        this.removeClass("disabled");
    },

    ToSlug: function () {
        var str = this.val();
        str = str.replace(/^\s+|\s+$/g, ''); // trim
        str = str.toLowerCase();

        // remove accents, swap ñ for n, etc
        var from = "àáäâèéëêìíïîòóöôùúüûñç·/_,:;";
        var to = "aaaaeeeeiiiioooouuuunc------";
        for (var i = 0, l = from.length ; i < l ; i++) {
            str = str.replace(new RegExp(from.charAt(i), 'g'), to.charAt(i));
        }

        str = str.replace(/[^a-z0-9 -]/g, '') // remove invalid chars
          .replace(/\s+/g, '-') // collapse whitespace and replace by -
          .replace(/-+/g, '-'); // collapse dashes

        return str;
    }
});

$.fn.GetExtension = function () {
    var val = this.selector.match(/\.[^.]+$/);
    return val == null || val.length == 0 ? undefined : val[0].toLowerCase();
}

$.fn.shake = function (intShakes, intDistance, intDuration) {
    this.each(function () {
        $(this).css("position", "relative");
        for (var x = 1; x <= intShakes; x++) {
            $(this).animate({ left: (intDistance * -1) }, (((intDuration / intShakes) / 4)))
        .animate({ left: intDistance }, ((intDuration / intShakes) / 2))
        .animate({ left: 0 }, (((intDuration / intShakes) / 4)));
        }
    });
    return this;
};