using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SDSE1_s_Sidekick
{
    public partial class TranslationStatus : Form
    {
        public TranslationStatus(string UMDIMAGEpath)
        {
            InitializeComponent();
            ComputeProgress(UMDIMAGEpath);
        }

        private void ComputeProgress(string UMDIMAGEpath)
        {
            int InterventionsTranslated = 0, TotalD = 0,
            PrologueIT = 0, Chap1IT = 0, Chap2IT = 0, Chap3IT = 0, Chap4IT = 0, Chap5IT = 0, Chap6IT = 0, // IT stands for "Interventions Translated".
            EpilogueIT = 0, FreeTimeIT = 0, OtherIT = 0,
            PrologueTOT = 0, Chap1TOT = 0, Chap2TOT = 0, Chap3TOT = 0, Chap4TOT = 0, Chap5TOT = 0, Chap6TOT = 0,
            EpilogueTOT = 0, FreeTimeTOT = 0, OtherTOT = 0;

            List<string> FilesYetToBeTranslated = new List<string>(), CorruptedFiles = new List<string>();

            foreach (string TXTFile in Directory.GetFiles(UMDIMAGEpath, "*.txt", SearchOption.AllDirectories))
            {
                // The txt's content is saved inside "lineENG".
                using (FileStream TXT = new FileStream(TXTFile, FileMode.Open, FileAccess.Read))
                using (BinaryReader txtEditor = new BinaryReader(TXT))
                {
                    string DirName = TXTFile;

                    /* Saves the path and delete whatever comes before "UMDIMAGE".
                     This way it will be easier to find out from what chapter the txt is. */
                    DirName = DirName.Replace(UMDIMAGEpath, null);

                    byte flag = 0; // 0 = Not translated, 1 = Translated, 2 = File corrupted.

                    // START - Check the TXT to determine if it has been translated.
                    if (txtEditor.ReadUInt16() == 0xFEFF)
                    {
                        if (TXT.Length > 4) // If lenght <= 4, then the file doesn't have text. Just a empty space.
                        {
                            uint OP = txtEditor.ReadUInt32();

                            TXT.Seek(TXT.Length - 2, SeekOrigin.Begin);
                            short ED = txtEditor.ReadInt16();

                            if ((OP != 0x00000020 && ED != 0x0) || (OP == 0x00000020 && TXT.Length == 10))
                                flag = 1;
                        }
                        else
                            flag = 1;
                    }
                    else
                        flag = 2; // File corrupted!
                    // END - Check the TXT to determine if it has been translated.


                    if (flag == 0) // If the TXT hasn't been translated, then add it to the list "FilesYetToBeTranslated". 
                        FilesYetToBeTranslated.Add(DirName);
                    else if (flag == 1) // Let's count how many files have been translated until now.
                    {
                        InterventionsTranslated++;

                        if (DirName.Contains("e00"))
                            PrologueIT++;
                        else if (DirName.Contains("e01"))
                            Chap1IT++;
                        else if (DirName.Contains("e02"))
                            Chap2IT++;
                        else if (DirName.Contains("e03"))
                            Chap3IT++;
                        else if (DirName.Contains("e04"))
                            Chap4IT++;
                        else if (DirName.Contains("e05"))
                            Chap5IT++;
                        else if (DirName.Contains("e06"))
                            Chap6IT++;
                        else if (DirName.Contains("e07"))
                            EpilogueIT++;
                        else if (DirName.Contains("e08"))
                            FreeTimeIT++;
                        else
                            OtherIT++;
                    }
                    else if (flag == 2) // If the TXT is corrupted, then add it to the list "CorruptedFiles". 
                        CorruptedFiles.Add(DirName);

                    // Let's count the total amount of interventions the game has.
                    if (DirName.Contains("e00"))
                        PrologueTOT++;
                    else if (DirName.Contains("e01"))
                        Chap1TOT++;
                    else if (DirName.Contains("e02"))
                        Chap2TOT++;
                    else if (DirName.Contains("e03"))
                        Chap3TOT++;
                    else if (DirName.Contains("e04"))
                        Chap4TOT++;
                    else if (DirName.Contains("e05"))
                        Chap5TOT++;
                    else if (DirName.Contains("e06"))
                        Chap6TOT++;
                    else if (DirName.Contains("e07"))
                        EpilogueTOT++;
                    else if (DirName.Contains("e08"))
                        FreeTimeTOT++;
                    else
                        OtherTOT++;
                }
            }

            // Print the list of interventions that hasn't been translated yet.
            if (FilesYetToBeTranslated.Count > 0)
            {
                using (FileStream TXTFile = new FileStream("Files_yet_to_be_translated.txt", FileMode.Create, FileAccess.Write))
                using (BinaryWriter TXTBin = new BinaryWriter(TXTFile))
                    for (int i = 0; i < FilesYetToBeTranslated.Count; i++)
                        TXTBin.Write((FilesYetToBeTranslated[i] + "\n").ToCharArray());
            }

            // Print the corrupted files' list.
            if (FilesYetToBeTranslated.Count > 0)
            {
                using (FileStream TXTFile = new FileStream("Corrupted_Files.txt", FileMode.Create, FileAccess.Write))
                using (BinaryWriter TXTBin = new BinaryWriter(TXTFile))
                    for (int i = 0; i < CorruptedFiles.Count; i++)
                        TXTBin.Write((CorruptedFiles[i] + "\n").ToCharArray());
            }

            // TottalD = Total amount of interventions in the game. 
            TotalD = PrologueTOT + Chap1TOT + Chap2TOT + Chap3TOT + Chap4TOT + Chap5TOT + Chap6TOT + EpilogueTOT + FreeTimeTOT + OtherTOT;

            // Calculates the amount of interventions translated.
            double PercentageTOT = ((double)InterventionsTranslated / (double)TotalD) * 100;
            double PercentagePrologue = ((double)PrologueIT / (double)PrologueTOT) * 100;
            double PercentageChap1 = ((double)Chap1IT / (double)Chap1TOT) * 100;
            double PercentageChap2 = ((double)Chap2IT / (double)Chap2TOT) * 100;
            double PercentageChap3 = ((double)Chap3IT / (double)Chap3TOT) * 100;
            double PercentageChap4 = ((double)Chap4IT / (double)Chap4TOT) * 100;
            double PercentageChap5 = ((double)Chap5IT / (double)Chap5TOT) * 100;
            double PercentageChap6 = ((double)Chap6IT / (double)Chap6TOT) * 100;
            double PercentageEpilogue = ((double)EpilogueIT / (double)EpilogueTOT) * 100;
            double PercentageFreeTime = ((double)FreeTimeIT / (double)FreeTimeTOT) * 100;
            double PercentageOther = ((double)OtherIT / (double)OtherTOT) * 100;

            // Update the progressbar's value.
            progressBar1.Value = (int)PercentageTOT;

            // The data is saved inside the labels.
            labelPrologueIT.Text = PrologueIT + " / " + PrologueTOT + "  (" + PercentagePrologue.ToString("#.##") + "%)";
            labelChap1IT.Text = Chap1IT + " / " + Chap1TOT + "  (" + PercentageChap1.ToString("#.##") + "%)";
            labelChap2IT.Text = Chap2IT + " / " + Chap2TOT + "  (" + PercentageChap2.ToString("#.##") + "%)";
            labelChap3IT.Text = Chap3IT + " / " + Chap3TOT + "  (" + PercentageChap3.ToString("#.##") + "%)";
            labelChap4IT.Text = Chap4IT + " / " + Chap4TOT + "  (" + PercentageChap4.ToString("#.##") + "%)";
            labelChap5IT.Text = Chap5IT + " / " + Chap5TOT + "  (" + PercentageChap5.ToString("#.##") + "%)";
            labelChap6IT.Text = Chap6IT + " / " + Chap6TOT + "  (" + PercentageChap6.ToString("#.##") + "%)";
            labelEpilogueIT.Text = EpilogueIT + " / " + EpilogueTOT + "  (" + PercentageEpilogue.ToString("#.##") + "%)";
            labelFreeTimeIT.Text = FreeTimeIT + " / " + FreeTimeTOT + "  (" + PercentageFreeTime.ToString("#.##") + "%)";
            labelOtherIT.Text = OtherIT + " / " + OtherTOT + "  (" + PercentageOther.ToString("#.##") + "%)";
            labelTotalIT.Text = InterventionsTranslated + " / " + TotalD + "  (" + PercentageTOT.ToString("#.##") + "%)";

            if (FilesYetToBeTranslated.Count > 0)
                MessageBox.Show("The corrupted files' list has been saved inside \"Corrupted_Files.txt\".\nReplace the corrupted files one by one with an old working backup.", "One or more files are corrupted!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}