import axios from "axios";

var apiCall = axios.create({
    baseURL : "https://localhost:7004/api/",
    timeout : 10000,
    headers : {
        "Content-Type" : "application/json"
    }
});

export default apiCall

