namespace PostorderCompany.Chauffeur
{
    class PakketStatus
    {
        public string pakketId { get; set; }
        public bool onderweg { get; set; }
        public bool afgeleverd { get; set; }
        public string chauffeur { get; set; }
        public string handtekening { get; set; }
    }
}
