using NServiceBus;

namespace AddUserBalanceCommand
{
    public class AddUserBalance : ICommand
    {
        public int UserId { get; set; }
        public int Balance { get; set; }
    }
}
