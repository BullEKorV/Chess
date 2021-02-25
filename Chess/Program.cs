using System;
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
        // Raylib.ToggleFullscreen();
        Raylib.SetTargetFPS(120);

        int boardOffset = 100;
        Piece selectedPiece = null;
        int currentPlayer = 1;

        List<Piece> eliminatedPieces = new List<Piece>();

        Dictionary<String, Texture2D> Textures = LoadTextures();

        //Run game
        bool gameActive = true;
        while (gameActive)
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.WHITE);

            DrawBoard(Board, boardOffset, Textures);
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
            {
                // Move piece if legal move
                try
                {
                    if (CheckMousePos(Board, boardOffset).legalMove == true)
                    {
                        Board = MovePiece(selectedPiece, Board, CheckMousePos(Board, boardOffset), eliminatedPieces, currentPlayer);

                        currentPlayer = NextPlayer(currentPlayer);
                    }
                    else
                    {
                        selectedPiece = CheckMousePos(Board, boardOffset);
                    }

                    //Check and mark legal moves
                    MoveConditions.CleanLegalMoves(Board);
                    MoveConditions.CurrentLegalMoves(selectedPiece, Board, currentPlayer, true);
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Something went wrong");
                }
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_A))
            {
                currentPlayer = NextPlayer(currentPlayer);
            }

            Raylib.EndDrawing();
        }
    }
    static Piece[,] MovePiece(Piece selectedPiece, Piece[,] Board, Piece newPos, List<Piece> eliminatedPieces, int currentPlayer)
    {
        // Console.WriteLine("\n" + newPos.pieceType + " " + newPos.player);
        if (newPos.player != selectedPiece.player && newPos.player != 0)
        {
            eliminatedPieces.Add(new Piece(newPos.player, newPos.x, newPos.y, newPos.pieceType, newPos.legalMove)); // Work

            Console.WriteLine("Killed pieces: ");
            for (int i = 0; i < eliminatedPieces.Count; i++)
            {
                Console.Write(eliminatedPieces[i].pieceType + " " + eliminatedPieces[i].player + ", ");
            }
        }


        Board[newPos.x, newPos.y].player = selectedPiece.player;
        Board[newPos.x, newPos.y].pieceType = selectedPiece.pieceType;

        Board[selectedPiece.x, selectedPiece.y].pieceType = PieceType.None;
        Board[selectedPiece.x, selectedPiece.y].player = 0;

        Board = MoveConditions.SpecialPieceConditions(newPos, currentPlayer, Board);

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
    static void DrawBoard(Piece[,] Board, int boardOffset, Dictionary<String, Texture2D> Textures)
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
                if (Board[x, y].legalMove == true)
                {
                    Raylib.DrawRectangle((Raylib.GetScreenWidth() - boardPixelSize) / 2 + x * (boardPixelSize / 8), boardOffset + y * (boardPixelSize / 8), boardPixelSize / 8, boardPixelSize / 8, Color.GREEN);
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
                    // string pieceText = Board[x, y].pieceType.ToString();
                    // var color = Color.BLUE;
                    // if (Board[x, y].player == 1) color = Color.BEIGE;
                    // else if (Board[x, y].player == 2) color = Color.GRAY;
                    // Raylib.DrawText(pieceText.ToString(), boardOffset + x * (boardPixelSize / 8) + x, boardOffset + y * (boardPixelSize / 8) + y, 25, color);

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
                    Raylib.DrawTextureEx(Textures[fileName], new Vector2((Raylib.GetScreenWidth() - boardPixelSize) / 2 + x * (boardPixelSize / 8), boardOffset + y * (boardPixelSize / 8)), 0, (float)0.12, Color.WHITE);
                }
            }
        }
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
                if (y == 7)
                {
                    player = 1;
                    if (x == 0 || x == 7) tempType = PieceType.Rook;
                    else if (x == 1 || x == 6) tempType = PieceType.Knight;
                    else if (x == 2 || x == 5) tempType = PieceType.Bishop;
                    else if (x == 3) tempType = PieceType.Queen;
                    else if (x == 4) tempType = PieceType.King;
                }
                else if (y == 6)
                {
                    player = 1;
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