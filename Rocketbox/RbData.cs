using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using LiteDB;

namespace Rocketbox
{
    internal static class RbData
    {
        private static LiteDatabase _db;
        private static string _locale;

        // Regional search engine support
        private static string[] _allowedLocales = { "ca", "us", "uk" };

        // Master list of search engines
        internal static List<RbSearchEngine> SearchEngines { get; private set; }

        // Master list of converter units
        internal static List<RbConversionUnit> ConversionUnits { get; private set; }

        // Master list of Google Translate languages
        internal static List<RbTranslateLanguage> TranslateLanguages { get; private set; }

        // Master list of installed packages
        internal static List<string> Packages { get; private set; }

        // The load state (for the UI thread)
        public static RbLoadState LoadState { get; private set; }

        static RbData()
        {
            LoadState = RbLoadState.NotLoaded;
        }

        /// <summary>
        /// Loads the Rocketbox database
        /// </summary>
        private static void LoadDatabase()
        {
            if(!File.Exists(RbGlobals.DB_FILE_NAME))
            {
                LoadState = RbLoadState.Failed;
                _db = new LiteDatabase(RbGlobals.DB_FAIL_FILE_NAME);
            }
            else
            {
                _db = new LiteDatabase(RbGlobals.DB_FILE_NAME);
            }

            // Tries to read the locale from a local file
            // If first run/file is broken, assume Canada
            if(File.Exists(RbGlobals.LOCALE_FILE))
            {
                string localeFileText = File.ReadAllText(RbGlobals.LOCALE_FILE).Trim().ToLower();
                if(_allowedLocales.Contains(localeFileText))
                {
                    _locale = localeFileText;
                }
                else
                {
                    _locale = "ca";
                }
            }
            else
            {
                File.WriteAllText(RbGlobals.LOCALE_FILE, "ca");
                _locale = "ca";
            }

            SearchEngines = _db.GetCollection<RbSearchEngine>(RbGlobals.TABLE_SEARCH_ENGINES + "_" + _locale).FindAll().ToList<RbSearchEngine>();
            SearchEngines.AddRange(_db.GetCollection<RbSearchEngine>(RbGlobals.TABLE_SEARCH_ENGINES).FindAll().ToList<RbSearchEngine>());

            ConversionUnits = _db.GetCollection<RbConversionUnit>(RbGlobals.TABLE_CONVERSION_UNITS).FindAll().ToList<RbConversionUnit>();
            TranslateLanguages = _db.GetCollection<RbTranslateLanguage>(RbGlobals.TABLE_LANGUAGES).FindAll().ToList<RbTranslateLanguage>();

            Packages = new List<string>();

            foreach(RbSearchEngine engines in SearchEngines)
            {
                if(!Packages.Contains(engines.Collection.ToLower()))
                {
                    Packages.Add(engines.Collection.ToLower());
                }
            }
        }

        /// <summary>
        /// Must be called by the app before doing anything
        /// </summary>
        internal static void LoadData()
        {
            if(LoadState == RbLoadState.NotLoaded)
            {
                LoadDatabase();

                if(GetDbVersion() != RbGlobals.RB_VERSION)
                {
                    UpdateDatabase(GetDbVersion());
                }

                if(LoadState != RbLoadState.Failed)
                {
                    LoadState = RbLoadState.Loaded;
                    UpdateLoadedDate();
                }
            }
        }

        /// <summary>
        /// Searches the Rocketbox database for conversion units
        /// </summary>
        /// <param name="keyword">The keyword of the unit</param>
        /// <returns>The unit, or a null unit if invalid</returns>
        internal static RbConversionUnit GetConversionUnit(string keyword)
        {
            keyword = keyword.ToUpper();

            var results = ConversionUnits.Where(u => u.Keywords.Contains(keyword));

            if(results.Count() > 0)
            {
                return results.First();
            }
            else
            {
                return new RbConversionUnit { Name = "null", Multiplier = 0, Type = "null" };
            }
        }

        // ------------------------------------
        // Database modification
        // ------------------------------------

