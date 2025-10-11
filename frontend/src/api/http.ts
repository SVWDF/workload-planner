import axios from "axios";

const http = axios.create({
  baseURL: "https://localhost:7173/api",
  withCredentials: true,
  headers: { "Content-Type": "application/json" }
});

http.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.data?.errors) {
      error.customErrors = error.response.data.errors;
    }
    else {
      error.customErrors = [error.message || "An unknow error occured"];
    }
    return Promise.reject(error);
  }
);

export default http;