mergeInto(LibraryManager.library, {

    UploadFile: function(path, data, objectName, callback, fallback) {

        var parsedPath = stringToUTF8(path);
        var parsedData = stringToUTF8(data);
        var parsedObjectName = stringToUTF8(objectName);
        var parsedCallback = stringToUTF8(callback);
        var parsedFallback = stringToUTF8(fallback);

        try {

            firebase.storage().ref(parsedPath).put(base64ToArrayBuffer(parsedData)).then(function(snapshot) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: data was posted to " + parsedPath);
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }

        function base64ToArrayBuffer(base64) {
            var binary_string = window.atob(base64);
            var len = binary_string.length;
            var bytes = new Uint8Array(len);
            for (var i = 0; i < len; i++) {
                bytes[i] = binary_string.charCodeAt(i);
            }
            return bytes.buffer;
        }
    },

    DownloadFile: function(path, objectName, callback, fallback) {

        var parsedPath = stringToUTF8(path);
        var parsedObjectName = stringToUTF8(objectName);
        var parsedCallback = stringToUTF8(callback);
        var parsedFallback = stringToUTF8(fallback);

        try {

            firebase.storage().ref(parsedPath).getDownloadURL().then(function(url) {
                
                var xhr = new XMLHttpRequest();
                xhr.responseType = 'arraybuffer';
                xhr.onload = function(event) {
                  var data = xhr.response;
                  window.unityInstance.SendMessage(parsedObjectName, parsedCallback, arrayBufferToBase64(data));
                };
                xhr.open('GET', url);
                xhr.send();
              
              }).catch(function(error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
              });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }

        function arrayBufferToBase64( buffer ) {
            var binary = '';
            var bytes = new Uint8Array( buffer );
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode( bytes[ i ] );
            }
            return window.btoa( binary );
        }
    }
});
