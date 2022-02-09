using GameEstate.Formats;
using System;
using System.IO;
using static GameEstate.EstateDebug;

namespace GameEstate.AC
{
    public static class ACPakManager
    {
        private static int count;

        // End of retail Iteration versions.
        private static int ITERATION_CELL = 982;
        private static int ITERATION_PORTAL = 2072;
        private static int ITERATION_HIRES = 497;
        private static int ITERATION_LANGUAGE = 994;

        public static CellPakFile CellDat { get; private set; }
        public static PortalPakFile PortalDat { get; private set; }
        public static DatPakFile HighResDat { get; private set; }
        public static LanguagePakFile LanguageDat { get; private set; }

        public static void Initialize(BinaryPakFile source, bool loadCell = true)
        {
            if (CellDat != null) return;

            if (loadCell)
                try
                {
                    CellDat = new CellPakFile(source.Estate.OpenPakFile(new Uri("game:/client_cell_1.dat#AC")));
                    //count = CellDat.Files.Count;
                    //Log($"Successfully opened {datFile} file, containing {count} records, iteration {CellDat.Iteration}");
                    //if (CellDat.Iteration != ITERATION_CELL) Log($"{datFile} iteration does not match expected end-of-retail version of {ITERATION_CELL}.");
                }
                catch (FileNotFoundException ex)
                {
                    //Log($"An exception occured while attempting to open {datFile} file!  This needs to be corrected in order for Landblocks to load!");
                    Log($"Exception: {ex.Message}");
                }

            try
            {
                PortalDat = new PortalPakFile(source.Estate.OpenPakFile(new Uri("game:/client_portal.dat#AC")));
                //PortalDat.SkillTable.AddRetiredSkills();
                //count = PortalDat.AllFiles.Count;
                //Log($"Successfully opened {datFile} file, containing {count} records, iteration {PortalDat.Iteration}");
                //if (PortalDat.Iteration != ITERATION_PORTAL) Log($"{datFile} iteration does not match expected end-of-retail version of {ITERATION_PORTAL}.");
            }
            catch (FileNotFoundException ex)
            {
                //Log($"An exception occured while attempting to open {datFile} file!\n\n *** Please check your 'DatFilesDirectory' setting in the config.js file. ***\n *** ACE will not run properly without this properly configured! ***\n");
                Log($"Exception: {ex.Message}");
            }

            // Load the client_highres.dat file. This is not required for ACE operation, so no exception needs to be generated.
            //HighResDat = new PortalPakFile(source.Estate.OpenPakFile(new Uri("game:/client_highres.dat#AC")));
            //datFile = Path.Combine(datDir, "client_highres.dat");
            //if (File.Exists(datFile))
            //{
            //    HighResDat = new DatDatabase(datFile, keepOpen);
            //    count = HighResDat.AllFiles.Count;
            //    Log($"Successfully opened {datFile} file, containing {count} records, iteration {HighResDat.Iteration}");
            //    if (HighResDat.Iteration != ITERATION_HIRES) Log($"{datFile} iteration does not match expected end-of-retail version of {ITERATION_HIRES}.");
            //}

            try
            {
                LanguageDat = new LanguagePakFile(source.Estate.OpenPakFile(new Uri("game:/client_local_English.dat#AC")));
                //count = LanguageDat.AllFiles.Count;
                //Log($"Successfully opened {datFile} file, containing {count} records, iteration {LanguageDat.Iteration}");
                //if (LanguageDat.Iteration != ITERATION_LANGUAGE) Log($"{datFile} iteration does not match expected end-of-retail version of {ITERATION_LANGUAGE}.");
            }
            catch (FileNotFoundException ex)
            {
                //Log($"An exception occured while attempting to open {datFile} file!\n\n *** Please check your 'DatFilesDirectory' setting in the config.json file. ***\n *** ACE will not run properly without this properly configured! ***\n");
                Log($"Exception: {ex.Message}");
            }
        }
    }
}
