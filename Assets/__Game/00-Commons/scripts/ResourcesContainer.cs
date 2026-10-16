using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourcesContainer 
{

    private static int songIndex = 0;
    private static IList<string> songKeys;
    private static string selectedSong;
    private static IDictionary<string, MainMenuSong> menuSongs;

    public static void SetMenuSongs(IDictionary<string, MainMenuSong> menuSongs) 
    {
        ResourcesContainer.menuSongs = menuSongs;
        if( menuSongs == null )
        {
            songKeys = null;
        }
        else
        {
            songKeys = new List<string>(menuSongs.Keys);
        }
    }
    public static string GetSelectedSongPath()
    {
        GetSelectedSong();
        return selectedSong;
    }

    public static MainMenuSong GetSelectedSong()
    {
        MainMenuSong song = null;
        if (selectedSong == null) 
        {
            if(menuSongs != null) 
            {
                songIndex = 0;
                selectedSong = songKeys[0];
            }
        }

        if (selectedSong != null)
        {
            song = menuSongs[selectedSong];
        }
        return song;
    }

    public static MainMenuSong GetPrevius()
    {
        MainMenuSong song = null;
        songIndex--;
        if(menuSongs != null && menuSongs.Count > 0) 
        {
            if (songIndex < 0)
            {
                songIndex = menuSongs.Count - 1;
            }
            selectedSong = songKeys[songIndex];
            song = menuSongs[selectedSong];
        }

        return song;
    }

    public static MainMenuSong GetNext()
    {
        MainMenuSong song = null;
        songIndex++;
        if (menuSongs != null && menuSongs.Count > 0)
        {
            if (songIndex < 0 || songIndex >= menuSongs.Count)
            {
                songIndex = 0;
            }
            selectedSong = songKeys[songIndex];
            song = menuSongs[selectedSong];
        }

        return song;
    }

}
