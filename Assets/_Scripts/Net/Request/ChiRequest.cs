using LitJson;

namespace AssemblyCSharp
{
    public class ChiRequest : ClientRequest
    {
        public ChiRequest(CardVO cardvo)
        {
            headCode = APIS.CHIPAI_REQUEST;
            messageContent = JsonMapper.ToJson(cardvo); ;
        }
    }
}
