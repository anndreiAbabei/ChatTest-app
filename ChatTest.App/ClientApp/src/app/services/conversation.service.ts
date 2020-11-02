import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { UserService } from './user.service';
import { Observable } from 'rxjs';
import { CookieService } from './cookie.service';

@Injectable()
export class ConversationService {
    private readonly lastConversationCookieName = '_lastConversationId';

    constructor(private readonly api: ApiService,
                private readonly userService: UserService,
                private readonly cookieService: CookieService) {
      
    }

    public getAll(): Observable<Conversation[]> {
      if (!this.api.authentication) {
        const user = this.userService.getUser();
        if(user)
          this.api.authentication = user.token;
      }

      return this.api.get('Conversation');
    }



    public markConversationRead(id: string): Observable<any> {
      if (!this.api.authentication) {
        const user = this.userService.getUser();
        if(user)
          this.api.authentication = user.token;
      }

      return this.api.patch(`Conversation/${id}/read`, {});
    }



    public delete(id: string): Observable<any> {
      if (!this.api.authentication) {
        const user = this.userService.getUser();
        if(user)
          this.api.authentication = user.token;
      }

      return this.api.delete(`Conversation/${id}`);
    }

    public getLastConversation(): string {
      return this.cookieService.getCookie(this.lastConversationCookieName);
    }

    public setLastConversation(id: string): void {
      this.cookieService.setCookie(this.lastConversationCookieName, id, 100);
    }
}
