namespace SmartSolucionesCuba.Viderhe.Cloud.Helpers.Integrity
{
    public class PlayerIntegrityHelper
    {
        public static string GetSignature(Persistence.Entities.Player entity)
        {
            return MOROPO.SAPRESSC.Core.Persistence.Helpers.DataIntegrityHelper.GetEntitySignature(entity);
        }

        public static bool IsCorrectlySigned(Persistence.Entities.Player entity)
        {
            return entity.Signature == GetSignature(entity);
        }
    }
}
