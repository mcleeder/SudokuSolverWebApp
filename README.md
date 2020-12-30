# Sudoku Solver

## Introduction

This was a side project of mine while I was learning C# at boot camp. At boot camp I always tried to have one project going on the side where I could try to apply what I was learning, or often just get ahead of the game. Boot camp moves so fast that I rarely had the chance to complete any of them, this is one of the rare ones that ended up as a finished product. It does need a 2.0 version though, it only takes a few weeks of learning to leave your old code in the dust.

I'm only going to include the two main logic methods. The rest are generic MVC controllers or just things to translate back and forth from an array (front end) to a 2d array (back end logic).

Main logic:

-[The solver](https://github.com/mcleeder/CodeSamples/blob/main/Sudoku_Solver.md#the-solver)

-[The checker](https://github.com/mcleeder/CodeSamples/blob/main/Sudoku_Solver.md#the-checker)



### The solver

This method is the main event. I learned most of this by watching a youtube video (in Python though). It came with a couple of challenges. The first was wrapping my head around recursion and what the heck is happening. Once I started to understand recursion, I realized that the method doesn't want to stop and wants to find every last solution. Not only that, but it cycles extra times as is spins down so if you just wait until the end, the board doesn't have a solution on it. So I had to figure out where to to pull out my solved board.

My solution was to use a couple of fields. One to to indicate if we had a solved board, and one to hold the solution itself. The latter was necessry because the recurision will keep messing with the board as it spins down. The bool field tracking if I have a solution gets used at the top of the method to try to end the chain. It works, eventually. I think what is happening is that as the methods spin down, they exit and come back to the part where it tries numbers 1-9 and then still has to try the rest of the count before that instance can totally go away.


```c#
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
            if (SolutionCheck(board) && !solved) //only one output
            {
                solved = true;
                solutionboard = (int[,]) board.Clone();
            }
        }
```


### The checker

The second most important method. I am proud of how this one turned out. For a given number and sudoku board, it will tell you if that number is allowed in a given square. This final version is especially fancy because it can work with the back end Solver() checks, but also be used by the front end for validation. The main difference between those two checks being that in the back end, the square we're checking has a value of 0 and in the front that square has 1-9 in it. The solution to that pesky little problem was the realization that in both cases, we don't actually care what is in the square we're referencing. So I told it not to check that square.

Pardon the method name, it was early and I know better now, leaving it as-is for posterity. Mike from the future also realizes that there is an opportunity to get rid of a loop here, the X, Y axis checks can happen at the same time. Learning is fun.

```c#
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

            //3x3 square check using super coords as starting point
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
```



The Good:
- Fully functional ASP.NET MVC app that works as advertised, will solve a given sudoku puzzle.
- C# for logic, Bootstrap for pretty, jQuery for validation.
- My first use of recursion and 2D Arrays. Gained a lot of understanding of both.
- The method that checks if a number can be placed in a square is (I think) particularly well done.

The Bad:
- It's not able to check for a board with no solution.
- If you submit an invalid board, it won't tell you what about it is invalid, just that it is invalid.
- Loops inside loops inside loops, which then call other methods which are more loops. Luckily a sudoku board is only 81 numbers.

The Meh:
- The app's scale is small, so I just left all the logic in the HomeController.