        /// <summary>
        /// Generates the default database for Rocketbox
        /// </summary>
        internal static void GenerateDefaultDatabase()
        {
            LoadState = RbLoadState.NotLoaded;

            if(File.Exists(RbGlobals.DB_FILE_NAME))
            {
                File.Delete(RbGlobals.DB_FILE_NAME);
            }
            
            _db = new LiteDatabase(RbGlobals.DB_FILE_NAME);
            
            // create system keys
            
            RbSystemKeyVal versionKeyVal = new RbSystemKeyVal() { ConfigKey = RbGlobals.SYSTEM_KEY_VERSION, ConfigVal = RbGlobals.RB_VERSION };
            _db.GetCollection<RbSystemKeyVal>(RbGlobals.TABLE_SYSTEM).Insert(versionKeyVal);

            RbSystemKeyVal createdKeyVal = new RbSystemKeyVal() { ConfigKey = RbGlobals.SYSTEM_KEY_CREATED, ConfigVal = DateTime.Now.ToString(RbGlobals.SHORT_DATE_FORMAT) };
            _db.GetCollection<RbSystemKeyVal>(RbGlobals.TABLE_SYSTEM).Insert(createdKeyVal);

            UpdateLoadedDate();

            _db.GetCollection(RbGlobals.TABLE_SEARCH_ENGINES).InsertBulk(JsonSerializer.DeserializeArray(RbUtility.GetEmbeddedStringResource("DefaultData.DefaultSearchEngines.json")).Select(b => b.AsDocument));

            foreach (string locale in _allowedLocales)
            {
                _db.GetCollection(RbGlobals.TABLE_SEARCH_ENGINES + "_" + locale).InsertBulk(JsonSerializer.DeserializeArray(RbUtility.GetEmbeddedStringResource("DefaultData.DefaultSearchEngines_" + locale + ".json")).Select(b => b.AsDocument));
            }

            _db.GetCollection(RbGlobals.TABLE_CONVERSION_UNITS).InsertBulk(JsonSerializer.DeserializeArray(RbUtility.GetEmbeddedStringResource("DefaultData.DefaultConversionUnits.json")).Select(b => b.AsDocument));
            _db.GetCollection(RbGlobals.TABLE_LANGUAGES).InsertBulk(JsonSerializer.DeserializeArray(RbUtility.GetEmbeddedStringResource("DefaultData.DefaultLanguages.json")).Select(b => b.AsDocument));
        }

        /// <summary>
        /// Gets the version number from an existing Rocketbox database.
        /// </summary>
        /// <returns>The version of Rocketbox last used with this database</returns>
        internal static string GetDbVersion()
        {
            if (_db.CollectionExists(RbGlobals.TABLE_SYSTEM) && _db.GetCollection<RbSystemKeyVal>(RbGlobals.TABLE_SYSTEM).Exists(k => k.ConfigKey == RbGlobals.SYSTEM_KEY_VERSION))
            {
                return _db.GetCollection<RbSystemKeyVal>(RbGlobals.TABLE_SYSTEM).FindOne(k => k.ConfigKey == "version").ConfigVal;
            }
            else
            {
                // version 1.0.x does not have this table
                return "1.0.x";
            }
        }

        /// <summary>
        /// Updates the Rocketbox database to the latest version
        /// </summary>
        /// <param name="previousVersion">The version string of the version being updated from</param>
        /// <returns>Whether the update was successful</returns>
        internal static bool UpdateDatabase(string previousVersion)
        {
            // placeholder for future use - no significant updates have been made to DB schema

            bool success = true;

            switch(previousVersion)
            {
                case "1.0.x":
                    // changes from 1.0.x: added system table with version key
                    _db.GetCollection<RbSystemKeyVal>(RbGlobals.TABLE_SYSTEM).Insert(new RbSystemKeyVal() { ConfigKey = RbGlobals.SYSTEM_KEY_VERSION, ConfigVal = RbGlobals.RB_VERSION });
                    break;
            }

            if(previousVersion != "1.0.x")
            {
                RbSystemKeyVal verKey = _db.GetCollection<RbSystemKeyVal>(RbGlobals.TABLE_SYSTEM).FindOne(k => k.ConfigKey == RbGlobals.SYSTEM_KEY_VERSION);
                verKey.ConfigVal = RbGlobals.RB_VERSION;
                _db.GetCollection<RbSystemKeyVal>(RbGlobals.TABLE_SYSTEM).Update(verKey);
            }

            return success;
        }

        /// <summary>
        /// Updates the "loaded" key in the system table to the present day
        /// </summary>
        internal static void UpdateLoadedDate()
        {
            RbSystemKeyVal loadedKey;
            if(_db.GetCollection<RbSystemKeyVal>(RbGlobals.TABLE_SYSTEM).Exists(k => k.ConfigKey == RbGlobals.SYSTEM_KEY_LOADED))
            {
                loadedKey = _db.GetCollection<RbSystemKeyVal>(RbGlobals.TABLE_SYSTEM).FindOne(k => k.ConfigKey == RbGlobals.SYSTEM_KEY_LOADED);
                loadedKey.ConfigVal = DateTime.Now.ToString(RbGlobals.SHORT_DATE_FORMAT);
                _db.GetCollection<RbSystemKeyVal>(RbGlobals.TABLE_SYSTEM).Update(loadedKey);
            }
            else
            {
                loadedKey = new RbSystemKeyVal() { ConfigKey = RbGlobals.SYSTEM_KEY_LOADED, ConfigVal = DateTime.Now.ToString(RbGlobals.SHORT_DATE_FORMAT) };
                _db.GetCollection<RbSystemKeyVal>(RbGlobals.TABLE_SYSTEM).Insert(loadedKey);
            }
        }

