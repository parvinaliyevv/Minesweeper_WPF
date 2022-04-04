using System.IO;
using System.Media;

namespace Minesweeper.Services
{
    public static class SoundService
    {
        private static SoundPlayer Sound { get; } = new();


        public static void StopAllSounds() => Sound.Stop();

        public static void PlaySoundFromFullPath(string path)
        {
            Stream stream = null;

            try
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                return;
            }

            Sound.Stream = stream;
            Sound.Play();

            stream.Close();
        }

        public static void PlaySoundFromThisPath(string path)
        {
            Stream stream = null;

            try
            {
                stream = new FileStream(string.Format("{0}{1}", DirectoryService.GetProjectParentFolder(), path), FileMode.Open, FileAccess.Read);
            }
            catch
            {
                return;
            }

            Sound.Stream = stream;
            Sound.Play();

            stream.Close();
        }
    }
}
