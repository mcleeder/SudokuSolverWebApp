using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SudokuSolverWebApp.Models
{

    public class Sudoku
    {
        public int[] grid { get; set; }
        public int[] feedbackgrid { get; set; }
    }
}