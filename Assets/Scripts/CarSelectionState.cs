public static class CarSelectionState
{
    public static bool HasSelection;
    public static int SelectedIndex;
    public static string SelectedId = string.Empty;
    public static string SelectedName = string.Empty;

    public static void Set(int index, string id, string name)
    {
        HasSelection = true;
        SelectedIndex = index;
        SelectedId = id ?? string.Empty;
        SelectedName = name ?? string.Empty;
    }
}