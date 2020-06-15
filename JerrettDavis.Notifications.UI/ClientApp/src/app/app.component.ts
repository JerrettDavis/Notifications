import {Component, OnInit} from '@angular/core';
import {AuthorizeService} from "../api-authorization/authorize.service";
import {iif, of} from "rxjs";
import {mergeMap} from "rxjs/operators";
import {NotificationService} from "./services/notification.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  set accessToken(value: string) {
    if (this._accessToken != value) {
      this.notification.stop().then(() => {
        if (value)
          return this.notification.start(value);
      })
    }
    this._accessToken = value;
  }

  private _accessToken: string;

  constructor(private authorize: AuthorizeService,
              private notification: NotificationService) {
  }

  ngOnInit(): void {
    this.authorize.isAuthenticated()
      .pipe(mergeMap(a => iif(() => a,
        this.authorize.getAccessToken(),
        of(null))))
      .subscribe((accessToken: string | null) => {
        this.accessToken = accessToken;
      });
  }
}
