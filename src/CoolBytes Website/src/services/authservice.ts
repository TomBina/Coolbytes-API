import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import 'rxjs/add/operator/filter';
import * as auth0 from 'auth0-js';

@Injectable()
export class AuthService {
    private auth0;

    constructor(private router: Router) {
        this.auth0 = new auth0.WebAuth({
            clientID: '1172o11AfEVrHK8QTiqwixHdlTD2nwvA',
            domain: 'coolbytes.auth0.com',
            responseType: 'token id_token',
            audience: 'http://localhost:5000/api/',
            redirectUri: 'http://localhost:4200/processauth',
            scope: 'openid email admin'
        });
    }

    public login(): void {
        this.auth0.authorize();
    }

    public handleAuthentication(): void {
        this.auth0.parseHash((err, authResult) => {
            if (authResult && authResult.accessToken && authResult.idToken) {
                window.location.hash = '';
                this.setSession(authResult);
                this.router.navigate(['/admin']);
            } else if (err) {
                this.router.navigate(['/']);
                console.log(err);
            }
        });
    }

    private setSession(authResult): void {
        const expiresAt = JSON.stringify((authResult.expiresIn * 1000) + new Date().getTime());
        localStorage.setItem('access_token', authResult.accessToken);
        localStorage.setItem('id_token', authResult.idToken);
        localStorage.setItem('expires_at', expiresAt);
    }

    public logout(): void {
        localStorage.removeItem('access_token');
        localStorage.removeItem('id_token');
        localStorage.removeItem('expires_at');

        this.router.navigate(['/home']);
    }

    public isAuthenticated(): boolean {
        const expiresAt = JSON.parse(localStorage.getItem('expires_at'));
        return new Date().getTime() < expiresAt;
    }
}