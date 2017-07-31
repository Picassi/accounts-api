using System;

namespace Picassi.Generator.Accounts
{
    public interface IModelDataGenerator
    {
        Type Type { get; }
        void Generate(DataGenerationContext context);
    }
}
