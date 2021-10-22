import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject, Observable } from 'rxjs';
import {map} from 'rxjs/operators';
import { User } from '../models/user';
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'https://localhost:5001/api/';
  
  constructor(private httpClient: HttpClient) { }

  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  login(model: any) {
    return this.httpClient.post<User>(this.baseUrl+'account/login', model);
  }

  register(model: any) {
    return this.httpClient.post(this.baseUrl+'account/register', model).pipe(
      map((user: User) => {
        if(user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
        return user;
      })
    )
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }

  logout() {
    this.currentUserSource.next(null);
    localStorage.removeItem('user');
  }
}
