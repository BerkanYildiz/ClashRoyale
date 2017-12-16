namespace ClashRoyale.Server.Logic.Slots
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading.Tasks;

    using ClashRoyale.Server.Logic.Entries;
    using ClashRoyale.Server.Network.Packets.Server.Alliance;

    internal class AllianceMemberEntries : ConcurrentDictionary<long, AllianceMemberEntry>
    {
        internal Clan Clan;
        internal ConcurrentDictionary<long, Player> Connected;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceMemberEntries"/> class.
        /// </summary>
        internal AllianceMemberEntries()
        {
            this.Connected  = new ConcurrentDictionary<long, Player>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceMemberEntries"/> class.
        /// </summary>
        /// <param name="Clan">The alliance.</param>
        internal AllianceMemberEntries(Clan Clan) : this()
        {
            this.Clan = Clan;
        }

        /// <summary>
        /// Adds the specified player to the online list.
        /// </summary>
        internal async Task<bool> AddOnlinePlayer(Player Player)
        {
            if (this.Connected.TryAdd(Player.PlayerId, Player))
            {
                int Online = this.Connected.Count;

                await Task.Run(() =>
                {
                    foreach (Player Connected in this.Connected.Values.ToArray())
                    {
                        Connected.GameMode.Device.NetworkManager.SendMessage(new AllianceOnlineStatusUpdatedMessage(Connected.GameMode.Device, Online));
                    }
                });

                return true;
            }
            else
            {
                Logging.Error(this.GetType(), "AddOnlinePlayer() - Player can't be added, false has been returned.");
            }

            return false;
        }

        /// <summary>
        /// Removes the specified player of the online list.
        /// </summary>
        internal async Task<bool> RemoveOnlinePlayer(Player Player)
        {
            if (this.Connected.TryRemove(Player.PlayerId, out _))
            {
                int Online = this.Connected.Count;

                await Task.Run(() =>
                {
                    foreach (Player Connected in this.Connected.Values.ToArray())
                    {
                        Connected.GameMode.Device.NetworkManager.SendMessage(new AllianceOnlineStatusUpdatedMessage(Connected.GameMode.Device, Online));
                    }
                });

                return true;
            }
            else
            {
                Logging.Error(this.GetType(), "RemoveOnlinePlayer() - Player can't be removed, false has been returned.");
            }

            return false;
        }

        /// <summary>
        /// Adds the specified player.
        /// </summary>
        /// <param name="Player">The player.</param>
        /// <param name="NewAlliance">true if alliance is new.</param>
        internal async Task<bool> TryAdd(Player Player, bool NewAlliance)
        {
            if (!this.ContainsKey(Player.PlayerId))
            {
                if (NewAlliance || (this.Count < 50 && this.Count > 0))
                {
                    AllianceMemberEntry Entry = new AllianceMemberEntry(this.Clan, Player, NewAlliance ? 2 : 1);

                    if (this.TryAdd(Player.PlayerId, Entry))
                    {
                        await this.AddOnlinePlayer(Player);
                        return true;
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "TryAdd() - Player can't be added, false has been returned.");
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "TryAdd() - Player can't be added, the limit of 50 members has been reached.");
                }
            }
            else
            {
                Logging.Error(this.GetType(), "TryAdd() - Member can't be added, this instance is already in the list.");
            }

            return false;
        }

        /// <summary>
        /// Removes the specified member.
        /// </summary>
        /// <param name="Member">The member.</param>
        internal async Task<bool> TryRemove(AllianceMemberEntry Member)
        {
            if (Member != null)
            {
                if (this.TryRemove(Member.PlayerId, out AllianceMemberEntry _))
                {
                    if (this.Connected.TryGetValue(Member.PlayerId, out Player Player))
                    {
                        await this.RemoveOnlinePlayer(Player);
                    }

                    return true;
                }
                else
                {
                    Logging.Error(this.GetType(), "TryRemove(PlayerID, out _) returned false.");
                }
            }
            else
            {
                Logging.Error(this.GetType(), "TryRemove() - Member was null at Remove(Member).");
            }

            return false;
        }

        /// <summary>
        /// Removes the specified member.
        /// </summary>
        /// <param name="Member">The member.</param>
        internal async Task<bool> TryRemove(Player Member)
        {
            if (Member != null)
            {
                if (this.TryRemove(Member.PlayerId, out AllianceMemberEntry _))
                {
                    if (this.Connected.TryGetValue(Member.PlayerId, out Player Player))
                    {
                        await this.RemoveOnlinePlayer(Player);
                    }

                    return true;
                }
                else
                {
                    Logging.Error(this.GetType(), "TryRemove(PlayerID, out _) returned false.");
                }
            }
            else
            {
                Logging.Error(this.GetType(), "TryRemove() - Member was null at Remove(Member).");
            }

            return false;
        }

        /// <summary>
        /// Executes the specific action for each <see cref="AllianceMemberEntry"/>.
        /// </summary>
        /// <param name="Action">The action.</param>
        internal void ForEach(Action<AllianceMemberEntry> Action)
        {
            AllianceMemberEntry[] Entries = this.Values.ToArray();

            for (int I = 0; I < Entries.Length; I++)
            {
                Action.Invoke(Entries[I]);
            }
        }
    }
}