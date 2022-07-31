using UnityEngine;

namespace Mirror.Examples.NetworkRoom
{
    public class PlayerScore : NetworkBehaviour
    {
        [SyncVar]
        public int payerName;

        [SyncVar]
        public uint score;

        
    }
}
