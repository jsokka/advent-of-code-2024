using System.Diagnostics;

namespace AdventOfCode2024.Puzzles;

internal class Day06 : IPuzzle
{
    private enum Direction
    {
        Up, Right, Down, Left
    }

    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day06.txt")).Select(x => x.ToArray()).ToArray();

        return (Part1(inputData), Part2(inputData));
    }

    private static string Part1(char[][] inputData)
    {
        ((int x, int y) startingPosition, Direction startingDirection) = GetStartingPositionAndDirection(inputData);

        return GetRoutePoints(inputData, startingPosition, startingDirection).Select(s => s.position).ToHashSet().Count.ToString();
    }

    private static string Part2(char[][] inputData)
    {
        ((int x, int y) startingPosition, Direction startingDirection) = GetStartingPositionAndDirection(inputData);

        var routePoints = GetRoutePoints(inputData, startingPosition, startingDirection).DistinctBy(r => r.position).ToArray();

        var obstacles = new HashSet<(int, int)>();

        for (int i = 0; i < routePoints.Length; i++)
        {
            var (obstaclePosition, _) = routePoints[i];

            if (obstaclePosition != startingPosition &&
                GetRoutePoints(inputData, routePoints[i - 1].position, routePoints[i - 1].direction, obstaclePosition).Exists(x => x.position == (-1, -1)))
            {
                obstacles.Add(obstaclePosition);
            }
        }

        return obstacles.Count.ToString();
    }

    private static ((int x, int y) position, Direction direction) GetStartingPositionAndDirection(char[][] inputData)
    {
        for (int y = 0; y < inputData.Length; y++)
        {
            for (int x = 0; x < inputData[y].Length; x++)
            {
                var marker = inputData[y][x];
                var direction = marker switch
                {
                    '^' => Direction.Up,
                    '>' => Direction.Right,
                    'v' => Direction.Down,
                    '<' => Direction.Left,
                    _ => (Direction?)null
                };

                if (direction is not null)
                {
                    return ((x, y), direction.Value);
                }
            }
        }

        throw new UnreachableException();
    }

    private static List<((int x, int y) position, Direction direction)> GetRoutePoints(
        char[][] inputData, (int x, int y) startingPosition, Direction direction, (int x, int y)? additionalObstacleCoordinates = null)
    {
        var visited = new List<((int x, int y) position, Direction direction)> { (startingPosition, direction) };

        var position = startingPosition;

        while (true)
        {
            var movement = GetMovement(direction);
            (int x, int y) nextPosition = (position.x + movement.x, position.y + movement.y);

            if (nextPosition.y < 0 || nextPosition.x < 0 || nextPosition.y > inputData.Length - 1 || nextPosition.x > inputData[0].Length - 1)
            {
                break;
            }

            if (visited.Exists(x => x.position == nextPosition && x.direction == direction))
            {
                return [((-1, -1), direction)];
            }

            var marker = inputData[nextPosition.y][nextPosition.x];
            if (marker == '#' || nextPosition == additionalObstacleCoordinates)
            {
                direction = TurnRight(direction);
                continue;
            }

            position = nextPosition;
            visited.Add((position, direction));
        }

        return visited;
    }

    private static Direction TurnRight(Direction direction)
    {
        direction++;
        if (direction > Direction.Left)
        {
            direction = Direction.Up;
        }

        return direction;
    }

    private static (int x, int y) GetMovement(Direction direction)
    {
        return direction switch
        {
            Direction.Up => (0, -1),
            Direction.Right => (1, 0),
            Direction.Down => (0, 1),
            Direction.Left => (-1, 0),
            _ => throw new UnreachableException(),
        };
    }
}
