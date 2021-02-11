using System;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}

public class Piece
{
    public int player;
    public int x;
    public int y;
    public Type pieceType;
    public Piece(int player, int x, int y, Type pieceType)
    {
        this.player = player;
        this.x = x;
        this.y = y;
        this.pieceType = pieceType;
    }
}
enum PieceType
{
    King,
    Queen,
    Rook,
    Bishop,
    Knight,
    Pawn
}