using System;
using System.Collections.Generic;
using Raylib_cs;
class Program
{
    public static List<Piece> eliminatedPieced = new List<Piece>();
    static void Main(string[] args)
    {
        //Initialize board
        Piece[,] Board = new Piece[8, 8];
        Board = GenerateBoard(Board);

        Raylib.InitWindow(1000, 1000, "Chess");
        Raylib.SetTargetFPS(120);

        int boardOffset = 100;
        Piece selectedPiece = null;
        int currentPlayer = 1;


        //Run game
        bool gameActive = true;
        while (gameActive)
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.WHITE);

            DrawBoard(Board, boardOffset);
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
            {
                // Move piece if legal move
                try
                {
                    if (CheckMousePos(Board, boardOffset).legalMove == true)
                    {
                        Board = MovePiece(selectedPiece, Board, CheckMousePos(Board, boardOffset));
                        currentPlayer = NextPlayer(currentPlayer);
                    }
                    else
                    {
                        selectedPiece = CheckMousePos(Board, boardOffset);
                    }

                    //Check and mark legal moves
                    MoveConditions.CurrentLegalMoves(selectedPiece, Board, currentPlayer);
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Clicked outside of board");
                }


                // if (CheckMousePos(Board, boardOffset).pieceType != PieceType.None)
                // {
                //     Console.WriteLine(selectedPiece.pieceType + " " + selectedPiece.player);
                // }

            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_A))
            {
                currentPlayer = NextPlayer(currentPlayer);
            }

            Raylib.EndDrawing();
        }
    }
    static Piece[,] MovePiece(Piece selectedPiece, Piece[,] Board, Piece newPos)
    {
        Console.WriteLine("\n" + newPos.pieceType + " " + newPos.player);
        if (newPos.player != selectedPiece.player && newPos.player != 0)
        {
            eliminatedPieced.Add(newPos);
            Console.WriteLine("Killed pieces: ");
            for (int i = 0; i < eliminatedPieced.Count; i++)
            {
                Console.Write(eliminatedPieced[i].pieceType + ", ");
            }
        }


        Board[newPos.x, newPos.y].player = selectedPiece.player;
        Board[newPos.x, newPos.y].pieceType = selectedPiece.pieceType;

        // Console.WriteLine("\n" + selectedPiece.x + " " + selectedPiece.y);
        // Console.WriteLine("\n" + newPos.x + " " + newPos.y);

        Board[selectedPiece.x, selectedPiece.y].pieceType = PieceType.None;
        Board[selectedPiece.x, selectedPiece.y].player = 0;


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
                    if (Raylib.GetMouseX() >= boardOffset + x * (boardPixelSize / 8) && Raylib.GetMouseY() >= boardOffset + y * (boardPixelSize / 8)
                    && Raylib.GetMouseX() < boardOffset + x * (boardPixelSize / 8) + boardPixelSize / 8 && Raylib.GetMouseY() < boardOffset + y * (boardPixelSize / 8) + boardPixelSize / 8)
                    {
                        return Board[x, y];
                    }
                }
            }
        }
        return null;
    }
    static void DrawBoard(Piece[,] Board, int boardOffset)
    {
        //Draw checker board
        int boardPixelSize = Raylib.GetScreenHeight() - boardOffset * 2;
        for (int x = 0; x < Board.GetLength(0); x++)
        {
            for (int y = 0; y < Board.GetLength(1); y++)
            {
                if ((x + y) % 2 != 0)
                {
                    Raylib.DrawRectangle(boardOffset + x * (boardPixelSize / 8), boardOffset + y * (boardPixelSize / 8), boardPixelSize / 8, boardPixelSize / 8, Color.BLACK);
                }
                if (Board[x, y].legalMove == true)
                {
                    Raylib.DrawRectangle(boardOffset + x * (boardPixelSize / 8), boardOffset + y * (boardPixelSize / 8), boardPixelSize / 8, boardPixelSize / 8, Color.GREEN);
                }
            }
        }

        // Draw pieces
        for (int x = 0; x < Board.GetLength(0); x++)
        {
            for (int y = 0; y < Board.GetLength(1); y++)
            {
                if (Board[x, y].pieceType != PieceType.None)
                {
                    string pieceText = Board[x, y].pieceType.ToString();
                    var color = Color.BLUE;
                    if (Board[x, y].player == 1) color = Color.BEIGE;
                    else if (Board[x, y].player == 2) color = Color.GRAY;
                    Raylib.DrawText(pieceText.ToString(), boardOffset + x * (boardPixelSize / 8) + x, boardOffset + y * (boardPixelSize / 8) + y, 25, color);
                }
            }
        }
        // for (int x = 0; x < Board.GetLength(0); x++)
        // {
        //     for (int y = 0; y < Board.GetLength(1); y++)
        //     {
        //         int spaceTillEdge = 0;
        //         if (x > y)
        //         {
        //             spaceTillEdge = (Board.GetLength(0) - x) - 1;
        //         }
        //         else
        //         {
        //             spaceTillEdge = (Board.GetLength(0) - y) - 1;
        //         }

        //         Raylib.DrawText(spaceTillEdge.ToString(), boardOffset + x * (boardPixelSize / 8) + x, boardOffset + y * (boardPixelSize / 8) + y, 25, Color.BLUE);
        //     }
        // }
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
                    player = 1;
                    if (x == 0 || x == 7) tempType = PieceType.Rook;
                    else if (x == 1 || x == 6) tempType = PieceType.Knight;
                    else if (x == 2 || x == 5) tempType = PieceType.Bishop;
                    else if (x == 3) tempType = PieceType.Queen;
                    else if (x == 4) tempType = PieceType.King;
                }
                else if (y == 1)
                {
                    tempType = PieceType.Pawn;
                    player = 1;
                }
                if (y == 7)
                {
                    player = 2;
                    if (x == 0 || x == 7) tempType = PieceType.Rook;
                    else if (x == 1 || x == 6) tempType = PieceType.Knight;
                    else if (x == 2 || x == 5) tempType = PieceType.Bishop;
                    else if (x == 3) tempType = PieceType.King;
                    else if (x == 4) tempType = PieceType.Queen;
                }
                else if (y == 6)
                {
                    player = 2;
                    tempType = PieceType.Pawn;
                }
                Board[x, y] = new Piece(player, x, y, tempType, false);
            }
        }
        return Board;
    }
    static int NextPlayer(int currentPlayer)
    {
        if (currentPlayer == 1) currentPlayer = 2;
        else currentPlayer = 1;
        return currentPlayer;
    }
    static void LoadTextures()
    {
        // Image test = LoadImage("resources/raylib_logo.png");     // Loaded in CPU memory (RAM)
        // Texture2D texture = LoadTextureFromImage(image);
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