import { Injectable } from '@angular/core';
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';

@Injectable()
export class HubService {
  private connection: HubConnection;
  private messageRecievedCallBack: (message: Message) => void;
  private userConnectedCallBack: (user: string) => void;
  private userDisconnectedCallBack: (user: string) => void;



  public initialize(): Promise<void> {
    this.connection = new HubConnectionBuilder()
      .withUrl('/hub')
      .build();

    this.connection.on('messageReceived', m => this.messageReceived(m));
    this.connection.on('userConnected', u => this.userConnected(u));
    this.connection.on('userDisconnected', u => this.userDisconnected(u));

    return this.connection.start();
  }



  public getConnectionId(): string {
    return this.connection.connectionId;
  }



  public connected(): void {
    this.connection.send('userConnected');
  }



  public onMessageRecieved(callBack: (message: Message) => void) {
    this.messageRecievedCallBack = callBack;
  }



  public onUserConnected(callBack: (user: string) => void) {
    this.userConnectedCallBack = callBack;
  }



  public onUserDisconnected(callBack: (user: string) => void) {
    this.userDisconnectedCallBack = callBack;
  }



  private messageReceived(message: Message): void {
    if (this.messageRecievedCallBack)
      this.messageRecievedCallBack(message);
  }



  private userConnected(user: string): void {
    if (this.userConnectedCallBack)
      this.userConnectedCallBack(user);
  }



  private userDisconnected(user: string): void {
    if (this.userDisconnectedCallBack)
      this.userDisconnectedCallBack(user);
  }
}
