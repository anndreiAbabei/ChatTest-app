import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { ConversationService } from '../services/conversation.service';
import { MessagesService } from '../services/messages.service';
import { HubService } from '../services/hub.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {
  selConv: Conversation;
  conversations: Conversation[];
  messages: Message[];
  message: string;
  search: string;
  online: boolean;
  user: User;



  constructor(private readonly userService: UserService,
              private readonly conversationService: ConversationService,
              private readonly messagesService: MessagesService,
              private readonly hub: HubService) {
    this.online = true;

    this.conversations = [];
  }



  ngOnInit(): void {
    this.hub.initialize().then(value => this.initializeApplication());
  }



  public select(conv: Conversation): void {
    this.selConv = conv;
    if (!this.selConv) {
      this.conversationService.setLastConversation('');
      this.messages = [];
      return;
    }

    this.conversationService.setLastConversation(this.selConv.id);
    if (this.selConv.messages) {
      this.messages = this.selConv.messages;
      this.markConversationRead(this.selConv);
    }
    else {
      const selConv = this.selConv;
      this.messagesService.getAll(selConv.id)
        .subscribe(m => {
          selConv.messages = m;
          this.messages = selConv.messages;
          this.markConversationRead(selConv);
        });
    }
  }



  public send(): void {
    if (!this.message || this.message.trim().length === 0)
      return;

    this.messagesService.send(this.selConv.id, this.message)
    .subscribe(() => {

      this.messages.push({
        conversationId: this.selConv.id,
        id: '',
        sender: this.user.name,
        createdAt: new Date(Date.now()),
        text: this.message,
        isMine: true
      });

      this.message = '';
    });
  }


  public videoCall(): void {

  }


  public deleteConversation(): void {
    const selConv = this.selConv;
    if (!confirm(`Do you want to delete the conversation '${selConv.name}'`))
      return;

    this.conversationService.delete(selConv.id)
      .subscribe(() => {
        this.conversations = this.conversations.filter(c => c.id !== selConv.id);

        if(this.conversations.length > 0)
          this.select(this.conversations[0]);
        else
          this.select(null);
      });
  }

  private loadConversations(): void {

    this.conversationService.getAll()
      .subscribe(c => {
        this.conversations = c;

        if (!this.selConv && this.conversations.length > 0) {
          const lastConvId = this.conversationService.getLastConversation();

          if(lastConvId) {
            let conv = this.conversations.find(fc => fc.id === lastConvId);

            if (!conv)
              conv = this.conversations[0];

            this.select(conv);
          }
          else
            this.select(this.conversations[0]);
        }
      });
  }



  private markConversationRead(conversation: Conversation) {
    this.conversationService.markConversationRead(conversation.id)
      .subscribe(() => conversation.read = true);
  }

  private handleMessage(message: Message): void {
    if (message.sender === this.user.name)
      return;

    if (this.selConv && message.conversationId === this.selConv.id)
      this.messages.push(message);
    else {
      const conv = this.conversations.find(c => c.id === message.conversationId);

      if (conv) {
        conv.messages.push(message);
        conv.read = false;
        conv.text = message.text;
      }
    }
  }



  private updateUser(user: User): void {
    this.userService.save(user);
    this.user = this.userService.getUser();

    this.loadConversations();

    this.hub.connected();
  }



  private initializeApplication(): void {
    this.hub.onMessageRecieved(m => this.handleMessage(m));
    this.hub.onUserConnected(u => this.handleConnection(u, true));
    this.hub.onUserDisconnected(u => this.handleConnection(u, false));

    if (!this.userService.haveUser())
      this.userService.register(this.hub.getConnectionId()).subscribe(u => this.updateUser(u));
    else {
      this.user = this.userService.getUser();
      this.userService.updateConnnectionId(this.user.name, this.hub.getConnectionId())
        .subscribe(u => this.updateUser(u));
    }
  }



  private handleConnection(user: string, connected: boolean): void {
    this.conversations.filter(c => c.participants.length === 2 && c.participants.includes(user))
      .forEach(c => c.online = connected);
  }
}
