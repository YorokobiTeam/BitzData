
namespace BitzData.Services
{
    /// <summary>
    /// This class is a singleton service for managing account-related operations in the Bitz game.
    /// </summary>
    public class BitzAccountService
    {
        private static BitzAccountService instance;


        private BitzAccountService()
        {

        }
        public static BitzAccountService getInstance()
        {
            instance ??= new BitzAccountService();
            return instance;
        }
    }
}
