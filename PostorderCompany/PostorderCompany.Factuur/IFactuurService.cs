using System.Collections.Generic;

namespace PostorderCompany.Factuur {
    public interface IFactuurService {

        void SendMessage(Core.Models.Factuur factuur);
        List<Core.Models.Factuur> GetFacturen();
        void Remove(Core.Models.Factuur factuur);
        bool HandleEvent(string eventType, string eventData);
    }
}