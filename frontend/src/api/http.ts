import axios from "axios";

const http = axios.create({
  baseURL: `${import.meta.env.VITE_API_URL}/api`,
  withCredentials: true,
  headers: { "Content-Type": "application/json" }
});

http.interceptors.response.use(
  (response) => response,
  (error: unknown) => {
    if (axios.isAxiosError(error)) {
      const url = error.config?.url ?? "";
      const isAuthRequest = url.includes("/auth/login");

      if (error.response?.status === 401 && !isAuthRequest) {
        window.location.href = "/login";
      }

      const customError = error as typeof error & {
        customErrors?: string[];
      };

      if (error.response?.data?.errors) {
        customError.customErrors =
          error.response.data.errors;
      }
      else {
        customError.customErrors = [
          error.message ||
          "An unknown error occurred"
        ];
      }

      return Promise.reject(customError);
    }

    return Promise.reject({
      customErrors: ["Unexpected error"]
    });
  }
);

export default http;