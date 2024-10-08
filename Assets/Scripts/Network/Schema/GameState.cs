// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.35
// 

using Colyseus.Schema;
using Action = System.Action;
#if UNITY_5_3_OR_NEWER
using UnityEngine.Scripting;
#endif

public partial class GameState : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve] 
#endif
public GameState() { }
	[Type(0, "ref", typeof(PlayerState))]
	public PlayerState playerOne = new PlayerState();

	[Type(1, "ref", typeof(PlayerState))]
	public PlayerState playerTwo = new PlayerState();

	[Type(2, "int32")]
	public int currentPlayer = default(int);

	[Type(3, "ref", typeof(BoardState))]
	public BoardState board = new BoardState();

	[Type(4, "int32")]
	public int winner = default(int);

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<PlayerState> __playerOneChange;
	public Action OnPlayerOneChange(PropertyChangeHandler<PlayerState> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.playerOne));
		__playerOneChange += __handler;
		if (__immediate && this.playerOne != null) { __handler(this.playerOne, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(playerOne));
			__playerOneChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<PlayerState> __playerTwoChange;
	public Action OnPlayerTwoChange(PropertyChangeHandler<PlayerState> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.playerTwo));
		__playerTwoChange += __handler;
		if (__immediate && this.playerTwo != null) { __handler(this.playerTwo, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(playerTwo));
			__playerTwoChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<int> __currentPlayerChange;
	public Action OnCurrentPlayerChange(PropertyChangeHandler<int> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.currentPlayer));
		__currentPlayerChange += __handler;
		if (__immediate && this.currentPlayer != default(int)) { __handler(this.currentPlayer, default(int)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(currentPlayer));
			__currentPlayerChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<BoardState> __boardChange;
	public Action OnBoardChange(PropertyChangeHandler<BoardState> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.board));
		__boardChange += __handler;
		if (__immediate && this.board != null) { __handler(this.board, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(board));
			__boardChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<int> __winnerChange;
	public Action OnWinnerChange(PropertyChangeHandler<int> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.winner));
		__winnerChange += __handler;
		if (__immediate && this.winner != default(int)) { __handler(this.winner, default(int)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(winner));
			__winnerChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(playerOne): __playerOneChange?.Invoke((PlayerState) change.Value, (PlayerState) change.PreviousValue); break;
			case nameof(playerTwo): __playerTwoChange?.Invoke((PlayerState) change.Value, (PlayerState) change.PreviousValue); break;
			case nameof(currentPlayer): __currentPlayerChange?.Invoke((int) change.Value, (int) change.PreviousValue); break;
			case nameof(board): __boardChange?.Invoke((BoardState) change.Value, (BoardState) change.PreviousValue); break;
			case nameof(winner): __winnerChange?.Invoke((int) change.Value, (int) change.PreviousValue); break;
			default: break;
		}
	}
}

