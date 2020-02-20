import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Component({
  selector: 'app-players',
  templateUrl: './players.component.html',
})
export class PlayersComponent implements OnInit {
  public message: string = '';

  public messages: string[] = [];

  public hubConnection: HubConnection;

  ngOnInit() {
    this.hubConnection = new HubConnectionBuilder().withUrl("/playersHub").build();
    this.hubConnection.on("Receive", (msg) => {
      this.messages.push(msg);
    });

    this.hubConnection.start()
      .then(() => { console.log("Connection started"); })
      .catch(err => { console.error(err); });

  }

  echo() {
    this.hubConnection.invoke("Echo", this.message);
  }
}
