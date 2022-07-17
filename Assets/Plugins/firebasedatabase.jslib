mergeInto(LibraryManager.library, {

    GetJSON: function(path, objectName, callback, fallback) {
        var parsedPath = stringToUTF8(path);
        var parsedObjectName = stringToUTF8(objectName);
        var parsedCallback = stringToUTF8(callback);
        var parsedFallback = stringToUTF8(fallback);

        try {

            firebase.database().ref(parsedPath).once('value').then(function(snapshot) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(snapshot.val()));
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    PostJSON: function(path, value, objectName, callback, fallback) {
        var parsedPath = stringToUTF8(path);
        var parsedValue = stringToUTF8(value);
        var parsedObjectName = stringToUTF8(objectName);
        var parsedCallback = stringToUTF8(callback);
        var parsedFallback = stringToUTF8(fallback);

        try {

            firebase.database().ref(parsedPath).set(JSON.parse(parsedValue)).then(function(unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedValue + " was posted to " + parsedPath);
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    PushJSON: function(path, value, objectName, callback, fallback) {
        var parsedPath = stringToUTF8(path);
        var parsedValue = stringToUTF8(value);
        var parsedObjectName = stringToUTF8(objectName);
        var parsedCallback = stringToUTF8(callback);
        var parsedFallback = stringToUTF8(fallback);

        try {

            firebase.database().ref(parsedPath).push().set(JSON.parse(parsedValue)).then(function(unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedValue + " was pushed to " + parsedPath);
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    UpdateJSON: function(path, value, objectName, callback, fallback) {
        var parsedPath = stringToUTF8(path);
        var parsedValue = stringToUTF8(value);
        var parsedObjectName = stringToUTF8(objectName);
        var parsedCallback = stringToUTF8(callback);
        var parsedFallback = stringToUTF8(fallback);

        try {

            firebase.database().ref(parsedPath).update(JSON.parse(parsedValue)).then(function(unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedValue + " was updated in " + parsedPath);
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    DeleteJSON: function(path, objectName, callback, fallback) {
        var parsedPath = stringToUTF8(path);
        var parsedObjectName = stringToUTF8(objectName);
        var parsedCallback = stringToUTF8(callback);
        var parsedFallback = stringToUTF8(fallback);

        try {

            firebase.database().ref(parsedPath).remove().then(function(unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedPath + " was deleted");
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    ListenForValueChanged: function(path, objectName, callback, fallback) {
        var parsedPath = stringToUTF8(path);
        var parsedObjectName = stringToUTF8(objectName);
        var parsedCallback = stringToUTF8(callback);
        var parsedFallback = stringToUTF8(fallback);

        try {

            firebase.database().ref(parsedPath).on('value', function(snapshot) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(snapshot.val()));
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    StopListeningForValueChanged: function(path, parsedObjectName, callback, fallback) {
        var parsedPath = stringToUTF8(path);
        var parsedObjectName = stringToUTF8(objectName);
        var parsedCallback = stringToUTF8(callback);
        var parsedFallback = stringToUTF8(fallback);

        try {
            firebase.database().ref(parsedPath).off('value');
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: listener removed");
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    ListenForChildAdded: function(path, objectName, callback, fallback) {
        var parsedPath = stringToUTF8(path);
        var parsedObjectName = stringToUTF8(objectName);
        var parsedCallback = stringToUTF8(callback);
        var parsedFallback = stringToUTF8(fallback);

        try {

            firebase.database().ref(parsedPath).on('child_added', function(snapshot) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(snapshot.val()));
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    StopListeningForChildAdded: function(path, parsedObjectName, callback, fallback) {
        var parsedPath = stringToUTF8(path);
        var parsedObjectName = stringToUTF8(objectName);
        var parsedCallback = stringToUTF8(callback);
        var parsedFallback = stringToUTF8(fallback);

        try {
            firebase.database().ref(parsedPath).off('child_added');
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: listener removed");
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    ListenForChildChanged: function(path, objectName, callback, fallback) {
        var parsedPath = stringToUTF8(path);
        var parsedObjectName = stringToUTF8(objectName);
        var parsedCallback = stringToUTF8(callback);
        var parsedFallback = stringToUTF8(fallback);

        try {

            firebase.database().ref(parsedPath).on('child_changed', function(snapshot) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(snapshot.val()));
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    StopListeningForChildChanged: function(path, parsedObjectName, callback, fallback) {
        var parsedPath = stringToUTF8(path);
        var parsedObjectName = stringToUTF8(objectName);
        var parsedCallback = stringToUTF8(callback);
        var parsedFallback = stringToUTF8(fallback);

        try {
            firebase.database().ref(parsedPath).off('child_changed');
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: listener removed");
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    ListenForChildRemoved: function(path, objectName, callback, fallback) {
        var parsedPath = stringToUTF8(path);
        var parsedObjectName = stringToUTF8(objectName);
        var parsedCallback = stringToUTF8(callback);
        var parsedFallback = stringToUTF8(fallback);

        try {

            firebase.database().ref(parsedPath).on('child_removed', function(snapshot) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(snapshot.val()));
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    StopListeningForChildRemoved: function(path, parsedObjectName, callback, fallback) {
        var parsedPath = stringToUTF8(path);
        var parsedObjectName = stringToUTF8(objectName);
        var parsedCallback = stringToUTF8(callback);
        var parsedFallback = stringToUTF8(fallback);

        try {
            firebase.database().ref(parsedPath).off('child_removed');
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: listener removed");
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    ModifyNumberWithTransaction: function(path, amount, objectName, callback, fallback) {
        var parsedPath = stringToUTF8(path);
        var parsedObjectName = stringToUTF8(objectName);
        var parsedCallback = stringToUTF8(callback);
        var parsedFallback = stringToUTF8(fallback);

        try {

            firebase.database().ref(parsedPath).transaction(function(currentValue) {
                if (!isNaN(currentValue)) {
                    return currentValue + amount;
                } else {
                    return amount;
                }
            }).then(function(unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: transaction run in " + parsedPath);
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    ToggleBooleanWithTransaction: function(path, objectName, callback, fallback) {
        var parsedPath = stringToUTF8(path);
        var parsedObjectName = stringToUTF8(objectName);
        var parsedCallback = stringToUTF8(callback);
        var parsedFallback = stringToUTF8(fallback);

        try {

            firebase.database().ref(parsedPath).transaction(function(currentValue) {
                if (typeof currentValue === "boolean") {
                    return !currentValue;
                } else {
                    return true;
                }
            }).then(function(unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: transaction run in " + parsedPath);
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    }

});