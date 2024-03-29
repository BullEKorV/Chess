﻿using System;
using System.Collections.Generic;
using Raylib_cs;
using System.Numerics;
class Program
{
    static void Main(string[] args)
    {
        //Initialize board
        Piece[,] Board = new Piece[8, 8];
        Board = GenerateBoard(Board);

        Raylib.InitWindow(1920, 1080, "Chess");
        Raylib.ToggleFullscreen();
        Raylib.SetTargetFPS(120);

        int boardOffset = 100;
        Piece selectedPiece = null;
        int currentPlayer = 1;
        bool[] isChecked = { false, false };
        int round = 1;
        bool gameOver = false;

        bool hintsActivated = true;

        List<Piece> allMoves = new List<Piece>();

        Dictionary<String, Texture2D> Textures = LoadTextures();

        //Run game
        while (!Raylib.WindowShouldClose())
        {
            // Render frame
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.WHITE);

            DrawBoard(Board, boardOffset, Textures, isChecked, round, hintsActivated, currentPlayer, gameOver);

            Raylib.EndDrawing();

            // Game logic code
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && gameOver == false)
            {
                // Move piece if legal move
                try
                {
                    if (CheckMousePos(Board, boardOffset) != null)
                    {
                        if (CheckMousePos(Board, boardOffset).legalMove == true)
                        {
                            isChecked[currentPlayer - 1] = false;

                            Board = MovePiece(selectedPiece, Board, CheckMousePos(Board, boardOffset), currentPlayer, allMoves, round);

                            currentPlayer = NextPlayer(currentPlayer);

                            round++;

                            isChecked[currentPlayer - 1] = MoveConditions.IsKingChecked(Board, NextPlayer(currentPlayer));
                        }
                        else
                        {
                            selectedPiece = CheckMousePos(Board, boardOffset);
                        }


                        //Check and mark legal moves
                        MoveConditions.ClearLegalMoves(Board);
                        MoveConditions.CurrentLegalMoves(selectedPiece, Board, currentPlayer, true);

                        // if (selectedPiece.pieceType == PieceType.King)
                        //     MoveConditions.EnPassantCheck(currentPlayer, Board, allMoves);
                    }
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            // Special key commands
            // if (Raylib.IsKeyPressed(KeyboardKey.KEY_A)) // Next player
            //     currentPlayer = NextPlayer(currentPlayer);
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_B)) // Go back step
            {
                if (round > 1)
                {
                    Board = GoBackStep(Board, allMoves);
                    round--;
                    isChecked[currentPlayer - 1] = MoveConditions.IsKingChecked(Board, currentPlayer);
                    gameOver = false;
                    currentPlayer = NextPlayer(currentPlayer);
                    MoveConditions.ClearLegalMoves(Board);
                }
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_G) && (isChecked[0] || isChecked[1])) // Give up
                gameOver = true;
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_R)) // Reset game
                Board = ResetGame(Board, ref currentPlayer, ref round, ref allMoves, ref isChecked, ref gameOver);
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_H)) // Activate or deactivate tips
                hintsActivated = !hintsActivated;
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_Q)) // Quit game
                Raylib.CloseWindow();
        }
    }
    static Piece[,] GoBackStep(Piece[,] Board, List<Piece> allMoves)
    {
        Board[allMoves[allMoves.Count - 1].x, allMoves[allMoves.Count - 1].y].player = allMoves[allMoves.Count - 1].player;
        Board[allMoves[allMoves.Count - 1].x, allMoves[allMoves.Count - 1].y].pieceType = allMoves[allMoves.Count - 1].pieceType;

        Board[allMoves[allMoves.Count - 2].x, allMoves[allMoves.Count - 2].y].player = allMoves[allMoves.Count - 2].player;
        Board[allMoves[allMoves.Count - 2].x, allMoves[allMoves.Count - 2].y].pieceType = allMoves[allMoves.Count - 2].pieceType;

        allMoves.RemoveRange(allMoves.Count - 2, 2);

        return Board;
    }
    static Piece[,] MovePiece(Piece selectedPiece, Piece[,] Board, Piece newPos, int currentPlayer, List<Piece> allMoves, int round)
    {
        // Add moves to all registered moves
        allMoves.Add(new Piece(selectedPiece.player, selectedPiece.x, selectedPiece.y, selectedPiece.pieceType, selectedPiece.legalMove));
        allMoves.Add(new Piece(newPos.player, newPos.x, newPos.y, newPos.pieceType, newPos.legalMove));

        // Move player to new position
        Board[newPos.x, newPos.y].player = selectedPiece.player;
        Board[newPos.x, newPos.y].pieceType = selectedPiece.pieceType;

        // Clear old position
        Board[selectedPiece.x, selectedPiece.y].pieceType = PieceType.None;
        Board[selectedPiece.x, selectedPiece.y].player = 0;


        Board = MoveConditions.Promotion(newPos, currentPlayer, Board);

        return Board;
    }
    static Piece CheckMousePos(Piece[,] Board, int boardOffset)
    {
        int boardPixelSize = Raylib.GetScreenHeight() - boardOffset * 2;
        for (int x = 0; x < Board.GetLength(0); x++)
        {
            for (int y = 0; y < Board.GetLength(1); y++)
            {
                {
                    if (Raylib.GetMouseX() >= (Raylib.GetScreenWidth() - boardPixelSize) / 2 + x * (boardPixelSize / 8) && Raylib.GetMouseY() >= boardOffset + y * (boardPixelSize / 8)
                    && Raylib.GetMouseX() < (Raylib.GetScreenWidth() - boardPixelSize) / 2 + x * (boardPixelSize / 8) + boardPixelSize / 8 && Raylib.GetMouseY() < boardOffset + y * (boardPixelSize / 8) + boardPixelSize / 8)
                    {
                        return Board[x, y];
                    }
                }
            }
        }
        return null;
    }
    static void DrawBoard(Piece[,] Board, int boardOffset, Dictionary<String, Texture2D> Textures, bool[] isChecked, int round, bool hintsActivated, int currentPlayer, bool gameOver)
    {
        //Draw checker board
        int boardPixelSize = Raylib.GetScreenHeight() - boardOffset * 2;
        for (int x = 0; x < Board.GetLength(0); x++)
        {
            for (int y = 0; y < Board.GetLength(1); y++)
            {
                if ((x + y) % 2 != 0)
                {
                    Raylib.DrawRectangle((Raylib.GetScreenWidth() - boardPixelSize) / 2 + (x * (boardPixelSize / 8)), boardOffset + y * (boardPixelSize / 8), boardPixelSize / 8, boardPixelSize / 8, Color.DARKBROWN);
                }
                else
                    Raylib.DrawRectangle((Raylib.GetScreenWidth() - boardPixelSize) / 2 + x * (boardPixelSize / 8), boardOffset + y * (boardPixelSize / 8), boardPixelSize / 8, boardPixelSize / 8, Color.BEIGE);
                if (Board[x, y].legalMove == true && hintsActivated)
                {
                    Raylib.DrawRectangle((Raylib.GetScreenWidth() - boardPixelSize) / 2 + x * (boardPixelSize / 8) + 10, boardOffset + y * (boardPixelSize / 8) + 10, boardPixelSize / 8 - 20, boardPixelSize / 8 - 20, new Color(0, 130, 20, 190));
                }
            }
        }

        // Write instructions  
        if (round < 3)
            Raylib.DrawText("Press \"H\"\nto toggle guide\n\nPress \"B\"\nto go back\na move", Raylib.GetScreenWidth() - boardPixelSize / 3 - (boardPixelSize / 8), Raylib.GetScreenHeight() / 3, 40, Color.BLACK);

        // Draw check message
        if (!gameOver)
        {
            if (isChecked[0])
                Raylib.DrawText("Check!\nPress \"G\" \nif checkmate", boardOffset / 2, Raylib.GetScreenHeight() / 3, 60, Color.BLACK);
            else if (isChecked[1])
                Raylib.DrawText("Check!\nPress \"G\" \nif checkmate", boardOffset / 2, Raylib.GetScreenHeight() / 3, 60, Color.BLACK);
        }

        // Draw checkMate message
        if (gameOver)
        {
            if (isChecked[0])
                Raylib.DrawText("Checkmate!\nPress \"R\"\nto restart", boardOffset / 2, Raylib.GetScreenHeight() / 3, 60, Color.BLACK);
            if (isChecked[1])
                Raylib.DrawText("Checkmate!\nPress \"R\"\nto restart", boardOffset / 2, Raylib.GetScreenHeight() / 3, 60, Color.BLACK);
        }


        // Show current player
        if (currentPlayer == 1)
            Raylib.DrawText("White's turn", boardOffset / 2, boardOffset, 60, Color.BLACK);
        else if (currentPlayer == 2)
            Raylib.DrawText("Black's turn", boardOffset / 2, boardOffset, 60, Color.BLACK);

        // Draw current round
        Raylib.DrawText("Round " + round, Raylib.GetScreenWidth() - boardPixelSize / 3 - (boardPixelSize / 8), boardOffset, 60, Color.BLACK);

        // Draw letters and numbers around board
        for (int x = 0; x < Board.GetLength(0); x++)
        {
            char[] chars = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            Raylib.DrawText(chars[x].ToString(), (Raylib.GetScreenWidth() - boardPixelSize) / 2 + x * (boardPixelSize / 8) + (boardPixelSize / 8) / 3, boardOffset - (boardPixelSize / 8) / 2, 40, Color.BLACK);
            Raylib.DrawText(chars[x].ToString(), (Raylib.GetScreenWidth() - boardPixelSize) / 2 + x * (boardPixelSize / 8) + (boardPixelSize / 8) / 3, boardOffset + boardPixelSize + (boardPixelSize / 8) / 6, 40, Color.BLACK);
        }
        for (int y = 0; y < Board.GetLength(1); y++)
        {
            Raylib.DrawText((8 - y).ToString(), (Raylib.GetScreenWidth() - boardPixelSize) / 2 - (boardPixelSize / 8) / 2, boardOffset + y * (boardPixelSize / 8) + (boardPixelSize / 8) / 3, 40, Color.BLACK);
            Raylib.DrawText((8 - y).ToString(), (Raylib.GetScreenWidth() - boardPixelSize) / 2 + boardPixelSize + (boardPixelSize / 8) / 3, boardOffset + y * (boardPixelSize / 8) + (boardPixelSize / 8) / 3, 40, Color.BLACK);
        }


        // Draw pieces
        for (int x = 0; x < Board.GetLength(0); x++)
        {
            for (int y = 0; y < Board.GetLength(1); y++)
            {
                if (Board[x, y].pieceType != PieceType.None)
                {
                    // string pieceText = Board[x, y].pieceType.ToString();
                    // var color = Color.BLUE;
                    // if (Board[x, y].player == 1) color = Color.WHITE;
                    // else if (Board[x, y].player == 2) color = Color.BLACK;
                    // Raylib.DrawText(pieceText.ToString(), (Raylib.GetScreenWidth() - boardPixelSize) / 2 + x * (boardPixelSize / 8), boardOffset + y * (boardPixelSize / 8) + y, 25, color);

                    string fileName = "";
                    if (Board[x, y].pieceType != PieceType.Knight)
                        fileName += Board[x, y].pieceType.ToString().ToLower()[0];
                    else
                        fileName += "n";
                    if (Board[x, y].player == 1)
                        fileName += "l";
                    else
                        fileName += "d";

                    // TO DO // Make size scale with boardPixelSize
                    Raylib.DrawTextureEx(Textures[fileName], new Vector2((Raylib.GetScreenWidth() - boardPixelSize) / 2 + x * (boardPixelSize / 8), boardOffset + y * (boardPixelSize / 8)), 0, (float)0.13, Color.WHITE);
                }
            }
        }
    }
    static Piece[,] ResetGame(Piece[,] Board, ref int currentPlayer, ref int round, ref List<Piece> allMoves, ref bool[] isChecked, ref bool gameOver)
    {
        Board = GenerateBoard(Board);
        currentPlayer = 1;
        gameOver = false;
        allMoves.Clear();
        isChecked[0] = false;
        isChecked[1] = false;
        round = 1;

        return Board;
    }
    static Piece[,] GenerateBoard(Piece[,] Board)
    {
        for (int x = 0; x < Board.GetLength(0); x++)
        {
            for (int y = 0; y < Board.GetLength(1); y++)
            {
                PieceType tempType = PieceType.None;
                int player = 0;
                if (y == 0)
                {
                    player = 2;
                    if (x == 0 || x == 7) tempType = PieceType.Rook;
                    else if (x == 1 || x == 6) tempType = PieceType.Knight;
                    else if (x == 2 || x == 5) tempType = PieceType.Bishop;
                    else if (x == 3) tempType = PieceType.Queen;
                    else if (x == 4) tempType = PieceType.King;
                }
                else if (y == 1)
                {
                    tempType = PieceType.Pawn;
                    player = 2;
                }
                if (y == Board.GetLength(1) - 1)
                {
                    player = 1;
                    if (x == 0 || x == 7) tempType = PieceType.Rook;
                    else if (x == 1 || x == 6) tempType = PieceType.Knight;
                    else if (x == 2 || x == 5) tempType = PieceType.Bishop;
                    else if (x == 3) tempType = PieceType.Queen;
                    else if (x == 4) tempType = PieceType.King;
                }
                else if (y == Board.GetLength(1) - 2)
                {
                    player = 1;
                    tempType = PieceType.Pawn;
                }
                Board[x, y] = new Piece(player, x, y, tempType, false);
            }
        }
        return Board;
    }
    public static int NextPlayer(int currentPlayer)
    {
        if (currentPlayer == 1) currentPlayer = 2;
        else currentPlayer = 1;
        return currentPlayer;
    }
    static Dictionary<String, Texture2D> LoadTextures()
    {
        Dictionary<String, Texture2D> Textures = new Dictionary<string, Texture2D>();
        Textures.Add("kd", Raylib.LoadTexture("ChessPieces/kd.png")); // Black king
        Textures.Add("kl", Raylib.LoadTexture("ChessPieces/kl.png")); // White king
        Textures.Add("qd", Raylib.LoadTexture("ChessPieces/qd.png")); // Black queen
        Textures.Add("ql", Raylib.LoadTexture("ChessPieces/ql.png")); // White king
        Textures.Add("bd", Raylib.LoadTexture("ChessPieces/bd.png")); // Black bishop
        Textures.Add("bl", Raylib.LoadTexture("ChessPieces/bl.png")); // White bishop
        Textures.Add("nd", Raylib.LoadTexture("ChessPieces/nd.png")); // Black knight
        Textures.Add("nl", Raylib.LoadTexture("ChessPieces/nl.png")); // White knight
        Textures.Add("rd", Raylib.LoadTexture("ChessPieces/rd.png")); // Black rook
        Textures.Add("rl", Raylib.LoadTexture("ChessPieces/rl.png")); // White rook
        Textures.Add("pd", Raylib.LoadTexture("ChessPieces/pd.png")); // Black pawn
        Textures.Add("pl", Raylib.LoadTexture("ChessPieces/pl.png")); // White pawn

        return Textures;
    }
}

public class Piece
{
    public int player;
    public int x;
    public int y;
    public PieceType pieceType;
    public bool legalMove;
    public Piece(int player, int x, int y, PieceType pieceType, bool legalMove)
    {
        this.player = player;
        this.x = x;
        this.y = y;
        this.pieceType = pieceType;
        this.legalMove = legalMove;
    }
}
public enum PieceType
{
    None, // Ingen lol
    King, //Kung
    Queen, //Drottning
    Rook, //Torn
    Bishop, //Löpare
    Knight, //Häst
    Pawn //Bonde
}