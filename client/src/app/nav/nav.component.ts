import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { User } from '../models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  model: any = {};
  loggedIn: boolean = false;
  userName: string;
  constructor(public accountService: AccountService, 
              private router: Router, 
              private toastr: ToastrService) { }

  ngOnInit(): void {
    this.accountService.currentUser$.subscribe((user: User) => {
      if(user) {
        this.userName = user.userName;
        this.router.navigateByUrl('/members');
      }
      else {
        this.router.navigateByUrl('/');
      }
    })
  }

  login() {
    this.accountService.login(this.model).subscribe(response => {
      const user = response;
      if (user) {
        localStorage.setItem('user', JSON.stringify(user));
        this.accountService.setCurrentUser(user);
      }
    },error => {
      this.toastr.error(error.error);
    });
  }

  logout() {
    this.accountService.logout();
  }

}
