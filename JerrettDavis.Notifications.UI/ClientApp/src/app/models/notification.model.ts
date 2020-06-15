export interface NotificationModel {
  identifier: string;
  message: string;
  state: NotificationState,
  timeStamp: Date,
  userIdentifier: string
}

export enum NotificationState {
  unacknowledged,
  acknowledged
}
