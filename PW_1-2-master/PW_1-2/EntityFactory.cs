using PW_1_2.Database;

namespace PW_1_2
{
    public class EntityFactory
    {
        public EntityDAO CreateEntity(string select)
        {
            return select switch
            {
                "1" => new DirectorDAO(),
                "2" => new CompanyDAO(),
                "3" => new PhoneDAO(),
                "4" => new NetworkDAO(),
                _ => null,
            };
        }
    }

}
