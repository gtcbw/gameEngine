using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Audio;

namespace Sound
{
    public class Initializer
    {
        private static AudioContext _context = null;

        public static bool Init()
        {
            try
            {
                _context = new AudioContext();

            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static void CleanUp()
        {
            if (_context != null)
                _context.Dispose();
        }
    }
}
