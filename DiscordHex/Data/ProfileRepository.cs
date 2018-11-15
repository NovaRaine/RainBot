using Dapper;
using DiscordHex.Core;
using DiscordHex.Domain;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Data
{
    public class ProfileRepository
    {
        public UserProfileEntity GetUserProfile(string id)
        {
            var profile = new UserProfileEntity();

            var userSql = $@"SELECT up.guid, up.discordUser, buffsCasted, buffsReceived, damageCasted, damageReceived, hexCasted, hexReceived, gamesStarted, du.discordId
                         FROM ""RainBot"".""UserProfiles"" AS up
                         LEFT JOIN ""RainBot"".""DiscordUsers"" AS du
                         ON du.guid = up.discordUser
                         WHERE du.discordId = @id";

            var effectsSql = $@"SELECT * FROM ""RainBot"".""ActiveEffects"" WHERE discordId = @id AND (endTime > LOCALTIMESTAMP AND startTime < LOCALTIMESTAMP)";

            using (var conn = new NpgsqlConnection(BotSettings.Instance.Config.ConnectionString))
            {
                conn.Open();
                var cmd = new NpgsqlCommand(userSql, conn);
                cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Text, id);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    try
                    {
                        profile.ProfileGuid = (int)reader["guid"];
                        profile.DiscordUserGuid = (int)reader["discordUser"];
                        profile.BuffsCasted = (int)reader["buffsCasted"];
                        profile.BuffsReceived = (int)reader["buffsReceived"];
                        profile.DamageCasted = (int)reader["damageCasted"];
                        profile.DamageReceived = (int)reader["damageReceived"];
                        profile.HexCasted = (int)reader["hexCasted"];
                        profile.HexReceived = (int)reader["hexReceived"];
                        profile.GamesStarted = (int)reader["gamesStarted"];
                        profile.DiscordId = (string)reader["discordId"];
                    }
                    catch (Exception ex)
                    {
                        // ignore for now
                    }
                }

                reader.Close();

                if (string.IsNullOrEmpty(profile.DiscordId)) return profile;

                var efCmd = new NpgsqlCommand(effectsSql, conn);
                efCmd.Parameters.AddWithValue("@id", profile.DiscordId);

                var effectReader = efCmd.ExecuteReader();

                var effects = new List<SpellEffectEntity>();

                while (effectReader.Read())
                {
                    try
                    {
                        var effect = new SpellEffectEntity
                        {
                            Guid = (int) effectReader["guid"],
                            DiscordId = (string) effectReader["discordId"],
                            SpellName = (string) effectReader["spellName"],
                            StartTime = (DateTime) effectReader["startTime"],
                            EndTime = (DateTime) effectReader["endTime"]
                        };
                        effects.Add(effect);
                    }
                    catch (Exception ex)
                    {
                        // ignore for now
                    }
                }

                profile.ActiveEffects = effects;

            }
            return profile;
        }

        public  void UpdateProfile(UserProfileEntity profile)
        {
            const string sql = @"UPDATE ""RainBot"".""UserProfiles"" SET buffsCasted = @bc, buffsReceived = @br, damageCasted = @dc, damageReceived = @dr, hexCasted = @hc, hexReceived = @hr, gamesStarted = @gs WHERE discordUser = @du";

            using (var conn = new NpgsqlConnection(BotSettings.Instance.Config.ConnectionString))
            {
                conn.Open();
                var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@bc", NpgsqlDbType.Integer, profile.BuffsCasted);
                cmd.Parameters.AddWithValue("@br", NpgsqlDbType.Integer, profile.BuffsReceived);
                cmd.Parameters.AddWithValue("@dc", NpgsqlDbType.Integer, profile.DamageCasted);
                cmd.Parameters.AddWithValue("@dr", NpgsqlDbType.Integer, profile.DamageReceived);
                cmd.Parameters.AddWithValue("@hc", NpgsqlDbType.Integer, profile.HexCasted);
                cmd.Parameters.AddWithValue("@hr", NpgsqlDbType.Integer, profile.HexReceived);
                cmd.Parameters.AddWithValue("@du", NpgsqlDbType.Integer, profile.DiscordUserGuid);
                cmd.Parameters.AddWithValue("@gs", NpgsqlDbType.Integer, profile.GamesStarted);

                cmd.ExecuteNonQuery();
            }
        }

        public bool SaveNewUserProfile(string id)
        {
            var res = 0;
            using (var conn = new NpgsqlConnection(BotSettings.Instance.Config.ConnectionString))
            {
                conn.Open();
                var inserted = conn.Query<int>(@"INSERT INTO ""RainBot"".""DiscordUsers"" (discordId) VALUES (@id) RETURNING ""guid""", new { id }).Single();
                if (inserted > 0)
                    res = conn.Execute(@"INSERT INTO ""RainBot"".""UserProfiles"" (discordUser, buffsCasted, buffsReceived, damageCasted, damageReceived, hexCasted, hexReceived, gamesStarted) VALUES (@inserted, 0, 0, 0, 0, 0, 0, 0)", new { inserted });
            }

            return res > 0;
        }

        public void AddSpellEffect(List<string> userIds, string spellName, int duration)
        {
            using (var conn = new NpgsqlConnection(BotSettings.Instance.Config.ConnectionString))
            {
                conn.Open();
                foreach (var id in userIds)
                {
                    conn.Query<int>($@"INSERT INTO ""RainBot"".""ActiveEffects"" (discordid, spellname, starttime, endtime) VALUES (@id, @spellname, LOCALTIMESTAMP, LOCALTIMESTAMP +interval '{duration} hour')", new { id, spellName });
                }
            }
        }
    }
}
