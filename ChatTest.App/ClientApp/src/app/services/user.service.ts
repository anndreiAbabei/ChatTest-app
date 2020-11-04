import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { CookieService } from './cookie.service';
import { Observable } from 'rxjs';

@Injectable()
export class UserService {
  private user: User;
  private static userCookieKey = 'user';

  constructor(private readonly api: ApiService,
              private readonly cookieService: CookieService) {

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



  public getAll(): Observable<UserModel[]> {
    if (!this.api.authentication) {
      const user = this.getUser();
      if(user)
        this.api.authentication = user.token;
    }

    return this.api.get<UserModel[]>('Users');
  }

  public save(user: User): void {
    this.user = user;

    const userJson = JSON.stringify(user);

    this.cookieService.setCookie(UserService.userCookieKey, userJson, 31);
  }

  public register(connectionId: string): Observable<User> {
    return this.api.post<User>('Users/register',
      {
        id: 'chat-app-test',
        connectionId: connectionId
      });
  }



  public updateConnnectionId(name: string, connectionId: string): Observable<User> {
    return this.api.put<User>(`Users/${name}`,
      {
        id: 'chat-app-test',
        connectionId: connectionId
      });
  }
}
