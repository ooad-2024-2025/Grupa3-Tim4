import axios from "axios";

const api = axios.create({
  baseURL: "/api", // sada koristi proxy
});

export default api;
