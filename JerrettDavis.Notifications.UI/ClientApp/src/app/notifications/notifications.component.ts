import {Component, Inject, OnInit} from '@angular/core';
import {NotificationService} from '../services/notification.service';
import {NotificationModel} from '../models/notification.model';
import {map} from 'rxjs/operators';
import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit {
  notifications: NotificationModel[] = [];
  loading = true;

  constructor(private _notification: NotificationService,
              private _http: HttpClient,
              @Inject('BASE_URL') private _baseUrl: string) {
  }

  ngOnInit(): void {
    this._notification.getNotifications()
      .pipe(
        map((notifications: NotificationModel[]) =>
          notifications.sort((a: NotificationModel, b: NotificationModel) =>
            new Date(a.timeStamp).getTime() - new Date(b.timeStamp).getTime()).reverse()))
      .subscribe((notifications: NotificationModel[]) => {
        this.notifications = notifications;
      }).add(() => this.loading = false);

    this._notification.notificationReceived
      .subscribe((notification) => this.notifications.unshift(notification));

    this._notification.notificationAcknowledged
      .subscribe((notification) => {
        if (!notification) {
          return;
        }
        const el = this.notifications.find(n => n.identifier === notification);

        el.state = 1;
      });
  }

  sendNotification() {
    this._http.post(this._baseUrl + 'Api/Notifications', {})
      .subscribe(
        () => {
        },
        console.error);
  }

  acknowledge(notification: NotificationModel) {
    this._notification.acknowledge(notification).subscribe(() => notification.state = 1);
  }
}
