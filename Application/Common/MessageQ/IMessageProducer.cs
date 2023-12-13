namespace Application.Common.MessageQ
{
    public interface IMessageProducer
    {
        void SendMessage<T> (T message);
    }
}
//Todo nereye konumlandırılması gerektiğinden emin olamadm. 