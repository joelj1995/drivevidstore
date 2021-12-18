import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AngularFireAuth } from '@angular/fire/auth';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  email: string = '';
  password: string = '';

  constructor(private fireAuth: AngularFireAuth, private router: Router) { }

  ngOnInit(): void {
  }

  submitLogin(e) {
    e.preventDefault();
    this.fireAuth.signInWithEmailAndPassword(this.email, this.password)
      .then(value => {
        localStorage.setItem('firebaseUserID', value.user.uid)
        return value.user.getIdToken();
      })
      .then(token => {
        localStorage.setItem('firebaseJWT', token);
        this.router.navigate(['']);
      })
      .catch(reason => {
        alert('login failed');
        console.log(reason);
      });
  }

}
