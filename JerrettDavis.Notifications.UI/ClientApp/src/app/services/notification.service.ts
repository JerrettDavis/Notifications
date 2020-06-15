import {Inject, Injectable} from '@angular/core';
import * as signalR from "@microsoft/signalr";
import {NotificationModel} from "../models/notification.model";
import {HttpClient} from "@angular/common/http";
import {BehaviorSubject, from, Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private connection: signalR.HubConnection;

  private _notificationReceived: BehaviorSubject<NotificationModel> = new BehaviorSubject({} as NotificationModel);
  private _notificationAcknowledged: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public notificationReceived: Observable<NotificationModel> = this._notificationReceived.asObservable();
  public notificationAcknowledged: Observable<string> = this._notificationAcknowledged.asObservable();

  constructor(private _http: HttpClient,
              @Inject('BASE_URL') private _baseUrl: string) {
  }

  public start(accessToken: string): Promise<void> {
    return this.setupConnection(accessToken);
  }

  public stop(): Promise<void> {
    return this.shutdownNotifications();
  }

  public getNotifications(): Observable<NotificationModel[]> {
    return this._http.get<NotificationModel[]>(this._baseUrl + 'api/Notifications');
  }

  public acknowledge(notification: NotificationModel): Observable<void> {
    return from(this.connection.send("acknowledge", notification.identifier));
  }

  private setupConnection(accessToken: string): Promise<void> {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/hubs/notifications', {accessTokenFactory: () => accessToken})
      .withAutomaticReconnect()
      .build();

    this.connection.on("notify", (notification) => {
      this._notificationReceived.next(notification);
    });

    this.connection.on('acknowledge', (identifier: string) => {
      this._notificationAcknowledged.next(identifier);
    });

    return this.connection.start().catch(console.warn);
  }

  private shutdownNotifications(): Promise<void> {
    if (this.connection) {
      return this.connection.stop();
    }

    return new Promise<void>((resolve) => resolve())
  }
}
