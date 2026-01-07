using System.Threading.Tasks;

namespace DistantStars.Tools.Contracts;

public interface ILoadOnce
{
    Task LoadOnce();
}