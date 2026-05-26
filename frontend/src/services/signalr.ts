import * as signalR from "@microsoft/signalr";

const signalRConnection = new signalR
    .HubConnectionBuilder()
    .withUrl(
        `${import.meta.env.VITE_API_URL}/scrumboardHub`,
        {
             withCredentials: true, 
        })
    .withAutomaticReconnect()
    .build();

export const startSignalR = async () => {
    if (signalRConnection.state === signalR.HubConnectionState.Disconnected) 
        await signalRConnection.start();
};

export const stopSignalR = async () => {
    if (signalRConnection.state === signalR.HubConnectionState.Connected)
        await signalRConnection.stop();
};

export default signalRConnection;