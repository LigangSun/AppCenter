using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.BlockPuzzle.Data;
using System.Reflection;

namespace SoonLearning.BlockPuzzle.Puzzle
{
    public class PuzzleSetting
    {
        private static PuzzleSetting setting;

        public static PuzzleSetting Instance
        {
            get
            {
                if (setting == null)
                {
                    setting = new PuzzleSetting();
                }

                return setting;
            }
        }

        private PuzzleSetting()
        {
        }

        public int Rows
        {
            get;
            set;
        }

        public int Cols
        {
            get;
            set;
        }

        public PuzzleType Type
        {
            get;
            set;
        }

        internal string DataFolder
        {
            get
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                string dataFolder = System.IO.Path.GetDirectoryName(assembly.Location);
                dataFolder = System.IO.Path.Combine(dataFolder, @"Data\Puzzle\" + PuzzleSetting.Instance.Type.ToString());
                return dataFolder;
            }
        }

        public void Uninstall()
        {
            try
            {
                System.IO.Directory.Delete(DataFolder, true);
            }
            catch
            {
            }
        }
    }
}
