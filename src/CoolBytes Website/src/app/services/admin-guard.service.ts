import { Injectable } from "@angular/core";
import { CanActivate } from "@angular/router";
import { AuthService } from "./auth.service";

@Injectable()
export class AdminGuardService implements CanActivate {

  constructor(private _authService: AuthService) {

  }

  canActivate(): boolean {
    if (this._authService.isAuthenticated())
      return true;

    this._authService.login();

    return false;
  }
}
