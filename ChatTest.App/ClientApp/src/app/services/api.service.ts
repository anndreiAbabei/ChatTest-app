import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class ApiService {
  private uri = "/api/";

  constructor(private http: HttpClient) {

  }

  public get<T>(resource: string): Observable<T> {
    return this.http.get<T>(this.createRoute(resource));
  }

  public post<T>(resource: string, body: any): Observable<T> {
    return this.http.post<T>(this.createRoute(resource), body);
  }

  private createRoute(resource: string): string {
    return this.uri + resource;
  }
}
