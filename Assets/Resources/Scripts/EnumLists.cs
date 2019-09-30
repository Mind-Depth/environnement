using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
class EnumLists
{
    [Serializable]
    public class KeyValue
    {
        public string key;
        public int value;
    }

    static IEnumerable<KeyValue> IterEnum<E>()
    {
        foreach (E e in Enum.GetValues(typeof(E)))
        {
            KeyValue nv = new KeyValue();
            nv.key = Enum.GetName(typeof(E), e);
            nv.value = Convert.ToInt32(e);
            yield return nv;
        }
    }

    static List<KeyValue> EnumToList<E>()
    {
        return IterEnum<E>().ToList();
    }

    public List<KeyValue> GenerationMessageType = EnumToList<GenerationMessage.Type>();
    public List<KeyValue> EnvironmentMessageType = EnumToList<EnvironmentMessage.Type>();
    public List<KeyValue> Fears = EnumToList<Fear>();
    public List<KeyValue> ModelConfigurationType = EnumToList<ModelConfiguration.Type>();

    public static readonly EnumLists instance = new EnumLists();
}
