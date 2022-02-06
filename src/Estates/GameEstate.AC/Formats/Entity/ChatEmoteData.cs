using System.IO;
using System.Text;

namespace GameEstate.AC.Formats.Entity
{
    public class ChatEmoteData
    {
        public readonly string MyEmote; // What the emote string is to the character doing the emote
        public readonly string OtherEmote; // What the emote string is to other characters

        public ChatEmoteData(BinaryReader r)
        {
            MyEmote = r.ReadL16ANSI(Encoding.Default); r.AlignBoundary();
            OtherEmote = r.ReadL16ANSI(Encoding.Default); r.AlignBoundary();
        }
    }
}
