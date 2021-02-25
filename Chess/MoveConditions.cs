using System;

public class MoveConditions
{
    public static Piece[,] CurrentLegalMoves(Piece selectedPiece, Piece[,] Board, int currentPlayer)
    {
        Console.WriteLine(currentPlayer + " " + selectedPiece.pieceType);
        for (int x = 0; x < Board.GetLength(0); x++)
        {
            for (int y = 0; y < Board.GetLength(1); y++)
            {
                Board[x, y].legalMove = false;
            }
        }
        switch (selectedPiece.pieceType)
        {
            // Rook moving conditions
            case PieceType.King: // King move conditions
                KingCheck(selectedPiece, currentPlayer, Board);
                break;
            case PieceType.Queen: // Queen move conditions
                HorizontalCheck(selectedPiece, currentPlayer, Board);
                DiagonalCheck(selectedPiece, currentPlayer, Board);
                break;
            case PieceType.Rook: // Rook move conditions
                HorizontalCheck(selectedPiece, currentPlayer, Board);
                break;
            case PieceType.Bishop: // Bishop move conditions
                DiagonalCheck(selectedPiece, currentPlayer, Board);
                break;
            case PieceType.Knight: // Knight move conditions
                HorseCheck(selectedPiece, currentPlayer, Board);
                break;
            case PieceType.Pawn: // Pawn move conditions
                PawnCheck(selectedPiece, currentPlayer, Board);
                break;
            default:
                break;
        }
        return Board;
    }
    public static Piece[,] SpecialPieceConditions(Piece selectedPiece, int currentPlayer, Piece[,] Board)
    {
        if (selectedPiece.y == 0 && selectedPiece.player == 1 && selectedPiece.pieceType == PieceType.Pawn) // Make player 1 pawn queen when reach top
            selectedPiece.pieceType = PieceType.Queen;
        if (selectedPiece.y == Board.GetLength(1) && selectedPiece.player == 2 && selectedPiece.pieceType == PieceType.Pawn) // Make player 2 pawn queen when reach bot
            selectedPiece.pieceType = PieceType.Queen;

        return Board;
    }

