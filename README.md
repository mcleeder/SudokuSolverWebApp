# Sudoku Solver

The Good:
- Fully functional ASP.NET MVC app that works as advertised, will solve a given sudoku puzzle.
- C# for logic, Bootstrap for pretty, jQuery for validation.
- My first use of recursion and 2D Arrays. Gained a lot of understanding of both.
- The method that checks if a number can be placed in a square is (I think) particularly good.

The Bad:
- It's not able to check for a board with no solution.
- If you submit an invalid board, it won't tell you what about it is invalid, just that it is invalid.
- Loops inside loops inside loops, which then call other methods which are more loops. Luckily a sudoku board is only 81 numbers.

The Meh:
- The app's scale is small, so I just left all the logic in the HomeController.
