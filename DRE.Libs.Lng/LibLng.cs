using DRE.Libs.Lng.Models;
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

            } else if (String.IsNullOrEmpty(L))
            {
                XmlReader d = XmlReader.Create($"{dir}/default.xml");
                d.Read(); // <xml>
                d.Read(); // <default>
                d.Read(); // Default Language Code
                setLng(d.Value);
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

        public List<Localization> LngList()
        {
            var list = new List<Localization>();

            var LngFiles = Directory.EnumerateFiles(dir, "*.xml");

            list.Add(new Localization()
            {
                Code = "IT",
                ImgSrc = $"{dir}/IT.png"
            });

            foreach (String lng in LngFiles)
            {
                String code = lng.Substring(dir.Length+1);
                code = code.Substring(0, code.Length - 4);

                if (!String.IsNullOrEmpty(code) && !code.Equals("IT") && !code.Equals("default") && File.Exists($"{dir}/{code.ToUpper()}.PNG"))
                {
                    list.Add(new Localization()
                    {
                        Code = code,
                        ImgSrc = $"{dir}/{code.ToUpper()}.png"
                    });
                } 

            }

                return list;
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

            var fpng = File.Create($"{dir}/IT.PNG");

            fpng.Write(Convert.FromBase64String(
                $"iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAF6klEQVR42u1ZPXMcRRB93bunMy5VUdhAUfwAIHPM7yAiIKBInBHyHyAh418QEPAL" +
                $"IAaKGKr4LIRtbJd1Puluux9Bz8zuSbeWTreyzta26rS6vdvVzJvX7/X0AmOMMcYYY4wxxhhjjDHGdQxJxxqfvb8c8sY/fvjVoAP94YOPBr3fx7/" +
                $"8PAHQ1On9DQD4+pPP8ffsP4hKC82GMa0muFHt4c67dwYd8NtffgFfHMMXF1sngnACkzdu49u7n+Y5H9aFAQSMjntHjyCiFwbgsmL++x9bXU8AJDG" +
                $"59Vp6h7r8AoAKgsfHM9x7+hgiAsh2CJAcFIDlw0dbj8dB7M2egqrlfN390sHsIWbLo0iBHQubz7cEAHA6Fg8erJxvAXDir8P7MBrEt0+BoRng5lvegSCJo4N/" +
                $"4WsBIGBuMHeIcOdSgGZba4CTUCe6Q6u7HPH0knLJDsXWgBJwgu5gZ24rKRAMsNAAyo6lwBAMcKhZXwoQ7h4pMIAH7hoAZUzkytjqVYYECwKAHdMA963HQ6Y0WMsAI5bWvNQMcBJ0g/" +
                $"dpAOnwDMCO2eDWDMgscMLYa4MOo0MoccUu1UNbAgDGfoBmPSnghLljaUsoqzJ5uWA9sGsp4EkDSIevFUFzNNbALL4guV6WHUmBLQAg292gN30aQKKxVAdAAXq7+v" +
                $"LiMqCUdcnm6Q5nTwo01qAxg9ABlVUxlCsGoNkyBYAAoGlgaxlghDUGswYKBRmTb1kgV5sCFxDB7PhR/xCWiz32AWCWGMC2KyRyoVTYJREkwuEcAcB6DTDHYjbHoplD6" +
                $"ypEMLthTocNQHjy5MmgAMyOjzeedGEBQwCNBOdzNGS/DVIQCNEAUUgCAbrZgKuqGhQA3ZBR7JbAIIQIbWNPKUwjnB4uIAwRZE5/" +
                $"hSQrOa8WiAxcRW0IgJCxoWUqoiR0hGY9GpBtkA4BwwoRNxFwZUKDT+5cdYBvDBZBOLIAhv09WwRTPwAUCBg9AZEiiuUyef4iyKbZrO5PbHX31BANDfDGYGtL4ayU7jF" +
                $"Dic2AqAAuEHT04CoaQhvYINMiMU++86JbTynsYRGNW0xcFCAgVECl1FSU86XA8HWAbZQBFJSV9+T/nnaDa21QvCOCKf+lTFxAClRDHZCUdXfrAIm2VxpnboY42F8KB1VSP0" +
                $"AAuqRbafBJBe5tdcjnnAybVYLSTloAT6tOetoO96VA7gp77AVIgdKDTy6gplbZOVZ3+BTgBhqQf+K6Vgc8tsPrRFDSZqGIIKL6s/y3KuCpDjhDAvQybDJb23nAR/" +
                $"T+wwK96EDbFMUzmqJ0iKdng94CATCJYVbCk6STEwO4pM1Q5759/4NA8X/PuV9cgP39gDxwF4b6p3l6KoyiGgxsyFU36OaV8jIKpWcw4AQolFiwWHAvDRG4A+49L" +
                $"bFSJUVPMJqjsQnI7cF4YJJEMJMif6tL+8vLgFOsOAkI8/lOD8BzN6i4QO9zAS+dU6iEE0gAEh3j2BqLtLuNdrEzEAoVGTwFkFt05OrE0+aGawEgPD0HyJui/ucCKUe" +
                $"MKdc8rE9ciihCAFFN5wBVXWNAkhoqlwMAPfVzpLW6aBoTkCR+xfeD+uYdV2BPChSRIEsfQBONwNYX2nyXFfFTiQHWWqFWPVUt5oFKhx0rGnLG51pXMXAXwJcrk2BH+UmuvtI3Vl1g" +
                $"TQpUoqhEUIsmq8tKH3+rKCgCFSkrr6KoNfb9k3KsMdG6X6HPUPHez6fTGE7ThB4IgEbAXJRZHHOlR0iadIAj+X6qkE6vogBQa4XXX3kVU5vGKmjQPQpBhVaxJxARTOs93JxMsac1pv" +
                $"UElVTY07jVtJ5govXAAkjsv/dOUH2xiJcZfLHEcjaDNw2sMWhqeUmyTC/t8GCCkpjcvgX9rT4NwHT/Jg5++hWa6CvSaYpK6EF+fwTg8RnFzjcP3hwUhD+//+5UoxNJ6PIql+NJJmWRdMLv" +
                $"H0An9SnD2gfwVjpehzgE8A+AQ+kw4cbJ/sBLHA2Ao3QcY4wxxri+IYPX7C9Y6HVnwAjACMA1j/8BriThnliM8QkAAAAASUVORK5CYII="
            ));

            fpng.Close();

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

            fpng = File.Create($"{dir}/EN.PNG");
            fpng.Write(Convert.FromBase64String(
                $"iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAOwElEQVR42u1aB3RUVRr+3pvJpBdSBEIQBRVBhISuSEdB8aAr6mJHFGk2hCUWdgGlSwRdFwtHWZaiIBiaLQF" +
                $"BIQgmdAgtghogCSEJIT0z897+/31lZlIFYc9hmXvOzX155Zbv//5y/zuAt3iLt3iLt3iLt3iLt3iLt1yNRdJb6/r+/e2NX30VlgYNzIdn8sswZcFOpOzNuSSD7Vk2WLTpsb" +
                $"E1Pm+9Z49oYx9ddUnGa9MiHFNHd0TEoVRkvfUWHAUFsNL6GtE6e8TH+9ArDgOAoGVA0V3r18O/ZUv4t2ihoSNpj9OPF2DbvhxU2pV6B/W1yfCzWWG1SLBaZQ1duua+BvVoJv4v/" +
                $"PrrGr8Nvece0a798TeoqgqHUxX/OxyKuC6vdKCisv458Hhd2lyDW5sGoHjLFpRnZGj3r7sOQT17Iv3YMcTFxQXTrWKr8Y1/hw5Qy8qQv2QJAtq0QRhNRgoI0CTTvAGaNgwSE8vI" +
                $"LLxoiRgAlKam1gnA7sNnL3qMGJrn/b2aITDrV+S+vw5KaSlkWkfI3XfD1qqVAJYrr9n8w8Vv5jzYooJRvG0bijZvRmlaGsKHDIF/u3YaRQKseHRAC6EOazb/" +
                $"ijKSxoUWfWDYc3PrfJ57ruyC+/axWnBv92vR6+YQFHyxDGd27RL3ef68DikwEIqisSe/sMLFFuNiyGsbsXHRCLSeMAEFCxei4uhR5Lz7LgI7d0b4Y4/BEhws3uvWriFaXx+GheuO4kBG/" +
                $"kVJyVlSUufzktILA7dF0xA8c19LhPx2GKfemAVnUZGYL8+b5+8O7tHfCzHk9eTqABSXVeDTNYfRLDoEw0a+hIDtP6AgMRHFP/2EsgMHEDF0KAJITbiEBdsw9tE22LzzNFYkH0dpueOCGKA6" +
                $"nXU+dyrKH+rP5mPB4D7X4c42Ychb8m/k7Ngh7vM8I2m+MoFg9Hm+uBJLvzmKQycKUHC+vDoA9CYN7MSeI2fx6vs7iO63otubcTi7YAHKDx9Gzrx5COjUyeyYS8/" +
                $"2jdH2hnB8suYI9h3Lu3QAOOsH4IZrQzH8/psR+tshZMbPgEJS53lFPPkkArt08ehv56Gz+HTtYQKhAqFBNk+D6TY8GHiFPiopc2BB4mHsuDEcw58fB/+U73Fu1SqUEMLlhw4hctgwBHTsKL5" +
                $"qEOKL8U+0xabU0/gsKYPY4KwfiHokrKh1SJ08y0N3Nkf/W8NwdtGnyNm+XdwPpPlEPvOMKRwuRSV2LP76mPBgvD7TAKpqLQwg5Llq3k8S1nj8vO14/J62uGN6B+R++KEAIDshQaAc+fTTk" +
                $"ENCxOe9OjZGu5vC8TEBt/do3mVhwI0k9ZGDWyH0xEFkjp8K5/nzsND4rJ6BXbt69JGanotPVpPUCQRDwKqq6K61RgAUYSWd9IIk0wRUSUexEh98kY5tLSMw4oW/wX/" +
                $"rBrKyX6CIvEUp2YbI4cMRSKph2IYJTxIb0k5j8VfHqtkGY3LKBQJg85Hx17taYEBbkvrCj5FFdklInVWSxmcQTF2nBS9afwQpe6oEbxIBQNRSBAOUWlRA2AFigFi85PH9rkO5GEs" +
                $"G5MmB7dB9VkecnT8fZenpyJ49W6Af+eyzYiKCDR002/Dhl+lkU/L+lBG8qVkoRj/YWpP62Cmm1FkNA2+/3eO7nw/m4mMa0yV1j941FaiiX24McMLucOoqIHvQxKVTlfjXioNIaRmJ0S/" +
                $"Hw+/HZBQsX46ilBSU7t+PqBEjTAPUIMSG14bGYmPqKSwil8lsuBAGsIV/pH8L3BPbAGc/+QhZNIaQOrm1qJEjPaVOFn7huiPYsju7HiNMKqDUygBFowgzQEY1Bniw4XAuXpxTgKH3xqFHQ" +
                $"meKuN5H2cGDyJo1C0G33YbI554z2dCnYzRiyZjOX5nuYkAtRtB4fuO1IXj+4VsQevwAfn9xkkvqxLKgbt083t1x4Aw+XFWb1D06d2OAUpsb1KqkGoaidhDYwv5zObHh5kiMGfc6AjZ/h/z" +
                $"PPkPx1q0o27cPUaNHm4YpPNQPE59p7xYJ1e0p3nq8JXIXfIBs6ktInfqJGjUKltBQ851CkvonFLf8uCv7j8ah+v5CqcUIgg0gqYHdCVl2LV6SpTq7TU0/g9Ez8ykSa49e87rgDEWPHDidnjZ" +
                $"NSEvQ1W3if0QFfhszBs7CQvFdFLEpqHt3j+c/7T+D+SsOoLA+qVdxvfy9IICbGzRWF7Zy5cqCnrRTslqtsFgsBILssSO8lCVj4MCag5uvvrose37D/7N6+/j4YNOmTRg4cCDv+89ZXax0iso" +
                $"LNxbNLX94qUGojwGXevFGa2yGnG7jmwyYZrEU9O7bFxJTjxYs6QBIVV680opqKrgW5QZERCDp228xzun0ZIBdR0ZidIgFMku+CghX6uJVffGKzj5HTXEA33SwCjgcvGoKBCXIuuRNlbgCF+/" +
                $"aX+gAkBootQFQXFkJiaqVGGCoAVtMWZIuizH8nwBhGEBmONfy8poZYNDDwkZDNxaG9A11uOIWz4JjAHRBMgukWhkgoiRFxOn8oWp0oLkDrV5BQKhuXowFqurz5/U5awKgMYWxMb17w8/PT/" +
                $"hKIx7gTqrGBX9WHY5QvFFTafnDD5fF9Wm7XA7y7PD390fMN98AFGB5AMAPjQ+MeIALL964f6nsgGq3X9Y4wNB792oAwdXhcFRnwMQP0rA0xYqiUhViNyRZRCt2hkLqWoAklKDKPMNo5xf/" +
                $"VJzYBruX8xs2ICchAc5z52AJC8ONjPwfyAiZwVlBAbLffhtFFLmJXF9cHBpNnIhvMyrx3ucHUVJqr/W4R/LIACliJwjVgfAGIdifttV8VXbfDgt0nFp16kzQWtd9PqQwNk1c+3Vpgq" +
                $"VT+6Jn+0bmgI78fGROmIBTr78OR14egvr0wfVLl7okwnamplpFajKBFk17iujp00XmqSQtDccfeQTdMlOwbGpvdG4T5TEXo2oHKW7rMNbiZCZ4bodlF2haTtDppjOKvnhXy5ZUEcnT4EArZ" +
                $"j7fGVNGdEBYkI856fNJSfjloYdQRNLn/FyTmTNhHxOPZ9/b61ocUbDGqj9/4e0UcSxn/M+nOdfTTjOoVy+R/GRWlca/hNlDmuG1p9vBz9fiEhDPU9XnqrjSfIpiqIHndlh2Txa49EQ1pa61Wq" +
                $"bI+P+uLjFYMaOfyAO60/XkuHE4GR+vSb1fPzRfsQKrS5uIMwfONnvszGqoRtm6OwuDJyRh9eZfzXusQgxmk9mzIdMusSQ1Fb88/DDuyNxKc+mLrrde4zF/DQyXEA0mi3RbbUlRAzVJUoT+Cxr" +
                $"K+sRUGWGhPnhjWAf06RTtcjGs6999h2zyIsbhY2PS07xWnfD0vJ3Yd7R6SkypJyHCc+Asz+SP0pC0/SQmDW+Pa8L9tRMq8lTNY2ORRWrBNiZ7zhwEko1ImDIFX3dqjIQl+1FcWmnGMYqRBVY" +
                $"1kBXVMynqqQKGF9DRYlaw3gipd22C5dP7ojdJ3UPXx47FSdJ3lnrInXei+cqVWF0eI6S+72jen7bo2/ZmExuSkbjphAkS2wZmQgwt3mKw4cEH0f33LfhiFrOhoSl1dzugqIYNUGs7F9AMnCSp" +
                $"IosKWnyDECteI6n369JUuEZje3yedlQsBaY+H6lHT5pEUu+MoXPTaj0kudiTocLiCkzS2TD5uQ4mG4LJuPoTG7KnTsX55GRkcYL2++/xzptvYn3HRkhYvJe8mkMzeqY3qIUBImUEjdZOXRX" +
                $"Ywi+bxlKPNoMgIfWXXxb67qTrkAEDcMOaNUgsb4oH4zdg95E8uM4eJDNwco8hzHucezBqTc8lIzstif627snGfeOSsHLDcZcEw8MR8847iJk7V9iJ4p9/" +
                $"xi8PPIDumcyGfsI2qKqR8eaKmo2gYIBTY0AoWfWpozoJtDnXb1C+kPx4xqBBAm02RDyofezf8UTCbrz9n72oqHRAckdXMqbvuZNU9d2mIQsj7HZPUhiV+3DPyrF+8482Rs7YguyzJS5" +
                $"PQWxonpiIYDK+zuJikaAtGT8Gcx5phleHtkOAr2waencVkD2MINW+ZOAWTe6N7nENxcumrpPUM195RdP1/v3RYu1arCprigfGJ2OP23k+T9Yia1US15KoPlbJnKxks9VYjef8Llf+ztiG" +
                $"GH0aYLCnuG/cd1ie9Itp/UXcQHYhmtykRNdFZBsyyDZ0I0+x5K3e6NQ6qtqxm2kD/AmhcY+1Rbe4aDP8FVInC59LugXdwseQfuXf0hWPzWFdz4cse4qXI0ZZn6Us9hHaNS/" +
                $"GDHVttmr5RrcfLtDiZd0WqGYC2Zi4cbbHn/I5xaSPUpG8IxNTntM8BT8PJE9xfdu2OD1jBs6xhyJA/MhTTKfAbOe5EMRP3oTSqgx4dMANIrIycoIcvp7iaE7368Gs6+vWYVX5tfgLS" +
                $"X237tcNXeXvuFpZeiQqrnyk5etjEZUPOoz3ZQKAq+Tra1bZDRSb/r72ndaP1qckwGFgZZ0ZDGHKniwMGvstViRlmAEcs6ExGekmFErzdfHOnTjx+OO47eQ2jHigVU0McG16igmt3HnzoBII" +
                $"PlFRiHnjDRSS1IfO3YX9GflioTZ9IsbvgHzcWh+33wZZLca17HJjQUF1bmQC/bRpacGMpCcyNQpUOpzCavBGnUGwi82tJH4/NO3TXdiYehITh8UiItQm1hLQoweu+/xzZJOh" +
                $"LNi4Ebnz58NBO95qADDyChmPnIULUUYxt0xSCR08GI1GjsRXewuxZM42GlxFo4gA+NqsCPDzMSXMNDck7MvSslpMMFhFLLR4AwDjeKuuEndzhBkQsc2y63sQLhV2/qGUdoTHbVFJBewOLepj" +
                $"1Thx6hyen/Ujnrr3JvTrHK3ZHNpHNPzHPxBItgtkKO36kbonA86cQeKoUTRChfajor59Ud6wKZInLUZOfrnGN8adYC+ntvAiUiNtozJFm71+fY3PG91yi2g3JO2tP9un+3PTr6umI0cZ3ZtLu9" +
                $"svE4Mpam1MwrKa7HJ27Qp/EjTIXbrnOZmTjfT2aijFLAduJTcm+HlGhv/XhTMi5XrrLd7iLd5y9RbpchxJX0lFvtoZ4AXAC8BVXv4LgPzgo1orXicAAAAASUVORK5CYII="
            ));
            fpng.Close();
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

            fpng = File.Create($"{dir}/ES.PNG");
            fpng.Write(Convert.FromBase64String(
                $"iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAI+UlEQVR42u2ay48cVxXGf+fe6p6eh3tmPJPYMY7jEJOEhwwBpEgIEGQVsWDBigUCseUv4W9AQghWwJ4oQogFIYZILIjNIwp" +
                $"BCfFrbM+MZzw9j6669xwW91Z1z7hnnBiNbcZ9FtM1perqOt8933cet2BsYxvb2MY2trGNbWxjG9vjaJI/i19MnqweJ8e/v73UAkKR/+8Y8Oovf0a1dBMHiAhgH/vGrj2B60wghYci3V68z/" +
                $"ejObevhQCAmWExNucsRHSnj5b9+1pnM0OB1sknef27PwToAL36aQoDiJFwawUn0oTGUTED1IzWEwv1shbNHwDznurOBtXyCnJEATAz2r0eeN+cL4YvqG4uEze3UgQcMQTMUgSUN5Z3EbsBQD" +
                $"F2ri2BKnqEI6B/bQkbgqAYRshixGLEHbXlH9IA0YgZoylgpqAKIveh//83PEBHUSBiWEgRgAhHjQSGoWa4GPfTADBVLCp2RCPAzJLGjQLg0z++ydmvreBtDeeOaBZQw7crbp2/AT/aA0AVIMY" +
                $"KJxEz4X8KARkSlkcsC5gF6gJztwZogklNAcHJfTou0K8EM+i0Lf/yoxEBZoZgyde7NECTFGIRLDnwkWkgoObZKR0VnstrkwR1PHt8kzaRdhHxTh8uEJYXWANq+0RAjBGxCsGBkEG4Nwqb/" +
                $"TarvYKt7RbT5Q66WQLCnc3ARmuKY1N95qcD053yoYGgmrKAR/Nij9CAhE5EUFzNAblHyHvYqgo+/H3EbjmuiscKxTysbHtavuL2fKT4ljHVCQ+FEin8swZoGE2BGCHGgCOmVde0+gdRQ" +
                $"QNYCfPTW8x+yvNhf5I4Fym8YRoJweHveE6f7bM4FdAqOS/uwTrfZIGoYDpaBEMEjYFISEWQo+kKjT0g5OM3/+TZKeGVrwfOvRAoi4pL77forArT7TZhwfPUZ0tePGtYDLxxwSPAV78Suc9xw30" +
                $"DoAamB0RAFSDEiCOiODBDJIMgdwuKAe4Jw0VhZ0v560+7bAZjZjsyHWbo/uB7+JkO5eYS/1if5dzkT+D4FtHALD5Q5+saQDX9dhgVAVWEGCJRAiB4E5xLK+9yZbg3Ck6e/TYboUtbfk5/" +
                $"XbnyXA/fisz+ewbX7uDe69GJXcLcLEWr5NS577C5rcCvMX0wzjeNUAZAVfePgDu9khYlrcLhfV0LWK4Mk/c1CKEvrFy+zJ3Vgkulo7dVscQCGpXjG1sU23ewCcfUekm5s8Pf/yD0qnex6" +
                $"YKVZwPtjj2g1TcMyxoHfd2hCvuIoGlEvCEoArhMAeescVwEaMPVvzhuXBWmXpjCrgeMkmIrdZJ/m+4ib1zk5ZfP0293mZg+hgXFHZtm5YOSa2vGc9+IUB02AJKrP8BZKoRkvzQYDTPFN" +
                $"GJOMs8lF3d1d2hJ/wKcOq9snpznxiXjzAs7FDccz62tsb3e4nfPf56+KS++fYHZpxcIt/ucXthg+R1h9twCZ05XSHwQOUDqPpCmsNlPA6JC1IBKRFXSVEhAJcHQ1AUG0hLWrgi3LnzA3O" +
                $"fmmf+kIlf6dLoVxaLx/M1/EWKbqe5F6EeqTcf0q8ri89tc+eMyK1+Gp15SiIe1+rab+wYaLUf5ARpgGlEiEZqpkHdp3RVBsARqBZPH4IlvfwnpziH6OlYKsfSoh/Ozl5joOcoTk/" +
                $"RN6KwH0B4z58/TPRaY0jexcHhpsCl8kJz6agFMxd7ICIAEgKGYJE2og14Bj9Ak7wjTJ4Dl37JxbQJ5SomVojFVR6VM4lotOqubRAoigi+U1f/8Bhcj3VMK4bDrfgHTRgNMDY0pDY7UgBB" +
                $"TilACMYufNvxPA5NaEHHC8lqX1955mqCexcnrnH12mXf/2SYwz/biS/Q22szeehtXrnL6xW3W2l1eu3gKEJ6cuM4n5ldBD48CZjIIf00UMCNVgqO7weSmasQ5wRSUVAuoCiKG4FBNQli4P" +
                $"l84sYyaZ0K2OPmZPnMnIuv6DNfjDlXocVq6zHGZuVN9bm94vngijaTbfgfTcIgASJ5pWKZAXQyluec+ABimCqKggpJSn6ogruZUSo+oMDuxwTfP/Hlwpz7MzpTMcpEzXNz9RCXMT6zzyjMX" +
                $"Bufi4aq/Wer+hiPBFNiPAgkVxUzzipMRdDWdMGcDjj2iIzOzpvXPcw3NGlBTQ0fPA678apZ335qH9QIPOMCLGxw7hyT6Z2U4aAHkcD28h/5BKnpUJJW+IkQzAsbcwgIfvLcMrOzdF0hfIsa8" +
                $"MyRo3RoDYoajFsZ7OC2HrPD7gGHDSSDvc2gugzSXxHVpPHIsPjQ5GDhkQzXwsJN7u6Phh3Euf0UOjgbZx7ERK25NX6sjZ/67LpfhIajdtfEzcixugKVyEHPSqL3loYiJNPsFTmTgcHZ2" +
                $"eIpa7yqYcx9Htw6mU5ZuG8ZcB3NG21MFqlnWLkOlbory9t++W2OaJiYWkwt17k+TIWtmhGYy0MF8t0YVnBuMfER2R8NH0QizuzYy0n197TWoDpodjRkYab4zCHeIGQDNL0igdvDOEKqZQ5" +
                $"ILKmmOFXAuZQXJjgy2AGTgqHcNGJKPpdmTH6LFKArZ0NAwRizW1+YIiBl6SYVbTQCL6bnJAGjdC2Apxdd3tf0okLkvOiQlUneFhg1FQv3Y4lwT+lKvdFE0zor36VUZaF5K2BUJw5sPanc" +
                $"1MxIjlgt3y3t6TgTViro8rZ/GTJsd4Drcm0hgiAK6eyrbACDeJ84WfleSs2a1BlwX59IVziE+vwfUqj9bg+OiaACor8sjpgPm14bUwhUDTecSY9KksqrDL9GrCslJl2mqOkiBQ2A0O" +
                $"uHdUDTuAaC1eByZaOPE4cxwzuFz7vfO4RCcCH6ijZ+cRNotXLudVrrdSvhMTOwGw0n6waGXo+QAcTTV5GQOV1PDqoDll6diWaL9fnppqiypNnpoFYgxJF1SRVVxQ5ktWopgZ0ZrcRG5" +
                $"efVuALqdDm9du4LzrhE+VzdDkrvBHIKy+fAKQWs0y3ZVfppFr06LDaNkaD5ghr5/G5cXazj5zAAn8+fjYD1gCejJUCR09s4HjrAFYIfDnUqMbWxjG9ujb2JmjzUA7nGPgDEAYwAec/" +
                $"svcFK3Xl1mErcAAAAASUVORK5CYII="  
            ));
            fpng.Close();
            #endregion
        }
    }
}