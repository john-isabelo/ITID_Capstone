using System.Collections.Generic;
using WeeklyTask.Models;

public class FoodEqualityComparer : IEqualityComparer<Food>
{
    public bool Equals(Food x, Food y)
    {
        return x.Id == y.Id && x.Name == y.Name;
    }

    public int GetHashCode(Food obj)
    {
        return obj.Id.GetHashCode() ^ obj.Name.GetHashCode();
    }
}
