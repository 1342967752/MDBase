using System;

public class HeadRequest : ClientRequest
{
    public HeadRequest()
    {
        headCode = APIS.head;
        messageContent = "";
    }

}
