using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WindowUIUtil
{
    public static List<Pair<int>> GetAvailableResolutions()
    {
        List<Pair<int>> availableResolutions = new List<Pair<int>>();

        Resolution[] resolutions = Screen.resolutions;

        foreach (var res in resolutions)
        {
            var newRes = new Pair<int>(res.width, res.height);

            if (!availableResolutions.Contains(newRes))
            {
                availableResolutions.Add(newRes);
            }
        }

        return availableResolutions;
    }

    public static List<int> GetAvailableRefreshRates()
    {
        List<int> availableRefreshRates = new List<int>();

        Resolution[] resolutions = Screen.resolutions;

        foreach (var res in resolutions)
        {
            if (!availableRefreshRates.Contains(res.refreshRate))
            {
                availableRefreshRates.Add(res.refreshRate);
            }
        }

        availableRefreshRates.Sort();

        return availableRefreshRates;
    }

    public static List<string> GetAvailableScreenModes()
    {
        List<string> availableScreenModes = new List<string>();
        availableScreenModes.Add("windowed");
        availableScreenModes.Add("borderless");
        availableScreenModes.Add("fullscreen");

        return availableScreenModes;
    }

    public static FullScreenMode StringToScreenMode(string screenMode)
    {
        if (screenMode == "fullscreen")
        {
            return FullScreenMode.ExclusiveFullScreen;
        }

        if (screenMode == "borderless")
        {
            return FullScreenMode.FullScreenWindow;
        }

        return FullScreenMode.Windowed;
    }

    public static string ScreenModeToString(FullScreenMode screenMode)
    {
        if (screenMode == FullScreenMode.ExclusiveFullScreen)
        {
            return "fullscreen";
        }

        if (screenMode == FullScreenMode.FullScreenWindow)
        {
            return "borderless";
        }

        return "windowed";
    }

    public static T GetNextInList<T>(List<T> list, T currentItem, bool forward = true)
    {
        // Find the index of the current resolution
        int index = list.IndexOf(currentItem);

        int newIndex;

        // If clicking next
        if (forward)
        {
            // If not index out of range, use the next one, else the first one
            newIndex = index + 1 < list.Count ? index + 1 : 0;
        }
        // If clicking previous
        else
        {
            // If not index out of range, use the previous one, else the last one
            newIndex = index - 1 >= 0 ? index - 1 : list.Count - 1;
        }

        // Return the new resolution
        return list[newIndex];
    }

    public static void SetScreen()
    {
        var resolution = PlayerPrefsController.GetResolution();

        Screen.SetResolution(
            resolution.first,
            resolution.second,
            StringToScreenMode(PlayerPrefsController.GetSCreenMode())
        );

        Application.targetFrameRate = PlayerPrefsController.GetRefreshRate();
    }
}
