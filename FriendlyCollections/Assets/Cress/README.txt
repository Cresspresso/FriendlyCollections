==================================
    FriendlyCollections

Beautiful custom property drawers for serializable data structures such as Dictionary.

Title:    FriendlyCollections
Version:  1.0
Author:   Elijah Shadbolt (Cresspresso)
Email:    cresspresso@gmail.com



==================================
    Features

- Serializable data structures with custom property drawers.
  - Dictionary<TKey, TValue>                  FriDict and FriDictPair
  - Dictionary<UnityEngine.Object, TValue>    FriDictOfObjects and FriDictPair
  - List<T>                                   FriList
- Reorderable lists with insert and remove options.
- Duplicate dictionary keys highlighted in red. Red entries are not deserialized.
- Ability to extend custom property drawers for complex object types.



==================================
    Examples
    


----------------------------------
Example 1. List

    using System;
    using System.Collections.Generic
    using UnityEngine;
    using Cress;

    public class MyScript : MonoBehaviour
    {
        public StringList friendlyList;
    }

    [Serializable]
    public class StringList : FriList<string> {}
    


----------------------------------
Example 2. Dictionary

    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Cress;

    public class MyScript : MonoBehaviour
    {
        // Serializable dictionary.
        [SerializeField]
        private StringIntDict friendlyDictionary;

        // Deserialized dictionary.
        public Dictionary<string, int> dictionary { get { return friendlyDictionary.data; } }

        void Start()
        {
            // Use the deserialized dictionary.
            Debug.Log(dictionary.Count);
        }
    }

    // Create serializable class which uses the property drawer.
    // Make sure the class is not generic.
    [Serializable]
    public class StringIntDict : FriDict<string, int, StringIntPair> {}

    [Serializable]
    public class StringIntPair : FriDictPair<string, int> {}



==================================
    Requires External Packages

- MoreLinq (MoreLinq 2.10, for .NETFramework 3.5)



All the best!
