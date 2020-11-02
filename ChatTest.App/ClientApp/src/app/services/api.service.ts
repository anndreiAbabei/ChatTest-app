import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class ApiService {
  private uri = '/api/';



  authentication: string;

  constructor(private readonly http: HttpClient) {
    this.authentication = '';
  }

  public get<T>(resource: string): Observable<T> {
    const headers = {
      "Authorisation": this.authentication 
    };

    return this.http.get<T>(this.createRoute(resource), {headers: headers});
  }



  public post<T>(resource: string, body: any): Observable<T> {
    const headers = {
      "Authorisation": this.authentication 
    };

    return this.http.post<T>(this.createRoute(resource), body, {headers: headers});
  }



  public patch<T>(resource: string, body: any): Observable<T> {
    const headers = {
      "Authorisation": this.authentication 
    };

    return this.http.patch<T>(this.createRoute(resource), body, {headers: headers});
  }



  public put<T>(resource: string, body: any): Observable<T> {
    const headers = {
      "Authorisation": this.authentication 
    };

    return this.http.put<T>(this.createRoute(resource), body, {headers: headers});
  }



  public delete<T>(resource: string): Observable<any> {
    const headers = {
      "Authorisation": this.authentication 
    };

    return this.http.delete<T>(this.createRoute(resource), {headers: headers});
  }

  private createRoute(resource: string): string {
    return this.uri + resource;
  }
}
