using UnityEngine;

namespace Code.Feed
{
    [CreateAssetMenu(fileName = "FeedInfo", menuName = "SO/Feed/Info", order = 0)]
    public class FeedInfoSO : ScriptableObject
    {
        public string feedName;
        public int feedLevel;
        public int hungerValue;
        public int feedPrice;
        public float fallSpeed;
    }
}