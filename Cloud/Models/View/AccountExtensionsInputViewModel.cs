namespace SmartSolucionesCuba.Viderhe.Cloud.Models.View
{
    public class AccountExtensionsInputViewModel
    {
        public AccountExtensionsInputViewModel()
        {
            SelectableExtensions = new System.Collections.Generic.List<MOROPO.SAPRESSC.Core.Common.Models.View.SelectionDescriptedItemInputViewModel<System.Guid>>();
        }

        public System.Guid AccountId { get; set; }

        public Persistence.Entities.Account Account { get; set; }

        public System.Collections.Generic.List<MOROPO.SAPRESSC.Core.Common.Models.View.SelectionDescriptedItemInputViewModel<System.Guid>> SelectableExtensions { get; set; }
    }
}