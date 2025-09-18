import axios from "axios"

const apiCall = axios.create({
    baseURL : "https://localhost:7004/api/arenainformation",
    timeout : 10000,
    headers : {
        "Content-Type" : "application/json"
    }
});

export default apiCall

