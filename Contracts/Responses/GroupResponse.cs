using StoreMarket.Models;

namespace StoreMarket.Contracts.Responses
{
    public class GroupResponse
    {
        public int Id { get; set; }
        public int Count { get; set; }

        public GroupResponse(Group group)
        {
            Id = group.Id;
            Count = group.Count;
        }
    }
}
