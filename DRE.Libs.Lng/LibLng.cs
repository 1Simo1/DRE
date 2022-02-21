using System.Xml;

namespace DRE.Libs.Lng
{
    public class LibLng
    {

        private String dir = $"{AppDomain.CurrentDomain.BaseDirectory}db/locale";

        private String L { get; set; }

        private XmlWriterSettings settings { get; set; }

        private XmlWriter writer { get; set; }

        public LibLng()
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                writer = XmlWriter.Create($"{dir}/default.xml", new XmlWriterSettings());

                writer.WriteStartElement("default");
                writer.WriteString("it");
                writer.WriteEndElement();
                writer.Close();

                setLng("it");

                LocalizationSetupHelper();

            }
        }

        public void setLng(String newLanguageCode) => L = newLanguageCode;

        /// <summary>
        /// Translation method
        /// </summary>
        /// <param name="id">String code to localize</param>
        /// <returns>Localized string (if id not found returns id string)</returns>
        public String _(String id)
        {
            if (!Directory.Exists(dir) || String.IsNullOrEmpty(L) || !File.Exists($"{dir}/{L.ToUpper()}.xml")) return id;

            XmlReader d = XmlReader.Create($"{dir}/{L.ToUpper()}.xml");
            while (d.Read())
            {
                if (d.IsStartElement() && d.Name.Equals("item") && d.GetAttribute("id").Equals(id))
                {
                    d.Read();
                    return d.Value;
                }
            }

            return id;
        }

        /// <summary>
        /// Writes into active localization file a new code with corresponding localization
        /// </summary>
        /// <param name="code">String code</param>
        /// <param name="translatedText">Localized string</param>
        public void N(String code, String translatedText)
        {
            writer.WriteStartElement("item");
            writer.WriteAttributeString("id", code);
            writer.WriteString(translatedText);
            writer.WriteEndElement();
        }


        /// <summary>
        /// Helper method to write first xml localization files
        /// </summary>

        private void LocalizationSetupHelper()
        {
            settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            #region Italian Language

            writer = XmlWriter.Create($"{dir}/IT.xml", settings);

            writer.WriteStartElement("it");

            N("DRE", "DRE");
            N("p_dre", "Progetto");
            N("c_dre", "Cartella di gioco");
            N("dre_v", "Versione Progetto");
            N("opt_bpa", "Gestione Archivio BPA");
            N("opt_sg", "Editor Partite");
            N("opt_trk", "Editor Circuiti");
            N("n_prog", "Nuovo Progetto");
            N("s_drf", "Scegli la cartella di DR");
            N("conf", "Conferma");
            N("setup", "Caricamento dati di gioco, attendi ...");
            N("setup_bpa", "Caricamento File BPA");
            N("setup_cfg", "Caricamento configurazione e record circuiti");
            N("setup_haf", "Caricamento file HAF");
            N("setup_def", "Salvataggio dati di default");
            N("setup_sg", "Caricamento partite salvate (se disponibili)");
            N("setup_dr", "Setup Progetto DRE");
            N("setup_ok", "Progetto DRE pronto! Clicca qui per iniziare.");
            N("bpa_a", "Archivio BPA");
            N("bpa_s", "Scegli File BPA");
            N("sf", "Scegli File");
            N("file_bpa", "Nome File Caricato");
            N("ext_imgs", "Estrai TUTTE le immagini di default del gioco!");
            N("file_n", "Nome File");
            N("dim", "Dimensione");
            N("gst", "Gestisci");
            N("agg", "Aggiorna");
            N("ext_bpk", "Estrai BPK");
            N("ext_img", "Estrai Immagine");
            N("ext_file", "Estrai File");
            N("key", "Chiave");
            N("did", "ID Pilota");
            N("wps", "Armi");
            N("diff", "Difficoltà");
            N("sgn", "Nome Partita");
            N("sgp", "Salva Partita");
            N("np", "Nome Pilota");
            N("dm", "Danno");
            N("lm", "Livello Motore");
            N("la", "Livello Armatura");
            N("auto", "Auto");
            N("?#1", "?#1");
            N("?#2", "?#2");
            N("?#3", "?#3");
            N("?#4", "?#4");
            N("col", "Colore Auto");
            N("mon", "Monete");
            N("tp", "Tipo Prestito");
            N("lrf", "Gare Rimanenti Prestito");
            N("cv", "Valore Auto");
            N("fd", "ID Faccia");
            N("pts", "Punti");
            N("pos", "Posizione");
            N("rw", "Gare Vinte");
            N("rt", "Gare Totali");
            N("tnc", "Ricavo Totale");
            N("mn", "Mine");
            N("sp", "Punte");
            N("rf", "Benzina Razzi");
            N("sab", "Sabotaggio");
            N("cdp", "Conferma Dati Pilota");
            N("n", "No");
            N("y", "Sì");
            N("agg_file", "Aggiorna da file");
            N("drbpa", "Scrivi File BPA nella cartella di gioco");
            N("n_d", "In Costruzione");

            writer.WriteEndElement();
            writer.Close();

            #endregion

            #region English Language

            writer = XmlWriter.Create($"{dir}/EN.xml", settings);

            writer.WriteStartElement("en");

            N("DRE", "DRE");
            N("p_dre", "Project");
            N("c_dre", "Game Folder");
            N("dre_v", "Project Version");
            N("opt_bpa", "Manage BPA Archive");
            N("opt_sg", "Savegame Editor");
            N("opt_trk", "Track Editor");
            N("n_prog", "New Project");
            N("s_drf", "Select DR Folder");
            N("conf", "Confirm");
            N("setup", "Loading game data, please wait ...");
            N("setup_bpa", "Loading BPA files");
            N("setup_cfg", "Loading config and track records");
            N("setup_haf", "Loading HAF files");
            N("setup_def", "Saving default data");
            N("setup_sg", "Loading savegames (if available)");
            N("setup_dr", "DRE Project Setup");
            N("setup_ok", "DRE Project ready! Click here to begin.");
            N("bpa_a", "BPA Archive");
            N("bpa_s", "Select BPA File");
            N("sf", "Select File");
            N("file_bpa", "Loaded BPA File");
            N("ext_imgs", "Extract ALL default game images!");
            N("file_n", "Filename");
            N("dim", "Size");
            N("gst", "Manage");
            N("agg", "Update");
            N("ext_bpk", "Extract BPK");
            N("ext_img", "Extract Image");
            N("ext_file", "Extract File");
            N("key", "Key");
            N("did", "Driver ID");
            N("wps", "Weapons");
            N("diff", "Difficulty");
            N("sgn", "Savegame Name");
            N("sgp", "Save Game");
            N("np", "Name");
            N("dm", "Damage");
            N("lm", "Engine Level");
            N("la", "Armor Level");
            N("auto", "Car");
            N("?#1", "?#1");
            N("?#2", "?#2");
            N("?#3", "?#3");
            N("?#4", "?#4");
            N("col", "Car Color");
            N("mon", "Money");
            N("tp", "Loan Type");
            N("lrf", "Loan Races Left");
            N("cv", "Car Value");
            N("fd", "Face ID");
            N("pts", "Points");
            N("pos", "Rank");
            N("rw", "Races Won");
            N("rt", "Total Races");
            N("tnc", "Total Income");
            N("mn", "Mines");
            N("sp", "Spikes");
            N("rf", "Rocket fuel");
            N("sab", "Sabotage");
            N("cdp", "Confirm Driver Data");
            N("n", "No");
            N("y", "Yes");
            N("agg_file", "Update from file");
            N("drbpa", "Write BPA File in game folder");
            N("n_d", "Under Construction");


            writer.WriteEndElement();
            writer.Close();

            #endregion

            #region Spanish Language (Please Translate resulting File in Spanish)

            writer = XmlWriter.Create($"{dir}/ES.xml", settings);

            writer.WriteStartElement("es");

            N("DRE", "DRE (Please Translate File in Spanish)");
            N("p_dre", "Project");
            N("c_dre", "Game Folder");
            N("dre_v", "Project Version");
            N("opt_bpa", "Manage BPA Archive");
            N("opt_sg", "Savegame Editor");
            N("opt_trk", "Track Editor");
            N("n_prog", "New Project");
            N("s_drf", "Select DR Folder");
            N("conf", "Confirm");
            N("setup", "Loading game data, please wait ...");
            N("setup_bpa", "Loading BPA files");
            N("setup_cfg", "Loading config and track records");
            N("setup_haf", "Loading HAF files");
            N("setup_def", "Saving default data");
            N("setup_sg", "Loading savegames (if available)");
            N("setup_dr", "DRE Project Setup");
            N("setup_ok", "DRE Project ready! Click here to begin.");
            N("bpa_a", "BPA Archive");
            N("bpa_s", "Select BPA File");
            N("sf", "Select File");
            N("file_bpa", "Loaded BPA File");
            N("ext_imgs", "Extract ALL default game images!");
            N("file_n", "Filename");
            N("dim", "Size");
            N("gst", "Manage");
            N("agg", "Update");
            N("ext_bpk", "Extract BPK");
            N("ext_img", "Extract Image");
            N("ext_file", "Extract File");
            N("key", "Key");
            N("did", "Driver ID");
            N("wps", "Weapons");
            N("diff", "Difficulty");
            N("sgn", "Savegame Name");
            N("sgp", "Save Game");
            N("np", "Name");
            N("dm", "Damage");
            N("lm", "Engine Level");
            N("la", "Armor Level");
            N("auto", "Car");
            N("?#1", "?#1");
            N("?#2", "?#2");
            N("?#3", "?#3");
            N("?#4", "?#4");
            N("col", "Car Color");
            N("mon", "Money");
            N("tp", "Loan Type");
            N("lrf", "Loan Races Left");
            N("cv", "Car Value");
            N("fd", "Face ID");
            N("pts", "Points");
            N("pos", "Rank");
            N("rw", "Races Won");
            N("rt", "Total Races");
            N("tnc", "Total Income");
            N("mn", "Mines");
            N("sp", "Spikes");
            N("rf", "Rocket fuel");
            N("sab", "Sabotage");
            N("cdp", "Confirm Driver Data");
            N("n", "No");
            N("y", "Yes");
            N("agg_file", "Update from file");
            N("drbpa", "Write BPA File in game folder");
            N("n_d", "Under Construction");


            writer.WriteEndElement();
            writer.Close();


            #endregion
        }
    }
}