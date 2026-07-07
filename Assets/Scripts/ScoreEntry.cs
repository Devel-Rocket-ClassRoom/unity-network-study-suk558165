// using System;
// using Unity.Collections;
// using Unity.Netcode;
// using UnityEngine;

// public struct ScoreEntry : INetworkSerializable, IEquatable<ScoreEntry>
// {
//     public ulong ClientID;
//     public FixedString32Bytes Name;
//     public int Score;

//     public bool Equals(ScoreEntry other)
//     {
//         return ClientID == other.ClientID && Name.Equals(other.Name) && Score == other.Score;
//     }

//     public void NetworkSerialize<T>(BufferSerializer<T> serializer)
//         where T : IReaderWriter
//     {
//         serializer.SerializeValue(ref ClientID);
//         serializer.SerializeValue(ref Name);
//         serializer.SerializeValue(ref Score);
//     }
// }
