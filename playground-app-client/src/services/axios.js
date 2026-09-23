import axios from "axios";

var apiCall = axios.create({
    baseURL : "https://playgroundarenaapp.onrender.com/api/",
    timeout : 100000,
    headers : {
        "Content-Type" : "application/json"
    }
});

export default apiCall