import * as signalR from "@microsoft/signalr";

const signalRConnection = new signalR
    .HubConnectionBuilder()
    .withUrl(
        `${import.meta.env.VITE_API_URL}/scrumboardHub`,
        {
             withCredentials: true, 
             transport: signalR.HttpTransportType.WebSockets,
             skipNegotiation: true
        })
    .withAutomaticReconnect()
    .build();

export default signalRConnection;