    static void KingCheck(Piece selectedPiece, int currentPlayer, Piece[,] Board)
    {

        int opponentPlayer = 0;
        if (currentPlayer == 1) opponentPlayer = 2;
        else opponentPlayer = 1;

        for (int x = 0; x < Board.GetLength(0); x++)
        {
            for (int y = 0; y < Board.GetLength(1); y++)
            {
                if (Board[x, y].player == opponentPlayer && Board[x, y].pieceType != PieceType.King)
                {
                    CurrentLegalMoves(selectedPiece, Board, Board[x, y].player);
                }
            }
        }

        if (currentPlayer == selectedPiece.player)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (selectedPiece.x + x >= 0 && selectedPiece.y + y >= 0
                    && selectedPiece.x + x < Board.GetLength(0) && selectedPiece.y + y < Board.GetLength(1))
                        if (Board[selectedPiece.x + x, selectedPiece.y + y].player != currentPlayer)
                            Board[selectedPiece.x + x, selectedPiece.y + y].legalMove = true;
                }
            }
        }
    }
    static void HorizontalCheck(Piece selectedPiece, int currentPlayer, Piece[,] Board)
    {
        int opponentPlayer = 0;
        if (currentPlayer == 1) opponentPlayer = 2;
        else opponentPlayer = 1;

        if (currentPlayer == selectedPiece.player)
        {
            // X conditions
            for (int i = selectedPiece.x + 1; i < Board.GetLength(0); i++)
            {
                if (Board[i, selectedPiece.y].player == currentPlayer) break;
                Board[i, selectedPiece.y].legalMove = true;
                if (Board[i, selectedPiece.y].player == opponentPlayer) break;
            }
            for (int i = selectedPiece.x - 1; i >= 0; i--)
            {
                if (Board[i, selectedPiece.y].player == currentPlayer) break;
                Board[i, selectedPiece.y].legalMove = true;
                if (Board[i, selectedPiece.y].player == opponentPlayer) break;
            }
            // Y conditions
            for (int i = selectedPiece.y + 1; i < Board.GetLength(1); i++)
            {
                if (Board[selectedPiece.x, i].player == currentPlayer) break;
                Board[selectedPiece.x, i].legalMove = true;
                if (Board[selectedPiece.x, i].player == opponentPlayer) break;
            }
            for (int i = selectedPiece.y - 1; i >= 0; i--)
            {
                if (Board[selectedPiece.x, i].player == currentPlayer) break;
                Board[selectedPiece.x, i].legalMove = true;
                if (Board[selectedPiece.x, i].player == opponentPlayer) break;
            }
        }
    }
    static void DiagonalCheck(Piece selectedPiece, int currentPlayer, Piece[,] Board)
    {
        int opponentPlayer = 0;
        if (currentPlayer == 1) opponentPlayer = 2;
        else opponentPlayer = 1;

        if (currentPlayer == selectedPiece.player)
        {
            for (int i = 1; i < Board.GetLength(0); i++) // X+ Y+ conditions
            {
                if (selectedPiece.x + i >= Board.GetLength(0) || selectedPiece.y + i >= Board.GetLength(1)) break;
                // Console.WriteLine("++");
                if (Board[selectedPiece.x + i, selectedPiece.y + i].player == currentPlayer) break;
                Board[selectedPiece.x + i, selectedPiece.y + i].legalMove = true;
                if (Board[selectedPiece.x + i, selectedPiece.y + i].player == opponentPlayer) break;
            }

            for (int i = 1; i < Board.GetLength(0); i++) // X- Y+ conditions
            {
                if (selectedPiece.x - i < 0 || selectedPiece.y + i >= Board.GetLength(1)) break;
                // Console.WriteLine("-+");
                if (Board[selectedPiece.x - i, selectedPiece.y + i].player == currentPlayer) break;
                Board[selectedPiece.x - i, selectedPiece.y + i].legalMove = true;
                if (Board[selectedPiece.x - i, selectedPiece.y + i].player == opponentPlayer) break;
            }

            // Y conditions
            for (int i = 1; i < Board.GetLength(1); i++) // X+ Y- conditions
            {
                if (selectedPiece.x + i >= Board.GetLength(0) || selectedPiece.y - i < 0) break;
                // Console.WriteLine("+-");
                if (Board[selectedPiece.x + i, selectedPiece.y - i].player == currentPlayer) break;
                Board[selectedPiece.x + i, selectedPiece.y - i].legalMove = true;
                if (Board[selectedPiece.x + i, selectedPiece.y - i].player == opponentPlayer) break;
            }
            for (int i = 1; i < Board.GetLength(1); i++) // X- Y- conditions
            {
                if (selectedPiece.x - i < 0 || selectedPiece.y - i < 0) break;
                // Console.WriteLine("--");
                if (Board[selectedPiece.x - i, selectedPiece.y - i].player == currentPlayer) break;
                Board[selectedPiece.x - i, selectedPiece.y - i].legalMove = true;
                if (Board[selectedPiece.x - i, selectedPiece.y - i].player == opponentPlayer) break;
            }
        }
    }
    static void HorseCheck(Piece selectedPiece, int currentPlayer, Piece[,] Board)
    {
        if (currentPlayer == selectedPiece.player)
        {
            // x +- 1
            if (selectedPiece.x >= 1 && selectedPiece.y >= 2)
            {
                if (Board[selectedPiece.x - 1, selectedPiece.y - 2].player != currentPlayer)
                    Board[selectedPiece.x - 1, selectedPiece.y - 2].legalMove = true;
            }
            if (selectedPiece.x <= Board.GetLength(0) - 2 && selectedPiece.y >= 2)
            {
                if (Board[selectedPiece.x + 1, selectedPiece.y - 2].player != currentPlayer)
                    Board[selectedPiece.x + 1, selectedPiece.y - 2].legalMove = true;
            }
            if (selectedPiece.x >= 1 && selectedPiece.y <= Board.GetLength(1) - 3)
            {
                if (Board[selectedPiece.x - 1, selectedPiece.y + 2].player != currentPlayer)
                    Board[selectedPiece.x - 1, selectedPiece.y + 2].legalMove = true;
            }
            if (selectedPiece.x <= Board.GetLength(0) - 2 && selectedPiece.y <= Board.GetLength(1) - 3)
            {
                if (Board[selectedPiece.x + 1, selectedPiece.y + 2].player != currentPlayer)
                    Board[selectedPiece.x + 1, selectedPiece.y + 2].legalMove = true;
            }

            // y +- 1
            if (selectedPiece.x >= 2 && selectedPiece.y >= 1)
            {
                if (Board[selectedPiece.x - 2, selectedPiece.y - 1].player != currentPlayer)
                    Board[selectedPiece.x - 2, selectedPiece.y - 1].legalMove = true;
            }
            if (selectedPiece.x <= Board.GetLength(0) - 3 && selectedPiece.y >= 1)
            {
                if (Board[selectedPiece.x + 2, selectedPiece.y - 1].player != currentPlayer)
                    Board[selectedPiece.x + 2, selectedPiece.y - 1].legalMove = true;
            }
            if (selectedPiece.x >= 2 && selectedPiece.y <= Board.GetLength(1) - 2)
            {
                if (Board[selectedPiece.x - 2, selectedPiece.y + 1].player != currentPlayer)
                    Board[selectedPiece.x - 2, selectedPiece.y + 1].legalMove = true;
            }
            if (selectedPiece.x <= Board.GetLength(0) - 3 && selectedPiece.y <= Board.GetLength(1) - 2)
            {
                if (Board[selectedPiece.x + 2, selectedPiece.y + 1].player != currentPlayer)
                    Board[selectedPiece.x + 2, selectedPiece.y + 1].legalMove = true;
            }
            // Board[selectedPiece.x - 2, selectedPiece.y - 1].legalMove = true;
            // Board[selectedPiece.x + 2, selectedPiece.y - 1].legalMove = true;
            // Board[selectedPiece.x - 2, selectedPiece.y + 1].legalMove = true;
            // Board[selectedPiece.x + 2, selectedPiece.y + 1].legalMove = true;
        }
    }
    static void PawnCheck(Piece selectedPiece, int currentPlayer, Piece[,] Board)
    {
        if (selectedPiece.player == 2 && currentPlayer == 2) //Moving conditions for player 2
        {
            if (Board[selectedPiece.x, selectedPiece.y + 1].pieceType == PieceType.None)
            {
                Board[selectedPiece.x, selectedPiece.y + 1].legalMove = true;
                if (selectedPiece.y == 1)
                {
                    if (Board[selectedPiece.x, selectedPiece.y + 2].pieceType == PieceType.None)
                    {
                        Board[selectedPiece.x, selectedPiece.y + 2].legalMove = true;
                    }
                }
            }
            //Attack diagonally
            if (selectedPiece.x < Board.GetLength(0) - 1 && Board[selectedPiece.x + 1, selectedPiece.y + 1].player == 1)
            {
                Board[selectedPiece.x + 1, selectedPiece.y + 1].legalMove = true;
            }
            if (selectedPiece.x > 0 && Board[selectedPiece.x - 1, selectedPiece.y + 1].player == 1)
            {
                Board[selectedPiece.x - 1, selectedPiece.y + 1].legalMove = true;
            }
        }
        // Moving conditions for player 1
        else if (selectedPiece.player == 1 && currentPlayer == 1)
        {
            if (Board[selectedPiece.x, selectedPiece.y - 1].pieceType == PieceType.None)
            {
                Board[selectedPiece.x, selectedPiece.y - 1].legalMove = true;
                if (selectedPiece.y == 6)
                {
                    if (Board[selectedPiece.x, selectedPiece.y - 2].pieceType == PieceType.None)
                    {
                        Board[selectedPiece.x, selectedPiece.y - 2].legalMove = true;
                    }
                }
            }
            // Attack diagonally
            if (selectedPiece.x < Board.GetLength(0) - 1 && Board[selectedPiece.x + 1, selectedPiece.y - 1].player == 2)
            {
                Board[selectedPiece.x + 1, selectedPiece.y - 1].legalMove = true;
            }
            if (selectedPiece.x > 0 && Board[selectedPiece.x - 1, selectedPiece.y - 1].player == 2)
            {
                Board[selectedPiece.x - 1, selectedPiece.y - 1].legalMove = true;
            }
        }
    }
}