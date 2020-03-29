using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Living;

namespace WanderlustRealms.Services
{
    public class LanguageService
    {
        public string GetFluency(double level)
        {
            if(level >= 0 && level < 11)
            {
                return "";
            }else if(level > 10 && level < 21)
            {
                return "Beginner";
            }
            else if (level > 20 && level < 31)
            {
                return "Inept";
            }
            else if (level > 30 && level < 41)
            {
                return "Bungling";
            }
            else if (level > 40 && level < 51)
            {
                return "Communicative";
            }
            else if (level > 50 && level < 61)
            {
                return "Conversant";
            }
            else if (level > 60 && level < 71)
            {
                return "Skilled";
            }
            else if (level > 70 && level < 81)
            {
                return "Proficient";
            }
            else if (level > 80 && level < 91)
            {
                return "Near Fluent";
            }
            else
            {
                return "Fluent";
            }
        }

        public string GetAccentAdjective(double level)
        {
            if (level >= 0 && level < 11)
            {
                return "totally inept";
            }
            else if (level > 10 && level < 21)
            {
                return "barely understandable";
            }
            else if (level > 20 && level < 31)
            {
                return "broken";
            }
            else if (level > 30 && level < 41)
            {
                return "somewhat understandable";
            }
            else if (level > 40 && level < 51)
            {
                return "heavily accented";
            }
            else if (level > 50 && level < 61)
            {
                return "halting";
            }
            else if (level > 60 && level < 71)
            {
                return "somewhat accented";
            }
            else if (level > 70 && level < 81)
            {
                return "lightly accented";
            }
            else if (level > 80 && level < 91)
            {
                return "nearly flawless";
            }
            else
            {
                return "flawless";
            }
        }

        public string GetLanguageModifier(Language lan)
        {
            Random rnd = new Random();

            var roll = rnd.Next(1, 3);

            if(roll == 1)
            {
                return lan.Adjective;
            }
            else
            {
                return lan.Adjective2;
            }
        }

        public string RandomizeString(string input)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, input.Length)
              .Select(s => s[random.Next(s.Length)]).ToArray()).ToLower();
        }

        public void UnderstandSpeech(string msg, Language targetLang, Living listener, Living speaker)
        {

        }

        public string TransformSpeech(string msg, double level)
        {
            Random random = new Random();
            var parsedMsg = msg.Split(" ");
            var returnMsg = new List<string>();

            if(level > 89)
            {
                return msg;
            }
            else
            {
                foreach(string s in parsedMsg)
                {
                    var roll = random.Next(1, 101);
                    if(roll <= level)
                    {
                        returnMsg.Add(s);
                    }
                    else
                    {
                        returnMsg.Add(RandomizeString(s));
                    }
                    
                }

                return string.Join(" ", returnMsg);
            }

        }
    }
}
