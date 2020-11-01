import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { CookieService } from './cookie.service';
import { Observable } from 'rxjs';

@Injectable()
export class UserService {
  private user: User;
  private static userCookieKey = "user";

  constructor(private api: ApiService, private cookieService: CookieService) {

  }

  public haveUser(): boolean {
    if (!this.user)
      this.user = this.getUser();

    return !!this.user;
  }

  public getUser(): User | null {
    if (this.user)
      return this.user;

    const cookie = this.cookieService.getCookie(UserService.userCookieKey);

    if (!cookie)
      return null;

    return JSON.parse(cookie);
  }

  public save(user: User): void {
    this.user = user;

    const userJson = JSON.stringify(user);

    this.cookieService.setCookie(UserService.userCookieKey, userJson, 31);
  }

  public register(): Observable<User> {
    return this.api.post<User>("Users/register", { id: "chat-app-test" });
  }
}
