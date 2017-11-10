import { AuthorsService } from './authorsservice/authors.service';
import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { Observable } from "rxjs/Rx";

@Injectable()
export class AdminAuthorGuardService implements CanActivate {

  constructor(private _router: Router, private _authorsService: AuthorsService) { }

  private _hasAuthor: boolean;

  public canActivate(): Observable<boolean> | true {
    if (this._hasAuthor)
      return true;

    return Observable.create(observer => {
      this._authorsService.get().subscribe(
        a => { observer.next(true); this._hasAuthor = true; },
        e => { observer.next(false); this._router.navigateByUrl("admin/author"); },
        () => observer.complete());
    });
  }
}
