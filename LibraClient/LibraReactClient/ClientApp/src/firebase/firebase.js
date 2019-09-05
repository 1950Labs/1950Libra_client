import firebase from "firebase/app";
import "firebase/auth";

const config = {
    apiKey: "AIzaSyD8gl1Nla_j0bS6vlN4TZ8NQ5mIMDH_d6Q",
    authDomain: "librapoc.firebaseapp.com",
    databaseURL: "https://librapoc.firebaseio.com",
    projectId: "librapoc",
    storageBucket: "",
    messagingSenderId: "237999975660",
    appId: "1:237999975660:web:8c7ca8104678348b"
};

if (!firebase.apps.length) {
  firebase.initializeApp(config);
}

const auth = firebase.auth();

export { auth };
