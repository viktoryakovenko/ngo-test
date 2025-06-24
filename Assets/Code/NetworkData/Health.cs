using Unity.Netcode;

namespace Code.NetworkData
{
    public struct Health : INetworkSerializable
    {
        public int Max;
        public int Current;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Max);
            serializer.SerializeValue(ref Current);
        }
    }
}
