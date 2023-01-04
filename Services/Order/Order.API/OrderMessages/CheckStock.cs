using NServiceBus;

namespace OrderMessages
{
    public class CheckStock : ICommand
    {
        public int Id { get; set; }
    }
}
