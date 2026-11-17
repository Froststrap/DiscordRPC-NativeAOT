using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscordRPC.Message
{
    /// <summary>
    /// The connection to the discord client was successful. This is called before <see cref="MessageType.Ready"/>.
    /// </summary>
    public class ConnectionEstablishedMessage : IMessage
    {
        /// <summary>
        /// The type of message received from discord
        /// </summary>
        public override MessageType Type { get { return MessageType.ConnectionEstablished; } }

        /// <summary>
        /// The pipe we ended up connecting too
        /// </summary>
        [System.Obsolete("The connected pipe is not necessary information.")]
        public int ConnectedPipe { get; internal set; }
    }
}
