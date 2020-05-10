using System;
using UnityEngine;
using Util.Control;

namespace Util
{
    public class Pair<T, V>
    {
        public T first;
        public V second;
    }

    [Serializable] public class KeyCodeStringPair: Pair<KeyCode, string>{}
    [Serializable] public class KeyCodeSpritePair: Pair<KeyCode, Sprite>{}
    [Serializable] public class StringSpritePair: Pair<string, Sprite>{}
    [Serializable] public class StringStringPair: Pair<string, string>{}
    [Serializable] public class AxisSpritePair: Pair<Axis, Sprite>{}
    [Serializable] public class AxisStringPair: Pair<Axis, string>{}
}