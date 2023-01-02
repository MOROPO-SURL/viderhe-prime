namespace SmartSolucionesCuba.Viderhe.Cloud.Helpers.Integrity
{
    public static class AccountExtensionBridgeIntegrityHelper
    {
        public static string GetSignature(Persistence.Entities.AccountExtensionBridge entity)
        {
            return MOROPO.SAPRESSC.Core.Persistence.Helpers.DataIntegrityHelper.GetEntitySignature(entity);
        }

        public static bool IsCorrectlySigned(Persistence.Entities.AccountExtensionBridge entity)
        {
            return entity.Signature == GetSignature(entity);
        }
    }
}
