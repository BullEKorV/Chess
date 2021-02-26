using System;

public class MoveConditions
{
    public static void ClearLegalMoves(Piece[,] Board)
    {
        for (int x = 0; x < Board.GetLength(0); x++)
        {
            for (int y = 0; y < Board.GetLength(1); y++)
            {
                Board[x, y].legalMove = false;
            }
        }
    }
    public static Piece[,] CurrentLegalMoves(Piece selectedPiece, Piece[,] Board, int currentPlayer, bool enableMove)
    {
        // Console.WriteLine(currentPlayer + " " + selectedPiece.pieceType);
        switch (selectedPiece.pieceType)
        {
            // Rook moving conditions
            case PieceType.King: // King move conditions
                KingCheck(selectedPiece, currentPlayer, Board, enableMove);
                break;
            case PieceType.Queen: // Queen move conditions
                HorizontalCheck(selectedPiece, currentPlayer, Board, enableMove);
                DiagonalCheck(selectedPiece, currentPlayer, Board, enableMove);
                break;
            case PieceType.Rook: // Rook move conditions
                HorizontalCheck(selectedPiece, currentPlayer, Board, enableMove);
                break;
            case PieceType.Bishop: // Bishop move conditions
                DiagonalCheck(selectedPiece, currentPlayer, Board, enableMove);
                break;
            case PieceType.Knight: // Knight move conditions
                HorseCheck(selectedPiece, currentPlayer, Board, enableMove);
                break;
            case PieceType.Pawn: // Pawn move conditions
                PawnCheck(selectedPiece, currentPlayer, Board, enableMove);
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
        if (selectedPiece.y == Board.GetLength(1) - 1 && selectedPiece.player == 2 && selectedPiece.pieceType == PieceType.Pawn) // Make player 2 pawn queen when reach bot
            selectedPiece.pieceType = PieceType.Queen;

        return Board;
    }
    public static bool IsKingChecked(Piece[,] Board, int currentPlayer)
    {
        Piece king = null;
        for (int x = 0; x < Board.GetLength(0); x++)
        {
            for (int y = 0; y < Board.GetLength(1); y++)
            {
                if (Board[x, y].pieceType == PieceType.King && Board[x, y].player == Program.NextPlayer(currentPlayer))
                {
                    king = Board[x, y];
                }
            }
        }

        // Check if legal move is dangerous
        for (int x = 0; x < Board.GetLength(0); x++)
        {
            for (int y = 0; y < Board.GetLength(1); y++)
            {
                if (Board[x, y].player == currentPlayer && Board[x, y].pieceType != PieceType.King)
                {
                    MoveConditions.CurrentLegalMoves(Board[x, y], Board, Board[x, y].player, true);
                }
            }
        }
        return Board[king.x, king.y].legalMove;
    }

    static void KingCheck(Piece selectedPiece, int currentPlayer, Piece[,] Board, bool enableMove)
    {

        int opponentPlayer = 0;
        if (currentPlayer == 1) opponentPlayer = 2;
        else opponentPlayer = 1;

        if (currentPlayer == selectedPiece.player)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (selectedPiece.x + x >= 0 && selectedPiece.y + y >= 0
                    && selectedPiece.x + x < Board.GetLength(0) && selectedPiece.y + y < Board.GetLength(1))
                        if (Board[selectedPiece.x + x, selectedPiece.y + y].player != currentPlayer)
                            Board[selectedPiece.x + x, selectedPiece.y + y].legalMove = enableMove;
                }
            }

