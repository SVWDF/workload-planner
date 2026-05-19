namespace WorkloadPlanner.Constants
{
    public static class BoardColors
    {
        public static readonly List<string> Colors = [
            "#6366F1",
            "#8B5CF6",
            "#EC4899",
            "#F43F5E",
            "#F97316",
            "#10B981",
            "#06B6D4",
            "#3B82F6"
        ];

        public static string GetRandomColor()
        {
            return Colors[Random.Shared.Next(Colors.Count)];
        }
    }
}