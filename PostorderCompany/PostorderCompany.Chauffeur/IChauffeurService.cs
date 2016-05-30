using System.Collections.Generic;

namespace PostorderCompany.Chauffeur
{
    public interface IChauffeurService {
        List<PakketStatus> GetStatuses();
        void SendOrder(PakketStatus status, string chauffeur);
        void OrderDelivered(PakketStatus status, string handtekening);
        void StartListening();
        bool HandleEvent(string eventType, string eventData);
    }
}