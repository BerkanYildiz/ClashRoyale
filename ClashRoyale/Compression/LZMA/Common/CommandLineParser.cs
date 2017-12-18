namespace ClashRoyale.Compression.LZMA.Common
{
    using System;
    using System.Collections;

    public enum SwitchType
    {
        Simple,

        PostMinus,

        LimitedPostString,

        UnLimitedPostString,

        PostChar
    }

    public class SwitchForm
    {
        public string IdString;

        public int MaxLen;

        public int MinLen;

        public bool Multi;

        public string PostCharSet;

        public SwitchType Type;

        public SwitchForm(string IdString, SwitchType Type, bool Multi, int MinLen, int MaxLen, string PostCharSet)
        {
            this.IdString = IdString;
            this.Type = Type;
            this.Multi = Multi;
            this.MinLen = MinLen;
            this.MaxLen = MaxLen;
            this.PostCharSet = PostCharSet;
        }

        public SwitchForm(string IdString, SwitchType Type, bool Multi, int MinLen)
            : this(IdString, Type, Multi, MinLen, 0, string.Empty)
        {
        }

        public SwitchForm(string IdString, SwitchType Type, bool Multi)
            : this(IdString, Type, Multi, 0)
        {
        }
    }

    public class SwitchResult
    {
        public int PostCharIndex;

        public ArrayList PostStrings = new ArrayList();

        public bool ThereIs;

        public bool WithMinus;

        public SwitchResult()
        {
            this.ThereIs = false;
        }
    }

    public class Parser
    {
        private const string kStopSwitchParsing = "--";

        private const char kSwitchID1 = '-';

        private const char kSwitchID2 = '/';

        private const char kSwitchMinus = '-';

        public ArrayList NonSwitchStrings = new ArrayList();

        private readonly SwitchResult[] Switches;

        public Parser(int NumSwitches)
        {
            this.Switches = new SwitchResult[NumSwitches];
            for (int i = 0; i < NumSwitches; i++)
            {
                this.Switches[i] = new SwitchResult();
            }
        }

        public SwitchResult this[int Index] => this.Switches[Index];

        public static int ParseCommand(CommandForm[] CommandForms, string CommandString, out string PostString)
        {
            for (int i = 0; i < CommandForms.Length; i++)
            {
                string id = CommandForms[i].IdString;
                if (CommandForms[i].PostStringMode)
                {
                    if (CommandString.IndexOf(id) == 0)
                    {
                        PostString = CommandString.Substring(id.Length);
                        return i;
                    }
                }
                else if (CommandString == id)
                {
                    PostString = string.Empty;
                    return i;
                }
            }

            PostString = string.Empty;
            return -1;
        }

        public void ParseStrings(SwitchForm[] SwitchForms, string[] CommandStrings)
        {
            int NumCommandStrings = CommandStrings.Length;
            bool StopSwitch = false;
            for (int i = 0; i < NumCommandStrings; i++)
            {
                string s = CommandStrings[i];
                if (StopSwitch)
                {
                    this.NonSwitchStrings.Add(s);
                }
                else if (s == Parser.kStopSwitchParsing)
                {
                    StopSwitch = true;
                }
                else if (!this.ParseString(s, SwitchForms))
                {
                    this.NonSwitchStrings.Add(s);
                }
            }
        }

        private static bool IsItSwitchChar(char C)
        {
            return C == Parser.kSwitchID1 || C == Parser.kSwitchID2;
        }

        private static bool ParseSubCharsCommand(int NumForms, CommandSubCharsSet[] Forms, string CommandString, ArrayList Indices)
        {
            Indices.Clear();
            int NumUsedChars = 0;
            for (int i = 0; i < NumForms; i++)
            {
                CommandSubCharsSet CharsSet = Forms[i];
                int CurrentIndex = -1;
                int len = CharsSet.Chars.Length;
                for (int j = 0; j < len; j++)
                {
                    char c = CharsSet.Chars[j];
                    int NewIndex = CommandString.IndexOf(c);
                    if (NewIndex >= 0)
                    {
                        if (CurrentIndex >= 0)
                        {
                            return false;
                        }

                        if (CommandString.IndexOf(c, NewIndex + 1) >= 0)
                        {
                            return false;
                        }

                        CurrentIndex = j;
                        NumUsedChars++;
                    }
                }

                if (CurrentIndex == -1 && !CharsSet.EmptyAllowed)
                {
                    return false;
                }

                Indices.Add(CurrentIndex);
            }

            return NumUsedChars == CommandString.Length;
        }

        private bool ParseString(string SrcString, SwitchForm[] SwitchForms)
        {
            int len = SrcString.Length;
            if (len == 0)
            {
                return false;
            }

            int pos = 0;
            if (!Parser.IsItSwitchChar(SrcString[pos]))
            {
                return false;
            }

            while (pos < len)
            {
                if (Parser.IsItSwitchChar(SrcString[pos]))
                {
                    pos++;
                }

                const int KNoLen = -1;
                int MatchedSwitchIndex = 0;
                int MaxLen = KNoLen;
                for (int SwitchIndex = 0; SwitchIndex < this.Switches.Length; SwitchIndex++)
                {
                    int SwitchLen = SwitchForms[SwitchIndex].IdString.Length;
                    if (SwitchLen <= MaxLen || pos + SwitchLen > len)
                    {
                        continue;
                    }

                    if (string.Compare(SwitchForms[SwitchIndex].IdString, 0, SrcString, pos, SwitchLen, true) == 0)
                    {
                        MatchedSwitchIndex = SwitchIndex;
                        MaxLen = SwitchLen;
                    }
                }

                if (MaxLen == KNoLen)
                {
                    throw new Exception("maxLen == kNoLen");
                }

                SwitchResult MatchedSwitch = this.Switches[MatchedSwitchIndex];
                SwitchForm SwitchForm = SwitchForms[MatchedSwitchIndex];
                if (!SwitchForm.Multi && MatchedSwitch.ThereIs)
                {
                    throw new Exception("switch must be single");
                }

                MatchedSwitch.ThereIs = true;
                pos += MaxLen;
                int TailSize = len - pos;
                SwitchType type = SwitchForm.Type;
                switch (type)
                {
                    case SwitchType.PostMinus:
                        {
                            if (TailSize == 0)
                            {
                                MatchedSwitch.WithMinus = false;
                            }
                            else
                            {
                                MatchedSwitch.WithMinus = SrcString[pos] == Parser.kSwitchMinus;
                                if (MatchedSwitch.WithMinus)
                                {
                                    pos++;
                                }
                            }

                            break;
                        }

                    case SwitchType.PostChar:
                        {
                            if (TailSize < SwitchForm.MinLen)
                            {
                                throw new Exception("switch is not full");
                            }

                            string CharSet = SwitchForm.PostCharSet;
                            const int KEmptyCharValue = -1;
                            if (TailSize == 0)
                            {
                                MatchedSwitch.PostCharIndex = KEmptyCharValue;
                            }
                            else
                            {
                                int index = CharSet.IndexOf(SrcString[pos]);
                                if (index < 0)
                                {
                                    MatchedSwitch.PostCharIndex = KEmptyCharValue;
                                }
                                else
                                {
                                    MatchedSwitch.PostCharIndex = index;
                                    pos++;
                                }
                            }

                            break;
                        }

                    case SwitchType.LimitedPostString:
                    case SwitchType.UnLimitedPostString:
                        {
                            int MinLen = SwitchForm.MinLen;
                            if (TailSize < MinLen)
                            {
                                throw new Exception("switch is not full");
                            }

                            if (type == SwitchType.UnLimitedPostString)
                            {
                                MatchedSwitch.PostStrings.Add(SrcString.Substring(pos));
                                return true;
                            }

                            string StringSwitch = SrcString.Substring(pos, MinLen);
                            pos += MinLen;
                            for (int i = MinLen; i < SwitchForm.MaxLen && pos < len; i++, pos++)
                            {
                                char c = SrcString[pos];
                                if (Parser.IsItSwitchChar(c))
                                {
                                    break;
                                }

                                StringSwitch += c;
                            }

                            MatchedSwitch.PostStrings.Add(StringSwitch);
                            break;
                        }
                }
            }

            return true;
        }
    }

    public class CommandForm
    {
        public string IdString = string.Empty;

        public bool PostStringMode;

        public CommandForm(string IdString, bool PostStringMode)
        {
            this.IdString = IdString;
            this.PostStringMode = PostStringMode;
        }
    }

    internal class CommandSubCharsSet
    {
        public string Chars = string.Empty;

        public bool EmptyAllowed = false;
    }
}