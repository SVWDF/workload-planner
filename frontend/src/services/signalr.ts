import * as signalR from "@microsoft/signalr";

const signalRConnection = new signalR
    .HubConnectionBuilder()
    .withUrl("https://localhost:7173/scrumboardHub", { withCredentials: true })
    .withAutomaticReconnect()
    .build();

export default signalRConnection;