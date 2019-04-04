using System;
using System.Collections.Generic;
using UnityEngine.Networking;

public class DefaultUDP : ICustomUDP
{
    public int MaxConnections
    {
        get
        {
            return 10;
        }
    }

    public Dictionary<string, QosType> Chanels
    {
        get
        {
            var dict = new Dictionary<string, QosType>
            {
                { "UnreliableChanel", QosType.Reliable },
                { "ReliableSequencedChanel", QosType.ReliableSequenced },
                { "UnreliableFragmented", QosType.UnreliableFragmented }
            };
            return dict;
        }
    }
}

public interface ICustomUDP
{
    int MaxConnections { get; }
    Dictionary<string, QosType> Chanels { get; }
}
