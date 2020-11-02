import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { UserService } from './user.service';
import { Observable } from 'rxjs';

@Injectable()
export class MessagesService {
    constructor(private readonly api: ApiService,
                private readonly userService: UserService) {

    }

    public getAll(conversationId: string): Observable<Message[]> {
      if (!this.api.authentication) {
        const user = this.userService.getUser();
        if(user)
          this.api.authentication = user.token;
      }

      return this.api.get(`Messages/${conversationId}`);
    }

    public send(conversationId: string, text: string): Observable<any> {
      if (!this.api.authentication) {
        const user = this.userService.getUser();
        if(user)
          this.api.authentication = user.token;
      }

      return this.api.post(`Messages/${conversationId}`, {text: text});
    }
}
