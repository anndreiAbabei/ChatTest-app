import { Injectable } from '@angular/core';
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';

@Injectable()
export class HubService {
  private connection: HubConnection;
  private messageRecievedCallBack: (message: Message) => void;
  private conversationRecievedCallBack: (conversation: Conversation) => void;
  private conversationRemovedCallBack: (conversationId: string) => void;
  private userConnectedCallBack: (user: string) => void;
  private userDisconnectedCallBack: (user: string) => void;



  public initialize(): Promise<void> {
    this.connection = new HubConnectionBuilder()
      .withUrl('/hub')
      .build();

    this.connection.on('messageReceived', m => this.messageReceived(m));
    this.connection.on('conversationRecieved', c => this.conversationRecieved(c));
    this.connection.on('conversationRemoved', cid => this.conversationRemoved(cid));
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

  public onConversationRecieved(callBack: (message: Conversation) => void) {
    this.conversationRecievedCallBack = callBack;
  }
  public onConversationRemovedRecieved(callBack: (conversationId: string) => void) {
    this.conversationRemovedCallBack = callBack;
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



  private conversationRemoved(conversation: Conversation): void {
    if (this.conversationRecievedCallBack)
      this.conversationRecievedCallBack(conversation);
  }



  private conversationRecieved(conversationId: string): void {
    if (this.conversationRemovedCallBack)
      this.conversationRemovedCallBack(conversationId);
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
