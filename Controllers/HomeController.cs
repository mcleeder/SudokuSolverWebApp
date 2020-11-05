using Microsoft.Ajax.Utilities;
using SudokuSolverWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SudokuSolverWebApp.Controllers
{
    public class HomeController : Controller
    {
        public static int[,] solutionboard = new int[9,9];
        public static bool solved = false;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Result(Sudoku board)
        {
            solved = false;

            var calcboard = ConvertTo9x9Array(board.grid);

            if (ValidSubmissionCheck(calcboard))
            {
                Solver(calcboard);

                board.grid = ConvertToArrayFrom9x9(solutionboard);

                return View(board);
            }
            else
            {
                return View("Index");
            }

            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Solve(Sudoku board)
        {

            return View("Result","Home",board);
        }

        private bool ValidSubmissionCheck(int[,] board)
        {
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (board[y,x] != 0)
                    {
                        if (!CanItBe(y,x,board[y,x],board))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private int[,] ConvertTo9x9Array(int[] arr)
        {
            var result = new int[9,9];
            int i = 0;

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    result[y, x] = arr[i];
                    i++;
                }
            }
            return result;
        }

        private int[] ConvertToArrayFrom9x9(int[,] board)
        {
            int[] result = new int[81];
            int pos = 0;

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    result[pos] = board[y,x];
                    pos++;
                }
            }
            return result;
        }

        private bool SolutionCheck(int[,] board)
        {
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (board[y, x] == 0)
                        return false;
                }
            }
            return true;
        }

        private void Solver(int[,] board)
        {
            if (solved)
            {
                return; //stop early pls
            }
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (board[y,x] == 0)
                    {
                        for (int n = 1; n < 10; n++)
                        {
                            if (CanItBe(y, x, n, board))
                            {
                                board[y,x] = n;
                                Solver(board);
                                board[y,x] = 0;
                            }
                        }
                        return;
                    }
                }
            }
            if (SolutionCheck(board) && !solved)
            {
                solved = true;
                solutionboard = (int[,]) board.Clone();
            }
        }

        private bool CanItBe(int y, int x, int n, int[,] board)
        {
            //check Y axis
            for (int i = 0; i < 9; i++)
            {
                if (board[i,x] == n && i != y)
                    return false;
            }

            //check X axis
            for (int i = 0; i < 9; i++)
            {
                if (board[y,i] == n && i != x)
                    return false;
            }

            //get x & y super columns
            int gridY = 0;
            int gridX = 0;

            if (x > 2) gridX = 3;
            if (x > 5) gridX = 6;

            if (y > 2) gridY = 3;
            if (y > 5) gridY = 6;

            //9x9 square check using above coords as starting point
            for (int i = gridY; i < gridY + 3; i++)
            {
                for (int j = gridX; j < gridX + 3; j++)
                {
                    if (board[i,j] == n && i != y && j != x)
                        return false;
                }
            }
            return true;
        }

    }






    
}