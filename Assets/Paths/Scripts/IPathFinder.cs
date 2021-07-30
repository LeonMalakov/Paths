namespace Paths
{
    public interface IPathFinder
    {
        bool FindPath(GameTile from, GameTile to, out Path path);
    }
}