            // Check if legal move is dangerous
            for (int x = 0; x < Board.GetLength(0); x++)
            {
                for (int y = 0; y < Board.GetLength(1); y++)
                {
                    if (Board[x, y].player == opponentPlayer && Board[x, y].pieceType != PieceType.King)
                    {
                        CurrentLegalMoves(Board[x, y], Board, Board[x, y].player, false);
                    }
                }
            }
        }

    }
    static void HorizontalCheck(Piece selectedPiece, int currentPlayer, Piece[,] Board, bool enableMove)
    {
        int opponentPlayer = 0;
        if (currentPlayer == 1) opponentPlayer = 2;
        else opponentPlayer = 1;

        if (currentPlayer == selectedPiece.player)
        {
            // X conditions
            for (int i = selectedPiece.x + 1; i < Board.GetLength(0); i++)
            {
                if (Board[i, selectedPiece.y].player == currentPlayer && enableMove == true) break;
                if (Board[i, selectedPiece.y].player == opponentPlayer && enableMove == false && Board[i, selectedPiece.y].pieceType == PieceType.King && Board[i, selectedPiece.y].x < Board.GetLength(0) - 1)
                    Board[i + 1, selectedPiece.y].legalMove = enableMove; // Make so king can't back up
                Board[i, selectedPiece.y].legalMove = enableMove;
                if (Board[i, selectedPiece.y].player == opponentPlayer || Board[i, selectedPiece.y].player == currentPlayer) break;
            }
            for (int i = selectedPiece.x - 1; i >= 0; i--)
            {
                if (Board[i, selectedPiece.y].player == currentPlayer && enableMove == true) break;
                if (Board[i, selectedPiece.y].player == opponentPlayer && enableMove == false && Board[i, selectedPiece.y].pieceType == PieceType.King && Board[i, selectedPiece.y].x > 0)
                    Board[i - 1, selectedPiece.y].legalMove = enableMove;// Make so king can't back up
                Board[i, selectedPiece.y].legalMove = enableMove;
                if (Board[i, selectedPiece.y].player == opponentPlayer || Board[i, selectedPiece.y].player == currentPlayer) break;
            }
            // Y conditions
            for (int i = selectedPiece.y + 1; i < Board.GetLength(1); i++)
            {
                if (Board[selectedPiece.x, i].player == currentPlayer && enableMove == true) break;
                if (Board[selectedPiece.x, i].player == opponentPlayer && enableMove == false && Board[selectedPiece.x, i].pieceType == PieceType.King && Board[selectedPiece.x, i].y < Board.GetLength(1) - 1)
                    Board[selectedPiece.x, i + 1].legalMove = enableMove;// Make so king can't back up
                Board[selectedPiece.x, i].legalMove = enableMove;
                if (Board[selectedPiece.x, i].player == opponentPlayer || Board[selectedPiece.x, i].player == currentPlayer) break;
            }
            for (int i = selectedPiece.y - 1; i >= 0; i--)
            {
                if (Board[selectedPiece.x, i].player == currentPlayer && enableMove == true) break;
                if (Board[selectedPiece.x, i].player == opponentPlayer && enableMove == false && Board[selectedPiece.x, i].pieceType == PieceType.King && Board[selectedPiece.x, i].y > 0)
                    Board[selectedPiece.x, i - 1].legalMove = enableMove;// Make so king can't back up
                Board[selectedPiece.x, i].legalMove = enableMove;
                if (Board[selectedPiece.x, i].player == opponentPlayer || Board[selectedPiece.x, i].player == currentPlayer) break;
            }
        }
    }
    static void DiagonalCheck(Piece selectedPiece, int currentPlayer, Piece[,] Board, bool enableMove)
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
                if (Board[selectedPiece.x + i, selectedPiece.y + i].player == currentPlayer && enableMove == true) break;

                if (Board[selectedPiece.x + i, selectedPiece.y + i].player == opponentPlayer && enableMove == false && Board[selectedPiece.x + i, selectedPiece.y + i].pieceType == PieceType.King
                && Board[selectedPiece.x + i, selectedPiece.y + i].x + 1 <= Board.GetLength(0) - 1 && Board[selectedPiece.x + i, selectedPiece.y + i].y + 1 <= Board.GetLength(1) - 1)
                    Board[selectedPiece.x + i + 1, selectedPiece.y + i + 1].legalMove = enableMove; // Make so king can't ++

                Board[selectedPiece.x + i, selectedPiece.y + i].legalMove = enableMove;
                if (Board[selectedPiece.x + i, selectedPiece.y + i].player == opponentPlayer || Board[selectedPiece.x + i, selectedPiece.y + i].player == currentPlayer) break;
            }

            for (int i = 1; i < Board.GetLength(0); i++) // X- Y+ conditions
            {
                if (selectedPiece.x - i < 0 || selectedPiece.y + i >= Board.GetLength(1)) break;
                // Console.WriteLine("-+");
                if (Board[selectedPiece.x - i, selectedPiece.y + i].player == currentPlayer && enableMove == true) break;

                if (Board[selectedPiece.x - i, selectedPiece.y + i].player == opponentPlayer && enableMove == false && Board[selectedPiece.x - i, selectedPiece.y + i].pieceType == PieceType.King
                && Board[selectedPiece.x - i, selectedPiece.y + i].x - 1 >= 0 && Board[selectedPiece.x - i, selectedPiece.y + i].y + 1 <= Board.GetLength(1) - 1)
                    Board[selectedPiece.x - i - 1, selectedPiece.y + i + 1].legalMove = enableMove; // Make so king can't -+

                Board[selectedPiece.x - i, selectedPiece.y + i].legalMove = enableMove;
                if (Board[selectedPiece.x - i, selectedPiece.y + i].player == opponentPlayer || Board[selectedPiece.x - i, selectedPiece.y + i].player == currentPlayer) break;
            }

            // Y conditions
            for (int i = 1; i < Board.GetLength(1); i++) // X+ Y- conditions
            {
                if (selectedPiece.x + i >= Board.GetLength(0) || selectedPiece.y - i < 0) break;
                // Console.WriteLine("+-");
                if (Board[selectedPiece.x + i, selectedPiece.y - i].player == currentPlayer && enableMove == true) break;

                if (Board[selectedPiece.x + i, selectedPiece.y - i].player == opponentPlayer && enableMove == false && Board[selectedPiece.x + i, selectedPiece.y - i].pieceType == PieceType.King
                && Board[selectedPiece.x + i, selectedPiece.y - i].x + 1 <= Board.GetLength(0) - 1 && Board[selectedPiece.x + i, selectedPiece.y - i].y - 1 >= 0)
                    Board[selectedPiece.x + i + 1, selectedPiece.y - i - 1].legalMove = enableMove; // Make so king can't +-

                Board[selectedPiece.x + i, selectedPiece.y - i].legalMove = enableMove;
                if (Board[selectedPiece.x + i, selectedPiece.y - i].player == opponentPlayer || Board[selectedPiece.x + i, selectedPiece.y - i].player == currentPlayer) break;
            }
            for (int i = 1; i < Board.GetLength(1); i++) // X- Y- conditions
            {
                if (selectedPiece.x - i < 0 || selectedPiece.y - i < 0) break;
                // Console.WriteLine("--");
                if (Board[selectedPiece.x - i, selectedPiece.y - i].player == currentPlayer && enableMove == true) break;

                if (Board[selectedPiece.x - i, selectedPiece.y - i].player == opponentPlayer && enableMove == false && Board[selectedPiece.x - i, selectedPiece.y - i].pieceType == PieceType.King
                && Board[selectedPiece.x - i, selectedPiece.y - i].x - 1 >= 0 && Board[selectedPiece.x - i, selectedPiece.y - i].y - 1 >= 0)
                    Board[selectedPiece.x - i - 1, selectedPiece.y - i - 1].legalMove = enableMove; // Make so king can't ++

                Board[selectedPiece.x - i, selectedPiece.y - i].legalMove = enableMove;
                if (Board[selectedPiece.x - i, selectedPiece.y - i].player == opponentPlayer || Board[selectedPiece.x - i, selectedPiece.y - i].player == currentPlayer) break;
            }
        }
    }
    static void HorseCheck(Piece selectedPiece, int currentPlayer, Piece[,] Board, bool enableMove)
    {
        if (currentPlayer == selectedPiece.player)
        {
            // x +- 1
            if (selectedPiece.x >= 1 && selectedPiece.y >= 2)
            {
                if (Board[selectedPiece.x - 1, selectedPiece.y - 2].player != currentPlayer || enableMove == false)
                    Board[selectedPiece.x - 1, selectedPiece.y - 2].legalMove = enableMove;
            }
            if (selectedPiece.x <= Board.GetLength(0) - 2 && selectedPiece.y >= 2)
            {
                if (Board[selectedPiece.x + 1, selectedPiece.y - 2].player != currentPlayer || enableMove == false)
                    Board[selectedPiece.x + 1, selectedPiece.y - 2].legalMove = enableMove;
            }
            if (selectedPiece.x >= 1 && selectedPiece.y <= Board.GetLength(1) - 3)
            {
                if (Board[selectedPiece.x - 1, selectedPiece.y + 2].player != currentPlayer || enableMove == false)
                    Board[selectedPiece.x - 1, selectedPiece.y + 2].legalMove = enableMove;
            }
            if (selectedPiece.x <= Board.GetLength(0) - 2 && selectedPiece.y <= Board.GetLength(1) - 3)
            {
                if (Board[selectedPiece.x + 1, selectedPiece.y + 2].player != currentPlayer || enableMove == false)
                    Board[selectedPiece.x + 1, selectedPiece.y + 2].legalMove = enableMove;
            }

            // y +- 1
            if (selectedPiece.x >= 2 && selectedPiece.y >= 1)
            {
                if (Board[selectedPiece.x - 2, selectedPiece.y - 1].player != currentPlayer || enableMove == false)
                    Board[selectedPiece.x - 2, selectedPiece.y - 1].legalMove = enableMove;
            }
            if (selectedPiece.x <= Board.GetLength(0) - 3 && selectedPiece.y >= 1)
            {
                if (Board[selectedPiece.x + 2, selectedPiece.y - 1].player != currentPlayer || enableMove == false)
                    Board[selectedPiece.x + 2, selectedPiece.y - 1].legalMove = enableMove;
            }
            if (selectedPiece.x >= 2 && selectedPiece.y <= Board.GetLength(1) - 2)
            {
                if (Board[selectedPiece.x - 2, selectedPiece.y + 1].player != currentPlayer || enableMove == false)
                    Board[selectedPiece.x - 2, selectedPiece.y + 1].legalMove = enableMove;
            }
            if (selectedPiece.x <= Board.GetLength(0) - 3 && selectedPiece.y <= Board.GetLength(1) - 2)
            {
                if (Board[selectedPiece.x + 2, selectedPiece.y + 1].player != currentPlayer || enableMove == false)
                    Board[selectedPiece.x + 2, selectedPiece.y + 1].legalMove = enableMove;
            }
        }
    }
    static void PawnCheck(Piece selectedPiece, int currentPlayer, Piece[,] Board, bool enableMove)
    {
        if (selectedPiece.player == 2 && currentPlayer == 2) //Moving conditions for player 2
        {
            if (Board[selectedPiece.x, selectedPiece.y + 1].pieceType == PieceType.None && enableMove == true)
            {
                Board[selectedPiece.x, selectedPiece.y + 1].legalMove = enableMove;
                if (selectedPiece.y == 1)
                {
                    if (Board[selectedPiece.x, selectedPiece.y + 2].pieceType == PieceType.None)
                    {
                        Board[selectedPiece.x, selectedPiece.y + 2].legalMove = enableMove;
                    }
                }
            }
            //Attack diagonally
            if (selectedPiece.x < Board.GetLength(0) - 1 && (Board[selectedPiece.x + 1, selectedPiece.y + 1].player == 1 || enableMove == false))
            {
                Board[selectedPiece.x + 1, selectedPiece.y + 1].legalMove = enableMove;
            }
            if (selectedPiece.x > 0 && (Board[selectedPiece.x - 1, selectedPiece.y + 1].player == 1 || enableMove == false))
            {
                Board[selectedPiece.x - 1, selectedPiece.y + 1].legalMove = enableMove;
            }
        }
        // Moving conditions for player 1
        else if (selectedPiece.player == 1 && currentPlayer == 1)
        {
            if (Board[selectedPiece.x, selectedPiece.y - 1].pieceType == PieceType.None && enableMove == true)
            {
                Board[selectedPiece.x, selectedPiece.y - 1].legalMove = enableMove;
                if (selectedPiece.y == 6)
                {
                    if (Board[selectedPiece.x, selectedPiece.y - 2].pieceType == PieceType.None)
                    {
                        Board[selectedPiece.x, selectedPiece.y - 2].legalMove = enableMove;
                    }
                }
            }
            // Attack diagonally
            if (selectedPiece.x < Board.GetLength(0) - 1 && (Board[selectedPiece.x + 1, selectedPiece.y - 1].player == 2 || enableMove == false))
            {
                Board[selectedPiece.x + 1, selectedPiece.y - 1].legalMove = enableMove;
            }
            if (selectedPiece.x > 0 && (Board[selectedPiece.x - 1, selectedPiece.y - 1].player == 2 || enableMove == false))
            {
                Board[selectedPiece.x - 1, selectedPiece.y - 1].legalMove = enableMove;
            }
        }
    }
}