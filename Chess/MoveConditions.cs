using System;

public class MoveConditions
{
    public static Piece[,] CurrentLegalMoves(Piece selectedPiece, Piece[,] Board, int currentPlayer)
    {
        // Console.WriteLine(currentPlayer + " " + selectedPiece.pieceType);
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
            case PieceType.Rook:
                HorizontalCheck(selectedPiece, currentPlayer, Board);
                break;
            case PieceType.Bishop:
                DiagonalCheck(selectedPiece, currentPlayer, Board);
                break;
            case PieceType.Knight:
                HorseCheck(selectedPiece, currentPlayer, Board);
                break;
            // Pawn moving conditions
            case PieceType.Pawn:
                if (selectedPiece.player == 1 && currentPlayer == 1) //Moving conditions for player 1
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
                    if (selectedPiece.x < Board.GetLength(0) - 1 && Board[selectedPiece.x + 1, selectedPiece.y + 1].player == 2)
                    {
                        Board[selectedPiece.x + 1, selectedPiece.y + 1].legalMove = true;
                    }
                    if (selectedPiece.x > 0 && Board[selectedPiece.x - 1, selectedPiece.y + 1].player == 2)
                    {
                        Board[selectedPiece.x - 1, selectedPiece.y + 1].legalMove = true;
                    }

                    // Turn to queen
                    //  selectedPiece.pieceType = PieceType.Queen;
                }
                // Moving conditions for player 2
                else if (selectedPiece.player == 2 && currentPlayer == 2)
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
                    if (selectedPiece.x < Board.GetLength(0) - 1 && Board[selectedPiece.x + 1, selectedPiece.y - 1].player == 1)
                    {
                        Board[selectedPiece.x + 1, selectedPiece.y - 1].legalMove = true;
                    }
                    if (selectedPiece.x > 0 && Board[selectedPiece.x - 1, selectedPiece.y - 1].player == 1)
                    {
                        Board[selectedPiece.x - 1, selectedPiece.y - 1].legalMove = true;
                    }
                    // Turn to queen
                    // selectedPiece.pieceType = PieceType.Queen;
                }
                break;
            default:
                break;
        }
        return Board;
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
    static void DiagonalCheck(Piece selectedPiece, int currentPlayer, Piece[,] Board)
    {
        int opponentPlayer = 0;
        if (currentPlayer == 1) opponentPlayer = 2;
        else opponentPlayer = 1;

        if (currentPlayer == selectedPiece.player)
        {
            for (int i = 1; i < Board.GetLength(0); i++)
            {
                if (selectedPiece.x + i >= Board.GetLength(0) || selectedPiece.y + i >= Board.GetLength(1)) break;
                if (Board[selectedPiece.x + i, selectedPiece.y + i].player == currentPlayer) break;
                Board[selectedPiece.x + i, selectedPiece.y + i].legalMove = true;
                if (Board[selectedPiece.x + i, selectedPiece.y + i].player == opponentPlayer) break;
            }

            // X- Y+conditions
            for (int i = selectedPiece.x - selectedPiece.y; i < Board.GetLength(0); i++)
            {
                if (selectedPiece.x - i <= 0 || selectedPiece.y + i >= Board.GetLength(1)) break;
                if (Board[selectedPiece.x - i, selectedPiece.y + i].player == currentPlayer) break;
                Board[selectedPiece.x - i, selectedPiece.y + i].legalMove = true;
                if (Board[selectedPiece.x - i, selectedPiece.y + i].player == opponentPlayer) break;
            }

            // // Y conditions
            // for (int i = selectedPiece.y + 1; i < Board.GetLength(1); i++)
            // {
            //     if (Board[selectedPiece.x, i].player == currentPlayer) break;
            //     Board[selectedPiece.x, i].legalMove = true;
            //     if (Board[selectedPiece.x, i].player == opponentPlayer) break;
            // }
            // for (int i = selectedPiece.y - 1; i >= 0; i--)
            // {
            //     if (Board[selectedPiece.x, i].player == currentPlayer) break;
            //     Board[selectedPiece.x, i].legalMove = true;
            //     if (Board[selectedPiece.x, i].player == opponentPlayer) break;
            // }
        }
    }
}