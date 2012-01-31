namespace Suteki.TardisBank.Infrastructure.NHibernateMaps
{
    using FluentNHibernate.Automapping;
    using FluentNHibernate.Automapping.Alterations;

    using Suteki.TardisBank.Model;

    public class MessageMap : IAutoMappingOverride<Message>
    {
        public void Override(AutoMapping<Message> mapping)
        {
            mapping.References(m => m.User);
        }
    }
}