import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';

@Component({
    selector: 'app-chat',
    templateUrl: './chat.component.html',
    styleUrls: ['./chat.component.scss']
})
/** chat component*/
export class ChatComponent implements OnInit {
  selConv: Conversation;
  conversations: Conversation[];
  messages: Message[];
  message: string;
  search: string;
  online: boolean;
  user: User;

  constructor(private userService: UserService) {
    this.online = true;

    this.conversations = [
      {

        id: "1",
        name: "Test",
        text: "Hellooo",
        online: true,
        read: false,
        messages: [
          {
            text: "Heellloooo",
            isMine: false
          },
          {
            text: "Hi",
            isMine: true
          },
          {
            text: "What?",
            isMine: true
          },
          {
            text: "What are you doing tnit",
            isMine: false
          },
          {
            text: "tonight*",
            isMine: false
          },
          {
            text: "?",
            isMine: false
          }
        ]
      },
      {

        id: "2",
        name: "Test2",
        text: "Hi again",
        read: true,
        online: true,
      },
      {

        id: "3",
        name: "Group",
        text: "user1: Hy",
        read: false,
        online: false
      }
    ];

    this.select(this.conversations[0]);
  }

  ngOnInit(): void {
    if (!this.userService.haveUser()) 
      this.userService.register().subscribe(u => {
        this.userService.save(u)
        this.user = this.userService.getUser();
      });
    else
      this.user = this.userService.getUser();
  }

  public select(conv: Conversation): void {
    this.selConv = conv;
    this.selConv.read = true;
    this.messages = this.selConv.messages;
  }

  public send(): void {
    if (!this.message || this.message.trim().length === 0)
      return;

    this.messages.push({
      text: this.message,
      isMine: true
    });

    this.message = "";
  }
}


interface Conversation {
  id: string;
  name: string;
  text: string;
  read: boolean;
  online: boolean;
  messages?: Message[];
}


interface Message {
  text: string;
  isMine: boolean;
}
