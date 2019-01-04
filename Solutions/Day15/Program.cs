using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day15
{
    /// <summary>
    /// Beverage Bandits
    /// </summary>
    /// <remarks>
    /// <para>
    /// Combat proceeds in Rounds. For each Round, we process one Turn for each Unit, proceeding in
    /// reading order. If a Unit is adjacent to a Unit of the other type (e.g., a Goblin is adjacent
    /// to an Elf) it does not move, and will instead attack. But if it is not, it needs to perform
    /// the most expensive part of this problem: working out which is the nearest 'in range' square
    /// and moving towards it.
    /// </para>
    /// <para>
    /// The key will be avoiding unnecessary repetition of work when finding the nearest In Range
    /// square. My plan is to keep a map tracking distanced to in-range cells, and to update that
    /// map each time anything moves or vanishes. At a minimum this will enable us to avoid redoing
    /// the 'nearest' calcluations when no units have moved (and it seems that battles often spend
    /// a fair amount of time in states where all units are static for multiple turns until the
    /// hit points run down far enough that a unit dies and something changes). If necessary, we
    /// might be able to improve performance by performing only partial recalculations when
    /// something changes, although that might not in fact be necessary. In any case, consider this
    /// map:
    /// </para>
    /// <code>
    /// #   #   #   #   #   #   #
    /// 
    /// #   .   .   .   .   .   #
    /// 
    /// 
    /// #   .   E   .   .   .   #
    ///        (1)
    ///    
    /// #   .   .   .   .   G   #
    ///                    (3)
    ///    
    /// #   .   .   .   .   .   #
    /// 
    ///
    /// #   .   .   .   .   .   #
    ///
    /// 
    /// #   .   .   E   .   .   #
    ///            (2)
    /// 
    /// #   .   .   .   .   .   #
    /// 
    /// #   #   #   #   #   #   #
    /// </code>
    /// <para>
    /// We will perform a series of iterations through every cell in the map, and if a cell is a)
    /// not yet populated and b) adjacent to some other cell, we update it. Since Elves and Goblins
    /// have different targets (they are only interested in targetting each other) we need to track
    /// the nearest of each. After the first pass it looks like this:
    /// </para>
    /// <code>
    /// #    #     #     #     #     #     #
    /// 
    /// #    .    0/.    .     .     .     #
    ///          (1/_) 
    /// 
    /// #   0/.   E    0/.     .    ./0    #
    ///    (1/_) (1/_) (1/_)       (_/3)
    ///    
    /// #    .    0/.    .    ./0    G     #
    ///          (1/_)       (_/3)  (3)
    ///
    /// #    .     .     0     .    ./0    #
    ///                (2/_)       (_/3)
    /// 
    /// #    .     0     E     0     .     #
    ///          (2/_)  (2)  (2/_)
    /// 
    /// #    .     .     0     .     .     #
    ///                (2/_)
    /// 
    /// #    #     #     #     #     #     #
    /// </code>
    /// <para>
    /// In this pass, we have identified in-range cells - ones adjacent to Elves or Goblins.
    /// After the next pass it looks like this:
    /// </para>
    /// <code>
    /// #    #     #     #     #     #     #
    /// 
    /// #   1/.   0/.   1/.    .    ./1    #
    ///    (1/_) (1/_) (1/_)       (_/3)
    /// 
    /// #   0/.   E    0/.    1/1   ./0    #
    ///    (1/_) (1/_) (1/_) (1/3) (_/3)
    ///    
    /// #   1/.   0/.   1/1   ./0    G     #
    ///    (1/_) (1/_) (1/3) (_/3)  (3)
    ///
    /// #    .    1/.    0    1/1   ./0    #
    ///          (1/_) (2/_) (2/3) (_/3)
    /// 
    /// #   1/.    0     E     0    1/1    #
    ///    (2/_) (2/_)  (2)  (2/_) (2/3)
    /// 
    /// #    .    1/.    0    1/.    .     #
    ///          (2/_) (2/_) (2/_)
    /// 
    /// #    #     #     #     #     #     #
    /// </code>
    /// <para>
    /// Eventually we will have filled in all reachable cells, so for every cell on the map, we
    /// will know the distance to the closest elf and goblin. We also track (not shown on that map)
    /// which 'in range' cell for that target elf and goblin is the best for each cell on the map.
    /// </para>
    /// <para>
    /// This is a slightly oblique approach to the problem, because the problem statement is
    /// specified entirely in terms of the start point, whereas this grows everything outward from
    /// the destination. It probably does unnecessary work: it certainly produces more information
    /// than is often required. (It produces answers for all positions on the board, and in the
    /// case where a unit then moves, we the needs to recalculate.) On the other hand, it's a style
    /// of processing that current CPUs get on with quite well: it passes through the data in order.
    /// It's fairly well bounded. And it copes very well with the 'temporary steady state'
    /// scenarios in which nothing is moving because all units are either blocked or engaged.
    /// </para>
    /// </remarks>
    public static class Program
    {
        private static void Main()
        {
            string map = InputReader.ReadAll(typeof(Program));
            //(int part1, _) = SolvePart1(map, show: true);
            //Console.WriteLine("Part 1: " + part1);

            Console.WriteLine(SolvePart2(map));
        }

        public static (int score, bool totalElfVictory) SolvePart1(string map, bool show = false, int elfAttackPower = 3)
        {
            int rounds = 0;
            GameState g;
            int startLine = Console.CursorTop;
            bool combatEnds = false;
            for (g = GameState.Start(GridOperations.ParseGrid(map), elfAttackPower); !combatEnds; (g, combatEnds) = g.PlayRound())
            {
                rounds += 1;
                if (show)
                {
                    Console.SetCursorPosition(0, startLine);
                    ShowGrid(g.Grid);
                }
            }

            bool goblinsWon = g.GoblinHitPoints.Any(kv => kv.Value > 0);
            int winningTeamHitPointsRemaining = (goblinsWon ? g.GoblinHitPoints : g.ElfHitPoints)
                .Select(kv => kv.Value)
                .Where(p => p > 0)
                .Sum();
            return (winningTeamHitPointsRemaining * (rounds - 1), g.ElfHitPoints.All(kv => kv.Value > 0));
        }

        public static (int power, int outcome) SolvePart2(string map)
        {
            // We're searching for the lowest power that enables total elf victory, but since
            // there's no guarantee that all powers above that lowest power will also achieve
            // victory, we cant just do a binary search. (Actually, it happens to work for my
            // input, but it's not guaranteed.)
            // So we just have to try each number. To make it run a bit faster, we're going to
            // execute in parallel.
            var sync = new object();
            bool run = true;
            var x = EnumerableEx.Generate(4, _ => { lock (sync) { return run; } }, i => i + 1, i => i)
                .AsParallel()
                .Select(i =>
                {
                    Console.WriteLine("Trying " + i);
                    var r = (power: i, r: SolvePart1(map, elfAttackPower: i));
                    if (r.r.totalElfVictory)
                    {
                        lock (sync)
                        {
                            run = false;
                        }
                    }
                    Console.WriteLine(r);
                    return r;
                })
                .First(xx => xx.r.totalElfVictory);
            return (x.power, x.r.score);
        }

        public static void ShowGrid(GridCell[,] grid)
        {
            int height = grid.GetLength(0);
            int width = grid.GetLength(1);
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    GridCell cell = grid[y, x];
                    if (cell.IsWall)
                    {
                        Console.Write('#');
                    }
                    else if (cell.IsGoblin)
                    {
                        Console.Write('G');
                    }
                    else if (cell.IsElf)
                    {
                        Console.Write('E');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