        /// <summary>
        /// Attempts to install a search engine pack
        /// </summary>
        /// <param name="packName">The name of the .rbx file in the Rocketbox directory</param>
        /// <returns></returns>
        internal static bool InstallSearchPack(string packName)
        {
            // sanity check
            if(!File.Exists(packName + ".rbx"))
            {
                return false;
            }

            string[] lines = File.ReadAllLines(packName + ".rbx");
            List<RbSearchEngine> newItems = new List<RbSearchEngine>();

            /*
             * .rbx line format:
             *      Name of Search Engine;;alias,alias,alias;;https://search.prefix/?q=
             */

            foreach(string line in lines)
            {
                if(!RbUtility.ValidPackLine(line))
                {
                    return false;
                }

                string[] fields = line.Split(new string[] { ";;" }, StringSplitOptions.RemoveEmptyEntries);

                RbSearchEngine thisEngine = new RbSearchEngine();
                thisEngine.Name = fields[0];

                List<string> aliases = new List<string>();
                foreach(string alias in fields[1].Split(','))
                {
                    aliases.Add(alias.ToUpper());
                }

                thisEngine.Aliases = aliases.ToArray<string>();
                thisEngine.Url_Prefix = fields[2];
                thisEngine.Icon = string.Empty;

                thisEngine.Collection = packName;

                newItems.Add(thisEngine);
            }

            if(_db.GetCollection<RbSearchEngine>(RbGlobals.TABLE_SEARCH_ENGINES).InsertBulk(newItems) != 0)
            {
                LoadState = RbLoadState.NotLoaded;
                LoadData();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to remove a search engine pack
        /// </summary>
        /// <param name="packName">The name of the pack</param>
        /// <returns></returns>
        internal static bool UninstallSearchPack(string packName)
        {
            // sanity check
            if(!Packages.Contains(packName.ToLower()))
            {
                return false;
            }

            var dbCollection = _db.GetCollection<RbSearchEngine>("searchengines");
            foreach(RbSearchEngine engine in SearchEngines)
            {
                if(engine.Collection.ToLower() == packName.ToLower())
                {
                    dbCollection.Delete(engine.Id);
                }
            }

            LoadState = RbLoadState.NotLoaded;
            LoadData();

            return true;
        }

        /// <summary>
        /// Writes the list of installed packs to RocketboxPackages.txt
        /// </summary>
        internal static void DumpInstalledPacks()
        {
            string dt = DateTime.Now.ToString("d MMM yyyy - HH:mm:ss");

            List<string> lines = new List<string>();
            lines.Add("Rocketbox - Installed Search Packages");
            lines.Add("(as of " + dt + ")");
            lines.Add("------------------------------------------");
            lines.AddRange(Packages);

            File.WriteAllLines("RocketboxPackages.txt", lines);
        }
    }



    /// <summary>
    /// Bundles information for a certain search engine
    /// </summary>
    internal class RbSearchEngine
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }
        public string Url_Prefix { get; set; }
        public string[] Aliases { get; set; }
        public string Icon { get; set; }
        public string Collection { get; set; }
    }

    /// <summary>
    /// Unit for the inline converter
    /// </summary>
    internal class RbConversionUnit
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Multiplier { get; set; }
        public string[] Keywords { get; set; }

        public RbUnitType GetUnitType()
        {
            switch (Type)
            {
                case "DIST":
                    return RbUnitType.Distance;
                case "VOL":
                    return RbUnitType.Volume;
                case "MASS":
                    return RbUnitType.Mass;
                case "DATA":
                    return RbUnitType.Data;
                default:
                    return RbUnitType.Null;
            }
        }
    }

    /// <summary>
    /// Google Translate language definition
    /// </summary>
    internal class RbTranslateLanguage
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string[] Keywords { get; set; }
    }

    /// <summary>
    /// Data type for configuration keys in Rocketbox database
    /// </summary>
    internal class RbSystemKeyVal
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string ConfigKey { get; set; }
        public string ConfigVal { get; set; }
    }

    internal enum RbLoadState
    {
        NotLoaded,
        Loaded,
        Failed
    }
}
