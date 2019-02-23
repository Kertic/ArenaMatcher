using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ArenaMatcher
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Player[] dudes = TSVReader.GeneratePlayers("./Files/TSVExport.tsv", 23);
            Team test1 = new Team();
            Team test2 = new Team();
            test1.members = dudes;
            test2.members = dudes;
            bool test = test1 == test2;

            for (int i = 0; i < dudes.Length; i++)
            {
                dudes[i].teamPairings = dudes[i].findPossibleTeams(dudes, Composition.GenerateViableCompArray());
            }

            for (int i = 0; i < dudes.Length; i++)
            {
                dudes[i].PrintPairings("./Files/Export/");
            }
        }
    }

    public class Player
    {
        private string mainName;
        private string charName;
        private bool[] myClasses;
        public Team[] teamPairings;

        public int lowRatingBound;
        public int highRatingBound;
        public int curRate;

        public Player(string inMainName, string inCharName, bool[] inClasses)
        {
            mainName = inMainName;
            charName = inCharName;
            myClasses = inClasses;
        }

        public bool CanBeSpec(TSVReader.ClassSpec spec)
        {
            return myClasses[(int) spec];
        }

        public bool Equals(Player other)
        {
            return other.mainName == this.mainName;
        }

        public Team[] findPossibleTeams(Player[] playerList, Composition[] viableComps)
        {
            List<Team> teamsThisPlayerCanBeIn = new List<Team>();
            List<Player> playerListWithoutMe = playerList.ToList();
            playerListWithoutMe.Remove(this);
            for (int i = 0; i < viableComps.Length; i++)
            {
                List<TSVReader.ClassSpec> specList = viableComps[i].Comp.ToList();
                for (int j = 0; j < specList.Count; j++)
                {
                    if (!CanBeSpec(specList[j])) continue;
                    List<Player> currentTeam = new List<Player> {this};
                    specList.RemoveAt(j);
                    List<TSVReader.ClassSpec> remainingSpecs = specList.ToList();

                    for (int k = 0; k < playerListWithoutMe.Count; k++)
                    {
                        if (playerListWithoutMe[k].curRate < this.lowRatingBound ||
                            playerListWithoutMe[k].curRate > this.highRatingBound) continue;
                        bool foundPlayer = false;
                        for (int l = remainingSpecs.Count - 1; l >= 0; l--)
                        {
                            if (playerListWithoutMe[k].CanBeSpec(remainingSpecs[l]))
                            {
                                currentTeam.Add(playerListWithoutMe[k]);
                                remainingSpecs.RemoveAt(l);
                                foundPlayer = true;
                                if (playerListWithoutMe.Count == 0)
                                    break;
                            }
                        }

                        if (foundPlayer)
                            playerListWithoutMe.RemoveAt(k);
                    }

                    if (currentTeam.Count == viableComps[i].Comp.Length)
                        teamsThisPlayerCanBeIn.Add(new Team(currentTeam.ToArray()));
                }
            }


            return teamsThisPlayerCanBeIn.ToArray();
        }

        public void PrintPairings(string folderLocation)
        {
            StreamWriter sw = new StreamWriter(folderLocation + "" + this.mainName + ".txt");
            for (int i = 0; i < teamPairings.Length; i++)
            {
                string line = "\nTeam:";
                for (int j = 0; j < teamPairings[i].members.Length; j++)
                {
                    line += "\n" + teamPairings[i].members[j].mainName + ": " +
                            teamPairings[i].members[j].GetMyClasses();
                }

                sw.Write(line);
            }

            sw.Close();
        }

        public string GetMyClasses()
        {
            string returnString = "Classes: ";
            if (myClasses[(int) TSVReader.ClassSpec.UNHOLYDK])
                returnString += "UnholyDK, ";
            if (myClasses[(int) TSVReader.ClassSpec.FROSTDK])
                returnString += "FrostDK, ";
            if (myClasses[(int) TSVReader.ClassSpec.BLOODDK])
                returnString += "BloodDK, ";
            if (myClasses[(int) TSVReader.ClassSpec.FERALDRUID])
                returnString += "Feral, ";
            if (myClasses[(int) TSVReader.ClassSpec.RESTODRUID])
                returnString += "RestoDruid, ";
            if (myClasses[(int) TSVReader.ClassSpec.BALANCEDRUID])
                returnString += "Boomkin, ";
            if (myClasses[(int) TSVReader.ClassSpec.GUARDDRUID])
                returnString += "GuardianDruid, ";
            if (myClasses[(int) TSVReader.ClassSpec.SURVIVALHUNT])
                returnString += "Survival, ";
            if (myClasses[(int) TSVReader.ClassSpec.MARKSMANHUNT])
                returnString += "Marksman, ";
            if (myClasses[(int) TSVReader.ClassSpec.BEASTHUNT])
                returnString += "BeastMaster, ";
            if (myClasses[(int) TSVReader.ClassSpec.FROSTMAGE])
                returnString += "FrostMage, ";
            if (myClasses[(int) TSVReader.ClassSpec.FIREMAGE])
                returnString += "FireMage, ";
            if (myClasses[(int) TSVReader.ClassSpec.ARCANEMAGE])
                returnString += "ArcaneMage, ";
            if (myClasses[(int) TSVReader.ClassSpec.MWMONK])
                returnString += "Mistweaver, ";
            if (myClasses[(int) TSVReader.ClassSpec.WWMONK])
                returnString += "Windwalker, ";
            if (myClasses[(int) TSVReader.ClassSpec.BREWMONK])
                returnString += "Brewmaster, ";
            if (myClasses[(int) TSVReader.ClassSpec.RETPALADIN])
                returnString += "RetPal, ";
            if (myClasses[(int) TSVReader.ClassSpec.HOLYPALADIN])
                returnString += "HolyPal, ";
            if (myClasses[(int) TSVReader.ClassSpec.PROTPALADIN])
                returnString += "ProtPal, ";
            if (myClasses[(int) TSVReader.ClassSpec.DISCPRIEST])
                returnString += "Disc, ";
            if (myClasses[(int) TSVReader.ClassSpec.HOLYPRIEST])
                returnString += "HolyPriest, ";
            if (myClasses[(int) TSVReader.ClassSpec.SHADOWPRIEST])
                returnString += "Shadow, ";
            if (myClasses[(int) TSVReader.ClassSpec.SUBROGUE])
                returnString += "SubRogue, ";
            if (myClasses[(int) TSVReader.ClassSpec.ASSROGUE])
                returnString += "AssassinRogue, ";
            if (myClasses[(int) TSVReader.ClassSpec.OUTLAWROGUE])
                returnString += "Outlaw, ";
            if (myClasses[(int) TSVReader.ClassSpec.RESTOSHAM])
                returnString += "RestoSham, ";
            if (myClasses[(int) TSVReader.ClassSpec.ENHSHAM])
                returnString += "EnhSham, ";
            if (myClasses[(int) TSVReader.ClassSpec.ELESHAM])
                returnString += "EleSham, ";
            if (myClasses[(int) TSVReader.ClassSpec.DESTROLOCK])
                returnString += "Destro, ";
            if (myClasses[(int) TSVReader.ClassSpec.AFFLOCK])
                returnString += "Affliction, ";
            if (myClasses[(int) TSVReader.ClassSpec.DEMOLOCK])
                returnString += "Demonology, ";
            if (myClasses[(int) TSVReader.ClassSpec.ARMSWARRIOR])
                returnString += "ArmsWar, ";
            if (myClasses[(int) TSVReader.ClassSpec.FURYWARRIOR])
                returnString += "FuryWar, ";
            if (myClasses[(int) TSVReader.ClassSpec.PROTWARRIOR])
                returnString += "ProtWar ";
            return returnString;
        }
    }

    public class Team
    {
        public Player[] members;

        public override bool Equals(object other)
        {
            Team t2 = (Team) other;

            for (int i = 0; i < this.members.Length; i++)
            {
                if (!t2.members.Contains(this.members[i]))
                    return false;
            }

            return true;
        }

        public Team()
        {
        }

        public Team(Player[] dudes)
        {
            members = dudes.ToArray();
        }
    }

    public class Composition
    {
        public TSVReader.ClassSpec[] Comp;

        public static Composition[] GenerateViableCompArray()
        {
            return new[]
            {
                #region 2's comps

                #region DK

                #region Unholy

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #region Frost

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FROSTDK, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FROSTDK, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FROSTDK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FROSTDK, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FROSTDK, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FROSTDK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #endregion

                #region Hunt

                #region Surv

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SURVIVALHUNT, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SURVIVALHUNT, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SURVIVALHUNT, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SURVIVALHUNT, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SURVIVALHUNT, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SURVIVALHUNT, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #region BM

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.BEASTHUNT, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.BEASTHUNT, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.BEASTHUNT, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.BEASTHUNT, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.BEASTHUNT, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.BEASTHUNT, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #region Marksman

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.MARKSMANHUNT, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.MARKSMANHUNT, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.MARKSMANHUNT, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.MARKSMANHUNT, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.MARKSMANHUNT, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.MARKSMANHUNT, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #endregion

                #region Mage

                #region Frost

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #region Fire

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #region Arcane

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #endregion

                #region WWMonk

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.WWMONK, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.WWMONK, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.WWMONK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.WWMONK, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.WWMONK, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.WWMONK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #region Ret

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #region Shadow

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SHADOWPRIEST, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SHADOWPRIEST, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SHADOWPRIEST, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SHADOWPRIEST, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SHADOWPRIEST, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SHADOWPRIEST, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #region Rogue

                #region Assassin

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ASSROGUE, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ASSROGUE, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ASSROGUE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ASSROGUE, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ASSROGUE, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ASSROGUE, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #region Sub

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SUBROGUE, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SUBROGUE, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SUBROGUE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SUBROGUE, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SUBROGUE, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SUBROGUE, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #region Outlaw

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.OUTLAWROGUE, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.OUTLAWROGUE, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.OUTLAWROGUE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.OUTLAWROGUE, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.OUTLAWROGUE, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.OUTLAWROGUE, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #endregion

                #region Shaman

                #region Elemental

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ELESHAM, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ELESHAM, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ELESHAM, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ELESHAM, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ELESHAM, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ELESHAM, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #region Enhancement

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ENHSHAM, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ENHSHAM, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ENHSHAM, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ENHSHAM, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ENHSHAM, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ENHSHAM, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #endregion

                #region Warlock

                #region Destruction

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.DESTROLOCK, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.DESTROLOCK, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.DESTROLOCK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.DESTROLOCK, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.DESTROLOCK, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.DESTROLOCK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #region Aff

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.AFFLOCK, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.AFFLOCK, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.AFFLOCK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.AFFLOCK, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.AFFLOCK, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.AFFLOCK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #region Demon

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.DEMOLOCK, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.DEMOLOCK, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.DEMOLOCK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.DEMOLOCK, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.DEMOLOCK, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.DEMOLOCK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #endregion

                #region Warrior

                #region Fury

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FURYWARRIOR, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FURYWARRIOR, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FURYWARRIOR, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FURYWARRIOR, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FURYWARRIOR, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FURYWARRIOR, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #region Arms

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #endregion

                #endregion

                #region 3's

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.BALANCEDRUID, TSVReader.ClassSpec.RESTOSHAM, TSVReader.ClassSpec.HAVOCDH
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.BALANCEDRUID, TSVReader.ClassSpec.SUBROGUE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                #region Cupid Cleave

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.BEASTHUNT, TSVReader.ClassSpec.RESTODRUID
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.SURVIVALHUNT, TSVReader.ClassSpec.RESTODRUID
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.MARKSMANHUNT, TSVReader.ClassSpec.RESTODRUID
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.BEASTHUNT, TSVReader.ClassSpec.MWMONK
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.SURVIVALHUNT, TSVReader.ClassSpec.MWMONK
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.MARKSMANHUNT, TSVReader.ClassSpec.MWMONK
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.BEASTHUNT, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.SURVIVALHUNT,
                        TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.MARKSMANHUNT,
                        TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.BEASTHUNT, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.SURVIVALHUNT, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.MARKSMANHUNT, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.BEASTHUNT, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.SURVIVALHUNT, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.MARKSMANHUNT, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.BEASTHUNT, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.SURVIVALHUNT, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.MARKSMANHUNT, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.HAVOCDH, TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.RESTODRUID
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.HAVOCDH, TSVReader.ClassSpec.HAVOCDH, TSVReader.ClassSpec.RESTODRUID
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.HAVOCDH, TSVReader.ClassSpec.HAVOCDH, TSVReader.ClassSpec.MWMONK
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ELESHAM, TSVReader.ClassSpec.BALANCEDRUID, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ELESHAM, TSVReader.ClassSpec.BALANCEDRUID, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #region Feral Lock Heals

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.AFFLOCK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.DEMOLOCK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.DESTROLOCK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.AFFLOCK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.DEMOLOCK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.DESTROLOCK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #region Feral Mage Paladin

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                #endregion

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.SHADOWPRIEST,
                        TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                #region Jungle Cleave

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.BEASTHUNT, TSVReader.ClassSpec.RESTODRUID
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.SURVIVALHUNT, TSVReader.ClassSpec.RESTODRUID
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.MARKSMANHUNT, TSVReader.ClassSpec.RESTODRUID
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.BEASTHUNT, TSVReader.ClassSpec.MWMONK
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.SURVIVALHUNT, TSVReader.ClassSpec.MWMONK
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.MARKSMANHUNT, TSVReader.ClassSpec.MWMONK
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.BEASTHUNT, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.SURVIVALHUNT,
                        TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.MARKSMANHUNT,
                        TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.BEASTHUNT, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.SURVIVALHUNT, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.MARKSMANHUNT, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.BEASTHUNT, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.SURVIVALHUNT, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.MARKSMANHUNT, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.BEASTHUNT, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.SURVIVALHUNT, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.MARKSMANHUNT, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FERALDRUID, TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.DEMOLOCK, TSVReader.ClassSpec.ELESHAM, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                #region Mage Lock Pal/sham

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.AFFLOCK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.AFFLOCK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.AFFLOCK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.AFFLOCK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.AFFLOCK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.AFFLOCK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.DESTROLOCK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.DESTROLOCK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.DESTROLOCK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.DESTROLOCK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.DESTROLOCK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.DESTROLOCK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.DEMOLOCK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.DEMOLOCK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.DEMOLOCK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.DEMOLOCK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.DEMOLOCK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.DEMOLOCK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ASSROGUE, TSVReader.ClassSpec.AFFLOCK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ASSROGUE, TSVReader.ClassSpec.AFFLOCK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                #region RMP

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ASSROGUE, TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SUBROGUE, TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.OUTLAWROGUE, TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ASSROGUE, TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SUBROGUE, TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.OUTLAWROGUE, TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ASSROGUE, TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SUBROGUE, TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.OUTLAWROGUE, TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ASSROGUE, TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SUBROGUE, TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.OUTLAWROGUE, TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ASSROGUE, TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SUBROGUE, TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.OUTLAWROGUE, TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ASSROGUE, TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SUBROGUE, TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.OUTLAWROGUE, TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ASSROGUE, TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.RESTODRUID
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SUBROGUE, TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.RESTODRUID
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.OUTLAWROGUE, TSVReader.ClassSpec.FROSTMAGE, TSVReader.ClassSpec.RESTODRUID
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ASSROGUE, TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.RESTODRUID
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SUBROGUE, TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.RESTODRUID
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.OUTLAWROGUE, TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.RESTODRUID
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ASSROGUE, TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.RESTODRUID
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SUBROGUE, TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.RESTODRUID
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.OUTLAWROGUE, TSVReader.ClassSpec.ARCANEMAGE, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                #endregion

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ASSROGUE, TSVReader.ClassSpec.SHADOWPRIEST, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SUBROGUE, TSVReader.ClassSpec.SHADOWPRIEST, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.DESTROLOCK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.SHADOWPRIEST, TSVReader.ClassSpec.DESTROLOCK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #region Thunder Cleave

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.ELESHAM, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.ELESHAM, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.ELESHAM, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.ELESHAM, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.ELESHAM, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.ELESHAM, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #region TSG

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #region TURBO Cleave

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.ENHSHAM, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.ENHSHAM, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.ENHSHAM, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.ENHSHAM, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.ENHSHAM, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.ENHSHAM, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                #region Vanguard Cleave

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.RETPALADIN, TSVReader.ClassSpec.UNHOLYDK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.DESTROLOCK, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.AFFLOCK, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.ARMSWARRIOR, TSVReader.ClassSpec.FIREMAGE, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.WWMONK, TSVReader.ClassSpec.HAVOCDH, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.WWMONK, TSVReader.ClassSpec.HAVOCDH, TSVReader.ClassSpec.RESTODRUID
                    }
                },


                #region Turbo monk

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.WWMONK, TSVReader.ClassSpec.ENHSHAM, TSVReader.ClassSpec.RESTODRUID
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.WWMONK, TSVReader.ClassSpec.ENHSHAM, TSVReader.ClassSpec.MWMONK
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.WWMONK, TSVReader.ClassSpec.ENHSHAM, TSVReader.ClassSpec.HOLYPALADIN
                    }
                },

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.WWMONK, TSVReader.ClassSpec.ENHSHAM, TSVReader.ClassSpec.DISCPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.WWMONK, TSVReader.ClassSpec.ENHSHAM, TSVReader.ClassSpec.HOLYPRIEST
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.WWMONK, TSVReader.ClassSpec.ENHSHAM, TSVReader.ClassSpec.RESTOSHAM
                    }
                },

                #endregion

                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.WWMONK, TSVReader.ClassSpec.HAVOCDH, TSVReader.ClassSpec.RESTOSHAM
                    }
                },
                new Composition
                {
                    Comp = new[]
                    {
                        TSVReader.ClassSpec.WWMONK, TSVReader.ClassSpec.HAVOCDH, TSVReader.ClassSpec.HOLYPALADIN
                    }
                }

                #endregion
            };
        }
    }

    public class TSVReader
    {
        public enum ClassSpec
        {
            UNHOLYDK,
            FROSTDK,
            BLOODDK,
            VENGDH,
            HAVOCDH,
            BALANCEDRUID,
            RESTODRUID,
            GUARDDRUID,
            FERALDRUID,
            BEASTHUNT,
            SURVIVALHUNT,
            MARKSMANHUNT,
            FROSTMAGE,
            FIREMAGE,
            ARCANEMAGE,
            WWMONK,
            MWMONK,
            BREWMONK,
            RETPALADIN,
            HOLYPALADIN,
            PROTPALADIN,
            DISCPRIEST,
            HOLYPRIEST,
            SHADOWPRIEST,
            ASSROGUE,
            SUBROGUE,
            OUTLAWROGUE,
            ENHSHAM,
            ELESHAM,
            RESTOSHAM,
            AFFLOCK,
            DESTROLOCK,
            DEMOLOCK,
            ARMSWARRIOR,
            FURYWARRIOR,
            PROTWARRIOR,
            NUMOFCLASSSPEC
        }

        public enum PlayerInfoIndexes
        {
            TIMESTAMP,
            CLASS,
            DKSPEC,
            DHSPEC,
            DRUIDSPEC,
            HUNTSPEC,
            MAGESPEC,
            MONKSPEC,
            PALADINSPEC,
            PRIESTSPEC,
            ROGUESPEC,
            SHAMSPEC,
            WARLOCKSPEC,
            WARSPEC,
            CURRRATE,
            RATEDEV,
            DAYSTART,
            DAYEND,
            ENDSTART,
            ENDEND,
            DAYS,
            CHARNAME,
            MAINNAME,
            INDEXESCOUNT
        }

        public static Player[] GeneratePlayers(string fileLocation, int headerSize)
        {
            List<Player> playerArray = new List<Player>();
            StreamReader reader = new StreamReader(fileLocation);
            string[] header = reader.ReadLine().Split('\t'); //This will display all the data points
            while (!reader.EndOfStream)
            {
                string[] playerInfo = reader.ReadLine().Split('\t');
                bool[] classes = GenerateSpecs(playerInfo);
                Player currentPlayer = new Player(playerInfo[(int) PlayerInfoIndexes.MAINNAME],
                                                  playerInfo[(int) PlayerInfoIndexes.CHARNAME],
                                                  classes);
                currentPlayer.curRate = Convert.ToInt32(playerInfo[(int) PlayerInfoIndexes.CURRRATE]);
                currentPlayer.lowRatingBound =
                    currentPlayer.curRate - Convert.ToInt32(playerInfo[(int) PlayerInfoIndexes.RATEDEV]);
                currentPlayer.lowRatingBound = currentPlayer.lowRatingBound > 0 ? currentPlayer.lowRatingBound : 0;
                currentPlayer.highRatingBound =
                    currentPlayer.curRate + Convert.ToInt32(playerInfo[(int) PlayerInfoIndexes.RATEDEV]);
                playerArray.Add(currentPlayer);
            }

            reader.Close();
            return playerArray.ToArray();
        }

        public static bool[] GenerateSpecs(string[] playerLine)
        {
            bool[] possibleClasses = new bool[(int) ClassSpec.NUMOFCLASSSPEC];


            if (playerLine[(int) PlayerInfoIndexes.DKSPEC].Contains("Unholy"))
                possibleClasses[(int) ClassSpec.UNHOLYDK] = true;

            if (playerLine[(int) PlayerInfoIndexes.DKSPEC].Contains("Blood"))
                possibleClasses[(int) ClassSpec.BLOODDK] = true;

            if (playerLine[(int) PlayerInfoIndexes.DKSPEC].Contains("Frost"))
                possibleClasses[(int) ClassSpec.FROSTDK] = true;


            if (playerLine[(int) PlayerInfoIndexes.DHSPEC].Contains("Vengeance"))
                possibleClasses[(int) ClassSpec.VENGDH] = true;

            if (playerLine[(int) PlayerInfoIndexes.DHSPEC].Contains("Havoc"))
                possibleClasses[(int) ClassSpec.HAVOCDH] = true;


            if (playerLine[(int) PlayerInfoIndexes.DRUIDSPEC].Contains("Guardian"))
                possibleClasses[(int) ClassSpec.GUARDDRUID] = true;

            if (playerLine[(int) PlayerInfoIndexes.DRUIDSPEC].Contains("Balance"))
                possibleClasses[(int) ClassSpec.BALANCEDRUID] = true;

            if (playerLine[(int) PlayerInfoIndexes.DRUIDSPEC].Contains("Restoration"))
                possibleClasses[(int) ClassSpec.RESTODRUID] = true;

            if (playerLine[(int) PlayerInfoIndexes.DRUIDSPEC].Contains("Feral"))
                possibleClasses[(int) ClassSpec.FERALDRUID] = true;


            if (playerLine[(int) PlayerInfoIndexes.HUNTSPEC].Contains("Survival"))
                possibleClasses[(int) ClassSpec.SURVIVALHUNT] = true;

            if (playerLine[(int) PlayerInfoIndexes.HUNTSPEC].Contains("Marksman"))
                possibleClasses[(int) ClassSpec.MARKSMANHUNT] = true;

            if (playerLine[(int) PlayerInfoIndexes.HUNTSPEC].Contains("Beast Mastery"))
                possibleClasses[(int) ClassSpec.BEASTHUNT] = true;


            if (playerLine[(int) PlayerInfoIndexes.MAGESPEC].Contains("Frost"))
                possibleClasses[(int) ClassSpec.FROSTMAGE] = true;

            if (playerLine[(int) PlayerInfoIndexes.MAGESPEC].Contains("Arcane"))
                possibleClasses[(int) ClassSpec.ARCANEMAGE] = true;

            if (playerLine[(int) PlayerInfoIndexes.MAGESPEC].Contains("Fire"))
                possibleClasses[(int) ClassSpec.FIREMAGE] = true;


            if (playerLine[(int) PlayerInfoIndexes.MONKSPEC].Contains("Windwalker"))
                possibleClasses[(int) ClassSpec.WWMONK] = true;

            if (playerLine[(int) PlayerInfoIndexes.MONKSPEC].Contains("Mistweaver"))
                possibleClasses[(int) ClassSpec.MWMONK] = true;

            if (playerLine[(int) PlayerInfoIndexes.MONKSPEC].Contains("Brewmaster"))
                possibleClasses[(int) ClassSpec.BREWMONK] = true;


            if (playerLine[(int) PlayerInfoIndexes.PALADINSPEC].Contains("Retribution"))
                possibleClasses[(int) ClassSpec.RETPALADIN] = true;

            if (playerLine[(int) PlayerInfoIndexes.PALADINSPEC].Contains("Holy"))
                possibleClasses[(int) ClassSpec.HOLYPALADIN] = true;

            if (playerLine[(int) PlayerInfoIndexes.PALADINSPEC].Contains("Protection"))
                possibleClasses[(int) ClassSpec.PROTPALADIN] = true;


            if (playerLine[(int) PlayerInfoIndexes.PRIESTSPEC].Contains("Holy"))
                possibleClasses[(int) ClassSpec.HOLYPRIEST] = true;

            if (playerLine[(int) PlayerInfoIndexes.PRIESTSPEC].Contains("Discipline"))
                possibleClasses[(int) ClassSpec.DISCPRIEST] = true;

            if (playerLine[(int) PlayerInfoIndexes.PRIESTSPEC].Contains("Shadow"))
                possibleClasses[(int) ClassSpec.SHADOWPRIEST] = true;


            if (playerLine[(int) PlayerInfoIndexes.ROGUESPEC].Contains("Subtlety"))
                possibleClasses[(int) ClassSpec.SUBROGUE] = true;

            if (playerLine[(int) PlayerInfoIndexes.ROGUESPEC].Contains("Assassination"))
                possibleClasses[(int) ClassSpec.ASSROGUE] = true;

            if (playerLine[(int) PlayerInfoIndexes.ROGUESPEC].Contains("Outlaw"))
                possibleClasses[(int) ClassSpec.OUTLAWROGUE] = true;


            if (playerLine[(int) PlayerInfoIndexes.SHAMSPEC].Contains("Resto"))
                possibleClasses[(int) ClassSpec.RESTOSHAM] = true;

            if (playerLine[(int) PlayerInfoIndexes.SHAMSPEC].Contains("Enhance"))
                possibleClasses[(int) ClassSpec.ENHSHAM] = true;

            if (playerLine[(int) PlayerInfoIndexes.SHAMSPEC].Contains("Ele"))
                possibleClasses[(int) ClassSpec.ELESHAM] = true;


            if (playerLine[(int) PlayerInfoIndexes.WARLOCKSPEC].Contains("Destruction"))
                possibleClasses[(int) ClassSpec.DESTROLOCK] = true;

            if (playerLine[(int) PlayerInfoIndexes.WARLOCKSPEC].Contains("Affliction"))
                possibleClasses[(int) ClassSpec.AFFLOCK] = true;

            if (playerLine[(int) PlayerInfoIndexes.WARLOCKSPEC].Contains("Demon"))
                possibleClasses[(int) ClassSpec.DEMOLOCK] = true;


            if (playerLine[(int) PlayerInfoIndexes.WARSPEC].Contains("Arm"))
                possibleClasses[(int) ClassSpec.ARMSWARRIOR] = true;

            if (playerLine[(int) PlayerInfoIndexes.WARSPEC].Contains("Fury"))
                possibleClasses[(int) ClassSpec.FURYWARRIOR] = true;

            if (playerLine[(int) PlayerInfoIndexes.WARSPEC].Contains("Protection"))
                possibleClasses[(int) ClassSpec.PROTWARRIOR] = true;


            return possibleClasses;
        }
    }
}