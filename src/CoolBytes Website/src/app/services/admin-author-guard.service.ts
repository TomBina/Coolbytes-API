import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { Observable } from "rxjs/Rx";

import { AuthorsService } from "./authors.service";

@Injectable()
export class AdminAuthorGuardService implements CanActivate {

  constructor(private _router: Router, private _authorsService: AuthorsService) { }

  private isAuthorFound: boolean;

  public canActivate(): Observable<boolean> {
    if (this.isAuthorFound)
      return Observable.of(true);

    return this._authorsService.get()
                               .map(author => { this.isAuthorFound = true; return true; })
                               .catch(err => { this._router.navigate(["admin/author"]); return Observable.of(false); });
  }
}
