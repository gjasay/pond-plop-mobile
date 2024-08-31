import { Room, Client } from "@colyseus/core";
import { GameState } from "./schema/RoomState";

export class MyRoom extends Room<GameState> {
  maxClients = 2;

  //Called when room is initialized
  onCreate (options: any) {
    this.setState(new GameState()); //Set the initial state of the room

    /*------------------------------------------------
     * Create a room and set playerOne's sessionId
    -------------------------------------------------*/
    this.onMessage("createRoom", (client, message) => {
      this.state.playerOne.sessionId = client.sessionId;
      console.log(this.state.playerOne.sessionId, "created room");
      client.send("sessionId", client.sessionId);
      client.send("roomId", this.roomId);
    });
  }

  onJoin (client: Client, options: any) {
    console.log(client.sessionId, "joined!");
  }

  onLeave (client: Client, consented: boolean) {
    console.log(client.sessionId, "left!");
  }

  onDispose() {
    console.log("room", this.roomId, "disposing...");
  }
}
